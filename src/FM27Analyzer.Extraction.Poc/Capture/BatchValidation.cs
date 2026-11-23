using System.Collections.Immutable;
using System.Security.Cryptography;

namespace FM27Analyzer.Extraction.Poc.Capture;

public static class BatchValidation
{
    public static ObservationBatch Seal(BatchContent content, CaptureAuthorization access, CaptureLimits budget)
    {
        ValidateHeader(content, access, budget);
        ValidateRows(content.Players, content.Observations, access, content.Limits);
        var closure = Close(content, budget.MaxBytes);
        return new(content, closure);
    }

    internal static ObservationBatch Accept(ObservationBatch batch, CaptureAuthorization access, CaptureLimits budget)
    {
        if (batch?.Content is null || batch.Closure is null)
            throw new InvalidDataException("Falta contenido o cierre.");
        var content = batch.Content;
        ValidateHeader(content, access, budget);
        var projected = ValidateRows(content.Players, content.Observations, access, content.Limits, skipUnknown: true);
        if (batch.Closure != Close(content, budget.MaxBytes))
            throw new InvalidDataException("Recuento o huella de cierre incorrectos.");
        if (projected == content.Observations) return batch;
        var known = content with { Observations = projected };
        return new(known, Close(known, budget.MaxBytes));
    }

    private static BatchClosure Close(BatchContent content, int maxBytes)
    {
        var ordered = content with
        {
            Players = [.. content.Players.OrderBy(p => p.Key.SourceType, StringComparer.Ordinal)
                .ThenBy(p => p.Key.Uid, StringComparer.Ordinal)],
            Observations = [.. content.Observations.OrderBy(o => o.Player.SourceType, StringComparer.Ordinal)
                .ThenBy(o => o.Player.Uid, StringComparer.Ordinal).ThenBy(o => o.Field.Name, StringComparer.Ordinal)
                .ThenBy(o => o.Field.Qualifier, StringComparer.Ordinal)]
        };
        var bytes = BatchExchange.Encode(ordered, maxBytes);
        return new(content.Players.Length, content.Observations.Length, Convert.ToHexString(SHA256.HashData(bytes)));
    }

    internal static void CheckSchema(SchemaVersion? schema)
    {
        if (schema is not { Major: 1, Minor: >= 0 })
            throw new InvalidDataException("Versión del contrato incompatible.");
    }

    private static void ValidateHeader(BatchContent c, CaptureAuthorization access, CaptureLimits budget)
    {
        CheckSchema(c.Schema);
        if (!budget.IsValid || c.Context is not { IsVerified: true } || c.Context != access.Context
            || c.Id == Guid.Empty || string.IsNullOrWhiteSpace(c.Route) || string.IsNullOrWhiteSpace(c.AdapterRevision)
            || c.CapturedUtc == default || c.CapturedUtc.Offset != TimeSpan.Zero || !Enum.IsDefined(c.Extent)
            || c.Limits is null || !c.Limits.Fits(budget))
            throw new InvalidDataException("Cabecera, presupuesto o contexto de lote inválido.");
    }

    internal static ImmutableArray<PlayerObservation> ValidateRows(ImmutableArray<PlayerIdentity> players,
        ImmutableArray<PlayerObservation> observations, CaptureAuthorization access, CaptureLimits limits, bool skipUnknown = false)
    {
        if (players.IsDefault || observations.IsDefault || players.Length > limits.MaxPlayers
            || observations.Length > limits.MaxObservations)
            throw new InvalidDataException("Muestra incompleta o fuera de presupuesto.");

        var identities = new HashSet<PlayerKey>();
        foreach (var player in players)
        {
            if (player?.Key is null || player.Club is null || !Enum.IsDefined(player.Club.Status)
                || (player.Club.Status == ClubStatus.ConfirmedClub
                    ? string.IsNullOrWhiteSpace(player.Club.ClubReference) : player.Club.ClubReference is not null)
                || access.AuthorizedIdentity(player.Key) != player || !identities.Add(player.Key))
                throw new InvalidDataException("Identidad sin autorizar, contradictoria o duplicada.");
        }

        var fields = new HashSet<(PlayerKey, FieldKey)>();
        var known = ImmutableArray.CreateBuilder<PlayerObservation>();
        foreach (var row in observations)
        {
            if (row?.Player is null || row.Field is null || string.IsNullOrWhiteSpace(row.Field.Name) || !identities.Contains(row.Player)
                || !fields.Add((row.Player, row.Field)))
                throw new InvalidDataException("Observación huérfana o duplicada.");
            if (skipUnknown && !access.Recognizes(row.Field)) continue;
            var permission = access.Permission(row.Player, row.Field);
            if (permission is null)
                throw new InvalidDataException("Campo sin evidencia de visibilidad.");
            CaptureAuthorization.ValidateObservation(row, permission);
            known.Add(row);
        }
        return known.Count == observations.Length ? observations : known.ToImmutable();
    }
}
