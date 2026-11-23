using System.Collections.Immutable;

namespace FM27Analyzer.Extraction.Poc.Capture;

public sealed record SchemaVersion(int Major, int Minor);
public enum SampleExtent { Partial, Total }

public sealed record CaptureLimits(int MaxBytes, int MaxPlayers, int MaxObservations, TimeSpan MaxDuration)
{
    internal bool IsValid => MaxBytes is > 0 and <= 16 * 1024 * 1024
        && MaxPlayers is > 0 and <= 100_000 && MaxObservations is > 0 and <= 250_000
        && MaxDuration > TimeSpan.Zero && MaxDuration <= TimeSpan.FromHours(1);

    internal bool Fits(CaptureLimits ceiling) => IsValid && MaxBytes <= ceiling.MaxBytes
        && MaxPlayers <= ceiling.MaxPlayers && MaxObservations <= ceiling.MaxObservations
        && MaxDuration <= ceiling.MaxDuration;
}

public sealed record BatchClosure(int Players, int Observations, string Sha256);
public sealed record BatchContent(
    SchemaVersion Schema, Guid Id, CaptureContext Context, string Route, string AdapterRevision,
    DateTimeOffset CapturedUtc, SampleExtent Extent, CaptureLimits Limits,
    ImmutableArray<PlayerIdentity> Players, ImmutableArray<PlayerObservation> Observations);

public sealed record ObservationBatch(BatchContent Content, BatchClosure Closure) : ExchangeDocument;

public sealed record ComparisonReference(
    CaptureContext Context, DateTimeOffset FixedUtc, string IndependentSource,
    ImmutableArray<PlayerIdentity> Expected, ImmutableArray<PlayerObservation> Observations) : ExchangeDocument;
