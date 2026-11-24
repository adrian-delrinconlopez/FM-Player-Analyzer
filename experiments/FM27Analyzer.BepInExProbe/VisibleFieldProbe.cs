using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using FM27Analyzer.Extraction.Poc.Capture;
using UnityEngine;
using UnityEngine.UIElements;

namespace FM27Analyzer.BepInExProbe;

[BepInPlugin("fm27analyzer.sk01.visible-field", "FM27 Analyzer SK-01", "0.3.0")]
[BepInProcess("fm.exe")]
public sealed class VisibleFieldProbe : BasePlugin
{
    public override void Load() => AddComponent<ProbeComponent>();
}

public sealed class ProbeComponent(IntPtr pointer) : MonoBehaviour(pointer)
{
    private static readonly string RunFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FM27Analyzer", "SK-01", "viabilidad");
    private static readonly string RequestPath = Path.Combine(RunFolder, "bepinex-request.json");
    private static readonly DateTime ProcessStarted = Process.GetCurrentProcess().StartTime.ToUniversalTime();
    private static readonly JsonSerializerOptions Json = new() { PropertyNameCaseInsensitive = true };
    private Guid lastRequest;
    private long nextPoll;
    private ScoutedCensus? census;

    public void Update()
    {
        if (census is not null)
        {
            if (census.Tick()) census = null;
            return;
        }
        long now = Environment.TickCount64;
        if (now < nextPoll) return;
        nextPoll = now + 500;
        if (!File.Exists(RequestPath)) return;

        try
        {
            using var input = new FileStream(RequestPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (input.Length is <= 0 or > 4096) return;
            var request = JsonSerializer.Deserialize<ProbeRequest>(input, Json);
            if (request is null || request.Id == Guid.Empty || request.Id == lastRequest) return;
            lastRequest = request.Id;
            if (request.ScoutedCount is int count)
                StartCensus(request, count);
            else
                Capture(request);
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            // El operador puede renovar la petición local. El juego sigue su curso.
        }
    }

    private void StartCensus(ProbeRequest request, int count)
    {
        if (!request.Confirmed || request.View != "ojeados" || request.Targets is not null || request.Uid is not null
            || request.ProcessStartUtcTicks != ProcessStarted.Ticks || count is < 1 or > 500
            || request.ExpiresUtc <= DateTimeOffset.UtcNow || request.ExpiresUtc > DateTimeOffset.UtcNow.AddMinutes(2)
            || !DateOnly.TryParseExact(request.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return;
        var context = new CaptureContext("FM27", "26620374", "Steam", "es", request.Id, request.Id,
            request.Copy, null, true, request.Branch, request.Perspective, request.Club, date, request.Cut, new CapturePolicy(1));
        try
        {
            using var game = Process.GetCurrentProcess();
            long module = GameCut.FindModule(game);
            var cut = GameCut.Read(module);
            census = new ScoutedCensus(count, context, () =>
                File.Exists(RequestPath) && DateTimeOffset.UtcNow < request.ExpiresUtc
                && cut.Loads > 0 && cut.Date == date && GameCut.Read(module) == cut ? context : null,
                ProcessStarted.Ticks, cut.Loads,
                (result, commit) => Store(request.Id, result, commit));
        }
        catch (Exception ex) when (ex is InvalidOperationException or InvalidDataException or System.ComponentModel.Win32Exception)
        {
            Store(request.Id, new { Reason = "No se pudo vincular el censo al corte." }, publish => publish());
        }
    }

    private static void Capture(ProbeRequest request)
    {
        if (!request.Confirmed
            || (request.Targets is null ? !ValidUid(request.Uid) : !ValidTargets(request))
            || request.ProcessStartUtcTicks != ProcessStarted.Ticks
            || request.ExpiresUtc <= DateTimeOffset.UtcNow
            || request.ExpiresUtc > DateTimeOffset.UtcNow.AddMinutes(2)
            || !DateOnly.TryParseExact(request.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var expectedDate)) return;

        var context = new CaptureContext("FM27", "26620374", "Steam", "es", request.Id, request.Id,
            request.Copy, null, true, request.Branch, request.Perspective, request.Club,
            expectedDate, request.Cut, new CapturePolicy(1));
        var game = Process.GetCurrentProcess();
        try
        {
            long module = GameCut.FindModule(game);
            var cut = GameCut.Read(module);
            CaptureContext? Current() =>
                DateTimeOffset.UtcNow < request.ExpiresUtc
                && GameCut.Read(module) == cut && cut.Date == expectedDate && cut.Loads > 0
                    ? context : null;

            using var session = new CaptureSession(context, Current, _ => { });
            var clock = Stopwatch.StartNew();
            if (request.Targets is not null)
            {
                var rows = session.Read(() => ReadTable(request.Targets, request.View, clock));
                game.Refresh();
                Store(request.Id, new PageResult(request.Id, context.Build, request.Date, cut.Loads, request.View,
                    rows, clock.Elapsed.TotalMilliseconds, game.PeakWorkingSet64, ProcessStarted.Ticks), session.Commit);
                return;
            }
            var target = new PageTarget(request.Uid!, new() { ["passing"] = "visible" });
            var capture = session.Read(() => ReadTable([target], null, clock).SingleOrDefault()?.Fields.GetValueOrDefault("passing"))
                ?? throw new InvalidDataException("Pase y UID no visibles en una fila de la tabla.");
            var result = new ProbeResult(request.Id, context.Build, cut.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                cut.Loads, request.Uid,
                "passing", capture, clock.Elapsed.TotalMilliseconds, ProcessStarted.Ticks, null);
            Store(request.Id, result, session.Commit);
        }
        catch (Exception ex) when (ex is InvalidOperationException or InvalidDataException or OperationCanceledException
            or System.ComponentModel.Win32Exception)
        {
            Store(request.Id, new ProbeResult(request.Id, context.Build, null, null, null, "passing", null, null,
                ProcessStarted.Ticks, ex is InvalidDataException ? ex.Message : "Contexto o tabla no verificable."),
                publish => publish());
        }
        finally
        {
            game.Dispose();
        }
    }

    private static readonly Dictionary<string, string> Columns = new()
    {
        ["passing"] = "Pase", ["marking"] = "Marcaje", ["reflexes"] = "Reflejos",
        ["best-position"] = "Mejor pos.", ["position"] = "Posición", ["club"] = "Club"
    };

    internal static bool ValidUid(string? uid) => uid is { Length: > 0 and <= 12 } && uid.All(c => c is >= '0' and <= '9');

    private static bool ValidTargets(ProbeRequest request) =>
        request.View is "plantilla" or "ojeados" && request.Targets is { Length: > 0 and <= 30 } targets
        && targets.All(t => t is not null && ValidUid(t.Uid) && t.Fields is { Count: > 0 and <= 6 }
            && t.Fields.All(f => Columns.ContainsKey(f.Key) && f.Value is "exact" or "inclusiveRange" or "unknown"))
        && targets.Select(t => t.Uid).Distinct(StringComparer.Ordinal).Count() == targets.Length;

    private static PageRow[] ReadTable(PageTarget[] targets, string? view, Stopwatch clock)
    {
        var requested = targets.ToDictionary(t => t.Uid, StringComparer.Ordinal);
        var matches = new List<PageRow>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        int remaining = 12_000;
        string stage = "paneles";
        var panels = UIElementsRuntimeUtility.GetSortedPlayerPanels();
        for (int index = panels.Count - 1; index >= 0 && index >= panels.Count - 32; index--)
        {
            var root = panels[index].visualTree;
            var table = Walk(root).FirstOrDefault(e => e.name == "playertable" && e.worldBound.width > 0);
            if (table is null) continue;
            stage = "cabeceras";
            var headings = Walk(table, IsRow).FirstOrDefault(e => e.name == "column-headers");
            if (headings is null) throw new InvalidDataException("Cabeceras de la tabla no disponibles.");
            string[] names = Enumerable.Range(0, headings.childCount).Select(i =>
                Walk(headings.ElementAt(i)).FirstOrDefault(e => e.ClassListContains("table-header-xs"))
                    ?.TryCast<TextElement>()?.text?.Trim() ?? "").ToArray();
            int idColumn = Array.IndexOf(names, "Unique ID");
            if (idColumn < 0 || view == "plantilla" && !names.Contains("Mejor pos.")
                || view == "ojeados" && (!names.Contains("Posición") || !names.Contains("Club")))
                throw new InvalidDataException("La tabla activa no coincide con la vista de contraste.");

            stage = "filas";
            foreach (var row in Walk(table, IsRow).Where(IsRow))
            {
                var cells = Walk(row, IsCell).Where(IsCell).ToArray();
                if (cells.Length != names.Length) continue;
                var uid = Text(cells[idColumn]);
                if (!requested.TryGetValue(uid, out var target)) continue;
                if (!seen.Add(uid)) throw new InvalidDataException("UID de la muestra duplicado en la vista.");
                var fields = new Dictionary<string, string>(StringComparer.Ordinal);
                foreach (var (field, knowledge) in target.Fields)
                {
                    int column = Array.IndexOf(names, Columns[field]);
                    if (column < 0 || Array.LastIndexOf(names, Columns[field]) != column) continue;
                    var value = Text(cells[column]);
                    bool categorical = field is "club" or "position" or "best-position";
                    if (categorical ? knowledge == "exact" && value is { Length: > 0 and <= 100 } && !IsUnknown(value)
                        : MatchesPrecision(value, knowledge))
                        fields.Add(field, value);
                }
                matches.Add(new(uid, fields));
            }
            break;
        }
        return matches.ToArray();

        string Text(VisualElement element) => string.Join(" ", Walk(element)
            .Select(e => e.TryCast<TextElement>()?.text?.Trim())
            .Where(text => !string.IsNullOrEmpty(text)));

        static bool IsRow(VisualElement element) => element.ClassListContains("streamed-table__row");
        static bool IsCell(VisualElement element) => element.ClassListContains("streamed-table__cell");

        IEnumerable<VisualElement> Walk(VisualElement? root, Func<VisualElement, bool>? leaf = null)
        {
            if (root is null) yield break;
            var pending = new Stack<VisualElement>();
            pending.Push(root);
            while (pending.TryPop(out var element))
            {
                if (--remaining <= 0 || clock.ElapsedMilliseconds > 250)
                    throw new InvalidDataException($"Límite de recorrido visual ({stage}).");
                if (!element.visible || element.resolvedStyle.display == DisplayStyle.None) continue;
                yield return element;
                if (leaf?.Invoke(element) == true) continue;
                for (int child = element.childCount - 1; child >= 0; child--)
                    pending.Push(element.ElementAt(child));
            }
        }
    }

    internal static bool IsUnknown(string raw) => raw is "-" or "–" or "—" or "Se necesita ojeo";

    internal static bool MatchesPrecision(string raw, string knowledge)
    {
        if (knowledge == "unknown") return IsUnknown(raw);
        var parts = raw.Split(new[] { '-', '–' }, StringSplitOptions.TrimEntries);
        if (parts.Length is < 1 or > 2) return false;
        if (knowledge != "visible" && (parts.Length == 1 ? knowledge != "exact" : knowledge != "inclusiveRange"))
            return false;
        return int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out int low)
            && low is >= 1 and <= 20 && (parts.Length == 1
                || int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int high) && high >= low && high <= 20);
    }

    private static void Store<T>(Guid id, T result, Action<Action> commit)
    {
        string target = Path.Combine(RunFolder, "bepinex-" + id.ToString("N") + ".json");
        string temp = target + ".tmp";
        try
        {
            using (var file = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                JsonSerializer.Serialize(file, result);
            commit(() => File.Move(temp, target));
        }
        finally
        {
            if (File.Exists(temp)) File.Delete(temp);
        }
    }

    private sealed record ProbeRequest(Guid Id, long ProcessStartUtcTicks, DateTimeOffset ExpiresUtc, bool Confirmed,
        string? Uid, string Copy, string Branch, string Perspective, string Club, string Date, string Cut,
        string? View, PageTarget[]? Targets, int? ScoutedCount);

    private sealed record PageTarget(string Uid, Dictionary<string, string> Fields);
    private sealed record PageRow(string Uid, Dictionary<string, string> Fields);
    private sealed record PageResult(Guid Id, string Build, string Date, ulong Loads, string? View, PageRow[] Rows,
        double ElapsedMilliseconds, long GamePeakWorkingSetBytes, long ProcessStartUtcTicks);

    private sealed record ProbeResult(Guid Id, string Build, string? Date, ulong? Loads, string? Uid,
        string Field, string? Value, double? ElapsedMilliseconds, long ProcessStartUtcTicks, string? Reason);
}

internal readonly record struct GameCut(DateOnly Date, ulong Loads)
{
    private const int ImageSize = 0x18878000;
    private const int DateOffset = 0x58268E0;
    private const int LoadStateOffset = 0x587C470;

    public static long FindModule(Process process)
    {
        var module = process.Modules.Cast<ProcessModule>().Single(m => m.ModuleName == "game_plugin.dll");
        long image = module.BaseAddress.ToInt64();
        uint pe = Read32(image + 0x3C);
        if (module.ModuleMemorySize != ImageSize || pe is < 0x40 or > 0x1000 || Read32(image + pe) != 0x4550
            || Read32(image + pe + 8) != 0x6AF5C68F || Read32(image + pe + 80) != ImageSize)
            throw new InvalidDataException("Huella de la build incompatible.");
        return image;
    }

    public static GameCut Read(long image)
    {
        uint raw = Read32(image + DateOffset);
        int year = (int)(raw >> 16);
        int day = (int)(raw & 0x1FF);
        if (year is < 2000 or > 2200 || day is < 1 or > 366)
            throw new InvalidDataException("Fecha de juego sin confirmar.");
        var date = new DateOnly(year, 1, 1).AddDays(day - 1);
        ulong state = Read64(image + LoadStateOffset);
        if (date.Year != year || state is < 0x10000 or > 0x7FFFFFFFFFFF)
            throw new InvalidDataException("Estado de carga sin confirmar.");
        return new(date, Read64((long)state + 0x100));
    }

    private static uint Read32(long address) => BitConverter.ToUInt32(ReadExact(address, 4));
    private static ulong Read64(long address) => BitConverter.ToUInt64(ReadExact(address, 8));

    private static byte[] ReadExact(long address, int size)
    {
        var data = new byte[size];
        if (!ReadProcessMemory(GetCurrentProcess(), (nint)address, data, (nuint)size, out nuint read) || read != (nuint)size)
            throw new InvalidDataException("Metadatos de sesión no disponibles.");
        return data;
    }

    [DllImport("kernel32.dll")]
    private static extern nint GetCurrentProcess();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadProcessMemory(nint process, nint address, [Out] byte[] data, nuint size, out nuint read);
}
