using FM27Analyzer.Extraction.Poc.Capture;

namespace FM27Analyzer.Extraction.Tests;

[TestClass]
public sealed class CaptureGuardTests
{
    internal static CaptureContext Context() => new("FM27", "build-sintetica", null, null,
        Guid.NewGuid(), Guid.NewGuid(), "copia-A", "carrera-A", true, "rama-A", "manager-A",
        "club-A", new DateOnly(2026, 11, 23), "corte-A", new CapturePolicy(1));

    [TestMethod]
    public void StableSessionReadsAndPublishes()
    {
        var expected = Context();
        using var session = new CaptureSession(expected, () => expected, Assert.Fail);
        var result = session.Read(() => 42);
        bool published = false;
        session.Commit(() => published = true);
        Assert.AreEqual(42, result);
        Assert.IsTrue(published);
    }

    [TestMethod]
    [DataRow("instancia")]
    [DataRow("sesión")]
    [DataRow("carrera")]
    [DataRow("rama")]
    [DataRow("perspectiva")]
    [DataRow("fecha")]
    [DataRow("corte")]
    [DataRow("revisión")]
    [DataRow("incierto")]
    public void ChangeDuringReadCancelsAndDiscardsPendingWork(string change)
    {
        var expected = Context();
        CaptureContext? current = expected;
        var causes = new List<string>();
        using var session = new CaptureSession(expected, () => current, causes.Add);
        bool published = false;
        Assert.ThrowsExactly<OperationCanceledException>(() => session.Read(() =>
        {
            current = change switch
            {
                "instancia" => expected with { Instance = Guid.NewGuid() },
                "sesión" => expected with { Session = Guid.NewGuid() },
                "carrera" => expected with { CareerId = "otra" },
                "rama" => expected with { Branch = "otra" },
                "perspectiva" => expected with { Perspective = "otra" },
                "fecha" => expected with { GameDate = expected.GameDate.AddDays(1) },
                "corte" => expected with { Cut = "otro" },
                "revisión" => expected with { Policy = new CapturePolicy(2) },
                _ => null
            };
            return 42;
        }));
        current = expected;
        Assert.ThrowsExactly<OperationCanceledException>(() => session.Commit(() => published = true));
        Assert.IsFalse(published);
        Assert.IsTrue(session.Token.IsCancellationRequested);
        Assert.HasCount(1, causes);
    }

    [TestMethod]
    public void NotificationCancelsEvenIfTheSameContextReturnsBeforeCommit()
    {
        var expected = Context();
        var causes = new List<string>();
        using var session = new CaptureSession(expected, () => expected, causes.Add);
        Assert.ThrowsExactly<OperationCanceledException>(() => session.Read(() =>
        {
            session.ContextChanged();
            Assert.IsTrue(session.Token.IsCancellationRequested);
            return 42;
        }));
        Assert.ThrowsExactly<OperationCanceledException>(() => session.Commit(() => Assert.Fail()));
        Assert.HasCount(1, causes);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void UncertainContextBlocksBeforeReadOrCommit(bool beforeRead)
    {
        var expected = Context();
        CaptureContext? current = expected;
        var causes = new List<string>();
        using var session = new CaptureSession(expected, () => current, causes.Add);
        bool queried = false, published = false;
        if (beforeRead) current = null;
        else session.Read(() => queried = true);
        if (beforeRead)
            Assert.ThrowsExactly<OperationCanceledException>(() => session.Read(() => queried = true));
        current = null;
        Assert.ThrowsExactly<OperationCanceledException>(() => session.Commit(() => published = true));
        Assert.AreEqual(!beforeRead, queried);
        Assert.IsFalse(published);
        Assert.HasCount(1, causes);
    }
}
