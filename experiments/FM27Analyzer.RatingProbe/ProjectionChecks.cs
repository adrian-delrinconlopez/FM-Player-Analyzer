namespace FM27Analyzer.RatingProbe;

internal static class ProjectionChecks
{
    public static void Verify(Dictionary<string, Curve[]> curves, string[] attributes)
    {
        GrowthTrack Track(string attribute, int level, params Increment[] steps) =>
            new(Array.IndexOf(attributes, attribute), Observation.Exact(level), steps);

        GrowthTrack pace = Track("Pace", 18, new(2, 1), new(2, 1));
        GrowthTrack anticipation = Track("Anticipation", 19, new Increment(3, 3));
        GrowthTrack balance = Track("Balance", 17, new(1, 3), new(2, 2), new(4, 1));
        GrowthTrack reflexes = Track("Reflexes", 18, new(2, 1), new(3, 0));
        var example = new DevelopmentInput(100, 105, [pace, anticipation]);
        Allocation chosen = Solve(example);
        Check.That(chosen.Cost == 5 && chosen.Gain == 4, "Ejemplo del plan: coste 5, ganancia 4");
        // Anticipation precede a Pace en el diccionario.
        Check.That(Project(example.Tracks, chosen).SequenceEqual([20, 19]), "Ejemplo del plan: Pace 19, Anticipation 20");
        Check.That(Solve(example with { PotentialAbility = 100 }).Increments.All(step => step == 0), "CA = PA");
        Check.That(Solve(example with { PotentialAbility = 110 }).Cost == 7, "Saturación con margen sobrante");
        Check.That(Projection.Solve(() => example with { PotentialAbility = 99 }, () => true) is null, "PA menor que CA");
        Check.That(Projection.Solve(() => example with { CurrentAbility = 0 }, () => true) is null, "CA fuera de escala");
        Check.That(Projection.Solve(() => example with { PotentialAbility = 201 }, () => true) is null, "PA fuera de escala");
        Check.That(Projection.Solve(() => example with { Tracks = [] }, () => true) is null, "Sin cobertura");
        Check.That(Projection.Solve(() => example with { Tracks = [pace, pace] }, () => true) is null, "Atributo duplicado");

        GrowthTrack[] invalid =
        [
            pace with { Current = default },
            pace with { Current = new Observation(17, 18) },
            pace with { Current = Observation.Exact(21) },
            pace with { Steps = [new(0, 1), new(2, 1)] },
            pace with { Steps = [new(2, 1), new(2, 2)] },
            pace with { Steps = [new(2, 1)] }
        ];
        foreach (GrowthTrack track in invalid)
            Check.That(Projection.Solve(() => example with { Tracks = [track, anticipation] }, () => true) is null, "Entrada incompatible");

        var samePa = example with { Tracks = [Track("Pace", 17, new(2, 1), new(2, 1), new(2, 1)), anticipation] };
        Check.That(!Project(example.Tracks, chosen).SequenceEqual(Project(samePa.Tracks, Solve(samePa))), "Mismo PA, vectores distintos");
        var tie = new DevelopmentInput(100, 102, [pace, reflexes]);
        DevelopmentInput freeBenefitTie = tie with { Tracks = [reflexes], PotentialAbility = 105 };
        Check.That(Project(tie.Tracks, Solve(tie)).SequenceEqual([19, 18]), "Empate por primer atributo canónico");
        Check.That(Solve(freeBenefitTie).Cost == 2, "Empate por menor coste");

        int checks = 0;
        GrowthTrack[][] sets = [[pace, anticipation], [pace, reflexes], [pace, balance, anticipation],
            [balance, reflexes, anticipation], [pace, anticipation, reflexes, balance]];
        foreach (GrowthTrack[] tracks in sets)
        {
            int previousGain = 0;
            foreach (int budget in Enumerable.Range(0, 16))
            {
                var input = new DevelopmentInput(100, 100 + budget, tracks);
                Allocation expected = Enumerate(tracks, budget);
                Allocation actual = Solve(input);
                Check.That(expected.Cost == actual.Cost && expected.Gain == actual.Gain &&
                    expected.Increments.SequenceEqual(actual.Increments), "Programación dinámica frente a enumeración");
                Allocation reversed = Solve(input with { Tracks = tracks.Reverse().ToArray() });
                Check.That(actual.Increments.SequenceEqual(reversed.Increments) && actual.Cost == reversed.Cost &&
                    actual.Gain == reversed.Gain, "Invariancia al orden de entrada");
                Check.That(actual.Cost <= budget && actual.Gain >= previousGain &&
                    Project(tracks, actual).All(value => value is >= 1 and <= 20), "Presupuesto, beneficio monótono y saturación");
                previousGain = actual.Gain;
                checks++;
            }
        }

        VerifyPermissions(example, example with { CurrentAbility = 130, PotentialAbility = 150 });
        Allocation proportional = Proportional(example.Tracks, 5);
        Check.That(proportional.Cost == 5 && proportional.Gain == 4, "Escalado proporcional con margen 5");
        Allocation tight = Solve(example with { PotentialAbility = 102 });
        Allocation tightProportional = Proportional(example.Tracks, 2);
        Check.That(tight.Cost == 2 && tight.Gain == 1 && tightProportional.Cost == 0 && tightProportional.Gain == 0,
            "El escalado sincronizado devuelve cambio cero con margen 2");
        VerifyReuse(curves, attributes.Length, example, chosen);
        Console.WriteLine($"B-03 {checks} comparaciones exhaustivas y {checks} permutaciones. Permisos, saturación y reutilización IP/OOP verificados.");
        Console.WriteLine("B-03 Con margen 5, reparto y escalado gastan 5 y ganan 4. Con margen 2, el reparto gasta 2 y gana 1 y el escalado devuelve cambio cero.");
    }

