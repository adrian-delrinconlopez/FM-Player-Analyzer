namespace FM27Analyzer.Extraction.Poc.Capture;

public enum CaptureScope { SquadAndScouted, World }

// Consentimiento registrado por el operador para esta revisión de la sesión.
public sealed record CapturePolicy(
    long Revision, CaptureScope Scope = CaptureScope.SquadAndScouted,
    bool WorldAuthorized = false, bool HiddenAuthorized = false,
    bool AbilityWarningAcknowledged = false, bool CurrentAbilityAuthorized = false,
    bool PotentialAbilityAuthorized = false);
