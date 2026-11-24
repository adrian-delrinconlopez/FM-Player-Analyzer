namespace FM27Analyzer.RatingProbe;

internal readonly record struct Increment(int Cost, int Gain);
internal sealed record GrowthTrack(int Attribute, Observation Current, Increment[] Steps);
internal sealed record DevelopmentInput(int CurrentAbility, int PotentialAbility, GrowthTrack[] Tracks);
internal sealed record Allocation(int Cost, int Gain, int[] Increments);

internal static class Projection
{
    // El llamador de la prueba simula los permisos y la revisión vigente.
    public static Allocation? Solve(Func<DevelopmentInput?> readAuthorized, Func<bool> authorized)
    {
        if (!authorized())
            return null;
        DevelopmentInput? input = readAuthorized();
        if (input is null || input.CurrentAbility is < 1 or > 200 || input.PotentialAbility > 200 ||
            input.PotentialAbility < input.CurrentAbility || input.Tracks.Length == 0)
            return null;

        GrowthTrack[] tracks = input.Tracks.OrderBy(track => track.Attribute).ToArray();
        if (tracks.Select(track => track.Attribute).Distinct().Count() != tracks.Length || !tracks.All(Valid))
            return null;

        int budget = input.PotentialAbility - input.CurrentAbility;
        Allocation?[] states = new Allocation?[budget + 1];
        states[0] = new Allocation(0, 0, []);
        foreach (GrowthTrack track in tracks)
        {
            if (!authorized())
                return null;
            Allocation?[] next = new Allocation?[budget + 1];
            foreach (Allocation? prefix in states)
            {
                if (prefix is null)
                    continue;
                int cost = prefix.Cost;
                int gain = prefix.Gain;
                for (int count = 0; count <= track.Steps.Length; count++)
                {
                    var candidate = new Allocation(cost, gain, [.. prefix.Increments, count]);
                    if (Better(candidate, next[cost]))
                        next[cost] = candidate;
                    if (count == track.Steps.Length || track.Steps[count].Cost > budget - cost)
                        break;
                    cost += track.Steps[count].Cost;
                    gain += track.Steps[count].Gain;
                }
            }
            states = next;
        }

        Allocation? best = null;
        foreach (Allocation? candidate in states)
            if (candidate is not null && Better(candidate, best))
                best = candidate;
        return authorized() ? best : null;
    }

    private static bool Valid(GrowthTrack track) =>
        track.Attribute >= 0 && track.Current.Valid && track.Current.Low == track.Current.High &&
        track.Steps.Length == 20 - track.Current.High &&
        track.Steps.All(step => step.Cost is >= 1 and <= 200 && step.Gain is >= 0 and <= 1000) &&
        track.Steps.Zip(track.Steps.Skip(1), (earlier, later) => earlier.Gain >= later.Gain).All(pair => pair);

    private static bool Better(Allocation candidate, Allocation? incumbent)
    {
        if (incumbent is null || candidate.Gain != incumbent.Gain)
            return incumbent is null || candidate.Gain > incumbent.Gain;
        if (candidate.Cost != incumbent.Cost)
            return candidate.Cost < incumbent.Cost;
        for (int index = 0; index < candidate.Increments.Length; index++)
            if (candidate.Increments[index] != incumbent.Increments[index])
                return candidate.Increments[index] > incumbent.Increments[index];
        return false;
    }
}
