using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using FM27Analyzer.Extraction.Poc.Evidence;

namespace FM27Analyzer.Extraction.Poc.Capture;

[SupportedOSPlatform("windows")]
internal static partial class ExternalFieldProbe
{
    private const string PriorityRoute = "Memoria externa de FM27, perfil Steam 26620374";
    private const string EditorPositions = "FM27 · Editor del juego · Posiciones";
    private static readonly CaptureLimits PriorityLimits = new(96 * 1024, 20, 256, TimeSpan.FromSeconds(2));

    // Perfil v0.5.3 cotejado por campo. La lista excluye CA/PA y atributos ocultos.
    private static readonly Dictionary<string, (int Offset, string Group)> Attributes = new(StringComparer.Ordinal)
    {
        ["crossing"] = (0x15F, "técnico"), ["dribbling"] = (0x160, "técnico"),
        ["finishing"] = (0x161, "técnico"), ["heading"] = (0x162, "técnico"),
        ["long-shots"] = (0x163, "técnico"), ["marking"] = (0x164, "técnico"),
        ["passing"] = (0x166, "técnico"), ["tackling"] = (0x168, "técnico"),
        ["first-touch"] = (0x175, "técnico"), ["technique"] = (0x176, "técnico"),
        ["penalty-taking"] = (0x167, "balón parado"), ["corners"] = (0x17A, "balón parado"),
        ["long-throws"] = (0x17D, "balón parado"), ["free-kicks"] = (0x182, "balón parado"),
        ["off-the-ball"] = (0x165, "mental"), ["vision"] = (0x169, "mental"),
        ["anticipation"] = (0x170, "mental"), ["decisions"] = (0x171, "mental"),
        ["positioning"] = (0x173, "mental"), ["flair"] = (0x179, "mental"),
        ["teamwork"] = (0x17B, "mental"), ["work-rate"] = (0x17C, "mental"),
        ["leadership"] = (0x187, "mental"), ["bravery"] = (0x18A, "mental"),
        ["aggression"] = (0x18C, "mental"), ["determination"] = (0x192, "mental"),
        ["composure"] = (0x193, "mental"), ["concentration"] = (0x194, "mental"),
        ["acceleration"] = (0x181, "físico"), ["strength"] = (0x183, "físico"),
        ["stamina"] = (0x184, "físico"), ["pace"] = (0x185, "físico"),
        ["jumping-reach"] = (0x186, "físico"), ["balance"] = (0x189, "físico"),
        ["agility"] = (0x18D, "físico"), ["natural-fitness"] = (0x191, "físico"),
        ["handling"] = (0x16A, "portería"), ["aerial-reach"] = (0x16B, "portería"),
        ["command-of-area"] = (0x16C, "portería"), ["communication"] = (0x16D, "portería"),
        ["kicking"] = (0x16E, "portería"), ["throwing"] = (0x16F, "portería"),
        ["one-on-ones"] = (0x172, "portería"), ["reflexes"] = (0x174, "portería"),
        ["eccentricity"] = (0x17E, "portería"), ["rushing-out"] = (0x17F, "portería"),
        ["punching"] = (0x180, "portería")
    };

    private static readonly Dictionary<string, int> PositionOffsets = new(StringComparer.Ordinal)
    {
        ["GK"] = 0x150, ["DL"] = 0x152, ["DC"] = 0x153, ["DR"] = 0x154, ["DM"] = 0x155,
        ["ML"] = 0x156, ["MC"] = 0x157, ["MR"] = 0x158, ["AML"] = 0x159, ["AMC"] = 0x15A,
        ["AMR"] = 0x15B, ["ST"] = 0x15C, ["WBL"] = 0x15D, ["WBR"] = 0x15E
    };