    private static Allocation Solve(DevelopmentInput input) =>
        Projection.Solve(() => input, () => true) ?? throw new InvalidDataException("Proyección sintética ausente.");

    private static int[] Project(GrowthTrack[] tracks, Allocation allocation) => tracks.OrderBy(track => track.Attribute)
        .Select((track, index) => track.Current.Low + allocation.Increments[index]).ToArray();

    private static Allocation Enumerate(GrowthTrack[] tracks, int budget)
    {
        tracks = tracks.OrderBy(track => track.Attribute).ToArray();
        int[] choice = tracks.Select(track => track.Steps.Length).ToArray();
        var best = new Allocation(0, -1, []);
        // Enumera primero el mayor incremento del primer atributo canónico.
        while (true)
        {
            var activated = tracks.SelectMany((track, index) => track.Steps.Take(choice[index])).ToArray();
            int cost = activated.Sum(step => step.Cost);
            int gain = activated.Sum(step => step.Gain);
            if (cost <= budget && (gain > best.Gain || (gain == best.Gain && cost < best.Cost)))
                best = new Allocation(cost, gain, [.. choice]);

            int indexToDecrement = Array.FindLastIndex(choice, count => count > 0);
            if (indexToDecrement < 0)
                return best;
            choice[indexToDecrement]--;
            for (int index = indexToDecrement + 1; index < choice.Length; index++)
                choice[index] = tracks[index].Steps.Length;
        }
    }

    private static Allocation Proportional(GrowthTrack[] tracks, int budget)
    {
        tracks = tracks.OrderBy(track => track.Attribute).ToArray();
        var best = new Allocation(0, 0, new int[tracks.Length]);
        double bestFactor = 0;
        foreach (GrowthTrack breakpoint in tracks)
        for (int increase = 1; increase <= breakpoint.Steps.Length; increase++)
        {
            int denominator = breakpoint.Current.Low;
            double factor = (double)increase / denominator;
            if (factor <= bestFactor)
                continue;
            int[] increments = tracks.Select(track =>
                Math.Min(track.Steps.Length, track.Current.Low * increase / denominator)).ToArray();
            var active = tracks.SelectMany((track, index) => track.Steps.Take(increments[index])).ToArray();
            int cost = active.Sum(step => step.Cost);
            if (cost <= budget)
            {
                best = new Allocation(cost, active.Sum(step => step.Gain), increments);
                bestFactor = factor;
            }
        }
        return best;
    }

    private static void VerifyPermissions(DevelopmentInput first, DevelopmentInput second)
    {
        Check.That(!Solve(first).Increments.SequenceEqual(Solve(second).Increments),
            "Los secretos de las fuentes gemelas pueden cambiar la proyección con ambos permisos");
        foreach (var (allowCa, allowPa, certified) in new[] { (false, false, true), (true, false, true), (false, true, true), (true, true, false) })
        {
            int reads = 0;
            Allocation? From(DevelopmentInput source) => Projection.Solve(() =>
            {
                reads++;
                return source;
            }, () => allowCa && allowPa && certified);
            Check.That(From(first) is null && From(second) is null && reads == 0, "Fuentes gemelas sin permiso o fuente acreditada");
        }
        Check.That(Projection.Solve(() => null, () => true) is null, "Insumos no disponibles");
        foreach (bool revokeCa in new[] { false, true })
        foreach (int revocationAt in new[] { 3, 4 })
        {
            bool ca = true, pa = true;
            int checks = 0;
            bool Granted()
            {
                if (++checks == revocationAt)
                {
                    if (revokeCa)
                        ca = false;
                    else
                        pa = false;
                }
                return ca && pa;
            }
            Check.That(Projection.Solve(() => first, Granted) is null && checks == revocationAt,
                "Revocación durante la búsqueda o antes de entregar el resultado");
        }
    }

    private static void VerifyReuse(Dictionary<string, Curve[]> curves, int length, DevelopmentInput example, Allocation chosen)
    {
        GrowthTrack[] tracks = example.Tracks.OrderBy(track => track.Attribute).ToArray();
        Observation[] current = Enumerable.Repeat(Observation.Exact(12), length).ToArray();
        foreach (GrowthTrack track in tracks)
            current[track.Attribute] = track.Current;
        Observation[] projected = (Observation[])current.Clone();
        for (int index = 0; index < tracks.Length; index++)
            projected[tracks[index].Attribute] = Observation.Exact(tracks[index].Current.Low + chosen.Increments[index]);

        foreach (string role in new[] { "AM-IP", "AM-OOP", "GK-IP", "GK-OOP" })
        {
            Score? observed = Curves.Evaluate(curves[role], current).Score;
            Score? future = Curves.Evaluate(curves[role], projected).Score;
            Check.That(observed.HasValue && future.HasValue && future.Value.Normalized >= observed.Value.Normalized, role);
            Fit fit = Familiarity.Compose(future, Knowledge.Observed, () => 15, false, level => level / 20.0);
            Check.That(fit.Scenario20 == future?.Normalized && fit.Observed.HasValue &&
                fit.Observed < fit.Scenario20 && !fit.HasRatedCurrentEdge, "Proyección reutilizada por fase");
        }
    }
}
