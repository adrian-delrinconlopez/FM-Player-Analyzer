namespace FM27Analyzer.RatingProbe;

internal static class Equivalence
{
    private static readonly string[][] FieldTiers =
    [
        ["Pace", "Acceleration"],
        ["Jumping Reach", "Anticipation", "Balance", "Agility", "Concentration", "Finishing"],
        ["Work Rate", "Dribbling", "Stamina", "Strength", "Passing", "Determination", "Vision"],
        ["Long Shots", "Marking", "Decisions", "First Touch"],
        ["Off the Ball", "Tackling", "Teamwork", "Composure", "Technique", "Positioning"]
    ];

    private static readonly string[][] KeeperTiers =
    [
        ["Positioning", "Aerial Reach", "Handling"],
        ["Agility", "Anticipation", "Kicking"],
        ["Reflexes", "Command of Area", "Concentration", "Throwing"],
        ["Bravery", "One vs One", "Rushing Out (Tendency)"]
    ];

    private static readonly (double Weight, string[] Attributes)[] KeeperCurves =
    [
        (12, ["Anticipation", "Handling", "Reflexes", "Positioning"]),
        (8, ["Concentration", "Communication", "One vs One", "Agility"]),
        (4, ["Aerial Reach", "Command of Area", "Composure"]),
        (2, ["Kicking", "Throwing", "Bravery", "Decisions"])
    ];

    public static void VerifyClassic(string[] lines, Dictionary<string, Role> roles, string[] attributes)
    {
        double worstAbsolute = 0;
        int count = 0, same = 0, omitted = 0;
        foreach (bool custom in new[] { false, true })
        foreach (int start in new[] { 0, 8 })
        {
            string revision = custom ? "personalizada" : "inicial";
            string header = $"## FM27 · {revision} · muestras {start}–{start + 7}";
            string[][] rows = Oracle.Rows(Oracle.Section(lines, header)).ToArray();
            Check.That(rows.Length == roles.Count && rows.Select(row => row[0]).Distinct().Count() == roles.Count, header);
            Observation[][] samples = Enumerable.Range(start, 8)
                .Select(sample => Oracle.ClassicSample(sample, attributes.Length)).ToArray();
            foreach (string[] row in rows)
            {
                Role role = roles[row[0]];
                string[][] tiers = FieldTiers;
                double[] weights = [8, 4, 2, 1, 0.2];
                if (role.Goalkeeper)
                {
                    string[] existing = KeeperTiers.SelectMany(group => group).ToArray();
                    tiers = [.. KeeperTiers, role.Key.Except(existing).ToArray(),
                        role.Preferable.Except(existing).Except(role.Key).ToArray()];
                    weights = [3, 2, 1, 0.2, 0.5, 0.25];
                }
                ClassicGroup[] plan = Classic.Plan(attributes, role, custom, tiers, weights);
                double[] factors = attributes.Select(attribute => role.Factor(attribute, custom)).ToArray();
                for (int index = 0; index < samples.Length; index++)
                {
                    string context = $"Classic {role.Id}/{role.Phase} {revision} s={start + index}";
                    string[] cells = row[index + 1].Split('/', StringSplitOptions.TrimEntries);
                    double expectedAbsolute = Oracle.Number(cells[0]);
                    Score actual = Classic.Evaluate(plan, factors, samples[index], historical: true) ?? throw new InvalidDataException(context);
                    double error = Math.Abs(actual.Absolute - expectedAbsolute);
                    Check.Near(expectedAbsolute, actual.Absolute, 1e-12 * Math.Max(1, Math.Abs(expectedAbsolute)), context);
                    Check.Near(Oracle.Number(cells[1]), actual.Normalized, 0, context);
                    worstAbsolute = Math.Max(worstAbsolute, error);
                    count++;

                    bool hasMissing = plan.SelectMany(group => group.Indices).Any(attribute => samples[index][attribute].Missing);
                    Score? sporting = Classic.Evaluate(plan, factors, samples[index]);
                    Check.That(hasMissing ? sporting is null : sporting == actual, $"{context}: cobertura Classic");
                    if (hasMissing)
                        omitted++;
                    else
                        same++;
                }
            }
        }
        Check.That(count == 2528, "Recuento Classic");
        Console.WriteLine($"B-01 Classic {count}/{count} parejas, error absoluto máximo {worstAbsolute:G6}. En modo deportivo coinciden {same} y se omiten {omitted} por ausencia.");
    }

    public static Dictionary<string, Curve[]> BuildCurves(Role[] roles, string[] attributes, HashSet<string> keeperAttributes, string[] lines)
    {
        var measured = Oracle.Rows(Oracle.Section(lines, "## Medidas de campo de FM27"))
            .Where(row => row[2] == "Ajuste y corte")
            .ToDictionary(row => row[0], row => Curves.Fit(Oracle.List(row[1]).Select(Oracle.Number).ToArray()));
        var keeper = KeeperCurves.SelectMany(group => group.Attributes.Select(attribute => (Attribute: attribute, group.Weight)))
            .ToDictionary(pair => pair.Attribute, pair => pair.Weight);
        Role[] keepers = roles.Where(role => role.Goalkeeper).ToArray();
        var deltas = keepers.ToDictionary(role => role.Id, role => attributes.Select(attribute =>
            attribute == "Eccentricity" ? 0 : role.Mark(attribute) -
                keepers.Where(other => other.Phase == role.Phase).Min(other => other.Mark(attribute))).ToArray());
        double maxDelta = deltas.Values.Max(values => values.Sum());
        Curve zero = new(0, 0, Curve.Straight);

        return roles.ToDictionary(role => role.Id, role =>
        {
            Curve[] plan = attributes.Select(attribute =>
            {
                Curve curve = role.Goalkeeper ? new Curve(keeper.GetValueOrDefault(attribute), 0, Curve.Straight)
                    : measured.GetValueOrDefault(attribute, zero);
                return curve with { Weight = curve.Weight * role.Factor(attribute) };
            }).ToArray();
            double baseWeight = plan.Sum(curve => curve.Weight);
            double quota = role.Goalkeeper ? 0.20 : 0.33;
            double available = baseWeight * quota / (1 - quota);
            double[] marks = role.Goalkeeper ? deltas[role.Id] :
                attributes.Select(attribute => keeperAttributes.Contains(attribute) ? 0 : role.Mark(attribute)).ToArray();
            double total = role.Goalkeeper ? maxDelta : marks.Sum();
            if (total > 0)
                for (int index = 0; index < attributes.Length; index++)
                    plan[index] = plan[index] with { Linear = available * marks[index] / total };
            return plan;
        });
    }

