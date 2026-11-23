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

[BepInPlugin("fm27analyzer.sk01.visible-field", "FM27 Analyzer SK-01", "0.1.0")]
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

    public void Update()
    {
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
            Capture(request);
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            // El operador puede renovar la petición local. El juego sigue su curso.
        }
    }

    private static void Capture(ProbeRequest request)
    {
        if (request is not { Confirmed: true, Uid.Length: > 0 and <= 12 }
            || !request.Uid.All(c => c is >= '0' and <= '9')
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
            var capture = session.Read(() => ReadPase(request.Uid, clock));
            var result = new ProbeResult(request.Id, context.Build, cut.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                cut.Loads, request.Uid,
                "passing", capture, clock.Elapsed.TotalMilliseconds, ProcessStarted.Ticks, null);
            Store(result, session.Commit);
        }
        catch (Exception ex) when (ex is InvalidOperationException or InvalidDataException or OperationCanceledException
            or System.ComponentModel.Win32Exception)
        {
            Store(new ProbeResult(request.Id, context.Build, null, null, null, "passing", null, null,
                ProcessStarted.Ticks, ex is InvalidDataException ? ex.Message : "Contexto o tabla no verificable."),
                publish => publish());
        }
        finally
        {
            game.Dispose();
        }
    }

    private static string ReadPase(string uid, Stopwatch clock)
    {
        var matches = new List<string>(1);
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
            int passColumn = Array.IndexOf(names, "Pase");
            if (idColumn < 0 || passColumn < 0)
                throw new InvalidDataException("Cabeceras Unique ID y Pase no disponibles.");

            stage = "filas";
            foreach (var row in Walk(table, IsRow).Where(IsRow))
            {
                var cells = Walk(row, IsCell).Where(IsCell).ToArray();
                if (cells.Length != names.Length || Text(cells[idColumn]) != uid) continue;
                var value = Text(cells[passColumn]);
                if (IsRangeOrExact(value)) matches.Add(value);
            }
            break;
        }
        if (matches.Count != 1)
            throw new InvalidDataException(matches.Count == 0 ? "Pase y UID no visibles en una fila de la tabla." : "UID duplicado en la vista.");
        return matches[0];

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

    private static bool IsRangeOrExact(string raw)
    {
        var parts = raw.Split(new[] { '-', '–' }, StringSplitOptions.TrimEntries);
        if (parts.Length is < 1 or > 2) return false;
        return int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out int low)
            && low is >= 1 and <= 20 && (parts.Length == 1
                || int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int high) && high >= low && high <= 20);
    }

    private static void Store(ProbeResult result, Action<Action> commit)
    {
        string target = Path.Combine(RunFolder, "bepinex-" + result.Id.ToString("N") + ".json");
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
        string Uid, string Copy, string Branch, string Perspective, string Club, string Date, string Cut);

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
