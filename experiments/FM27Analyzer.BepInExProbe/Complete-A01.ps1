param(
    [Parameter(Mandatory)]
    [switch]$ConfirmReadOnlyAndExclusion,

    [Parameter(Mandatory)]
    [switch]$SyntheticVisibilityPassed
)

$ErrorActionPreference = 'Stop'
if (!$ConfirmReadOnlyAndExclusion -or !$SyntheticVisibilityPassed) {
    throw 'Comprueba el estado del juego, la exclusión y el resultado de BaseVisibilityTests.'
}
$folder = Join-Path $env:LOCALAPPDATA 'FM27Analyzer/SK-01/ensayo-a01'
if (!(Get-Acl -LiteralPath $folder).AreAccessRulesProtected) { throw 'El ensayo requiere un directorio privado.' }
$dll = Join-Path $PSScriptRoot '../../src/FM27Analyzer.Extraction.Poc/bin/Release/net10.0/FM27Analyzer.Extraction.Poc.dll'
if (!(Test-Path -LiteralPath $dll)) { throw 'Compila la consola en Release.' }

function Read-Artifact([string]$name) {
    Get-Content -LiteralPath (Join-Path $folder $name) -Raw | ConvertFrom-Json
}

function Write-NewArtifact([string]$name, $value) {
    $file = Join-Path $folder $name
    $temp = Join-Path $folder ([guid]::NewGuid().ToString('N') + '.tmp')
    try {
        [IO.File]::WriteAllText($temp, ($value | ConvertTo-Json -Depth 20))
        [IO.File]::Move($temp, $file)
    } finally {
        if (Test-Path -LiteralPath $temp) { Remove-Item -LiteralPath $temp }
    }
}

$refFile = 'referencia-revinculada.json'
$reference = Read-Artifact $refFile
$context = $reference.context
$binding = Read-Artifact 'vinculacion.json'
$controls = Read-Artifact 'controles-de-pantalla.json'
if ((Get-FileHash -LiteralPath (Join-Path $folder 'referencia.json')).Hash -ne $binding.originalSha256 -or
    $context.instance -ne $binding.instance -or $context.session -ne $binding.session -or
    !$controls.exclusion.absentFromAll29WithFilterEnabled) {
    throw 'La referencia o el control de exclusión no coinciden con la vinculación.'
}

$schema = @{major=1; minor=0}
$limits = @{maxPlayers=30; maxObservations=34; maxBytes=131072; maxDuration='00:10:00'}
$route = 'BepInEx · tabla de FM27'
$visibility = 'FM27 · texto de la vista activa'
$extent = '30 jugadores del Valencia y su ojeo, con filtros de referencia'
$pageIds = [Collections.Generic.HashSet[string]]::new()
$batchNames = [Collections.Generic.List[string]]::new()
$metrics = [Collections.Generic.List[object]]::new()
$loads = $null

