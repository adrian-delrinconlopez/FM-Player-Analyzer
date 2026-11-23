using System.Buffers.Binary;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32.SafeHandles;

namespace FM27Analyzer.Extraction.Poc.Capture;

// Viabilidad T016. Únicamente el Pase visible del titular de la referencia fijada.
[SupportedOSPlatform("windows")]
internal static class ExternalFieldProbe
{
    private const string Build = "26620374";
    private const string CopyName = "SK-01 Valencia 2026-08-01";
    private const string ClubName = "Valencia CF";
    private const uint ClubId = 1775;
    private const uint PlayerUid = 67178177;
    private const uint ProcessQueryLimitedInformation = 0x1000;
    private const uint ProcessVmRead = 0x0010;
    private const int ImageSize = 0x18878000;
    private const long DateSlot = 0x58268E0;
    private const long UiSlot = 0x587C470;
    private const long ClubsSlot = 0x587BA70;
    private static readonly DateOnly Date = new(2026, 8, 1);
    private static readonly CapturePolicy Policy = new(1);
    private static readonly FieldKey Passing = new("passing", null);
    private static readonly PlayerKey Player = new("FM27", "67178177");
    private static readonly ObservationState ExactVisible = new(Knowledge.Exact, Availability.Observed, Disclosure.Visible);
    private static readonly CaptureLimits Limits = new(96 * 1024, 1, 2, TimeSpan.FromSeconds(2));

