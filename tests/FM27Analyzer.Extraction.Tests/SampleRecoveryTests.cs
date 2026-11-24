using FM27Analyzer.Extraction.Poc.Capture;

namespace FM27Analyzer.Extraction.Tests;

[TestClass]
public sealed class SampleRecoveryTests
{
    private static readonly CaptureLimits Limits = new(8192, 1, 1, TimeSpan.FromSeconds(2));
    private static readonly DateTimeOffset Captured = new(2026, 11, 24, 8, 0, 0, TimeSpan.Zero);
    private static readonly PlayerKey Player = new("fixture", "0001");
    private static readonly FieldKey Field = new("passing", null);
    private static readonly ObservationState Exact = new(Knowledge.Exact, Availability.Observed, Disclosure.Visible);
    private readonly CaptureContext context = CaptureGuardTests.Context();
    private bool visible = true;

    [TestMethod]
    public void ReplayHasNoNewEffectAndConflictKeepsOriginal()
    {
        using var session = new CaptureSession(context, () => context, Assert.Fail);
        var access = Access();
        var recovery = new SampleRecovery(session, Limits);
        var id = Guid.NewGuid();
        var first = Batch(access, id);
        var conflict = Batch(access, id, value: 12);

        Assert.AreEqual(SampleStatus.Accepted, recovery.Attempt("atributos", access, () => first).Status);
        var replay = recovery.Attempt("atributos", access, () => first with { });
        Assert.AreEqual(SampleStatus.Repeated, replay.Status);
        Assert.IsNull(replay.Sample);
        var rejected = recovery.Attempt("atributos", access, () => conflict);
        Assert.AreEqual(SampleStatus.Recovered, rejected.Status);
        Assert.AreEqual(SampleFailure.ConflictingId, rejected.Failure);
        Assert.AreEqual(first.Closure, rejected.Sample!.Closure);
        Assert.AreEqual(Captured, rejected.Sample.Content.CapturedUtc);
    }

    [TestMethod]
    [DataRow(SampleFailure.IncompatibleBuild)]
    [DataRow(SampleFailure.Truncated)]
    [DataRow(SampleFailure.Disconnected)]
    public void FailureIsScopedAndAnotherRouteMustBeValidated(SampleFailure failure)
    {
        using var session = new CaptureSession(context, () => context, Assert.Fail);
        var access = Access();
        var recovery = new SampleRecovery(session, Limits);
        var before = Batch(access, Guid.NewGuid());
        recovery.Attempt("atributos", access, () => before);

        var unavailable = recovery.Attempt("contrato", access, () => throw new SampleSourceException(failure));
        Assert.AreEqual(SampleStatus.Rejected, unavailable.Status);
        Assert.AreEqual("contrato", unavailable.Capability);
        Assert.AreEqual(failure, unavailable.Failure);
        Assert.IsNull(unavailable.Sample);

        var fallback = recovery.Attempt("atributos", access, () => throw new SampleSourceException(failure));
        Assert.AreEqual(SampleStatus.Recovered, fallback.Status);
        Assert.AreEqual(failure, fallback.Failure);
        Assert.AreEqual(before.Closure, fallback.Sample!.Closure);
        Assert.AreEqual(Captured, fallback.Sample.Content.CapturedUtc);

        const string otherSource = "fuente fixture independiente";
        var otherAccess = Access(otherSource);
        var other = Batch(otherAccess, Guid.NewGuid(), route: "fixture independiente", source: otherSource);
        var valid = recovery.Attempt("atributos", otherAccess, () => other);
        Assert.AreEqual(SampleStatus.Accepted, valid.Status);
        Assert.AreEqual("fixture independiente", valid.Sample!.Content.Route);
        Assert.AreEqual(otherSource, valid.Sample.Content.Observations.Single().DataSource);
    }

    [TestMethod]
    public void InvalidClosureIsNotAcceptedAndRevokedObservationIsNotRecovered()
    {
        using var session = new CaptureSession(context, () => context, Assert.Fail);
        var access = Access();
        var recovery = new SampleRecovery(session, Limits);
        var before = Batch(access, Guid.NewGuid());
        recovery.Attempt("atributos", access, () => before);

        var next = Batch(access, Guid.NewGuid());
        var damaged = next with { Closure = next.Closure with { Observations = 0 } };
        var invalid = recovery.Attempt("atributos", access, () => damaged);
        Assert.AreEqual(SampleStatus.Recovered, invalid.Status);
        Assert.AreEqual(SampleFailure.InvalidBatch, invalid.Failure);
        Assert.AreEqual(before.Closure, invalid.Sample!.Closure);

        visible = false;
        var revoked = recovery.Attempt("atributos", access, () => throw new SampleSourceException(SampleFailure.Disconnected));
        Assert.AreEqual(SampleStatus.Rejected, revoked.Status);
        Assert.IsNull(revoked.Sample);
        visible = true;
        Assert.IsNull(recovery.Attempt("atributos", access,
            () => throw new SampleSourceException(SampleFailure.Disconnected)).Sample);
    }

    [TestMethod]
    public void ClosedGameCannotReturnStaleSample()
    {
        CaptureContext? current = context;
        using var session = new CaptureSession(context, () => current, _ => { });
        var access = Access();
        var recovery = new SampleRecovery(session, Limits);
        recovery.Attempt("atributos", access, () => Batch(access, Guid.NewGuid()));
        var result = recovery.Attempt("atributos", access, () =>
        {
            current = null;
            throw new SampleSourceException(SampleFailure.ClosedGame);
        });

        Assert.AreEqual(SampleStatus.Rejected, result.Status);
        Assert.AreEqual(SampleFailure.ClosedGame, result.Failure);
        Assert.IsNull(result.Sample);
        Assert.IsTrue(session.Token.IsCancellationRequested);
    }

    private CaptureAuthorization Access(string source = "vista") => new(context, _ => IdentityScope.Squad, _ => Identity(),
        field => field == Field, (_, _) => visible
            ? new(FieldClass.Visible, FieldType.Numeric, "1–20", Exact, source, "ficha", null)
            : null);

    private PlayerIdentity Identity() => new(Player, context.CareerId!, new(ClubStatus.ConfirmedClub, context.ManagedClub));

    private ObservationBatch Batch(CaptureAuthorization access, Guid id, int value = 8, string route = "fixture",
        string source = "vista")
    {
        var row = new PlayerObservation(Player, Field, FieldType.Numeric, "1–20", new(value), Exact, source, "ficha", null);
        return BatchValidation.Seal(new(new(1, 0), id, context, route, "r1",
            Captured, SampleExtent.Partial, Limits, [Identity()], [row]), access, Limits);
    }
}
