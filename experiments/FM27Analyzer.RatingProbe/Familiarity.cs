namespace FM27Analyzer.RatingProbe;

internal enum Knowledge { Observed, Unknown, Restricted }

internal readonly record struct Fit(Score? Base, double? Observed, double? Scenario20, bool CurrentEligible, Knowledge Knowledge)
{
    public bool HasRatedCurrentEdge => CurrentEligible && Observed.HasValue;
}

internal static class Familiarity
{
    // Tabla sintética v0. Los tramos 1–5, 6–10, 11–15, 16–19 y 20 son hipótesis.
    private static double Affinity(int level) => level switch
    {
        <= 5 => 0.20,
        <= 10 => 0.50,
        <= 15 => 0.70,
        <= 19 => 0.90,
        _ => 1
    };

    private static double Linear(int level) => level / 20.0;

    internal static Fit Compose(Score? rating, Knowledge knowledge, Func<int> readFamiliarity,
        bool eligibleByPosition, Func<int, double> multiplier)
    {
        double? observed = null;
        if (rating.HasValue && knowledge == Knowledge.Observed)
        {
            int level = readFamiliarity();
            if (level is >= 1 and <= 20)
                observed = rating.Value.Normalized * multiplier(level);
            else
                knowledge = Knowledge.Unknown;
        }
        return new Fit(rating, observed, rating?.Normalized, rating.HasValue && eligibleByPosition, knowledge);
    }

    public static void Verify(Dictionary<string, Curve[]> curves, int length)
    {
        Func<int, double>[] candidates = [Linear, Affinity];
        double maximumDifference = Enumerable.Range(1, 20).Max(level => Math.Abs(Linear(level) - Affinity(level)));
        Check.Near(0.20, maximumDifference, 1e-12, "Diferencia máxima de multiplicadores");
        foreach (var candidate in candidates)
        {
            Check.Near(1, candidate(20), 0, "Familiaridad 20 neutra");
            for (int level = 1; level < 20; level++)
                Check.That(candidate(level) >= 0 && candidate(level) <= candidate(level + 1), $"Afinidad monótona en {level}");
        }

        Observation[] observations = Enumerable.Repeat(Observation.Exact(12), length).ToArray();
        string[] contexts = ["AM-IP", "AM-OOP", "GK-IP", "GK-OOP"];
        int checkedScenarios = 0;
        foreach (string context in contexts)
        foreach (var candidate in candidates)
        foreach (bool eligible in new[] { false, true })
        {
            Score basis = Curves.Evaluate(curves[context], observations).Score ?? throw new InvalidDataException(context);
            foreach (int level in new[] { 1, 5, 15, 20 })
            {
                Fit fit = Compose(basis, Knowledge.Observed, () => level, eligible, candidate);
                Check.That(fit.Base == basis && fit.Scenario20 == basis.Normalized &&
                    fit.CurrentEligible == eligible && fit.HasRatedCurrentEdge == eligible &&
                    fit.Knowledge == Knowledge.Observed, context);
                Check.Near(basis.Normalized * candidate(level), fit.Observed ?? double.NaN, 0, context);
                checkedScenarios++;
            }
            foreach (Knowledge knowledge in new[] { Knowledge.Unknown, Knowledge.Restricted })
            {
                Fit fit = Compose(basis, knowledge, () => throw new InvalidOperationException("Lectura indebida"), eligible, candidate);
                Check.That(fit.Base == basis && fit.Scenario20 == basis.Normalized && fit.Observed is null &&
                    fit.CurrentEligible == eligible && !fit.HasRatedCurrentEdge && fit.Knowledge == knowledge, context);
                checkedScenarios++;
            }
            Fit outOfScale = Compose(basis, Knowledge.Observed, () => 0, eligible, candidate);
            Check.That(outOfScale.Knowledge == Knowledge.Unknown && outOfScale.Observed is null &&
                outOfScale.Scenario20 == basis.Normalized && outOfScale.CurrentEligible == eligible, context);
            Fit noScore = Compose(null, Knowledge.Observed, () => throw new InvalidOperationException("Lectura sin nota"), true, candidate);
            Check.That(noScore.Base is null && noScore.Observed is null && noScore.Scenario20 is null && !noScore.CurrentEligible, context);
            checkedScenarios += 2;
        }

        Score synthetic = new(0, 80);
        Check.That(Compose(synthetic, Knowledge.Observed, () => 15, false, Linear).Observed == 60, "80 × 15/20 = 60");
        Check.That(Compose(synthetic, Knowledge.Observed, () => 15, false, Affinity).Observed == 56, "80 × 0,70 = 56");
        Score lower = new(0, 58);
        double? At(Score score, int level, Func<int, double> candidate) =>
            Compose(score, Knowledge.Observed, () => level, true, candidate).Observed;
        Check.That(At(synthetic, 15, Linear) > At(lower, 20, Linear) &&
            At(synthetic, 15, Affinity) < At(lower, 20, Affinity), "Inversión sintética de orden");
        Console.WriteLine($"B-02 {checkedScenarios} escenarios campo/portería IP/OOP. Diferencia máxima {maximumDifference:P0} del DWRS e inversión de orden 60/58 frente a 56/58.");
    }
}
