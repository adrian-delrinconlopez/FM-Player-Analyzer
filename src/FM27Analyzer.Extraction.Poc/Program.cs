using System.Runtime.InteropServices;
using FM27Analyzer.Extraction.Poc.Capture;

const string status = "SK-01 · Base de extracción FM27. El contraste A-01 está pendiente.";
if (args is ["probe-external", "--confirm-sk01", var pidText, var startText, var copyPath]
    && int.TryParse(pidText, out int pid) && long.TryParse(startText, out long startTicks))
{
    if (OperatingSystem.IsWindows() && RuntimeInformation.ProcessArchitecture == Architecture.X64)
        return ExternalFieldProbe.Run(pid, startTicks, copyPath);

    Console.Error.WriteLine("La sonda externa requiere Windows x64.");
    return 2;
}

if (args.Length > 0)
{
    Console.Error.WriteLine(status);
    return 2;
}
Console.WriteLine(status);
return 0;
