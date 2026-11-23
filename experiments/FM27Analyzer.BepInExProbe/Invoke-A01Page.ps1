# Ejecutar cada paso con la copia, corte y vista de FM27 comprobados en pantalla.
param(
    [Parameter(Mandatory)]
    [ValidateSet('Bind','Squad','ScoutedTop','ScoutedBottom')]
    [string]$Step,

    [ValidateSet(1,2)]
    [int]$Round = 1,

    [Parameter(Mandatory)]
    [switch]$ConfirmSk01Copy
)

$ErrorActionPreference = 'Stop'
if (!$ConfirmSk01Copy) { throw 'Confirma la copia, el corte y la vista en FM27.' }
$folder = Join-Path $env:LOCALAPPDATA 'FM27Analyzer/SK-01/ensayo-a01'
if (!(Get-Acl -LiteralPath $folder).AreAccessRulesProtected) {
    throw 'El directorio de A-01 debe tener permisos privados.'
}

function Read-Artifact([string]$name) {
    Get-Content -LiteralPath (Join-Path $folder $name) -Raw | ConvertFrom-Json
}

function Write-NewArtifact([string]$name, $value) {
    $path = Join-Path $folder $name
    $temp = Join-Path $folder ([guid]::NewGuid().ToString('N') + '.tmp')
    try {
        [IO.File]::WriteAllText($temp, ($value | ConvertTo-Json -Depth 12))
        [IO.File]::Move($temp, $path)
    } finally {
        if (Test-Path -LiteralPath $temp) { Remove-Item -LiteralPath $temp }
    }
}

$game = @(Get-Process -Name fm)
if ($game.Count -ne 1) { throw 'A-01 requiere una instancia de FM27.' }
$ticks = $game[0].StartTime.ToUniversalTime().Ticks

if ($Step -eq 'Bind') {
    $reference = Read-Artifact 'referencia.json'
    $reference.context.instance = [guid]::NewGuid()
    $reference.context.session = [guid]::NewGuid()
    $reference.fixedUtc = [DateTimeOffset]::UtcNow.ToString('o')
    Write-NewArtifact 'referencia-revinculada.json' $reference
    Write-NewArtifact 'vinculacion.json' @{
        instance = $reference.context.instance; session = $reference.context.session
        processStartUtcTicks = $ticks
        originalSha256 = (Get-FileHash -LiteralPath (Join-Path $folder 'referencia.json')).Hash
    }
    return
}

$reference = Read-Artifact 'referencia-revinculada.json'
$binding = Read-Artifact 'vinculacion.json'
if ($ticks -ne $binding.processStartUtcTicks -or $reference.context.session -ne $binding.session) {
    throw 'Cambió la instancia o la sesión confirmada.'
}
$controls = Read-Artifact 'controles-de-pantalla.json'
$pagePath = "pagina-$Round-$Step.json"
if (Test-Path -LiteralPath (Join-Path $folder $pagePath)) { throw 'La página del lote ya existe.' }
$view = 'ojeados'
$uids = switch ($Step) {
    'Squad' { $view = 'plantilla'; $controls.squad }
    'ScoutedTop' { $controls.scouted | Select-Object -First ($controls.scouted.Count - 1) }
    'ScoutedBottom' { $controls.scouted | Select-Object -Last 1 }
}
$targets = @(
    foreach ($uid in $uids) {
        $fields = @{}
        foreach ($row in $reference.observations | Where-Object { $_.player.uid -eq $uid }) {
            $fields[$row.field.name] = $row.state.knowledge
        }
        $player = $reference.expected | Where-Object { $_.key.uid -eq $uid }
        if ($player.club.status -eq 'confirmedClub') { $fields['club'] = 'exact' }
        @{ uid = $uid; fields = $fields }
    }
)

if (!$targets.Count -or @($targets | Where-Object { $_.uid -eq $controls.exclusion.stimulusUid }).Count) {
    throw 'La página queda fuera de la muestra autorizada.'
}
$request = Join-Path $PSScriptRoot 'Request-VisibleField.ps1'
$started = [DateTimeOffset]::UtcNow
$response = & $request -View $view -Targets $targets -ConfirmSk01Copy | ConvertFrom-Json
if (!$response.Rows -or $response.Build -ne $reference.context.build -or
    $response.Date -ne $reference.context.gameDate -or $response.View -ne $view -or
    $response.ProcessStartUtcTicks -ne $ticks -or $response.Loads -le 0) {
    throw "BepInEx no devolvió una página verificable. $($response.Reason)"
}
$received = @($response.Rows | ForEach-Object Uid)
if ($received.Count -ne @($received | Sort-Object -Unique).Count -or
    @($received | Where-Object { $_ -notin $uids }).Count) {
    throw 'La página contiene identificadores duplicados o fuera de la muestra.'
}
Write-NewArtifact $pagePath @{
    round = $Round; step = $Step; instance = $binding.instance; session = $binding.session
    startedUtc = $started.ToString('o'); finishedUtc = [DateTimeOffset]::UtcNow.ToString('o')
    response = $response
}
[pscustomobject]@{ View = $view; Received = $received.Count; Expected = $targets.Count
    MissingCells = ($targets | ForEach-Object { $_.fields.Count } | Measure-Object -Sum).Sum -
        ($response.Rows | ForEach-Object { @($_.Fields.PSObject.Properties.Name).Count } | Measure-Object -Sum).Sum
    ElapsedMs = $response.ElapsedMilliseconds; PeakGameBytes = $response.GamePeakWorkingSetBytes }