    internal static int Run(int pid, long startTicks, string copyPath)
    {
        try
        {
            var reference = new BatchExchange("viabilidad", Limits).ReadReference("referencia.json");
            if (reference.Context is not { IsVerified: true, Build: Build, Distribution: "Steam", CopyReference: CopyName,
                    ManagedClub: ClubName, GameDate: var date, Policy: var policy }
                || date != Date || policy != Policy || reference.FixedUtc == default
                || reference.FixedUtc.Offset != TimeSpan.Zero
                || reference.FixedUtc >= DateTimeOffset.UtcNow || string.IsNullOrWhiteSpace(reference.IndependentSource)
                || reference.Expected is not [{ Key: var key, CareerOrConfirmedCopy: CopyName,
                    Club: { Status: ClubStatus.ConfirmedClub, ClubReference: ClubName } }] || key != Player
                || reference.Observations is not [{ Player: var observedPlayer, Field: var field, Type: FieldType.Numeric,
                    Unit: "1–20", State: var state, Value: { Low: >= 1 and <= 20 and var visiblePase, High: null, Category: null } }]
                || observedPlayer != Player || field != Passing || state != ExactVisible)
                throw new InvalidDataException("Falta la referencia independiente del Pase exacto visible en el primer equipo.");
            if (visiblePase != decimal.Truncate(visiblePase))
                throw new InvalidDataException("La referencia visible de Pase debe ser un entero.");

            var copy = new FileInfo(copyPath);
            if (copy.Name != CopyName + ".fm" || !copy.Exists || copy.Attributes.HasFlag(FileAttributes.ReparsePoint))
                throw new InvalidDataException("No se ha identificado la copia de laboratorio.");

            var saveMetadata = (copy.Length, copy.LastWriteTimeUtc);
            using var game = Process.GetProcessById(pid);
            if (game.ProcessName != "fm" || game.StartTime.ToUniversalTime().Ticks != startTicks)
                throw new InvalidDataException("Instancia distinta de la confirmada.");

            using var handle = OpenProcess(ProcessQueryLimitedInformation | ProcessVmRead, false, pid);
            if (handle.IsInvalid) throw new Win32Exception(Marshal.GetLastWin32Error(), "Acceso de consulta y lectura denegado.");
            long confirmedCreation = new DateTime(startTicks, DateTimeKind.Utc).ToFileTimeUtc();
            bool SameProcess() => GetProcessTimes(handle, out long created, out long exited, out _, out _)
                && created == confirmedCreation && exited == 0;
            if (!SameProcess()) throw new InvalidDataException("No se pudo confirmar la instancia de lectura.");

            var module = FindModule(pid);
            var reader = new Reader(handle);
            long image = module.BaseAddress.ToInt64();
            VerifyBuild(reader, image, module.Size);
            long ui = reader.Pointer(image + UiSlot);
            ulong loads = reader.U64(ui + 0x100);
            if (loads is 0 or > 10_000) throw new InvalidDataException("Contador de cargas incierto.");

            var context = reference.Context with { Instance = Guid.NewGuid(), Session = Guid.NewGuid(), Cut = "2026-08-01T21:45:00" };
            long club = 0, team = 0, player = 0;
            CaptureContext? Current()
            {
                copy.Refresh();
                if (!copy.Exists || (copy.Length, copy.LastWriteTimeUtc) != saveMetadata
                    || !SameProcess() || reader.Pointer(image + UiSlot) != ui || reader.U64(ui + 0x100) != loads)
                    return null;

                uint raw = reader.U32(image + DateSlot);
                if ((raw >> 16) != Date.Year || (raw & 0x1FF) != Date.DayOfYear
                    || player != 0 && (reader.U32(player + 0x294) != PlayerUid || reader.Pointer(player + 0x130) != team
                        || reader.Pointer(team + 0x30) != club || reader.U32(club + 0x0C) != ClubId))
                    return null;
                return context;
            }

            using var session = new CaptureSession(context, Current, _ => { });
            club = session.Read(() => FindClub(reader, image));
            team = session.Read(() => FindTeam(reader, club));
            player = session.Read(() => FindPlayer(reader, team));
            var access = new CaptureAuthorization(context,
                candidate => candidate == Player ? IdentityScope.Squad : IdentityScope.Unverified,
                candidate => candidate == Player ? reference.Expected[0] : null,
                candidate => candidate == Passing,
                (_, _) => new FieldPermission(FieldClass.Visible, FieldType.Numeric, "1–20", ExactVisible,
                    "Memoria externa de FM27, perfil Steam 26620374", reference.IndependentSource, null,
                    ScaleMinimum: 1, ScaleMaximum: 20));

            PlayerObservation ReadPassing() => access.Project(session, Player, Passing, knowledge =>
            {
                if (knowledge != Knowledge.Exact) throw new InvalidDataException("Precisión sin autorizar.");
                byte raw = reader.U8(player + 0x166);
                if (raw > 100) throw new InvalidDataException("Pase fuera de la escala de origen.");
                return new FieldValue(Math.Clamp((raw + 2) / 5, 1, 20));
            }) ?? throw new InvalidDataException("Campo fuera de la política base.");

            var first = ReadPassing();
            var second = ReadPassing();
            bool matched = first.Value == reference.Observations[0].Value && first == second;
            using var self = Process.GetCurrentProcess();
            var report = BatchExchange.Encode(new
            {
                Task = "T016", Outcome = matched ? "contrastado" : "discrepancia",
                Context = context, CapturedUtc = DateTimeOffset.UtcNow, ProcessStartUtcTicks = startTicks, Loads = loads,
                Reference = reference.Observations[0], Reads = new[] { first, second },
                BytesRead = reader.Bytes, ElapsedMilliseconds = reader.Elapsed.TotalMilliseconds,
                PeakWorkingSetBytes = self.PeakWorkingSet64, Limits
            }, Limits.MaxBytes);
            session.Commit(() => Console.WriteLine(System.Text.Encoding.UTF8.GetString(report)));
            return matched ? 0 : 1;
        }
        catch (Exception ex) when (ex is IOException or InvalidDataException or InvalidOperationException
            or ArgumentException or UnauthorizedAccessException or Win32Exception or OperationCanceledException)
        {
            Console.Error.WriteLine($"Ensayo externo bloqueado. {ex.Message}");
            return 2;
        }
    }

    private static void VerifyBuild(Reader reader, long image, uint moduleSize)
    {
        uint pe = reader.U32(image + 0x3C);
        if (moduleSize != ImageSize || pe is < 0x40 or > 0x1000 || reader.U32(image + pe) != 0x4550
            || reader.U32(image + pe + 8) != 0x6AF5C68F || reader.U32(image + pe + 80) != ImageSize)
            throw new InvalidDataException("Huella de game_plugin.dll incompatible.");
    }

    private static long FindClub(Reader reader, long image)
    {
        foreach (long club in reader.Pointers(image + ClubsSlot, 65_536, allowNull: true))
            if (club != 0 && reader.U32(club + 0x0C) == ClubId) return club;
        throw new InvalidDataException("Club ausente del índice de la build.");
    }