    private static void ValidatePriorityReference(ComparisonReference reference)
    {
        if (reference.Expected.IsDefaultOrEmpty || reference.Expected.Length > PriorityLimits.MaxPlayers
            || reference.Observations.IsDefaultOrEmpty || reference.Observations.Length > PriorityLimits.MaxObservations)
            throw new InvalidDataException("Muestra de campos prioritarios fuera de presupuesto.");
        var keys = new HashSet<PlayerKey>();
        foreach (var identity in reference.Expected)
        {
            if (identity?.Key is not { SourceType: "FM27", Uid: var uid }
                || !uint.TryParse(uid, NumberStyles.None, CultureInfo.InvariantCulture, out uint numericUid)
                || numericUid == 0 || uid != numericUid.ToString(CultureInfo.InvariantCulture)
                || identity.CareerOrConfirmedCopy != CopyName || identity.Club != new ClubLink(ClubStatus.ConfirmedClub, ClubName)
                || !keys.Add(identity.Key))
                throw new InvalidDataException("La referencia sólo admite UID del primer equipo con club confirmado.");
        }
        var cells = new HashSet<(PlayerKey, FieldKey)>();
        foreach (var row in reference.Observations)
        {
            if (row?.Field is not { Name: var name, Qualifier: null } || !Attributes.ContainsKey(name)
                || !keys.Contains(row.Player) || row.Type != FieldType.Numeric || row.Unit != "1–20"
                || row.State != ExactVisible || row.Value is not { Low: >= 1 and <= 20 and var exact, High: null, Category: null }
                || exact != decimal.Truncate(exact) || row.DataSource != reference.IndependentSource
                || row.VisibilitySource != reference.IndependentSource || !cells.Add((row.Player, row.Field)))
                throw new InvalidDataException("La referencia requiere atributos exactos visibles con procedencia independiente.");
        }
        if (cells.Select(cell => cell.Item1).Distinct().Count() != keys.Count)
            throw new InvalidDataException("Alguna identidad carece de observaciones fijadas.");
    }

    private static Dictionary<string, Dictionary<string, int>> ReadPositionReference(ComparisonReference visible)
    {
        var line = Console.ReadLine();
        if (line is not { Length: > 0 and <= 4096 })
            throw new InvalidDataException("Falta la referencia efímera de posiciones del editor.");
        Dictionary<string, Dictionary<string, int>>? values;
        try { values = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, int>>>(line); }
        catch (JsonException) { throw new InvalidDataException("Referencia de posiciones ilegible."); }
        var sample = visible.Expected.Select(identity => identity.Key.Uid).ToHashSet(StringComparer.Ordinal);
        if (values is not { Count: > 0 } || values.Any(player => !sample.Contains(player.Key)
            || player.Value is not { Count: > 0 } || player.Value.Any(position =>
                !PositionOffsets.ContainsKey(position.Key) || position.Value is < 0 or > 20)))
            throw new InvalidDataException("Posición, escala o jugador ajeno a la referencia autorizada.");
        return values;
    }

