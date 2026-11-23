using System.Collections.Immutable;
using FM27Analyzer.Extraction.Poc.Capture;

namespace FM27Analyzer.Extraction.Poc.Evidence;

public static class A01Comparison
{
    internal const string Route = "BepInEx · tabla de FM27";
    internal const string Visibility = "FM27 · texto de la vista activa";

    public static EvidenceReport Compare(RunInputs inputs)
    {
        ImmutableArray<string> commands = ["verify --run <manifiesto> --out <informe>"];
        var manifest = inputs.Manifest;
        if (manifest.Mode == A01Mode.Bloqueada)
            return EvidenceReport.Assemble(inputs, Accreditation.Unaccredited, [], commands);
        if (inputs.Reference is not { } reference || inputs.Batches.Length != 2 || manifest.Context is not { } context)
            throw new InvalidDataException("El contraste requiere la referencia, el contexto y dos lotes.");

        var first = inputs.Batches[0].Content;
        var second = inputs.Batches[1].Content;
        var contextMatches = context == reference.Context && context == first.Context && context == second.Context
            && first.Route == second.Route && first.Route != reference.IndependentSource
            && first.AdapterRevision == second.AdapterRevision && first.Id != second.Id
            && first.CapturedUtc > reference.FixedUtc && second.CapturedUtc > reference.FixedUtc;

        var expectedPlayers = reference.Expected.ToDictionary(player => player.Key);
        var firstPlayers = first.Players.ToDictionary(player => player.Key);
        var secondPlayers = second.Players.ToDictionary(player => player.Key);
        var firstRows = first.Observations.ToDictionary(Key);
        var secondRows = second.Observations.ToDictionary(Key);
        var expectedRows = reference.Observations.ToDictionary(Key);

        var metrics = ImmutableArray.CreateBuilder<ComparisonMetrics>();
        int identityMatches = expectedPlayers.Count(pair => contextMatches
            && firstPlayers.GetValueOrDefault(pair.Key) == pair.Value && secondPlayers.GetValueOrDefault(pair.Key) == pair.Value);
        int identityExtra = firstPlayers.Keys.Union(secondPlayers.Keys).Count(key => !expectedPlayers.ContainsKey(key));
        metrics.Add(Metric("identidad", null, expectedPlayers.Count,
            expectedPlayers.Keys.Count(key => firstPlayers.ContainsKey(key) && secondPlayers.ContainsKey(key)),
            identityMatches, expectedPlayers.Count - identityMatches + identityExtra,
            contextMatches ? "UID y vínculo de club" : "Contexto, fecha o revisión de vía discrepantes"));

        foreach (var family in reference.Observations.GroupBy(row => row.Field.Name).OrderBy(group => group.Key, StringComparer.Ordinal))
        {
            int missing = 0, state = 0, value = 0, source = 0, matches = 0;
            foreach (var expected in family)
            {
                if (!firstRows.TryGetValue(Key(expected), out var left) || !secondRows.TryGetValue(Key(expected), out var right))
                    missing++;
                else if (left.State != expected.State || right.State != expected.State
                    || left.Type != expected.Type || right.Type != expected.Type || left.Unit != expected.Unit || right.Unit != expected.Unit)
                    state++;
                else if (left.Value != expected.Value || right.Value != expected.Value)
                    value++;
                else if (left.DataSource != right.DataSource || left.VisibilitySource != right.VisibilitySource
                    || left.CatalogRevision != right.CatalogRevision || left.CatalogRevision != expected.CatalogRevision
                    || string.IsNullOrWhiteSpace(left.DataSource) || string.IsNullOrWhiteSpace(left.VisibilitySource))
                    source++;
                else if (contextMatches)
                    matches++;
            }
            int unexpected = firstRows.Keys.Union(secondRows.Keys)
                .Count(key => key.Field.Name == family.Key && !expectedRows.ContainsKey(key));
            int count = family.Count();
            string? unit = family.Select(row => row.Unit).Distinct().Count() == 1 ? family.First().Unit : null;
            metrics.Add(Metric(family.Key, unit, count, count - missing, matches, count - matches + unexpected,
                $"Ausentes {missing}, estado o escala {state}, valor {value}, procedencia {source}, adicionales {unexpected}"));
        }

        bool outsideFields = firstRows.Keys.Union(secondRows.Keys)
            .Any(key => !reference.Observations.Any(row => row.Field.Name == key.Field.Name));
        bool HasPosition(bool keeper) => reference.Observations.Any(row => row.Field.Name == "best-position"
            && row.State is { Knowledge: Knowledge.Exact, Availability: Availability.Observed, Disclosure: Disclosure.Visible }
            && row.Value?.Category is { Length: > 0 } position && (position == "POR") == keeper
            && expectedPlayers.GetValueOrDefault(row.Player)?.Club.Status == ClubStatus.ConfirmedClub
            && reference.Observations.Any(value => value.Player == row.Player && value.Type == FieldType.Numeric
                && value.State is { Knowledge: Knowledge.Exact, Availability: Availability.Observed, Disclosure: Disclosure.Visible }
                && value.Value?.Low is not null));
        bool complete = expectedPlayers.Count >= 30 && HasPosition(keeper: true) && HasPosition(keeper: false)
            && reference.Observations.Any(row => row.State.Knowledge == Knowledge.InclusiveRange)
            && reference.Observations.Any(row => row.State.Knowledge == Knowledge.Unknown && row.Value is null)
            && !outsideFields && metrics.All(metric => metric.Discrepancies == 0);

        var classOfRun = manifest.Execution.Any(item => item.Scenario == Scenario.E01 && item.Class == EvidenceClass.Real)
            ? EvidenceClass.Real : EvidenceClass.Synthetic;
        bool Passed(Scenario scenario) => manifest.Execution.Any(item => item.Scenario == scenario
            && item.Class == classOfRun && item.Outcome == Outcome.Passed && item.Context == context);
        bool basePolicy = context.Policy == new CapturePolicy(context.Policy.Revision);
        var comparison = new ScenarioResult(Scenario.E02, classOfRun, complete ? Outcome.Passed : Outcome.Failed,
            context, complete ? null : "Cobertura, identidad, valores, estados o procedencia pendientes.", metrics.ToImmutable());
        var accreditation = complete && basePolicy && Passed(Scenario.E01) && Passed(Scenario.E03)
            ? Accreditation.Accredited : identityMatches > 0 ? Accreditation.Partial : Accreditation.Unaccredited;
        return EvidenceReport.Assemble(inputs, accreditation, [comparison], commands);

        ComparisonMetrics Metric(string field, string? unit, int expected, int recovered, int matched, int discrepancies, string details) =>
            new(reference.IndependentSource, "jugador", field, first.Route, context.Build, context.Perspective,
                details, unit, manifest.Extent, context.Policy, expected, recovered, matched, discrepancies, expected,
                TimeSpan.Zero, 0, discrepancies == 0 ? Accreditation.Accredited : matched > 0 ? Accreditation.Partial : Accreditation.Unaccredited,
                discrepancies == 0 ? Coverage.Complete : Coverage.Partial);
    }

    private static (PlayerKey Player, FieldKey Field) Key(PlayerObservation row) => (row.Player, row.Field);
}
