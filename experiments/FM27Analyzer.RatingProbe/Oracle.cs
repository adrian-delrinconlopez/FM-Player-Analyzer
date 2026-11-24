using System.Globalization;

namespace FM27Analyzer.RatingProbe;

internal sealed record Role(string Id, string Phase, string[] Positions, string[] Key, string[] Preferable)
{
    public bool Goalkeeper => Positions[0] == "GK";

    public double Mark(string attribute) => Key.Contains(attribute) ? 1.5 : Preferable.Contains(attribute) ? 1 : 0;

    public double Factor(string attribute, bool custom = false) =>
        Mark(attribute) switch { 1.5 => custom ? 1.7 : 1.5, 1 => custom ? 1.1 : 1.2, _ => 1 };
}

internal static class Oracle
{
    public const string RolesPath = "16-tablas-de-roles-y-tacticas.md";
    public const string AttributesPath = "17-parametros-y-atributos.md";
    public const string ClassicPath = "20-oraculo-dwrs-classic.md";
    public const string CurvesPath = "22-oraculo-de-curvas-fm27.md";

    public static IEnumerable<string[]> Rows(IEnumerable<string> lines) =>
        lines.Where(line => line.StartsWith("| ", StringComparison.Ordinal) && !line.StartsWith("| ---", StringComparison.Ordinal))
            .Select(line => line.Trim('|').Split('|', StringSplitOptions.TrimEntries))
            .Skip(1);

    public static IEnumerable<string> Section(string[] lines, string header)
    {
        int offset = Array.IndexOf(lines, header);
        if (offset < 0)
            throw new InvalidDataException($"No se encuentra la sección {header}.");
        return lines.Skip(offset + 1).TakeWhile(line => !line.StartsWith("##", StringComparison.Ordinal));
    }

    public static double Number(string value) => double.Parse(value, CultureInfo.InvariantCulture);

    public static string[] List(string value) =>
        value == "—" ? [] : value.Split(',', StringSplitOptions.TrimEntries);

    public static Role[] Roles(string[] lines) => Rows(Section(lines, "## FM27"))
        .Select(row => new Role(row[0], row[2], List(row[3]), List(row[4]), List(row[5])))
        .ToArray();

    public static Observation[] ClassicSample(int sample, int length)
    {
        int[] constants = [1, 6, 12, 18, 19, 20];
        return Enumerable.Range(0, length).Select(index =>
        {
            if (sample < constants.Length)
                return Observation.Exact(constants[sample]);
            if (sample == 15 || (sample >= 12 && index % 4 == 0))
                return default;
            int low = 1 + (7 * index + 3 * sample) % 20;
            return new Observation(low, sample < 8 ? low : Math.Min(20, low + (index + sample) % 6));
        }).ToArray();
    }
}
