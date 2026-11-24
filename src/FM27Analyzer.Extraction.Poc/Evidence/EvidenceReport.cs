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

public sealed record SpecialFieldMetric(string Field, string Unit, string ReferenceSource, string Route,
    SpecialFieldStatus Status, TimeSpan Duration, long PeakBytes);

public sealed record EvidenceReport(Guid RunId, Accreditation A01, string Extent, string? Limitation, ImmutableArray<ScenarioResult> Results,
    ImmutableArray<string> Commands, CaptureLimits? Limits)
{
    public static ScenarioResult Special(Scenario scenario, EvidenceClass evidence, CaptureContext context,
        IReadOnlyCollection<SpecialFieldMetric> fields)
    {
        if (scenario is not (Scenario.E04 or Scenario.E06))
            throw new ArgumentOutOfRangeException(nameof(scenario));
        var authorized = fields.Where(field => field.Status != SpecialFieldStatus.Unauthorized
            && AllowsMetric(scenario, context.Policy, field.Field)).ToArray();
        var metrics = authorized.Select(field =>
        {
            bool compared = field.Status is SpecialFieldStatus.Match or SpecialFieldStatus.Mismatch;
            bool match = field.Status == SpecialFieldStatus.Match;
            return new ComparisonMetrics(field.ReferenceSource, "futbolista", field.Field, field.Route,
                context.Build, context.Perspective, SpecialSemantics(field.Status), field.Unit, "muestra autorizada",
                context.Policy, 1, compared ? 1 : 0, match ? 1 : 0, !match && compared ? 1 : 0,
                compared ? 1 : null, field.Duration, field.PeakBytes,
                match ? Accreditation.Accredited : Accreditation.Unaccredited, Coverage.Indeterminate);
        }).ToImmutableArray();
        bool mismatch = authorized.Any(f => f.Status == SpecialFieldStatus.Mismatch);
        bool passed = authorized.Length > 0 && fields.Count == authorized.Length
            && authorized.All(f => f.Status == SpecialFieldStatus.Match);
        return new(scenario, evidence, passed ? Outcome.Passed : mismatch ? Outcome.Failed : Outcome.Omitted,
            context, passed ? null : mismatch ? "Hay una discrepancia en los campos contrastados."
                : "Falta permiso, referencia independiente o capacidad para algún campo.", metrics);
    }

    public static ScenarioResult World(EvidenceClass evidence, CaptureContext context, string route, string extent,
        WorldCoverageResult result, TimeSpan duration, long peakBytes)
    {
        if (context.Policy is not { Scope: CaptureScope.World, WorldAuthorized: true })
            throw new UnauthorizedAccessException("El informe mundial requiere autorización expresa.");
        var metric = new ComparisonMetrics(result.ReferenceSource ?? "Sin referencia comparable",
            "futbolista", "identidad", route, context.Build, context.Perspective,
            $"UID únicos. Duplicados de captura {result.Duplicates}, omisiones declaradas {result.Omissions}.",
            "UID", extent, context.Policy, result.ReferenceCount ?? 0, result.Unique,
            result.Matches ?? 0, (result.Missing ?? 0) + (result.Unexpected ?? 0), result.IndependentDenominator,
            duration, peakBytes, result.Coverage switch
            {
                Coverage.Complete => Accreditation.Accredited,
                Coverage.Partial => Accreditation.Partial,
                _ => Accreditation.Unaccredited
            }, result.Coverage);
        return new(Scenario.E05, evidence, result.Coverage == Coverage.Indeterminate ? Outcome.Omitted : Outcome.Passed,
            context, result.Coverage switch
            {
                Coverage.Complete => null,
                Coverage.Partial => "Sólo se acredita el subconjunto contrastado.",
                _ => "El universo completo carece de referencia comparable o conciliada."
            }, [metric]);
    }

    private static string SpecialSemantics(SpecialFieldStatus status) => status switch
    {
        SpecialFieldStatus.Match => "Coincidencia exacta comprobada en memoria.",
        SpecialFieldStatus.Mismatch => "No disponible por discrepancia.",
        SpecialFieldStatus.NoReference => "No disponible por falta de referencia independiente.",
        _ => "No disponible en el lector o su escala."
    };

    private static bool AllowsMetric(Scenario scenario, CapturePolicy policy, string field) => scenario switch
    {
        Scenario.E04 => policy.AbilityWarningAcknowledged && field switch
        {
            "current-ability" => policy.CurrentAbilityAuthorized,
            "potential-ability" => policy.PotentialAbilityAuthorized,
            _ => false
        },
        Scenario.E05 => policy is { Scope: CaptureScope.World, WorldAuthorized: true } && field == "identidad",
        Scenario.E06 => policy.HiddenAuthorized && field is not ("current-ability" or "potential-ability"),
        _ => true
    };

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
            && Enum.IsDefined(m.Data) && Enum.IsDefined(m.Coverage)
            && (result.Scenario is not (Scenario.E04 or Scenario.E05 or Scenario.E06)
                || result.Context is { } context && m.Policy == context.Policy && m.Build == context.Build
                && m.Perspective == context.Perspective
                && AllowsMetric(result.Scenario, context.Policy, m.FieldOrFamily)));
}
