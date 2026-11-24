using System.Runtime.InteropServices;
using FM27Analyzer.Extraction.Poc.Capture;
using FM27Analyzer.Extraction.Poc.Evidence;

const string status = "SK-01 · Base de extracción FM27. El contraste A-01 está pendiente.";
if (OperatingSystem.IsWindows())
{
    if (args is ["verify", "--run", var run, "--out", var report])
        return A01Commands.Verify(run, report);
    if (args is ["seal-a01", "--reference", var reference, "--content", var content, "--out", var batch])
        return A01Commands.Seal(reference, content, batch);
    if (args is ["check-recovery", "--reference", var recoveryReference, "--batch", var acceptedBatch, "--retry", var retryBatch])
        return A01Commands.CheckRecovery(recoveryReference, acceptedBatch, retryBatch);
}

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
