using System.Runtime.Versioning;
using FM27Analyzer.Extraction.Poc.Capture;

namespace FM27Analyzer.Extraction.Poc.Evidence;

[SupportedOSPlatform("windows")]
internal static class A01Commands
{
    private static readonly CaptureLimits Ceiling = new(1024 * 1024, 256, 4096, TimeSpan.FromMinutes(10));
    private static readonly HashSet<string> KnownFields = new(StringComparer.Ordinal)
        { "passing", "best-position", "reflexes", "marking" };

    internal static int Verify(string run, string output) => Execute(() =>
    {
        var exchange = Open(run, out var folder, out var input);
        var outputName = InFolder(output, folder);
        var manifest = exchange.ReadManifest(input);
        if (new[] { input, manifest.Reference, manifest.AdditionalCapabilities, manifest.Isolation }
            .Concat(manifest.Batches.IsDefault ? [] : manifest.Batches)
            .Contains(outputName, StringComparer.OrdinalIgnoreCase))
            throw new InvalidDataException("El informe debe tener un nombre distinto de las entradas.");
        var inputs = manifest.Reference is { } referenceFile
            ? ReadContrast(exchange, exchange.ReadReference(referenceFile), input)
            : exchange.ReadRun(input);
        var report = A01Comparison.Compare(inputs);
        exchange.WriteReport(outputName, report);
        Console.WriteLine($"A-01: {report.A01}.");
    });

    internal static int Seal(string reference, string content, string output) => Execute(() =>
    {
        var exchange = Open(reference, out var folder, out var input);
        var context = exchange.ReadReference(input);
        var captured = exchange.ReadContent(InFolder(content, folder));
        using var session = new CaptureSession(context.Context, () => context.Context, _ => { });
        exchange.Publish(InFolder(output, folder), captured, Access(context, fromReference: false), session);
    });

    internal static int CheckRecovery(string reference, string batch, string retry) => Execute(() =>
    {
        var exchange = Open(reference, out var folder, out var input);
        var observed = exchange.ReadReference(input);
        var firstName = InFolder(batch, folder);
        var retryName = InFolder(retry, folder);
        using var session = new CaptureSession(observed.Context, () => observed.Context, _ => { });
        var access = Access(observed, fromReference: false);
        var recovery = new SampleRecovery(session, Ceiling);
        SampleResult Attempt(string file) => recovery.Attempt("atributos visibles de A-01", access,
            () => exchange.ReadBatchEnvelope(file, Ceiling, diagnoseTruncation: true));
        var accepted = Attempt(firstName);
        if (accepted.Status != SampleStatus.Accepted)
            throw new InvalidDataException("Falta un lote inicial válido para comprobar la recuperación.");
        var repeated = Attempt(firstName);
        var retried = Attempt(retryName);
        var result = new
        {
            accepted.Capability,
            Accepted = accepted.Status,
            Repeated = repeated.Status,
            Retried = retried.Status,
            retried.Failure,
            OriginalCapturedUtc = accepted.Sample!.Content.CapturedUtc,
            ReturnedCapturedUtc = retried.Sample?.Content.CapturedUtc,
            SameClosure = accepted.Sample.Closure == retried.Sample?.Closure
        };
        session.Commit(() => Console.WriteLine(System.Text.Encoding.UTF8.GetString(BatchExchange.Encode(result, Ceiling.MaxBytes))));
    });

    // El cotejo local acepta evidencia histórica del corte fijado. Su contenido no se usa como sesión del juego.
    private static RunInputs ReadContrast(BatchExchange exchange, ComparisonReference reference, string input)
    {
        using var session = new CaptureSession(reference.Context, () => reference.Context, _ => { });
        return exchange.ReadRun(input, session, Access(reference, fromReference: false), Access(reference, fromReference: true));
    }

    private static CaptureAuthorization Access(ComparisonReference reference, bool fromReference)
    {
        var context = reference.Context;
        if (context is not { IsVerified: true } || context.Policy != new CapturePolicy(context.Policy.Revision)
            || reference.Expected.IsDefaultOrEmpty || reference.Observations.IsDefaultOrEmpty)
            throw new InvalidDataException("La referencia A-01 requiere contexto, muestra y permisos base.");
        var players = reference.Expected.ToDictionary(player => player.Key);
        var fields = reference.Observations.Where(row => KnownFields.Contains(row.Field.Name))
            .ToDictionary(row => (row.Player, row.Field));
        return new(context,
            key => players.ContainsKey(key) ? IdentityScope.Scouted : IdentityScope.Unverified,
            key => players.GetValueOrDefault(key), field => KnownFields.Contains(field.Name),
            (key, field) => fields.TryGetValue((key, field), out var row)
                ? new FieldPermission(FieldClass.Visible, row.Type, row.Unit, row.State,
                    fromReference ? row.DataSource : A01Comparison.Route,
                    fromReference ? row.VisibilitySource : A01Comparison.Visibility,
                    row.CatalogRevision, ScaleMinimum: 1, ScaleMaximum: 20)
                : null);
    }

    private static BatchExchange Open(string file, out string folder, out string name)
    {
        string full = Path.GetFullPath(file);
        name = Path.GetFileName(full);
        folder = Path.GetDirectoryName(full)!;
        string? parent = Path.GetDirectoryName(folder);
        string root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FM27Analyzer", "SK-01");
        if (!string.Equals(parent, root, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("La entrada debe estar en un directorio privado de SK-01.");
        return new BatchExchange(Path.GetFileName(folder), Ceiling);
    }

    private static string InFolder(string file, string folder)
    {
        var full = Path.GetFullPath(file);
        if (!string.Equals(Path.GetDirectoryName(full), folder, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("Los artefactos deben estar en el mismo directorio del ensayo.");
        return Path.GetFileName(full);
    }

    private static int Execute(Action operation)
    {
        try { operation(); return 0; }
        catch (Exception ex) when (ex is InvalidDataException or IOException or UnauthorizedAccessException
            or ArgumentException or InvalidOperationException or OperationCanceledException)
        {
            Console.Error.WriteLine("Entrada de A-01 inválida o inaccesible.");
            return 2;
        }
    }
}
