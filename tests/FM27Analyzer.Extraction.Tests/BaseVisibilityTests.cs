using FM27Analyzer.Extraction.Poc.Capture;

namespace FM27Analyzer.Extraction.Tests;

[TestClass]
public sealed class BaseVisibilityTests
{
    [TestMethod]
    public void HiddenDifferencesDoNotChangeRowsCountsOrDiagnostics()
    {
        var first = Project(199, 98);
        var second = Project(11, 63);
        CollectionAssert.AreEqual(first.Rows, second.Rows);
        CollectionAssert.AreEqual(first.Diagnostics, second.Diagnostics);
        Assert.HasCount(2, first.Rows);
        Assert.IsEmpty(first.Diagnostics);
    }

    private static (PlayerObservation[] Rows, string[] Diagnostics) Project(int hiddenA, int hiddenB)
    {
        var context = CaptureGuardTests.Context() with { Policy = new(1) };
        var player = new PlayerKey("fixture", "conocido");
        var excluded = new PlayerKey("fixture", "mundo");
        var identity = new PlayerIdentity(player, context.CareerId!, new(ClubStatus.Unknown, null));
        var range = new FieldKey("passing", null);
        var unknown = new FieldKey("marking", null);
        var exact = new FieldKey("current-ability", null);
        var diagnostics = new List<string>();
        using var session = new CaptureSession(context, () => context, diagnostics.Add);
        var access = new CaptureAuthorization(context,
            key => key == player ? IdentityScope.Scouted : IdentityScope.LoadedWorld,
            _ => identity, field => field == range || field == unknown || field == exact,
            (_, field) => new(field == exact ? FieldClass.CurrentAbility : FieldClass.Visible,
                FieldType.Numeric, "1–20",
                new(field == range ? Knowledge.InclusiveRange : Knowledge.Unknown, Availability.Observed, Disclosure.Visible),
                "fixture", "fixture", null));
        var rows = new[]
        {
            access.Project(session, player, range, _ => new(6, 10)),
            access.Project(session, player, unknown, _ => new(hiddenA)),
            access.Project(session, player, exact, _ => new(hiddenA)),
            access.Project(session, excluded, range, _ => new(hiddenB))
        }.OfType<PlayerObservation>().ToArray();
        Assert.AreEqual(new FieldValue(6, 10), rows[0].Value);
        Assert.IsNull(rows[1].Value);
        Assert.AreEqual(Knowledge.Unknown, rows[1].State.Knowledge);
        return (rows, [.. diagnostics]);
    }
}