    private static long FindTeam(Reader reader, long club)
    {
        foreach (long team in reader.Pointers(club + 0x18, 16))
            if (reader.Pointer(team + 0x30) == club && reader.U8(team + 0x28) == 0) return team;
        throw new InvalidDataException("Primer equipo sin verificar.");
    }

    private static long FindPlayer(Reader reader, long team)
    {
        foreach (long player in reader.Pointers(team + 0x38, 128))
            if (reader.U32(player + 0x294) == PlayerUid && reader.Pointer(player + 0x130) == team) return player;
        throw new InvalidDataException("UID ausente del primer equipo verificado.");
    }

    private sealed class Reader(SafeProcessHandle handle)
    {
        private readonly Stopwatch timer = Stopwatch.StartNew();
        private readonly byte[] scratch = new byte[8];
        public int Bytes { get; private set; }
        public TimeSpan Elapsed => timer.Elapsed;
        public byte U8(long address) { Read(address, 1); return scratch[0]; }
        public uint U32(long address) { Read(address, 4); return BinaryPrimitives.ReadUInt32LittleEndian(scratch); }
        public ulong U64(long address) { Read(address, 8); return BinaryPrimitives.ReadUInt64LittleEndian(scratch); }

        public long Pointer(long address, bool allowNull = false)
        {
            ulong value = U64(address);
            if (value == 0 && allowNull) return 0;
            if (value is < 0x10000 or >= 0x800000000000 || (value & 7) != 0)
                throw new InvalidDataException("Referencia de memoria incompatible con el perfil.");
            return (long)value;
        }

        public IEnumerable<long> Pointers(long address, int maximum, bool allowNull = false)
        {
            long start = Pointer(address);
            long end = Pointer(address + 8);
            if (end <= start || end - start > 8L * maximum)
                throw new InvalidDataException("Colección fuera del presupuesto de la sonda.");
            for (long item = start; item < end; item += 8)
                yield return Pointer(item, allowNull);
        }

        private void Read(long address, int length)
        {
            if (Elapsed > Limits.MaxDuration || Bytes + length > Limits.MaxBytes)
                throw new InvalidDataException("Presupuesto de tiempo o lectura agotado.");
            if (!ReadProcessMemory(handle, (nint)address, scratch, (nuint)length, out nuint read)
                || read != (nuint)length)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Lectura de FM27 incompleta.");
            Bytes += length;
        }
    }

    private static ModuleEntry FindModule(int pid)
    {
        const uint snapshotModules = 0x00000008;
        using var snapshot = CreateToolhelp32Snapshot(snapshotModules, (uint)pid);
        if (snapshot.IsInvalid) throw new Win32Exception(Marshal.GetLastWin32Error(), "No se pudo identificar el módulo.");
        var entry = new ModuleEntry { StructureSize = (uint)Marshal.SizeOf<ModuleEntry>() };
        if (!Module32First(snapshot, ref entry))
            throw new Win32Exception(Marshal.GetLastWin32Error(), "No se pudieron enumerar los módulos.");
        do
        {
            if (string.Equals(entry.Name, "game_plugin.dll", StringComparison.OrdinalIgnoreCase)) return entry;
        } while (Module32Next(snapshot, ref entry));

        throw new InvalidDataException("game_plugin.dll no está cargado.");
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct ModuleEntry
    {
        public uint StructureSize, ModuleId, ProcessId, GlobalUsage, ProcessUsage;
        public nint BaseAddress;
        public uint Size;
        public nint Module;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string Name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string Path;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern SafeProcessHandle OpenProcess(uint access, bool inherit, int pid);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern SafeFileHandle CreateToolhelp32Snapshot(uint flags, uint pid);

    [DllImport("kernel32.dll", EntryPoint = "Module32FirstW", SetLastError = true)]
    private static extern bool Module32First(SafeFileHandle snapshot, ref ModuleEntry entry);

    [DllImport("kernel32.dll", EntryPoint = "Module32NextW", SetLastError = true)]
    private static extern bool Module32Next(SafeFileHandle snapshot, ref ModuleEntry entry);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetProcessTimes(SafeProcessHandle handle, out long created, out long exited, out long kernel, out long user);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadProcessMemory(SafeProcessHandle handle, nint address, [Out] byte[] data, nuint size, out nuint read);
}
