using FM27Analyzer.Extraction.Poc.Capture;

namespace FM27Analyzer.Extraction.Poc.Evidence;

public enum WorldReferenceScope { Whole, VerifiedSubset }

public sealed record WorldReference(CaptureContext Context, string Source, WorldReferenceScope Scope,
    int Count, IEnumerable<PlayerKey>? Identities);

public sealed record WorldCoverageResult(Coverage Coverage, int Unique, int Duplicates, int Omissions,
    string? ReferenceSource, int? ReferenceCount, int? IndependentDenominator, int? Matches, int? Missing, int? Unexpected);

public static class WorldCoverageAssessment
{
    public static WorldCoverageResult Compare(CaptureSession session, string route, int maximumEntries,
        int omissions, Func<IEnumerable<PlayerKey>> readIdentities, WorldReference? reference)
    {
        if (session.Expected.Policy is not { Scope: CaptureScope.World, WorldAuthorized: true })
            throw new UnauthorizedAccessException("El ensayo mundial requiere autorización expresa.");
        ArgumentException.ThrowIfNullOrWhiteSpace(route);
        if (maximumEntries is < 1 or > 250_000 || omissions < 0 || omissions > maximumEntries)
            throw new ArgumentOutOfRangeException(nameof(maximumEntries));

        return session.Read(() =>
        {
            var actual = Collect(readIdentities(), maximumEntries - omissions, session.Token, out int duplicates);
            var unknown = new WorldCoverageResult(Coverage.Indeterminate, actual.Count, duplicates, omissions,
                null, null, null, null, null, null);
            if (reference is null || reference.Context != session.Expected || !Enum.IsDefined(reference.Scope)
                || reference.Count < 0 || reference.Count > maximumEntries || string.IsNullOrWhiteSpace(reference.Source)
                || string.Equals(reference.Source.Trim(), route.Trim(), StringComparison.OrdinalIgnoreCase))
                return unknown;

            bool whole = reference.Scope == WorldReferenceScope.Whole;
            HashSet<PlayerKey>? expected = null;
            if (reference.Identities is not null)
            {
                expected = Collect(reference.Identities, maximumEntries, session.Token, out int referenceDuplicates);
                if (referenceDuplicates != 0 || expected.Count != reference.Count) return unknown;
            }
            else if (!whole)
            {
                return unknown;
            }

            int? matches = expected?.Count(actual.Contains);
            int? missing = reference.Count - matches;
            int? unexpected = whole ? actual.Count - matches : null;
            bool full = whole && duplicates == 0 && omissions == 0 && actual.Count == reference.Count
                && (expected is null || matches == reference.Count);
            var coverage = full ? Coverage.Complete : matches > 0 ? Coverage.Partial : Coverage.Indeterminate;
            return unknown with
            {
                Coverage = coverage, ReferenceSource = reference.Source, ReferenceCount = reference.Count,
                IndependentDenominator = whole ? reference.Count : null, Matches = matches, Missing = missing,
                Unexpected = unexpected
            };
        });
    }

    private static HashSet<PlayerKey> Collect(IEnumerable<PlayerKey> source, int limit, CancellationToken token,
        out int duplicates)
    {
        var keys = new HashSet<PlayerKey>();
        int count = 0;
        duplicates = 0;
        foreach (var key in source)
        {
            token.ThrowIfCancellationRequested();
            if (++count > limit)
                throw new InvalidDataException("Inventario fuera del presupuesto autorizado.");
            if (key is null || string.IsNullOrWhiteSpace(key.SourceType) || string.IsNullOrWhiteSpace(key.Uid))
                throw new InvalidDataException("Identidad del inventario sin verificar.");
            if (!keys.Add(key)) duplicates++;
        }
        return keys;
    }
}
