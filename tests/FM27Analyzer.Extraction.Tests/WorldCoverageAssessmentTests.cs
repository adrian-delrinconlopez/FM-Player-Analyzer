using FM27Analyzer.Extraction.Poc.Capture;
using FM27Analyzer.Extraction.Poc.Evidence;

namespace FM27Analyzer.Extraction.Tests;

[TestClass]
public sealed class WorldCoverageAssessmentTests
{
    private readonly CaptureContext context = CaptureGuardTests.Context() with
    {
        Policy = new CapturePolicy(1, CaptureScope.World, WorldAuthorized: true)
    };
    private static readonly PlayerKey A = new("FM27", "009");
    private static readonly PlayerKey B = new("FM27", "010");
    private static readonly PlayerKey C = new("FM27", "11");

    [TestMethod]
    public void ExactSetsReconcileIndependentlyOfOrderAndOnlyAccreditIdentity()
    {
        var result = Compare([B, A], Reference(WorldReferenceScope.Whole, 2, [A, B]));
        Assert.AreEqual(Coverage.Complete, result.Coverage);
        Assert.AreEqual(2, result.IndependentDenominator);
        Assert.AreEqual(2, result.Matches);
        var report = EvidenceReport.World(EvidenceClass.Synthetic, context, "tabla", "plantilla o mundo cargado",
            result, TimeSpan.Zero, 0);
        var metric = report.Comparisons.Single();
        Assert.AreEqual("identidad", metric.FieldOrFamily);
        Assert.AreEqual(context.Policy, metric.Policy);
    }

    [TestMethod]
    public void EqualTotalsDoNotOverrideMissingAndExtraIdentifiers()
    {
        var result = Compare([B, C], Reference(WorldReferenceScope.Whole, 2, [A, B]));
        Assert.AreEqual(Coverage.Partial, result.Coverage);
        Assert.AreEqual(1, result.Matches);
        Assert.AreEqual(1, result.Missing);
        Assert.AreEqual(1, result.Unexpected);
    }

    [TestMethod]
    public void KnownOmissionsOrDuplicatesPreventFullCertification()
    {
        var omitted = Compare([A, B], Reference(WorldReferenceScope.Whole, 2, [A, B]), omissions: 1);
        Assert.AreEqual(Coverage.Partial, omitted.Coverage);
        Assert.AreEqual(1, omitted.Omissions);
        var duplicate = Compare([A, A, B], Reference(WorldReferenceScope.Whole, 2, [A, B]));
        Assert.AreEqual(Coverage.Partial, duplicate.Coverage);
        Assert.AreEqual(1, duplicate.Duplicates);
        Assert.AreEqual(2, duplicate.Unique);
    }

    [TestMethod]
    public void SampleProvidesBoundedPartialEvidenceWithoutWorldDenominator()
    {
        var result = Compare([A, B, C], Reference(WorldReferenceScope.VerifiedSubset, 2, [A, B]));
        Assert.AreEqual(Coverage.Partial, result.Coverage);
        Assert.AreEqual(2, result.Matches);
        Assert.IsNull(result.IndependentDenominator);
        Assert.IsNull(result.Unexpected);
    }

    [TestMethod]
    public void RejectsNonComparableTotalsContextsAndSources()
    {
        var refData = Reference(WorldReferenceScope.Whole, 2, [A, B]);
        foreach (var wrong in new WorldReference?[] {null, refData with { Context = context with { Branch = "otra" } },
            refData with { Count = 3 }, refData with { Source = "tabla" }, refData with { Identities = [A, A] } })
        {
            var result = Compare([A, B], wrong);
            Assert.AreEqual(Coverage.Indeterminate, result.Coverage);
            Assert.IsNull(result.IndependentDenominator);
        }
        Assert.AreEqual(Coverage.Complete, Compare([A, B], Reference(WorldReferenceScope.Whole, 2, null)).Coverage);
        Assert.AreEqual(Coverage.Indeterminate, Compare([A, B], Reference(WorldReferenceScope.Whole, 3, null)).Coverage);
    }

    [TestMethod]
    public void ScopeAndLimitAreCheckedBeforePublishing()
    {
        var baseScope = context with { Policy = new(1) };
        using var baseSession = new CaptureSession(baseScope, () => baseScope, Assert.Fail);
        Assert.ThrowsExactly<UnauthorizedAccessException>(() => WorldCoverageAssessment.Compare(baseSession, "tabla",
            4, 0, () => throw new AssertFailedException("Ámbito mundial no autorizado."), null));
        Assert.ThrowsExactly<InvalidDataException>(() => Compare([A, A, A, A, A], null));
        var active = context;
        using var session = new CaptureSession(context, () => active, _ => { });
        Assert.ThrowsExactly<OperationCanceledException>(() => WorldCoverageAssessment.Compare(session, "tabla", 4,
            0, () =>
            {
                active = baseScope;
                return [A];
            }, null));
    }

    private WorldCoverageResult Compare(PlayerKey[] actual, WorldReference? reference, int omissions = 0)
    {
        using var session = new CaptureSession(context, () => context, Assert.Fail);
        return WorldCoverageAssessment.Compare(session, "tabla", 4, omissions, () => actual, reference);
    }

    private WorldReference Reference(WorldReferenceScope scope, int count, PlayerKey[]? ids) =>
        new(context, "pantalla", scope, count, ids);
}
