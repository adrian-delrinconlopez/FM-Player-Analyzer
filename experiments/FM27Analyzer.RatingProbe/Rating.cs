namespace FM27Analyzer.RatingProbe;

internal readonly record struct Observation(int Low, int High)
{
    public static Observation Exact(int value) => new(value, value);
    public bool Missing => Low == 0 && High == 0;
    public bool Valid => Low >= 1 && High >= Low && High <= 20;
    public double Midpoint => (Low + High) / 2.0;
}

internal readonly record struct Score(double Absolute, double Normalized);
internal readonly record struct CurveScore(Score? Score, double Coverage);

internal readonly record struct ClassicGroup(double Weight, int[] Indices);

internal static class Classic
{
    public static ClassicGroup[] Plan(string[] attributes, Role role, bool custom, string[][] tiers, double[] weights)
    {
        List<ClassicGroup> groups = [];
        for (int index = 0; index < weights.Length; index++)
        {
            double weight = weights[index];
            if (custom)
                weight = role.Goalkeeper ? 0.8 * weight + 0.2 : 0.7 * weight + 0.3;
            groups.Add(new ClassicGroup(weight, tiers[index].Select(attribute => Array.IndexOf(attributes, attribute)).ToArray()));
        }
        return groups.ToArray();
    }

    public static Score? Evaluate(ClassicGroup[] groups, double[] factors, Observation[] values, bool historical = false)
    {
        double absolute = 0, low = 0, high = 0;
        foreach (ClassicGroup group in groups)
        {
            if (group.Indices.Length == 0 || group.Weight == 0)
                continue;

            double sum = 0, min = 0, max = 0;
            foreach (int index in group.Indices)
            {
                Observation value = values[index];
                if (!value.Valid && !(historical && value.Missing))
                    return null;
                double factor = factors[index];
                sum += value.Midpoint * factor;
                min += factor;
                max += 20 * factor;
            }
            absolute += sum / group.Indices.Length * group.Weight;
            low += min / group.Indices.Length * group.Weight;
            high += max / group.Indices.Length * group.Weight;
        }
        if (high == low)
            return historical ? new Score(absolute, 0) : null;
        return new Score(absolute, Math.Round((absolute - low) / (high - low) * 100, MidpointRounding.ToEven));
    }
}

internal sealed record Curve(double Weight, double Base, Func<int, double> Shape, double Linear = 0)
{
    public double TotalWeight => Weight + Linear;

    public static double Straight(int level) => (level - 1) / 19.0;
}

internal static class Curves
{
    private static readonly int[] Knots = [1, 6, 12, 18];

    public static Curve Fit(double[] samples)
    {
        var blocks = new List<(double Sum, int Count)>();
        foreach (double sample in samples)
        {
            blocks.Add((sample, 1));
            while (blocks.Count > 1 && blocks[^2].Sum / blocks[^2].Count > blocks[^1].Sum / blocks[^1].Count)
            {
                var last = blocks[^1];
                var previous = blocks[^2];
                blocks.RemoveAt(blocks.Count - 1);
                blocks[^1] = (previous.Sum + last.Sum, previous.Count + last.Count);
            }
        }
        double[] fitted = blocks.SelectMany(block => Enumerable.Repeat(block.Sum / block.Count, block.Count)).ToArray();
        double weight = fitted[^1] - fitted[0];
        if (weight <= 4)
            return new Curve(0, 0, _ => 0);

        double Shape(int level)
        {
            if (level >= Knots[^1])
                return 1;
            int upper = Array.FindIndex(Knots, knot => knot > level);
            int lower = upper - 1;
            double blend = (double)(level - Knots[lower]) / (Knots[upper] - Knots[lower]);
            return (fitted[lower] - fitted[0] + blend * (fitted[upper] - fitted[lower])) / weight;
        }
        return new Curve(weight, 0, Shape);
    }

    public static CurveScore Evaluate(Curve[] plan, Observation[] values)
    {
        double total = plan.Sum(curve => curve.TotalWeight);
        double known = 0, contribution = 0, baseline = 0;
        for (int index = 0; index < plan.Length; index++)
        {
            Curve curve = plan[index];
            if (curve.TotalWeight == 0)
                continue;
            if (curve.Weight > 0)
                baseline += curve.Base;
            Observation value = values[index];
            if (value.Missing)
                continue;
            if (!value.Valid)
                return new CurveScore(null, 0);
            double mean = Enumerable.Range(value.Low, value.High - value.Low + 1)
                .Average(level => curve.Weight * curve.Shape(level) + curve.Linear * Curve.Straight(level));
            known += curve.TotalWeight;
            contribution += Math.Clamp(mean, 0, curve.TotalWeight);
        }

        double coverage = total > 0 ? known / total : 0;
        Score? score = known > 0 && known >= 0.7 * total - 1e-9
            ? new Score(baseline + contribution, Math.Clamp(100 * contribution / known, 0, 100))
            : null;
        return new CurveScore(score, coverage);
    }
}
