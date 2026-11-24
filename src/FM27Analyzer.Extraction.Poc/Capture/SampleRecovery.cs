namespace FM27Analyzer.Extraction.Poc.Capture;

public enum SampleStatus { Accepted, Repeated, Recovered, Rejected }
public enum SampleFailure { IncompatibleBuild, ClosedGame, Truncated, Disconnected, InvalidBatch, ConflictingId, ContextChanged }

public sealed class SampleSourceException(SampleFailure reason) : IOException
{
    public SampleFailure Reason { get; } = reason;
}

public sealed record SampleResult(string Capability, SampleStatus Status, SampleFailure? Failure, ObservationBatch? Sample);

// Registro efímero de un contexto confirmado. No mezcla ni publica observaciones.
public sealed class SampleRecovery(CaptureSession session, CaptureLimits limits)
{
    private readonly Dictionary<Guid, (string Capability, BatchClosure Closure)> accepted = new();
    private readonly Dictionary<string, ObservationBatch> latest = new(StringComparer.Ordinal);

    public SampleResult Attempt(string capability, CaptureAuthorization access, Func<ObservationBatch> capture)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(capability);
        access.CheckSession(session);
        try
        {
            var proposed = session.Read(capture);
            var validated = session.Read(() => BatchValidation.Accept(proposed, access, limits));
            SampleStatus status = SampleStatus.Repeated;
            session.Commit(() =>
            {
                var key = validated.Content.Id;
                var entry = (capability, proposed.Closure);
                if (accepted.TryGetValue(key, out var previous))
                {
                    if (previous != entry) throw new SampleSourceException(SampleFailure.ConflictingId);
                    return;
                }

                accepted.Add(key, entry);
                if (!latest.TryGetValue(capability, out var previousSample)
                    || validated.Content.CapturedUtc >= previousSample.Content.CapturedUtc)
                    latest[capability] = validated;
                status = SampleStatus.Accepted;
            });
            return new(capability, status, null, status == SampleStatus.Accepted ? validated : null);
        }
        catch (SampleSourceException ex)
        {
            return Recover(capability, access, ex.Reason);
        }
        catch (EndOfStreamException)
        {
            return Recover(capability, access, SampleFailure.Truncated);
        }
        catch (InvalidDataException)
        {
            return Recover(capability, access, SampleFailure.InvalidBatch);
        }
        catch (IOException)
        {
            return Recover(capability, access, SampleFailure.Disconnected);
        }
        catch (OperationCanceledException)
        {
            return new(capability, SampleStatus.Rejected, SampleFailure.ContextChanged, null);
        }
    }

    private SampleResult Recover(string capability, CaptureAuthorization access, SampleFailure reason)
    {
        ObservationBatch? prior = null;
        try
        {
            session.Commit(() =>
            {
                if (reason == SampleFailure.ClosedGame || !latest.TryGetValue(capability, out var previous)) return;
                try { prior = session.Read(() => BatchValidation.Accept(previous, access, limits)); }
                catch (InvalidDataException) { latest.Remove(capability); }
            });
        }
        catch (OperationCanceledException) { }
        return new(capability, prior is null ? SampleStatus.Rejected : SampleStatus.Recovered, reason, prior);
    }
}
