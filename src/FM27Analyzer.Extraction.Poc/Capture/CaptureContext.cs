namespace FM27Analyzer.Extraction.Poc.Capture;

public sealed record CaptureContext(
    string Edition, string Build, string? Distribution, string? Language,
    Guid Instance, Guid Session, string CopyReference, string? CareerId,
    bool CopyAndCutConfirmed, string Branch, string Perspective, string ManagedClub,
    DateOnly GameDate, string Cut, CapturePolicy Policy)
{
    internal bool IsVerified => Edition == "FM27" && !string.IsNullOrWhiteSpace(Build)
        && Instance != Guid.Empty && Session != Guid.Empty && !string.IsNullOrWhiteSpace(CopyReference)
        && (CareerId is null ? CopyAndCutConfirmed : !string.IsNullOrWhiteSpace(CareerId))
        && !string.IsNullOrWhiteSpace(Branch) && !string.IsNullOrWhiteSpace(Perspective)
        && !string.IsNullOrWhiteSpace(ManagedClub) && GameDate != default && !string.IsNullOrWhiteSpace(Cut)
        && Policy is { Revision: > 0 } && Enum.IsDefined(Policy.Scope)
        && (Policy.Scope != CaptureScope.World || Policy.WorldAuthorized);
}
