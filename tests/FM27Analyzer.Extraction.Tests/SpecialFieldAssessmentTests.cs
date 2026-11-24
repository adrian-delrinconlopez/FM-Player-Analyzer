using System.Text.Json;
using FM27Analyzer.Extraction.Poc.Capture;
using FM27Analyzer.Extraction.Poc.Evidence;

namespace FM27Analyzer.Extraction.Tests;

[TestClass]
public sealed class SpecialFieldAssessmentTests
{
    private static readonly PlayerKey Player = new("fixture", "0012");
    private static readonly FieldKey Ca = new("current-ability", null);
    private static readonly FieldKey Pa = new("potential-ability", null);
    private static readonly FieldKey Passing = new("passing", null);
    private readonly CaptureContext context = CaptureGuardTests.Context() with
    {
        Policy = new CapturePolicy(1, AbilityWarningAcknowledged: true, CurrentAbilityAuthorized: true)
    };

    [TestMethod]
    public void AccreditsCaWhilePaIsDeniedBeforeReadingEitherValue()
    {
        using var session = new CaptureSession(context, () => context, Assert.Fail);
        var access = Access(context);
        var result = SpecialFieldAssessment.Compare(session, access, Player, Ca, FieldClass.CurrentAbility,
            Reference(context, Ca, () => 117), () => 117);
        Assert.AreEqual(SpecialFieldStatus.Match, result);
        Assert.AreEqual(SpecialFieldStatus.Unauthorized, SpecialFieldAssessment.Compare(session, access, Player,
            Pa, FieldClass.PotentialAbility, Reference(context, Pa, Forbidden), Forbidden));
        Assert.AreEqual(SpecialFieldStatus.Unauthorized, SpecialFieldAssessment.Compare(session, access, Player,
            Ca, FieldClass.HiddenExact, Reference(context, Ca, Forbidden), Forbidden));

        var report = EvidenceReport.Special(Scenario.E04, EvidenceClass.Synthetic, context,
            [new("current-ability", "1–200", "editor", "lector", result, TimeSpan.Zero, 0)]);
        var json = JsonSerializer.Serialize(report);
        Assert.IsFalse(json.Contains(":117", StringComparison.Ordinal));
        Assert.IsFalse(json.Contains("\"0012\"", StringComparison.Ordinal));
        Assert.AreEqual(Accreditation.Accredited, report.Comparisons.Single().Data);
        var forgedPa = EvidenceReport.Special(Scenario.E04, EvidenceClass.Synthetic, context,
            [new("potential-ability", "1–200", "editor", "lector", SpecialFieldStatus.Match, TimeSpan.Zero, 0)]);
        Assert.AreEqual(Outcome.Omitted, forgedPa.Outcome);
        Assert.IsEmpty(forgedPa.Comparisons);
    }

    [TestMethod]
    public void RequiresAnIndependentReferenceForTheSamePlayerFieldAndContext()
    {
        using var session = new CaptureSession(context, () => context, Assert.Fail);
        var access = Access(context);
        var reference = Reference(context, Ca, Forbidden);
        foreach (var invalid in new SpecialFieldReference?[] { null, reference with { Source = "lector" },
            reference with { Field = Pa }, reference with { Player = new("fixture", "0023") },
            reference with { Context = context with { Branch = "otra" } }, reference with { Unit = "1–20" } })
            Assert.AreEqual(SpecialFieldStatus.NoReference, SpecialFieldAssessment.Compare(session, access, Player,
                Ca, FieldClass.CurrentAbility, invalid, Forbidden));
    }

    [TestMethod]
    public void RevocationDiscardsPendingExactAndLeavesObservedRange()
    {
        var initial = context with { Policy = context.Policy with { HiddenAuthorized = true } };
        var active = initial;
        using var session = new CaptureSession(initial, () => active, _ => { });
        Assert.ThrowsExactly<OperationCanceledException>(() => SpecialFieldAssessment.Compare(session, Access(initial),
            Player, Passing, FieldClass.HiddenExact, Reference(initial, Passing, () => 12), () =>
            {
                active = initial with { Policy = initial.Policy with { Revision = 2, HiddenAuthorized = false } };
                return 12;
            }));
        using var next = new CaptureSession(active, () => active, Assert.Fail);
        Assert.AreEqual(SpecialFieldStatus.Unauthorized, SpecialFieldAssessment.Compare(next, Access(active), Player,
            Passing, FieldClass.HiddenExact, Reference(active, Passing, Forbidden), Forbidden));
        var visible = Access(active, visible: true).Project(next, Player, Passing, _ => new(8, 14));
        Assert.AreEqual(new FieldValue(8, 14), visible!.Value);
    }

    [TestMethod]
    [DataRow(null, SpecialFieldStatus.Unavailable)]
    [DataRow(201, SpecialFieldStatus.Unavailable)]
    [DataRow(118, SpecialFieldStatus.Mismatch)]
    public void ReportsUnavailableOrDisagreementWithoutReturningTheValues(int? value, SpecialFieldStatus status)
    {
        using var session = new CaptureSession(context, () => context, Assert.Fail);
        Assert.AreEqual(status, SpecialFieldAssessment.Compare(session, Access(context), Player, Ca,
            FieldClass.CurrentAbility, Reference(context, Ca, () => 117), () => value));
    }

    private static int? Forbidden() => throw new AssertFailedException("Un campo sin autorización o referencia no se consulta.");

    private static SpecialFieldReference Reference(CaptureContext cut, FieldKey field, Func<int?> read) =>
        new(cut, Player, field, "editor", field == Passing ? "1–20" : "1–200", read);

    private static CaptureAuthorization Access(CaptureContext cut, bool visible = false) =>
        new(cut, _ => IdentityScope.Scouted,
            key => new(key, cut.CareerId!, new(ClubStatus.Unknown, null)), _ => true,
            (_, field) => new(field == Ca ? FieldClass.CurrentAbility : field == Pa ? FieldClass.PotentialAbility
                : visible ? FieldClass.Visible : FieldClass.HiddenExact, FieldType.Numeric,
                field == Passing ? "1–20" : "1–200",
                new(visible ? Knowledge.InclusiveRange : Knowledge.Exact, Availability.Observed, Disclosure.Visible),
                "lector", "ficha", null, ScaleMinimum: 1, ScaleMaximum: field == Passing ? 20 : 200));
}
