using System.Collections.Immutable;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json;
using System.Text.Json.Serialization;
using FM27Analyzer.Extraction.Poc.Evidence;

namespace FM27Analyzer.Extraction.Poc.Capture;

public enum A01Mode { Contrastable, Bloqueada }
public sealed record RunManifest(SchemaVersion Schema, Guid Id, A01Mode Mode, CaptureContext? Context,
    CaptureLimits? Limits, string Extent, string? Reason, ImmutableArray<string> Batches,
    string? Reference, string? AdditionalCapabilities, string? Isolation,
    ImmutableArray<ScenarioResult> Execution) : ExchangeDocument;
public sealed record RunInputs(RunManifest Manifest, ImmutableArray<ObservationBatch> Batches,
    ComparisonReference? Reference, ImmutableArray<ScenarioResult> Additional);

// Extensión opcional del sobre. No participa en la proyección ni en la huella.
public abstract record ExchangeDocument
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonElement? Metadata
    {
        get => null;
        init
        {
            if (value is { ValueKind: not JsonValueKind.Object }) throw new JsonException();
        }
    }
}

public sealed class BatchExchange
{
    private static readonly JsonSerializerOptions Json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        RespectRequiredConstructorParameters = true,
        RespectNullableAnnotations = true,
        AllowDuplicateProperties = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 24,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false) }
    };

    private readonly DirectoryInfo directory;
    private readonly CaptureLimits ceiling;

    [SupportedOSPlatform("windows")]
    public BatchExchange(string runDirectory, CaptureLimits ceiling)
    {
        CheckName(runDirectory);
        if (!ceiling.IsValid) throw new ArgumentOutOfRangeException(nameof(ceiling));
        this.ceiling = ceiling;
        var root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        directory = new DirectoryInfo(Path.Combine(root, "FM27Analyzer", "SK-01", runDirectory));
        for (var part = directory; part != null; part = part.Parent)
            if (part.Exists && part.Attributes.HasFlag(FileAttributes.ReparsePoint))
                throw new InvalidDataException("El directorio de ensayo contiene una redirección.");

        using var identity = WindowsIdentity.GetCurrent();
        var owner = identity.User!;
        var security = new DirectorySecurity();
        security.SetAccessRuleProtection(true, false);
        security.AddAccessRule(new FileSystemAccessRule(owner, FileSystemRights.FullControl,
            InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
            PropagationFlags.None, AccessControlType.Allow));
        FileSystemAclExtensions.Create(directory, security);
        var actual = directory.GetAccessControl();
        if (!owner.Equals(actual.GetOwner(typeof(SecurityIdentifier))) ||
            actual.GetAccessRules(true, true, typeof(SecurityIdentifier)).Cast<FileSystemAccessRule>()
                .Any(rule => rule.AccessControlType == AccessControlType.Allow && !owner.Equals(rule.IdentityReference)
                    && !((SecurityIdentifier)rule.IdentityReference).IsWellKnown(WellKnownSidType.LocalSystemSid)
                    && !((SecurityIdentifier)rule.IdentityReference).IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid)))
            throw new InvalidDataException("El directorio de ensayo requiere acceso privado del usuario.");
    }

    public void Publish(string file, BatchContent content, CaptureAuthorization access, CaptureSession session)
    {
        var destination = LocalPath(file);
        access.CheckSession(session);
        var batch = session.Read(() => BatchValidation.Seal(content, access, ceiling));
        var bytes = Encode(batch, content.Limits.MaxBytes);
        var temp = Path.Combine(directory.FullName, Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            session.Commit(() =>
            {
                using var output = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                output.Write(bytes);
                output.Flush(flushToDisk: true);
            });
            session.Commit(() => File.Move(temp, destination));
        }
        finally { File.Delete(temp); }
    }

    public ObservationBatch ReadBatch(string file, CaptureAuthorization access, CaptureSession session)
    {
        access.CheckSession(session);
        return session.Read(() => LoadBatch(file, access, ceiling));
    }

    internal ComparisonReference ReadReference(string file) => ReadReference(file, ceiling.MaxBytes);
    internal RunManifest ReadManifest(string file) => Decode<RunManifest>(ReadBytes(file, ceiling.MaxBytes));
    internal BatchContent ReadContent(string file) => Decode<BatchContent>(ReadBytes(file, ceiling.MaxBytes));

    internal void WriteReport(string file, EvidenceReport report)
    {
        string target = LocalPath(file);
        string temp = Path.Combine(directory.FullName, Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            using (var stream = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                stream.Write(Encode(report, ceiling.MaxBytes));
                stream.Flush(flushToDisk: true);
            }
            File.Move(temp, target, overwrite: true);
        }
        finally { File.Delete(temp); }
    }

    private ComparisonReference ReadReference(string file, int maxBytes) =>
        Decode<ComparisonReference>(ReadBytes(file, maxBytes));

    private ObservationBatch LoadBatch(string file, CaptureAuthorization access, CaptureLimits limits)
    {
        var bytes = ReadBytes(file, limits.MaxBytes);
        var batch = Decode<ObservationBatch>(bytes);
        if (batch.Content?.Limits is null || bytes.Length > batch.Content.Limits.MaxBytes)
            throw new InvalidDataException("Lote fuera de su presupuesto declarado.");
        return BatchValidation.Accept(batch, access, limits);
    }

    public RunInputs ReadRun(string file, CaptureSession? session = null, CaptureAuthorization? access = null,
        CaptureAuthorization? referenceAccess = null)
    {
        var manifest = ReadManifest(file);
        BatchValidation.CheckSchema(manifest.Schema);
        if (manifest.Id == Guid.Empty || string.IsNullOrWhiteSpace(manifest.Extent)
            || manifest.Batches.IsDefault || manifest.Execution.IsDefault
            || manifest.Execution.Any(r => !EvidenceReport.Valid(r) || r.Scenario > Scenario.E03)
            || manifest.Limits is not null && !manifest.Limits.Fits(ceiling))
            throw new InvalidDataException("Manifiesto incompleto o presupuestos incompatibles.");
        bool contrastable = manifest.Mode == A01Mode.Contrastable;
        if (contrastable && (manifest.Batches.Length != 2 || manifest.Reference is null)
            || !contrastable && (manifest.Batches.Length > 1 || string.IsNullOrWhiteSpace(manifest.Reason)
                || !manifest.Execution.Any(r => r.Outcome is Outcome.Omitted or Outcome.Failed)))
            throw new InvalidDataException("Entradas inválidas para la modalidad de A-01.");

        var names = manifest.Batches.Cast<string?>().Append(manifest.Reference)
            .Append(manifest.AdditionalCapabilities).Append(manifest.Isolation).Append(file)
            .Where(n => n is not null).ToArray();
        if (names.Distinct(StringComparer.OrdinalIgnoreCase).Count() != names.Length)
            throw new InvalidDataException("Referencias de artefacto repetidas.");

        ImmutableArray<ObservationBatch> batches = [];
        ComparisonReference? reference = null;
        if (!manifest.Batches.IsEmpty || manifest.Reference is not null)
        {
            if (session is null || access is null || manifest.Context != access.Context || manifest.Limits is null)
                throw new InvalidDataException("Se requiere un contexto verificado y límites para la muestra.");
            access.CheckSession(session);
            batches = session.Read(() => manifest.Batches.Select(n => LoadBatch(n, access, manifest.Limits)).ToImmutableArray());
            if (manifest.Reference is not null)
            {
                reference = session.Read(() => ReadReference(manifest.Reference, manifest.Limits.MaxBytes));
                if (referenceAccess is null || referenceAccess.Context != reference.Context || reference.Context != manifest.Context
                    || reference.FixedUtc == default || reference.FixedUtc.Offset != TimeSpan.Zero
                    || string.IsNullOrWhiteSpace(reference.IndependentSource) || reference.Expected.IsDefaultOrEmpty
                    || batches.Any(b => b.Content.CapturedUtc <= reference.FixedUtc || b.Content.Route == reference.IndependentSource))
                    throw new InvalidDataException("Referencia independiente, contexto o fecha inválidos.");
                reference = reference with { Observations = BatchValidation.ValidateRows(reference.Expected,
                    reference.Observations, referenceAccess, manifest.Limits, skipUnknown: true) };
            }
            if (contrastable && batches[0].Content.Id == batches[1].Content.Id)
                throw new InvalidDataException("Las capturas requieren dos identificadores distintos.");
        }

        var additional = ImmutableArray.CreateBuilder<ScenarioResult>();
        ReadAdditional(manifest.AdditionalCapabilities, Scenario.E04, Scenario.E06, additional);
        ReadAdditional(manifest.Isolation, Scenario.E07, Scenario.E09, additional);
        var result = new RunInputs(manifest, batches, reference, additional.ToImmutable());
        return session is null ? result : session.Read(() => result);
    }

    private void ReadAdditional(string? file, Scenario first, Scenario last, ImmutableArray<ScenarioResult>.Builder output)
    {
        if (file is null) return;
        var results = Decode<ImmutableArray<ScenarioResult>>(ReadBytes(file, ceiling.MaxBytes));
        foreach (var result in results)
        {
            if (!EvidenceReport.Valid(result) || result.Scenario < first || result.Scenario > last
                || output.Any(r => r.Scenario == result.Scenario && r.Class == result.Class))
                throw new InvalidDataException("Resultado adicional fuera de escenario o repetido.");
            output.Add(result);
        }
    }

    private byte[] ReadBytes(string file, int maxBytes)
    {
        try
        {
            using var stream = new FileStream(LocalPath(file), FileMode.Open, FileAccess.Read, FileShare.Read);
            if (stream.Length <= 0 || stream.Length > maxBytes) throw new InvalidDataException("Tamaño de archivo inválido.");
            var bytes = new byte[(int)stream.Length];
            stream.ReadExactly(bytes);
            return bytes;
        }
        catch (IOException ex) when (ex is FileNotFoundException or DirectoryNotFoundException or EndOfStreamException)
        {
            throw new InvalidDataException("Artefacto referenciado ausente o truncado.");
        }
    }

    private string LocalPath(string file)
    {
        CheckName(file);
        if (!file.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("Se requiere un nombre de archivo JSON local.");
        var path = Path.Combine(directory.FullName, file);
        if (File.Exists(path) && File.GetAttributes(path).HasFlag(FileAttributes.ReparsePoint))
            throw new InvalidDataException("Referencia a un archivo redirigido.");
        return path;
    }

    private static void CheckName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 120 || name is "." or ".."
            || !name.All(c => char.IsAsciiLetterOrDigit(c) || c is '.' or '-' or '_')
            || name.EndsWith('.') || Path.GetFileName(name) != name)
            throw new InvalidDataException("Nombre de ensayo o referencia local inválidos.");
        var stem = name.Split('.')[0].ToUpperInvariant();
        if (stem is "" or "CON" or "PRN" or "AUX" or "NUL"
            || (stem.StartsWith("COM", StringComparison.Ordinal) || stem.StartsWith("LPT", StringComparison.Ordinal))
                && stem.Length == 4 && char.IsAsciiDigit(stem[3]))
            throw new InvalidDataException("Nombre reservado por Windows.");
    }

    internal static byte[] Encode<T>(T value, int maxBytes)
    {
        using var buffer = new LimitedBuffer(maxBytes);
        JsonSerializer.Serialize(buffer, value, Json);
        return buffer.ToArray();
    }

    private static T Decode<T>(byte[] bytes)
    {
        try { return JsonSerializer.Deserialize<T>(bytes, Json) ?? throw new JsonException(); }
        catch (JsonException) { throw new InvalidDataException("Documento JSON incompleto o incompatible."); }
    }

    private sealed class LimitedBuffer(int limit) : MemoryStream
    {
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (Length + count > limit) throw new InvalidDataException("Tamaño JSON superior al presupuesto.");
            base.Write(buffer, offset, count);
        }
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            if (Length + buffer.Length > limit) throw new InvalidDataException("Tamaño JSON superior al presupuesto.");
            base.Write(buffer);
        }
    }
}
