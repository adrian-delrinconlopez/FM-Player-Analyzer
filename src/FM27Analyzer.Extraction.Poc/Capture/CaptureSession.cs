namespace FM27Analyzer.Extraction.Poc.Capture;

// El adaptador notifica cada transición, incluso una recarga que recupera los mismos identificadores.
public sealed class CaptureSession(CaptureContext expected, Func<CaptureContext?> current, Action<string> logBlock)
    : IDisposable
{
    private readonly object gate = new();
    private readonly CancellationTokenSource cancellation = new();
    public CaptureContext Expected { get; } = expected;
    public CancellationToken Token => cancellation.Token;

    public T Read<T>(Func<T> query)
    {
        lock (gate) Verify();
        var result = query();
        lock (gate) Verify();
        return result;
    }

    public void ContextChanged()
    {
        lock (gate) Cancel("Cambio de contexto o revisión durante la captura.");
    }

    // La notificación del adaptador y el movimiento final del archivo se serializan.
    public void Commit(Action publish)
    {
        lock (gate)
        {
            Verify();
            publish();
        }
    }

    private void Verify()
    {
        Token.ThrowIfCancellationRequested();
        CaptureContext? observed;
        try { observed = current(); }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            observed = null;
        }
        if (observed is not { IsVerified: true } || observed != Expected)
        {
            Cancel("Contexto incierto, distinto o sin verificar.");
            Token.ThrowIfCancellationRequested();
        }
    }

    private void Cancel(string reason)
    {
        if (Token.IsCancellationRequested) return;
        cancellation.Cancel();
        logBlock(reason);
    }

    public void Dispose() => cancellation.Dispose();
}
