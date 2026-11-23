namespace FM27Analyzer.Extraction.Poc.Capture;

public enum IdentityScope { Unverified, Squad, Scouted, LoadedWorld }
public enum FieldClass { Visible, HiddenExact, CurrentAbility, PotentialAbility }

// Evidencia independiente del contenido recibido, vinculada al corte y al jugador.
// null significa que no se ha podido acreditar ni siquiera el estado publicable.
public sealed record FieldPermission(FieldClass Class, FieldType Type, string? Unit,
    ObservationState State, string DataSource, string VisibilitySource, string? CatalogRevision,
    bool GenericRestrictionVisible = false, decimal? ScaleMinimum = null, decimal? ScaleMaximum = null);

public sealed class CaptureAuthorization(CaptureContext context,
    Func<PlayerKey, IdentityScope> scopeOf, Func<PlayerKey, PlayerIdentity?> identify,
    Func<FieldKey, bool> recognizes,
    Func<PlayerKey, FieldKey, FieldPermission?> describe)
{
    public CaptureContext Context { get; } = context;

    public PlayerIdentity? AuthorizedIdentity(CaptureSession session, PlayerKey key)
    {
        CheckSession(session);
        return session.Read(() => AuthorizedIdentity(key));
    }

    internal PlayerIdentity? AuthorizedIdentity(PlayerKey key)
    {
        if (!Context.IsVerified || key is null || string.IsNullOrWhiteSpace(key.Uid)
            || string.IsNullOrWhiteSpace(key.SourceType)) return null;
        var allowed = scopeOf(key) switch
        {
            IdentityScope.Squad or IdentityScope.Scouted => true,
            IdentityScope.LoadedWorld => Context.Policy is { Scope: CaptureScope.World, WorldAuthorized: true },
            _ => false
        };
        if (!allowed) return null;
        var identity = identify(key);
        return identity?.Key == key && identity.CareerOrConfirmedCopy == (Context.CareerId ?? Context.CopyReference)
            ? identity : null;
    }

    internal bool Recognizes(FieldKey field) => field is not null && !string.IsNullOrWhiteSpace(field.Name) && recognizes(field);

    internal FieldPermission? Permission(PlayerKey player, FieldKey field)
    {
        if (AuthorizedIdentity(player) is null || !Recognizes(field))
            return null;
        return describe(player, field);
    }

    public bool CanCompareInMemory(CaptureSession session, PlayerKey player, FieldKey field)
    {
        CheckSession(session);
        var permission = session.Read(() => Permission(player, field));
        if (permission?.State?.Availability != Availability.Observed
            || string.IsNullOrWhiteSpace(permission.DataSource) || string.IsNullOrWhiteSpace(permission.VisibilitySource))
            return false;
        var policy = Context.Policy;
        return permission.Class switch
        {
            FieldClass.CurrentAbility => policy.AbilityWarningAcknowledged && policy.CurrentAbilityAuthorized,
            FieldClass.PotentialAbility => policy.AbilityWarningAcknowledged && policy.PotentialAbilityAuthorized,
            FieldClass.HiddenExact => policy.HiddenAuthorized,
            _ => false
        };
    }

    internal static bool Publishable(FieldPermission? permission) =>
        permission is { Class: FieldClass.Visible, State: not null }
        && Enum.IsDefined(permission.State.Knowledge) && Enum.IsDefined(permission.State.Availability)
        && Enum.IsDefined(permission.State.Disclosure) && Enum.IsDefined(permission.Type)
        && !string.IsNullOrWhiteSpace(permission.DataSource) && !string.IsNullOrWhiteSpace(permission.VisibilitySource)
        && (permission.State.Disclosure == Disclosure.Visible || permission.GenericRestrictionVisible);

    private static ObservationState PublicState(FieldPermission permission) =>
        permission.State.Disclosure == Disclosure.Restricted
            ? new(Knowledge.Unknown, Availability.NotAssessed, Disclosure.Restricted)
            : permission.State;

    internal static bool NeedsValue(ObservationState state) => state is
        { Availability: Availability.Observed, Disclosure: Disclosure.Visible, Knowledge: not Knowledge.Unknown };

    // El adaptador sólo recibe la precisión autorizada al consultar el valor.
    public PlayerObservation? Project(CaptureSession session, PlayerKey player, FieldKey field, Func<Knowledge, FieldValue> read)
    {
        CheckSession(session);
        var permission = session.Read(() => Permission(player, field));
        if (!Publishable(permission)) return null;
        var evidence = permission!;
        var state = PublicState(evidence);
        var value = NeedsValue(state) ? session.Read(() => read(state.Knowledge)) : null;
        var observation = new PlayerObservation(player, field, evidence.Type, evidence.Unit, value,
            state, evidence.DataSource, evidence.VisibilitySource, evidence.CatalogRevision);
        ValidateObservation(observation, evidence);
        return observation;
    }

    internal void CheckSession(CaptureSession session)
    {
        if (session.Expected != Context) throw new InvalidDataException("Política ajena a la sesión.");
    }

    internal static void ValidateObservation(PlayerObservation row, FieldPermission permission)
    {
        if (!Publishable(permission) || row.Type != permission.Type || row.Unit != permission.Unit
            || row.State != PublicState(permission) || row.DataSource != permission.DataSource
            || row.VisibilitySource != permission.VisibilitySource || row.CatalogRevision != permission.CatalogRevision)
            throw new InvalidDataException("Campo o procedencia fuera de la proyección autorizada.");

        bool valid;
        if (!NeedsValue(row.State)) valid = row.Value is null;
        else if (row.Type == FieldType.Category)
            valid = row.State.Knowledge == Knowledge.Exact && row.Value is { Low: null, High: null }
                && !string.IsNullOrWhiteSpace(row.Value.Category);
        else
        {
            valid = !string.IsNullOrWhiteSpace(row.Unit) && row.Value is { Category: null, Low: decimal low }
                && (permission.ScaleMinimum is null || low >= permission.ScaleMinimum)
                && (permission.ScaleMaximum is null || low <= permission.ScaleMaximum)
                && (row.State.Knowledge == Knowledge.Exact ? row.Value.High is null
                    : row.Value.High is decimal high && high >= low
                        && (permission.ScaleMaximum is null || high <= permission.ScaleMaximum));
        }
        if (!valid) throw new InvalidDataException("Tipo, escala o precisión incompatibles con el conocimiento autorizado.");
    }
}
