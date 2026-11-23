# Requiere PowerShell 7 y el directorio privado del banco SK-01.
param(
    [Parameter(Mandatory)]
    [ValidatePattern('^[0-9]{1,12}$')]
    [string]$Uid,

    [Parameter(Mandatory)]
    [switch]$ConfirmSk01Copy,

    [ValidatePattern('^[0-9]{4}-[0-9]{2}-[0-9]{2}$')]
    [string]$ExpectedDate = '2026-08-01'
)

$ErrorActionPreference = 'Stop'
if (!$ConfirmSk01Copy) {
    throw 'Confirma visualmente la copia SK-01 y el corte antes de pedir la lectura.'
}

$runFolder = Join-Path $env:LOCALAPPDATA 'FM27Analyzer/SK-01/viabilidad'
if (!(Get-Acl -LiteralPath $runFolder).AreAccessRulesProtected) {
    throw 'El directorio de ensayo debe tener permisos privados.'
}

$game = @(Get-Process -Name fm)
if ($game.Count -ne 1) {
    throw 'La prueba requiere una sola instancia de FM27.'
}

$id = [Guid]::NewGuid()
$request = [ordered]@{
    id = $id
    processStartUtcTicks = $game[0].StartTime.ToUniversalTime().Ticks
    expiresUtc = [DateTimeOffset]::UtcNow.AddSeconds(15).ToString('o')
    confirmed = $true
    uid = $Uid
    copy = 'SK-01 Valencia 2026-08-01'
    branch = 'SK-01 copia local'
    perspective = 'Mánager activo del Valencia CF'
    club = 'Valencia CF'
    date = $ExpectedDate
    cut = "${ExpectedDate}T21:45:00"
}

$key = $id.ToString('N')
$tempPath = Join-Path $runFolder "$key.tmp"
$requestPath = Join-Path $runFolder 'bepinex-request.json'
$resultPath = Join-Path $runFolder "bepinex-$key.json"
[IO.File]::WriteAllText($tempPath, ($request | ConvertTo-Json -Compress))
try {
    [IO.File]::Move($tempPath, $requestPath)
} finally {
    if (Test-Path -LiteralPath $tempPath) {
        Remove-Item -LiteralPath $tempPath
    }
}

$timer = [Diagnostics.Stopwatch]::StartNew()
try {
    while (!(Test-Path -LiteralPath $resultPath) -and $timer.ElapsedMilliseconds -lt 5000) {
        Start-Sleep -Milliseconds 125
    }
    if (!(Test-Path -LiteralPath $resultPath)) {
        throw 'Sin respuesta de BepInEx en cinco segundos.'
    }
    Get-Content -LiteralPath $resultPath -Raw
} finally {
    Remove-Item -LiteralPath $requestPath
}