foreach ($round in 1,2) {
    $pages = @(
        foreach ($step in 'Squad','ScoutedTop','ScoutedBottom') {
            $page = Read-Artifact "pagina-$round-$step.json"
            $response = $page.response
            if ($null -eq $loads) { $loads = $response.Loads }
            if ($step -ne $page.step -or $round -ne $page.round -or
                $page.instance -ne $binding.instance -or $page.session -ne $binding.session -or
                !$pageIds.Add($response.Id) -or $response.Build -ne $context.build -or $response.Date -ne $context.gameDate -or
                $response.Loads -ne $loads -or $loads -le 0 -or $binding.processStartUtcTicks -ne $response.ProcessStartUtcTicks) {
                throw 'Contexto inconsistente entre páginas.'
            }
            $page
        }
    )
    $started = ([DateTimeOffset]$pages[0].startedUtc).ToUniversalTime()
    $finished = ([DateTimeOffset]$pages[-1].finishedUtc).ToUniversalTime()
    if ($started -le [DateTimeOffset]$reference.fixedUtc -or $finished -le $started -or
        $finished - $started -gt [TimeSpan]::Parse($limits.maxDuration)) { throw 'Ventana de captura inválida.' }

    $byUid = @{}
    foreach ($row in $pages.response.Rows) {
        if ($row.Uid -eq $controls.exclusion.stimulusUid -or $byUid.ContainsKey($row.Uid) -or
            $row.Uid -notin $reference.expected.key.uid) { throw 'Identidad fuera de la muestra o duplicada.' }
        $byUid[$row.Uid] = $row.Fields
    }

    $players = @(
        foreach ($expected in $reference.expected) {
            $captured = $byUid[$expected.key.uid]
            if ($null -eq $captured) { continue }
            # UI Toolkit conserva la decoración del enlace. La pantalla muestra sólo el nombre.
            $club = if ($captured.club) {
                $clubLabel = $captured.club -creplace '</?(?:color|link|style)(?:="[^"]*")?>', ''
                @{status='confirmedClub'; clubReference=$clubLabel.Trim()}
            }
                else { @{status='unknown'; clubReference=$null} }
            @{ key=$expected.key; careerOrConfirmedCopy=$context.copyReference; club=$club }
        }
    )
    $rows = @(
        foreach ($expected in $reference.observations) {
            $fields = $byUid[$expected.player.uid]
            $name = $expected.field.name
            if (!$fields -or !$fields.PSObject.Properties[$name]) { continue }
            $raw = $fields.$name
            $knowledge = 'exact'
            if ($expected.type -eq 'category') {
                $value = @{low=$null; high=$null; category=$raw}
            } elseif ($raw -in '-', '–', '—', 'Se necesita ojeo') {
                $knowledge = 'unknown'
                $value = $null
            } else {
                $parts = $raw -split '[-–]'
                $high = $null
                if ($parts.Length -eq 2) { $knowledge = 'inclusiveRange'; $high = [int]$parts[1] }
                $value = @{low=[int]$parts[0]; high=$high; category=$null}
            }
            @{ player=$expected.player; field=$expected.field; type=$expected.type; unit=$expected.unit
                state=@{knowledge=$knowledge; availability='observed'; disclosure='visible'}
                value=$value; dataSource=$route; visibilitySource=$visibility; catalogRevision=$null }
        }
    )
    $batchId = [guid]::NewGuid()
    $intermediate = "contenido-$round.json"
    Write-NewArtifact $intermediate @{
        schema=$schema; id=$batchId; context=$context; route=$route; adapterRevision='0.2.0'
        capturedUtc=$finished.ToString('o'); extent='partial'; limits=$limits; players=$players; observations=$rows
    }
    $batchName = "lote-$round.json"
    dotnet $dll seal-a01 --reference (Join-Path $folder $refFile) --content (Join-Path $folder $intermediate) --out (Join-Path $folder $batchName)
    if ($LASTEXITCODE -ne 0) { throw 'Error al validar y sellar el lote de A-01.' }
    $batchNames.Add($batchName)
    $ms = ($pages.response.ElapsedMilliseconds | Measure-Object -Sum).Sum
    $peak = [long]($pages.response.GamePeakWorkingSetBytes | Measure-Object -Maximum).Maximum
    $metrics.Add(@{
        referenceSource=$reference.independentSource; entityType='jugador'; fieldOrFamily="lote-$round"; route=$route
        build=$context.build; perspective=$context.perspective; unit=$null; extent=$extent; policy=$context.policy
        semantics="Suma de recorridos en el hilo de Unity: $ms ms. Pico residente acumulado de FM27."
        expected=30; recovered=$players.Count; matches=0; discrepancies=0; independentDenominator=30
        duration=($finished-$started).ToString('c'); peakBytes=$peak; data='partial'; coverage='partial'
    })
}

$execution = @(
    @{scenario='e01'; class='real'; outcome='passed'; context=$context; reason=$null; comparisons=$metrics.ToArray()}
    @{scenario='e03'; class='real'; outcome='passed'; context=$context; reason=$null; comparisons=@()}
    @{scenario='e03'; class='synthetic'; outcome='passed'; context=$context; reason=$null; comparisons=@()}
)
Write-NewArtifact 'run.json' @{
    schema=$schema; id=[guid]::NewGuid(); mode='contrastable'; context=$context; limits=$limits; extent=$extent
    reason=$null; batches=$batchNames.ToArray(); reference=$refFile; additionalCapabilities=$null; isolation=$null
    execution=$execution
}
dotnet $dll verify --run (Join-Path $folder 'run.json') --out (Join-Path $folder 'informe.json')
if ($LASTEXITCODE -ne 0) { throw 'Error al generar el informe de A-01.' }