    public static void VerifyCurves(string[] lines, Dictionary<string, Curve[]> plans, int attributeCount)
    {
        string[][] rows = Oracle.Rows(lines).ToArray();
        Check.That(rows.Length == plans.Count && rows.Select(row => row[0]).Distinct().Count() == plans.Count, "Cobertura de roles en curvas");
        double worst = 0;
        int count = 0;
        Func<int, int>[] definitions = [_ => 12, index => 1 + (7 * index + 5) % 20, index => 20 - (7 * index + 5) % 20];
        Observation[][] values = definitions.Select(definition =>
            Enumerable.Range(0, attributeCount).Select(index => Observation.Exact(definition(index))).ToArray()).ToArray();
        foreach (string[] row in rows)
        for (int sample = 0; sample < values.Length; sample++)
        {
            string context = $"Curvas {row[0]} s={sample}";
            CurveScore result = Curves.Evaluate(plans[row[0]], values[sample]);
            Check.Near(1, result.Coverage, 0, context);
            double actual = result.Score?.Normalized ?? double.NaN;
            double expected = Oracle.Number(row[sample + 1]);
            Check.Near(expected, actual, 0.05, context);
            Check.Near(expected, Math.Round(actual, 1, MidpointRounding.AwayFromZero), 0, context);
            worst = Math.Max(worst, Math.Abs(expected - actual));
            count++;
        }
        Check.That(count == 237, "Recuento Curvas");
        Console.WriteLine($"B-01 Curvas {count}/{count} con las mismas cifras publicadas y error máximo {worst:F6} pp.");
    }

    public static void VerifyBoundaries()
    {
        ClassicGroup[] plan = [new(8, [0, 1])];
        double[] factors = [1, 1];
        Observation[] values = [new(12, 16), Observation.Exact(9)];
        Check.That(Classic.Evaluate(plan, factors, values) == new Score(92, 55), "Classic 92/55");
        values[1] = default;
        Check.That(Classic.Evaluate(plan, factors, values, historical: true) == new Score(56, 32), "Classic 56/32 histórico");
        Check.That(Classic.Evaluate(plan, factors, values) is null, "Classic 32 ausente en evaluación deportiva");
        Check.That(Classic.Evaluate([new(0, [0])], factors, values) is null, "Classic sin escala");

        Curve[] coverage = [new(7, 2, Curve.Straight), new(3, 5, Curve.Straight)];
        Observation level20 = Observation.Exact(20);
        Check.That(Curves.Evaluate(coverage, [default, level20]) == new CurveScore(null, 0.3), "Curvas 30% cobertura");
        Check.That(Curves.Evaluate(coverage, [level20, default]) == new CurveScore(new Score(14, 100), 0.7), "Curvas 70% exacto");
        Check.That(Curves.Evaluate(coverage, [level20, level20]) == new CurveScore(new Score(17, 100), 1), "Curvas 100% cobertura");
        Check.That(Curves.Evaluate([new(6.999, 0, Curve.Straight), new(3.001, 0, Curve.Straight)], [level20, default]).Score is null,
            "Curvas inmediatamente por debajo");
        Check.That(Curves.Evaluate([new(7.001, 0, Curve.Straight), new(2.999, 0, Curve.Straight)], [level20, default]).Score is not null,
            "Curvas inmediatamente por encima");
        Check.That(Curves.Evaluate(coverage, [new(0, 10), level20]).Score is null, "Curvas rango inválido");
        Check.That(Curves.Evaluate([new(0, 0, Curve.Straight)], [level20]).Score is null, "Curvas sin escala");
        CurveScore step = Curves.Evaluate([new(1, 0, level => level >= 11 ? 1 : 0)], [new(9, 11)]);
        Check.Near(1.0 / 3, step.Score?.Absolute ?? double.NaN, 1e-12, "Curva escalonada 9–11");
        Curve fitted = Curves.Fit([0, 6, 4, 12]);
        Check.Near(5, fitted.Weight * fitted.Shape(6), 1e-12, "PAVA a 6");
        Check.Near(5, fitted.Weight * fitted.Shape(9), 1e-12, "PAVA a 9");
        Check.Near(12, fitted.Weight * fitted.Shape(19), 1e-12, "Meseta a 19");
        Check.Near(0, Curves.Fit([0, 2, 3, 4]).Weight, 0, "Corte de amplitud exacto");
        Console.WriteLine("B-01 Fronteras verificadas para Classic 92/55 y 56/32 (histórico), cobertura 70%, rango 9–11 y PAVA.");
    }
}
