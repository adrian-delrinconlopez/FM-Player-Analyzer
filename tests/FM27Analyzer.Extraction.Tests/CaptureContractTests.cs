using System.Collections.Immutable;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using FM27Analyzer.Extraction.Poc.Capture;
using FM27Analyzer.Extraction.Poc.Evidence;

namespace FM27Analyzer.Extraction.Tests;

[TestClass]
[SupportedOSPlatform("windows")]
public sealed class CaptureContractTests
{
    private readonly CaptureContext context = CaptureGuardTests.Context();
    private readonly CaptureLimits budget = new(32_768, 4, 12, TimeSpan.FromSeconds(2));
    private readonly PlayerKey key = new("fixture", "FM:0001");
    private readonly FieldKey field = new("atributo-sintético", null);
    private readonly string folder = "fixture-" + Guid.NewGuid().ToString("N");
    private static readonly FieldPermission Range = new(FieldClass.Visible, FieldType.Numeric, "1–20",
        new(Knowledge.InclusiveRange, Availability.Observed, Disclosure.Visible), "vista-A", "ficha-B", "r1",
        ScaleMinimum: 1, ScaleMaximum: 20);
    private string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "FM27Analyzer", "SK-01", folder);
    private PlayerIdentity Identity => new(key, context.CareerId!, new(ClubStatus.ConfirmedClub, "club-A"));
    private CaptureAuthorization Access(FieldPermission? permission = null, IdentityScope scope = IdentityScope.Scouted)
        => new(context, _ => scope, _ => Identity, f => f == field, (_, _) => permission ?? Range);
    private CaptureSession Guard() => new(context, () => context, Assert.Fail);
    private BatchContent Batch() => new(new(1, 0), Guid.NewGuid(), context, "fixture", "r1",
        DateTimeOffset.UtcNow, SampleExtent.Partial, budget, [Identity],
        [new(key, field, Range.Type, Range.Unit, new(8, 13), Range.State, Range.DataSource, Range.VisibilitySource, "r1")]);
    private static readonly JsonSerializerOptions Json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    [TestCleanup]
    public void RemoveFixture()
    {
        if (Directory.Exists(Root)) Directory.Delete(Root, recursive: true);
    }

    [TestMethod]
    public void ReadsOnlyAuthorizedPrecision()
    {
        using var guard = Guard();
        int calls = 0;
        FieldValue Read(Knowledge precision)
        {
            Assert.AreEqual(Knowledge.InclusiveRange, precision);
            calls++;
            return new(8, 13);
        }
        var row = Access().Project(guard, key, field, Read);
        Assert.AreEqual(new FieldValue(8, 13), row!.Value);
        Assert.IsNull(Access(scope: IdentityScope.LoadedWorld).Project(guard, key, field, Read));
        Assert.IsNull(Access(Range with { Class = FieldClass.CurrentAbility }).Project(guard, key, field, Read));
        var unknown = Range with { State = Range.State with { Knowledge = Knowledge.Unknown } };
        Assert.IsNull(Access(unknown).Project(guard, key, field, Read)!.Value);
        Assert.AreEqual(1, calls);
    }

    [TestMethod]
    [DataRow("jugador duplicado")]
    [DataRow("campo duplicado")]
    [DataRow("identidad")]
    [DataRow("clave ajena")]
    [DataRow("intervalo invertido")]
    [DataRow("exacto bajo máscara")]
    [DataRow("fuera de escala")]
    [DataRow("procedencia")]
    [DataRow("cantidad")]
    [DataRow("contexto")]
    [DataRow("versión")]
    public void RejectsInvalidProjectionBeforeWriting(string change)
    {
        var exchange = new BatchExchange(folder, budget);
        using var guard = Guard();
        var batch = Batch();
        var row = batch.Observations.Single();
        batch = change switch
        {
            "jugador duplicado" => batch with { Players = [Identity, Identity] },
            "campo duplicado" => batch with { Observations = [row, row] },
            "identidad" => batch with { Players = [Identity with { CareerOrConfirmedCopy = "otra" }] },
            "clave ajena" => batch with { Observations = [row with { Player = new("fixture", "otro") }] },
            "intervalo invertido" => batch with { Observations = [row with { Value = new(13, 8) }] },
            "exacto bajo máscara" => batch with { Observations = [row with { Value = new(10) }] },
            "fuera de escala" => batch with { Observations = [row with { Value = new(8, 30) }] },
            "procedencia" => batch with { Observations = [row with { DataSource = "otra" }] },
            "cantidad" => batch with { Limits = budget with { MaxBytes = 100 } },
            "contexto" => batch with { Context = context with { Cut = "otro" } },
            _ => batch with { Schema = new(2, 0) }
        };
        Assert.ThrowsExactly<InvalidDataException>(() => exchange.Publish("lote.json", batch, Access(), guard));
        Assert.IsEmpty(Directory.GetFiles(Root));
    }

    [TestMethod]
    public void RestrictedStateRequiresPermissionEvenForAGenericMarker()
    {
        using var guard = Guard();
        var restricted = Range with { State = Range.State with { Disclosure = Disclosure.Restricted } };
        FieldValue ForbiddenRead(Knowledge _) => throw new AssertFailedException();
        Assert.IsNull(Access(restricted).Project(guard, key, field, ForbiddenRead));
        var permitted = Access(restricted with { GenericRestrictionVisible = true });
        var marker = permitted.Project(guard, key, field, ForbiddenRead);
        Assert.AreEqual(new ObservationState(Knowledge.Unknown, Availability.NotAssessed, Disclosure.Restricted), marker!.State);
        Assert.IsNull(marker.Value);
    }

    [TestMethod]
    public void ExcludedIdentityNeverInvokesTheIdentityOrFieldReader()
    {
        using var guard = Guard();
        var access = new CaptureAuthorization(context, _ => IdentityScope.Unverified,
            _ => throw new AssertFailedException(), _ => throw new AssertFailedException(),
            (_, _) => throw new AssertFailedException());
        Assert.IsNull(access.Project(guard, key, field, _ => throw new AssertFailedException()));
    }

    [TestMethod]
    [DataRow(FieldClass.CurrentAbility, true, true, false, false, true)]
    [DataRow(FieldClass.CurrentAbility, false, true, false, false, false)]
    [DataRow(FieldClass.PotentialAbility, true, true, false, true, false)]
    [DataRow(FieldClass.PotentialAbility, true, false, true, false, true)]
    [DataRow(FieldClass.HiddenExact, true, true, true, false, false)]
    [DataRow(FieldClass.HiddenExact, false, false, false, true, true)]
    public void SpecialPermissionsAreIndependentAndMemoryOnly(FieldClass kind, bool warning, bool ca, bool pa,
        bool hidden, bool allowed)
    {
        var revised = context with { Policy = new CapturePolicy(2, HiddenAuthorized: hidden,
            AbilityWarningAcknowledged: warning, CurrentAbilityAuthorized: ca, PotentialAbilityAuthorized: pa) };
        using var guard = new CaptureSession(revised, () => revised, Assert.Fail);
        var access = new CaptureAuthorization(revised, _ => IdentityScope.Squad, _ => Identity, _ => true,
            (_, _) => Range with { Class = kind });
        Assert.AreEqual(allowed, access.CanCompareInMemory(guard, key, field));
        Assert.IsNull(access.Project(guard, key, field, _ => throw new AssertFailedException()));
    }

    [TestMethod]
    public void CompatibleMinorPreservesKnownFieldsAndOmitsUnknownOnes()
    {
        using var guard = Guard();
        var exchange = new BatchExchange(folder, budget);
        var batch = Batch();
        var extra = batch.Observations[0] with { Field = new("otro-campo", null) };
        batch = batch with { Schema = new(1, 1), Observations = [.. batch.Observations, extra] };
        var producer = new CaptureAuthorization(context, _ => IdentityScope.Scouted, _ => Identity,
            _ => true, (_, _) => Range);
        exchange.Publish("lote.json", batch, producer, guard);
        var path = Path.Combine(Root, "lote.json");
        var envelope = JsonNode.Parse(File.ReadAllBytes(path))!;
        envelope["metadata"] = new JsonObject { ["opcional"] = true };
        File.WriteAllText(path, envelope.ToJsonString());
        var known = exchange.ReadBatch("lote.json", Access(), guard);
        Assert.AreEqual(1, known.Closure.Observations);
        Assert.AreEqual(field, known.Content.Observations.Single().Field);
        envelope["requisitoDesconocido"] = true;
        File.WriteAllText(path, envelope.ToJsonString());
        Assert.ThrowsExactly<InvalidDataException>(() => exchange.ReadBatch("lote.json", Access(), guard));
    }

    [TestMethod]
    [DataRow("ausente")]
    [DataRow("truncado")]
    [DataRow("sobredimensionado")]
    public void InvalidFileCannotSupplyABatch(string kind)
    {
        using var guard = Guard();
        var exchange = new BatchExchange(folder, budget);
        var path = Path.Combine(Root, "lote.json");
        if (kind == "truncado") File.WriteAllText(path, "{");
        if (kind == "sobredimensionado") File.WriteAllBytes(path, new byte[budget.MaxBytes + 1]);
        Assert.ThrowsExactly<InvalidDataException>(() => exchange.ReadBatch("lote.json", Access(), guard));
    }

    [TestMethod]
    public void InvalidatedJustBeforeRenameLeavesNoArtifact()
    {
        var exchange = new BatchExchange(folder, budget);
        using var guard = new CaptureSession(context,
            () => Directory.EnumerateFiles(Root, "*.tmp").Any() ? null : context, _ => { });
        Assert.ThrowsExactly<OperationCanceledException>(() => exchange.Publish("lote.json", Batch(), Access(), guard));
        Assert.IsEmpty(Directory.GetFiles(Root));
    }

    [TestMethod]
    public void RoundTripChecksClosureIdentityAndCurrentAuthorization()
    {
        var exchange = new BatchExchange(folder, budget);
        using var guard = Guard();
        var batch = Batch();
        exchange.Publish("lote.json", batch, Access(), guard);
        var result = exchange.ReadBatch("lote.json", Access(), guard);
        Assert.AreEqual(batch.Observations[0], result.Content.Observations[0]);
        Assert.AreEqual(batch.Players[0], result.Content.Players[0]);
        Assert.ThrowsExactly<InvalidDataException>(() =>
            exchange.ReadBatch("lote.json", Access(scope: IdentityScope.Unverified), guard));
        var path = Path.Combine(Root, "lote.json");
        var doc = JsonNode.Parse(File.ReadAllText(path))!;
        doc["closure"]!["sha256"] = new string('0', 64);
        File.WriteAllText(path, doc.ToJsonString());
        Assert.ThrowsExactly<InvalidDataException>(() => exchange.ReadBatch("lote.json", Access(), guard));
    }

    [TestMethod]
    public void ContrastableManifestRequiresPreexistingIndependentReferenceAndTwoCaptures()
    {
        using var guard = Guard();
        var exchange = new BatchExchange(folder, budget);
        var first = Batch();
        var second = Batch();
        exchange.Publish("uno.json", first, Access(), guard);
        exchange.Publish("dos.json", second, Access(), guard);
        var reference = new ComparisonReference(context, first.CapturedUtc.AddMinutes(-1), "ficha-independiente",
            first.Players, [first.Observations[0] with { DataSource = "ficha-independiente" }]);
        var referenceAccess = Access(Range with { DataSource = "ficha-independiente" });
        void Write(string name, object item) => File.WriteAllText(Path.Combine(Root, name), JsonSerializer.Serialize(item, Json));
        Write("referencia.json", reference);
        Write("run.json", new RunManifest(new(1, 0), Guid.NewGuid(), A01Mode.Contrastable, context, budget, "plantilla",
            null, ["uno.json", "dos.json"], "referencia.json", null, null, []));
        var inputs = exchange.ReadRun("run.json", guard, Access(), referenceAccess);
        Assert.HasCount(2, inputs.Batches);
        Assert.AreEqual(reference.Observations[0], inputs.Reference!.Observations[0]);
        Write("referencia.json", reference with { FixedUtc = second.CapturedUtc.AddSeconds(1) });
        Assert.ThrowsExactly<InvalidDataException>(() => exchange.ReadRun("run.json", guard, Access(), referenceAccess));
    }

    [TestMethod]
    public void BlockedManifestRetainsAdditionalEvidenceAndExplainsOmissions()
    {
        var exchange = new BatchExchange(folder, budget);
        var example = new ScenarioResult(Scenario.E01, EvidenceClass.Documentary, Outcome.Omitted, null, "Sin copia.", []);
        var manifest = new RunManifest(new(1, 0), Guid.NewGuid(), A01Mode.Bloqueada, null, null, "plantilla",
            "Sin copia.", [], null, "capacidades.json", "aislamiento.json", [example]);
        var capabilities = example with { Scenario = Scenario.E04, Class = EvidenceClass.ThirdParty };
        var isolation = example with { Scenario = Scenario.E07, Context = context, Class = EvidenceClass.Synthetic, Outcome = Outcome.Passed };
        void Write(string name, object item) => File.WriteAllText(Path.Combine(Root, name), JsonSerializer.Serialize(item, Json));
        Write("run.json", manifest);
        Write("capacidades.json", new[] { capabilities });
        Write("aislamiento.json", new[] { isolation });
        var inputs = exchange.ReadRun("run.json");
        var report = EvidenceReport.Assemble(inputs, Accreditation.Unaccredited, [], []);
        Assert.HasCount(9, report.Results);
        Assert.AreEqual(manifest.Reason, report.Limitation);
        Assert.AreEqual(Outcome.Passed, report.Results.Single(r => r.Scenario == Scenario.E07).Outcome);
        Assert.AreEqual(EvidenceClass.ThirdParty, report.Results.Single(r => r.Scenario == Scenario.E04).Class);
        Assert.IsFalse(string.IsNullOrEmpty(report.Results.Single(r => r.Scenario == Scenario.E09).Reason));
        Write("run.json", manifest with { AdditionalCapabilities = "../escape.json" });
        Assert.ThrowsExactly<InvalidDataException>(() => exchange.ReadRun("run.json"));
    }
}
