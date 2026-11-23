using System.Collections.Immutable;
using FM27Analyzer.Extraction.Poc.Capture;
using FM27Analyzer.Extraction.Poc.Evidence;

namespace FM27Analyzer.Extraction.Tests;

[TestClass]
public sealed class A01ComparisonTests
{
    [TestMethod]
    public void IndependentReferenceAndTwoEquivalentBatchesAccreditA01()
    {
        var inputs = Fixture();
        var report = A01Comparison.Compare(inputs);
        Assert.AreEqual(Accreditation.Accredited, report.A01);
        Assert.AreEqual(Outcome.Passed, report.Results.Single(r => r.Scenario == Scenario.E02).Outcome);
        Assert.AreEqual(EvidenceClass.ThirdParty, report.Results.Single(r => r.Scenario == Scenario.E04).Class);
        Assert.AreEqual(Outcome.Omitted, report.Results.Single(r => r.Scenario == Scenario.E09).Outcome);
    }

    [TestMethod]
    [DataRow("uid")]
    [DataRow("valor")]
    [DataRow("procedencia")]
    [DataRow("estado")]
    [DataRow("corte")]
    public void DiscrepanciesPreventAccreditation(string difference)
    {
        var inputs = Fixture();
        var second = inputs.Batches[1].Content;
        var row = second.Observations[0];
        second = difference switch
        {
            "uid" => second with { Players = second.Players.RemoveAt(2) },
            "valor" => second with { Observations = second.Observations.SetItem(0, row with { Value = new(14) }) },
            "procedencia" => second with { Observations = second.Observations.SetItem(0, row with { DataSource = "otra" }) },
            "estado" => second with { Observations = second.Observations.SetItem(0, row with {
                State = new(Knowledge.Unknown, Availability.Observed, Disclosure.Visible), Value = null }) },
            _ => second with { Context = second.Context with { Cut = "distinto" } }
        };
        var report = A01Comparison.Compare(inputs with { Batches = inputs.Batches.SetItem(1, new(second, new(30, 6, "fixture"))) });
        Assert.AreNotEqual(Accreditation.Accredited, report.A01);
        Assert.AreEqual(Outcome.Failed, report.Results.Single(r => r.Scenario == Scenario.E02).Outcome);
    }

    [TestMethod]
    public void BlockedModeProducesReportAndIncompleteContrastIsInvalid()
    {
        var inputs = Fixture();
        var blocked = inputs.Manifest with { Mode = A01Mode.Bloqueada, Reason = "Sin capturas.",
            Execution = [new(Scenario.E01, EvidenceClass.Real, Outcome.Omitted, null, "Sin capturas.", [])] };
        var report = A01Comparison.Compare(new(blocked, [], null, inputs.Additional));
        Assert.AreEqual(Accreditation.Unaccredited, report.A01);
        Assert.HasCount(9, report.Results);
        Assert.ThrowsExactly<InvalidDataException>(() => A01Comparison.Compare(inputs with { Batches = [] }));
    }

    private static RunInputs Fixture()
    {
        var context = CaptureGuardTests.Context();
        var fixedAt = DateTimeOffset.UnixEpoch.AddDays(7);
        var limits = new CaptureLimits(65_536, 30, 12, TimeSpan.FromSeconds(1));
        var players = Enumerable.Range(0, 30).Select(i => new PlayerIdentity(new("fixture", $"F{i:00}"),
            context.CareerId!, new(ClubStatus.ConfirmedClub, "club-fixture"))).ToImmutableArray();
        var exact = new ObservationState(Knowledge.Exact, Availability.Observed, Disclosure.Visible);
        PlayerObservation Row(int player, string name, FieldValue? value, ObservationState state, string? unit = "1–20") =>
            new(players[player].Key, new(name, null), unit is null ? FieldType.Category : FieldType.Numeric, unit,
                value, state, "pantalla-fixture", "pantalla-fixture", null);
        ImmutableArray<PlayerObservation> rows = [
            Row(0, "passing", new(13), exact),
            Row(1, "passing", new(11), exact),
            Row(0, "best-position", new(Category: "POR"), exact, null),
            Row(1, "best-position", new(Category: "DF (I)"), exact, null),
            Row(2, "passing", new(4, 8), exact with { Knowledge = Knowledge.InclusiveRange }),
            Row(2, "marking", null, exact with { Knowledge = Knowledge.Unknown })];
        var reference = new ComparisonReference(context, fixedAt, "pantalla-fixture", players, rows);
        ObservationBatch Batch(int ordinal) => new(new(new(1, 0), Guid.NewGuid(), context, "sonda-fixture", "r1",
            fixedAt.AddSeconds(ordinal), SampleExtent.Partial, limits, players,
            [.. rows.Select(row => row with { DataSource = "sonda-fixture", VisibilitySource = "sonda-fixture" })]), new(30, 6, "fixture"));
        var manifest = new RunManifest(new(1, 0), Guid.NewGuid(), A01Mode.Contrastable, context, limits, "fixture",
            null, ["uno.json", "dos.json"], "ref.json", null, null,
            [new(Scenario.E01, EvidenceClass.Synthetic, Outcome.Passed, context, null, []),
             new(Scenario.E03, EvidenceClass.Synthetic, Outcome.Passed, context, null, [])]);
        return new(manifest, [Batch(1), Batch(2)], reference,
            [new(Scenario.E04, EvidenceClass.ThirdParty, Outcome.Omitted, null, "Sin contraste propio.", [])]);
    }
}
