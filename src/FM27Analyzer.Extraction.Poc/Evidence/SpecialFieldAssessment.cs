using FM27Analyzer.Extraction.Poc.Capture;

namespace FM27Analyzer.Extraction.Poc.Evidence;

public enum SpecialFieldStatus { Match, Mismatch, Unauthorized, NoReference, Unavailable }

public sealed record SpecialFieldReference(CaptureContext Context, PlayerKey Player, FieldKey Field,
    string Source, string Unit, Func<int?> Read);

public static class SpecialFieldAssessment
{
    public static SpecialFieldStatus Compare(CaptureSession session, CaptureAuthorization access,
        PlayerKey player, FieldKey field, FieldClass kind, SpecialFieldReference? reference, Func<int?> read)
    {
        access.CheckSession(session);
        var permission = session.Read(() => access.Permission(player, field));
        if (permission?.Class != kind || !access.CanCompareInMemory(session, player, field))
            return SpecialFieldStatus.Unauthorized;

        if (permission.Type != FieldType.Numeric || string.IsNullOrWhiteSpace(permission.Unit)
            || permission.ScaleMinimum is not decimal minimum || permission.ScaleMaximum is not decimal maximum
            || minimum > maximum)
            return SpecialFieldStatus.Unavailable;

        if (reference is null || reference.Context != session.Expected || reference.Player != player
            || reference.Field != field || reference.Unit != permission.Unit || string.IsNullOrWhiteSpace(reference.Source)
            || string.Equals(reference.Source.Trim(), permission.DataSource.Trim(), StringComparison.OrdinalIgnoreCase))
            return SpecialFieldStatus.NoReference;

        return session.Read(() =>
        {
            try
            {
                if (reference.Read() is not int expected || expected < minimum || expected > maximum)
                    return SpecialFieldStatus.NoReference;
                if (session.Read(read) is not int found || found < minimum || found > maximum)
                    return SpecialFieldStatus.Unavailable;
                return expected == found ? SpecialFieldStatus.Match : SpecialFieldStatus.Mismatch;
            }
            catch (Exception ex) when (ex is IOException or InvalidDataException or UnauthorizedAccessException)
            {
                return SpecialFieldStatus.Unavailable;
            }
        });
    }
}