    private static int CheckPriorities(ComparisonReference reference,
        Dictionary<string, Dictionary<string, int>>? positions, Reader reader, long team, long startTicks, ulong loads,
        CaptureSession session, Func<CaptureContext?> current)
    {
        var expected = reference.Expected.ToDictionary(identity => identity.Key);
        var roster = session.Read(() =>
        {
            var selected = new Dictionary<PlayerKey, long>();
            foreach (long address in reader.Pointers(team + 0x38, 128))
            {
                uint uid = reader.U32(address + 0x294);
                var key = new PlayerKey("FM27", uid.ToString(CultureInfo.InvariantCulture));
                if (!expected.ContainsKey(key)) continue;
                if (reader.Pointer(address + 0x130) != team || !selected.TryAdd(key, address))
                    throw new InvalidDataException("Vínculo o UID ambiguo en el primer equipo.");
            }
            if (selected.Count != expected.Count)
                throw new InvalidDataException("La muestra fijada no pertenece íntegramente al primer equipo activo.");
            return selected;
        });

        byte ReadBound(PlayerKey player, int offset)
        {
            long address = roster[player];
            bool SamePlayer() => reader.U32(address + 0x294).ToString(CultureInfo.InvariantCulture) == player.Uid
                && reader.Pointer(address + 0x130) == team;
            if (!SamePlayer()) session.ContextChanged();
            session.Token.ThrowIfCancellationRequested();
            byte value = reader.U8(address + offset);
            if (!SamePlayer()) session.ContextChanged();
            session.Token.ThrowIfCancellationRequested();
            return value;
        }

        var fields = reference.Observations.ToDictionary(row => (row.Player, row.Field));
        var access = new CaptureAuthorization(session.Expected, key => roster.ContainsKey(key) ? IdentityScope.Squad : IdentityScope.Unverified,
            key => expected.GetValueOrDefault(key), key => key.Qualifier is null && Attributes.ContainsKey(key.Name),
            (key, field) => fields.ContainsKey((key, field))
                ? new(FieldClass.Visible, FieldType.Numeric, "1–20", ExactVisible, PriorityRoute,
                    reference.IndependentSource, null, ScaleMinimum: 1, ScaleMaximum: 20)
                : null);

        PlayerObservation ReadAttribute(PlayerObservation row) => access.Project(session, row.Player, row.Field, _ =>
        {
            byte raw = ReadBound(row.Player, Attributes[row.Field.Name].Offset);
            if (raw > 100) throw new InvalidDataException("Atributo de plantilla fuera de escala.");
            return new(Math.Clamp((raw + 2) / 5, 1, 20));
        }) ?? throw new InvalidDataException("Atributo fuera de la proyección autorizada.");

        var first = reference.Observations.Select(ReadAttribute).ToArray();
        var second = reference.Observations.Select(ReadAttribute).ToArray();
        var check = first.Select((row, index) => new
        {
            Group = Attributes[row.Field.Name].Group, Field = row.Field.Name,
            Expected = reference.Observations[index].Value!.Low,
            First = row.Value!.Low, Second = second[index].Value!.Low
        }).ToArray();
        _ = session.Read(() => BatchValidation.Seal(new(new(1, 0), Guid.NewGuid(), session.Expected, PriorityRoute,
            "Steam 26620374", DateTimeOffset.UtcNow, SampleExtent.Partial, PriorityLimits, reference.Expected, [.. first]),
            access, PriorityLimits));

        var positionChecks = new List<(string Position, SpecialFieldStatus Status)>();
        if (positions is not null)
        {
            var positionContext = session.Expected with { Policy = new CapturePolicy(2, HiddenAuthorized: true) };
            using var positionSession = new CaptureSession(positionContext,
                () => current() == session.Expected ? positionContext : null, _ => { });
            var positionAccess = new CaptureAuthorization(positionContext,
                key => roster.ContainsKey(key) ? IdentityScope.Squad : IdentityScope.Unverified,
                key => expected.GetValueOrDefault(key), field => field.Name == "positional-familiarity"
                    && field.Qualifier is string qualifier && PositionOffsets.ContainsKey(qualifier),
                (key, field) => positions.TryGetValue(key.Uid, out var selected) && selected.ContainsKey(field.Qualifier!)
                    ? new(FieldClass.HiddenExact, FieldType.Numeric, "0–20",
                        new(Knowledge.Exact, Availability.Observed, Disclosure.Restricted),
                        PriorityRoute, EditorPositions, null, ScaleMinimum: 0, ScaleMaximum: 20)
                    : null);
            foreach (var (uid, ratings) in positions)
            foreach (var (position, expectedValue) in ratings)
            {
                var key = new PlayerKey("FM27", uid);
                var field = new FieldKey("positional-familiarity", position);
                var comparison = new SpecialFieldReference(positionContext, key, field, EditorPositions, "0–20", () => expectedValue);
                int? Read() => ReadBound(key, PositionOffsets[position]);
                var result = SpecialFieldAssessment.Compare(positionSession, positionAccess, key, field, FieldClass.HiddenExact, comparison, Read);
                var repeated = SpecialFieldAssessment.Compare(positionSession, positionAccess, key, field, FieldClass.HiddenExact, comparison, Read);
                positionChecks.Add((position, result == repeated ? result : SpecialFieldStatus.Mismatch));
            }
            positionSession.Commit(() => { });
        }

        var groups = check.GroupBy(row => row.Group).Select(group => new
        {
            Group = group.Key, Expected = group.Count(),
            Matches = group.Count(row => row.Expected == row.First && row.First == row.Second)
        });
        var discrepancies = check.Where(row => row.Expected != row.First || row.First != row.Second).ToArray();
        var positionGroups = positionChecks.GroupBy(item => item.Position).Select(group => new
        {
            Position = group.Key, Expected = group.Count(),
            Matches = group.Count(item => item.Status == SpecialFieldStatus.Match)
        });
        using var self = Process.GetCurrentProcess();
        var resultBytes = BatchExchange.Encode(new
        {
            Task = "T021", Context = session.Expected, ProcessStartUtcTicks = startTicks, Loads = loads,
            Players = roster.Count, Observations = check.Length, Groups = groups, Discrepancies = discrepancies,
            PositionReference = positions is null ? null : EditorPositions, PositionScale = positions is null ? null : "0–20",
            PositionPlayers = positions?.Count ?? 0, PositionChecks = positionChecks.Count, PositionGroups = positionGroups,
            PositionsMatched = positionChecks.Count(item => item.Status == SpecialFieldStatus.Match),
            BytesRead = reader.Bytes, ElapsedMilliseconds = reader.Elapsed.TotalMilliseconds,
            PeakWorkingSetBytes = self.PeakWorkingSet64
        }, PriorityLimits.MaxBytes);
        session.Commit(() => Console.WriteLine(Encoding.UTF8.GetString(resultBytes)));
        return discrepancies.Length == 0 && positionChecks.All(item => item.Status == SpecialFieldStatus.Match) ? 0 : 1;
    }
}
