using System.Globalization;
using FM27Analyzer.RatingProbe;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

try
{
    if (args.Length != 1 && (args.Length != 5 || args[1] != "--presente"))
        throw new ArgumentException("Uso: <docs/as-is> [--presente <puesto> <rol IP> <rol OOP>].");

    string[] Read(string path) => File.ReadAllLines(Path.Combine(args[0], path));
    string[] attributeDocument = Read(Oracle.AttributesPath);
    string[][] dictionary = Oracle.Rows(Oracle.Section(attributeDocument, "## Diccionario de atributos visibles")).ToArray();
    string[] attributes = dictionary.Select(row => row[0]).ToArray();
    var goalkeeperAttributes = dictionary.Where(row => row[1] == "Portería").Select(row => row[0]).ToHashSet();
    Role[] roles = Oracle.Roles(Read(Oracle.RolesPath));
    var roleById = roles.ToDictionary(role => role.Id);
    Check.That(attributes.Length == 47 && attributes.Distinct().Count() == 47, "47 atributos canónicos");
    Check.That(roles.Length == 79 && roles.Count(role => role.Goalkeeper && role.Phase == "ip") == 3
        && roles.Count(role => role.Goalkeeper && role.Phase == "oop") == 3 && goalkeeperAttributes.Count == 11,
        "79 roles, 6 roles de portería, 11 atributos de portería");
    Check.That(roles.SelectMany(role => role.Key.Concat(role.Preferable)).All(attributes.Contains), "Marcas de atributos canónicos");

    var curves = Equivalence.BuildCurves(roles, attributes, goalkeeperAttributes, attributeDocument);
    if (args.Length == 5)
    {
        CurrentPilot.Run(Console.In, attributes, roleById, curves, args[2], args[3], args[4]);
        return 0;
    }

    Equivalence.VerifyClassic(Read(Oracle.ClassicPath), roleById, attributes);
    Equivalence.VerifyCurves(Read(Oracle.CurvesPath), curves, attributes.Length);
    Equivalence.VerifyBoundaries();
    Familiarity.Verify(curves, attributes.Length);
    ProjectionChecks.Verify(curves, attributes);
    return 0;
}
catch (Exception exception) when (exception is ArgumentException or InvalidDataException or IOException)
{
    Console.Error.WriteLine(exception.Message);
    return 1;
}

internal static class Check
{
    public static void That(bool condition, string context)
    {
        if (!condition)
            throw new InvalidDataException(context);
    }

    public static void Near(double expected, double actual, double tolerance, string context) =>
        That(double.IsFinite(actual) && Math.Abs(expected - actual) <= tolerance,
            $"{context} — esperado {expected:R}, obtenido {actual:R}, tolerancia {tolerance:R}");
}
