# Confirmar primero la copia, el corte, «Sólo jugadores ojeados» y el total en pantalla.
param(
    [Parameter(Mandatory)]
    [ValidateRange(1,500)]
    [int]$ExpectedCount,

    [Parameter(Mandatory)]
    [switch]$ConfirmScoutedView
)

$ErrorActionPreference = 'Stop'
if (!$ConfirmScoutedView) { throw 'Confirma la vista de ojeados y la copia de laboratorio antes de iniciar.' }
$folder = Join-Path $env:LOCALAPPDATA 'FM27Analyzer/SK-01/viabilidad'
if (!(Get-Acl -LiteralPath $folder).AreAccessRulesProtected) { throw 'El ensayo requiere un directorio privado.' }
$game = @(Get-Process -Name fm)
if ($game.Count -ne 1) { throw 'El censo requiere una instancia de FM27.' }
$id = [guid]::NewGuid()
$key = $id.ToString('N')
$request = [ordered]@{
    id = $id
    processStartUtcTicks = $game[0].StartTime.ToUniversalTime().Ticks
    expiresUtc = [DateTimeOffset]::UtcNow.AddSeconds(40).ToString('o')
    confirmed = $true
    copy = 'SK-01 Valencia 2026-08-01'
    branch = 'SK-01 copia local'
    perspective = 'Mánager activo del Valencia CF'
    club = 'Valencia CF'
    date = '2026-08-01'
    cut = '2026-08-01T21:45:00'
    view = 'ojeados'
    scoutedCount = $ExpectedCount
}
$temp = Join-Path $folder "$key.tmp"
$pending = Join-Path $folder 'bepinex-request.json'
$response = Join-Path $folder "bepinex-$key.json"
[IO.File]::WriteAllText($temp, ($request | ConvertTo-Json -Compress))
try {
    [IO.File]::Move($temp, $pending)
} finally {
    if (Test-Path -LiteralPath $temp) { Remove-Item -LiteralPath $temp }
}

$timer = [Diagnostics.Stopwatch]::StartNew()
try {
    while (!(Test-Path -LiteralPath $response) -and $timer.Elapsed.TotalSeconds -lt 35) {
        Start-Sleep -Milliseconds 150
    }
    if (!(Test-Path -LiteralPath $response)) { throw 'No se obtuvo una respuesta de BepInEx en 35 segundos.' }
    $result = Get-Content -LiteralPath $response -Raw | ConvertFrom-Json
    if ($result.Reason) { throw "Censo bloqueado. $($result.Reason)" }
    if ($result.ExpectedCount -ne $ExpectedCount -or $result.Rows.Count -ne $ExpectedCount -or
        @($result.Rows.Uid | Sort-Object -Unique).Count -ne $ExpectedCount -or
        $result.ProcessStartUtcTicks -ne $request.processStartUtcTicks -or $result.Build -ne '26620374' -or
        $result.Date -ne $request.date -or $result.Cut -ne $request.cut -or $result.Loads -le 0) {
        throw 'Respuesta fuera del alcance o contexto esperado.'
    }
    [pscustomobject]@{
        Artifact = $response
        Rows = $result.Rows.Count
        Exact = @($result.Rows | Where-Object Estado -eq 'exacto').Count
        Ranges = @($result.Rows | Where-Object Estado -eq 'intervalo').Count
        Unknown = @($result.Rows | Where-Object Estado -eq 'desconocido').Count
        Pages = $result.Pages
        Loads = $result.Loads
        ElapsedMs = $result.ElapsedMilliseconds
        ReadMs = $result.ReadMilliseconds
    }
} finally {
    if (Test-Path -LiteralPath $pending) { Remove-Item -LiteralPath $pending }
}
