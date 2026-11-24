namespace FM27Analyzer.RatingProbe;

internal static class CurrentPilot
{
    // Esquema de claves de la referencia T021; la entrada sólo admite atributos visibles exactos.
    private static string AttributeKey(string attribute) => attribute switch
    {
        "One vs One" => "one-on-ones",
        "Rushing Out (Tendency)" => "rushing-out",
        "Free Kick Taking" => "free-kicks",
        _ => attribute.ToLowerInvariant().Replace(' ', '-')
    };

    public static void Run(TextReader input, string[] attributes, Dictionary<string, Role> roles,
        Dictionary<string, Curve[]> curves, string position, string ip, string oop)
    {
        if (!roles.TryGetValue(ip, out Role? inPossession) || inPossession.Phase != "ip" ||
            !roles.TryGetValue(oop, out Role? outOfPossession) || outOfPossession.Phase != "oop" ||
            !inPossession.Positions.Contains(position) || !outOfPossession.Positions.Contains(position))
            throw new ArgumentException("Puesto y roles incompatibles.");

        var indices = attributes.Select((attribute, index) => (Key: AttributeKey(attribute), Index: index))
            .ToDictionary(pair => pair.Key, pair => pair.Index);
        var values = new Observation[attributes.Length];
        int count = 0;
        while (input.ReadLine() is { } line)
        {
            string[] pair = line.Split('=', StringSplitOptions.TrimEntries);
            if (pair.Length != 2 || !indices.TryGetValue(pair[0], out int index) || !values[index].Missing ||
                !int.TryParse(pair[1], out int level) || level is < 1 or > 20)
                throw new InvalidDataException("Entrada incompatible o fuera de la lista de atributos visibles.");
            values[index] = Observation.Exact(level);
            count++;
        }
        Check.That(count > 0, "Piloto sin atributos");

        var results = new[] { ip, oop }.Select(role =>
        {
            CurveScore result = Curves.Evaluate(curves[role], values);
            Score score = result.Score ?? throw new InvalidDataException($"Cobertura insuficiente para {role}.");
            Fit fit = Familiarity.Compose(score, Knowledge.Unknown, () => throw new InvalidOperationException(), false, level => level / 20.0);
            Check.That(fit.Scenario20 == score.Normalized && fit.Observed is null && !fit.HasRatedCurrentEdge, "Piloto sin insumo de familiaridad");
            return $"Piloto presente {position} {role} con {count} exactos, cobertura {result.Coverage:P2}, DWRS {score.Normalized:F1} y escenario 20 {fit.Scenario20:F1}.";
        }).ToArray();
        foreach (string result in results)
            Console.WriteLine(result);
    }
}
