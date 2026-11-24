using System.Runtime.Versioning;
using FM27Analyzer.Extraction.Poc.Capture;

namespace FM27Analyzer.Extraction.Tests;

[TestClass]
[SupportedOSPlatform("windows")]
public sealed class CaptureSessionTests
{
    private static readonly CaptureLimits Limits = new(8_192, 1, 1, TimeSpan.FromSeconds(2));
    private static readonly PlayerKey Player = new("fixture", "0001");
    private static readonly FieldKey Passing = new("passing", null);
    private static readonly FieldKey Ability = new("current-ability", null);
    private readonly string folder = "fixture-" + Guid.NewGuid().ToString("N");

    [TestCleanup]
    public void RemoveFixture()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FM27Analyzer", "SK-01", folder);
        if (Directory.Exists(path)) Directory.Delete(path, recursive: true);
    }

    [TestMethod]
    [DataRow("recarga")]
    [DataRow("carrera")]
    [DataRow("rama")]
    [DataRow("mánager")]
    public void SharedPlayerRequiresNewBindingBeforeReadingOrPublishing(string change)
    {
        var first = CaptureGuardTests.Context();
        var next = change switch
        {
            "recarga" => first with { Session = Guid.NewGuid() },
            "carrera" => first with { CareerId = "carrera-B", CopyReference = "copia-B" },
            "rama" => first with { Branch = "rama-B" },
            _ => first with { Perspective = "manager-B" }
        };
        var current = first;
        var messages = new List<string>();
        using var oldSession = new CaptureSession(first, () => current, messages.Add);
        var oldAccess = Access(first);
        var exchange = new BatchExchange(folder, Limits);
        var prior = Content(oldSession, oldAccess, 8);
        exchange.Publish("previo.json", prior, oldAccess, oldSession);

        current = next;
        Assert.ThrowsExactly<OperationCanceledException>(() =>
            exchange.Publish("pendiente.json", prior with { Id = Guid.NewGuid() }, oldAccess, oldSession));
        Assert.ThrowsExactly<OperationCanceledException>(() => exchange.ReadBatch("previo.json", oldAccess, oldSession));
        Assert.HasCount(1, messages);

        using var newSession = new CaptureSession(next, () => current, Assert.Fail);
        var newAccess = Access(next);
        Assert.ThrowsExactly<InvalidDataException>(() => exchange.ReadBatch("previo.json", newAccess, newSession));
        var updated = Content(newSession, newAccess, 9);
        exchange.Publish("pendiente.json", updated, newAccess, newSession);
        var recovered = exchange.ReadBatch("pendiente.json", newAccess, newSession);
        Assert.AreEqual(next, recovered.Content.Context);
        Assert.AreEqual(Player, recovered.Content.Players.Single().Key);
        Assert.AreEqual(next.CareerId, recovered.Content.Players.Single().CareerOrConfirmedCopy);
        Assert.AreEqual(new FieldValue(9), recovered.Content.Observations.Single().Value);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void PolicyChangeDuringReadRejectsPendingWorkAndAppliesCurrentVisibility(bool revokeAbility)
    {
        var first = CaptureGuardTests.Context() with
        {
            Policy = new(1, AbilityWarningAcknowledged: true, CurrentAbilityAuthorized: true)
        };
        var next = first with
        {
            Policy = first.Policy with { Revision = 2, CurrentAbilityAuthorized = !revokeAbility }
        };
        var current = first;
        var messages = new List<string>();
        using var oldSession = new CaptureSession(first, () => current, messages.Add);
        var oldAccess = Access(first);
        Assert.IsTrue(oldAccess.CanCompareInMemory(oldSession, Player, Ability));
        Assert.ThrowsExactly<OperationCanceledException>(() => oldAccess.Project(oldSession, Player, Passing, _ =>
        {
            current = next;
            return new(8);
        }));
        Assert.ThrowsExactly<OperationCanceledException>(() => oldSession.Commit(() => Assert.Fail()));
        Assert.HasCount(1, messages);

        using var newSession = new CaptureSession(next, () => current, Assert.Fail);
        var newAccess = Access(next, passingKnown: revokeAbility);
        if (revokeAbility)
        {
            Assert.IsFalse(newAccess.CanCompareInMemory(newSession, Player, Ability));
            var stillVisible = newAccess.Project(newSession, Player, Passing, _ => new(8));
            Assert.AreEqual(new FieldValue(8), stillVisible!.Value);
        }
        else
        {
            Assert.IsTrue(newAccess.CanCompareInMemory(newSession, Player, Ability));
            var masked = newAccess.Project(newSession, Player, Passing,
                _ => throw new AssertFailedException("La pérdida de conocimiento debe impedir leer el valor."));
            Assert.AreEqual(Knowledge.Unknown, masked!.State.Knowledge);
            Assert.IsNull(masked.Value);
        }
    }

    private static BatchContent Content(CaptureSession session, CaptureAuthorization access, int passing)
    {
        var identity = access.AuthorizedIdentity(session, Player)!;
        var observation = access.Project(session, Player, Passing, _ => new(passing))!;
        return new(new(1, 0), Guid.NewGuid(), session.Expected, "fixture", "r1",
            DateTimeOffset.UtcNow, SampleExtent.Partial, Limits, [identity], [observation]);
    }

    private static CaptureAuthorization Access(CaptureContext context, bool passingKnown = true) =>
        new(context, _ => IdentityScope.Scouted,
            player => new(player, context.CareerId!, new(ClubStatus.ConfirmedClub, context.ManagedClub)),
            field => field == Passing || field == Ability,
            (_, field) => new(field == Ability ? FieldClass.CurrentAbility : FieldClass.Visible,
                FieldType.Numeric, field == Ability ? "1–200" : "1–20",
                new(field == Ability || passingKnown ? Knowledge.Exact : Knowledge.Unknown,
                    Availability.Observed, Disclosure.Visible),
                "fixture", "fixture", null));
}
