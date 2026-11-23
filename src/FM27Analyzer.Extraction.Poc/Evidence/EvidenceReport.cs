using System.Collections.Immutable;
using FM27Analyzer.Extraction.Poc.Capture;

namespace FM27Analyzer.Extraction.Poc.Evidence;

public enum Scenario { E01 = 1, E02, E03, E04, E05, E06, E07, E08, E09 }
public enum EvidenceClass { Real, Synthetic, ThirdParty, Documentary }
public enum Outcome { Passed, Failed, Omitted }
public enum Accreditation { Unaccredited, Partial, Accredited }
public enum Coverage { Indeterminate, Partial, Complete }

// Etiquetas de protocolo y métricas agregadas. Los valores y los UID no forman parte del informe.
public sealed record ComparisonMetrics(string ReferenceSource, string EntityType, string FieldOrFamily,
    string Route, string Build, string Perspective, string Semantics, string? Unit, string Extent,
    CapturePolicy Policy, int Expected, int Recovered, int Matches, int Discrepancies,
    int? IndependentDenominator, TimeSpan Duration, long PeakBytes, Accreditation Data, Coverage Coverage);

public sealed record ScenarioResult(Scenario Scenario, EvidenceClass Class, Outcome Outcome,
    CaptureContext? Context, string? Reason, ImmutableArray<ComparisonMetrics> Comparisons);

public sealed record EvidenceReport(Guid RunId, Accreditation A01, string Extent, string? Limitation, ImmutableArray<ScenarioResult> Results,
    ImmutableArray<string> Commands, CaptureLimits? Limits)
{
    public static EvidenceReport Assemble(RunInputs inputs, Accreditation a01,
        IEnumerable<ScenarioResult> comparisons, ImmutableArray<string> commands)
    {
        var manifest = inputs.Manifest;
        var results = manifest.Execution.Concat(comparisons).Concat(inputs.Additional).ToList();
        var seen = new HashSet<(Scenario, EvidenceClass)>();
        foreach (var result in results)
        {
            if (!Valid(result) || !seen.Add((result.Scenario, result.Class)))
                throw new InvalidDataException("Resultado de escenario inválido o duplicado.");
        }
        foreach (var scenario in Enum.GetValues<Scenario>())
            if (!results.Any(r => r.Scenario == scenario))
                results.Add(new(scenario, EvidenceClass.Documentary, Outcome.Omitted, null,
                    "Sin evidencia aportada para este escenario.", []));
        return new(manifest.Id, manifest.Mode == A01Mode.Bloqueada ? Accreditation.Unaccredited : a01,
            manifest.Extent, manifest.Reason, [.. results.OrderBy(r => r.Scenario).ThenBy(r => r.Class)],
            commands, manifest.Limits);
    }

    internal static bool Valid(ScenarioResult? result) => result is not null
        && Enum.IsDefined(result.Scenario) && Enum.IsDefined(result.Class) && Enum.IsDefined(result.Outcome)
        && (result.Outcome == Outcome.Passed || !string.IsNullOrWhiteSpace(result.Reason))
        && (result.Context is null || result.Context.IsVerified)
        && (result.Outcome != Outcome.Passed || result.Context is { IsVerified: true })
        && !result.Comparisons.IsDefault && result.Comparisons.All(m => m is not null
            && m.Expected >= 0 && m.Recovered >= 0 && m.Matches >= 0 && m.Discrepancies >= 0
            && m.IndependentDenominator is not < 0 && m.PeakBytes >= 0 && m.Duration >= TimeSpan.Zero
            && Enum.IsDefined(m.Data) && Enum.IsDefined(m.Coverage));
}
