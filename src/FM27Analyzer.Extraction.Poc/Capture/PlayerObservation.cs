namespace FM27Analyzer.Extraction.Poc.Capture;

public sealed record PlayerKey(string SourceType, string Uid);
public enum ClubStatus { Unknown, ConfirmedClub, ConfirmedFreeAgent }
public sealed record ClubLink(ClubStatus Status, string? ClubReference);
public sealed record PlayerIdentity(PlayerKey Key, string CareerOrConfirmedCopy, ClubLink Club);

public sealed record FieldKey(string Name, string? Qualifier);
public enum Knowledge { Exact, InclusiveRange, Unknown }
public enum Availability { Observed, AbsentFromView, Unavailable, IncompatibleBuild, NotAssessed }
public enum Disclosure { Visible, Restricted }
public enum FieldType { Numeric, Category }
public sealed record FieldValue(decimal? Low = null, decimal? High = null, string? Category = null);
public sealed record ObservationState(Knowledge Knowledge, Availability Availability, Disclosure Disclosure);

public sealed record PlayerObservation(
    PlayerKey Player, FieldKey Field, FieldType Type, string? Unit, FieldValue? Value,
    ObservationState State, string DataSource, string VisibilitySource, string? CatalogRevision);
