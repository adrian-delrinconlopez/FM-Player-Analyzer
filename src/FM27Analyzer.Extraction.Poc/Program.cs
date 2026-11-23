const string status = "SK-01 · Base de extracción FM27. El contraste A-01 está pendiente.";
if (args.Length > 0)
{
    Console.Error.WriteLine(status);
    return 2;
}
Console.WriteLine(status);
return 0;
