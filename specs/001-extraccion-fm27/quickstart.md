# Guía de validación · SK-01

**Estado a 23 de noviembre de 2026.** Setup y Foundational disponen de una solución de consola
y pruebas MSTest. La orden `verify` y los ensayos A-01 de las secciones posteriores corresponden a US1.

### Repetir la comprobación de Setup y Foundational

Desde la raíz del clon, en Windows 11 x64 con el SDK .NET 10. En el equipo del ensayo
el SDK disponible en el PATH inicial era 3.1.426. Para usar la instalación privada verificada
de 10.0.404 en una nueva terminal PowerShell de ese equipo, ejecutar primero:

```powershell
$env:DOTNET_ROOT = Join-Path $env:LOCALAPPDATA 'FM27Analyzer/toolchain/dotnet'
$env:PATH = "$env:DOTNET_ROOT;$env:PATH"
```

```powershell
dotnet --version
dotnet restore FM27Analyzer.slnx
dotnet build FM27Analyzer.slnx -c Release --no-restore -warnaserror
dotnet test tests/FM27Analyzer.Extraction.Tests/FM27Analyzer.Extraction.Tests.csproj -c Release --no-build --filter FullyQualifiedName~CaptureGuardTests
dotnet test FM27Analyzer.slnx -c Release --no-build
```

**Resultado observado.** Compilación con cero errores y advertencias. Pasaron los 13 casos de
sesión y los 41 casos de la batería completa, incluidos esos 13. MSTest utiliza identidades,
permisos y valores sintéticos. Para comprobar el intercambio escribe archivos de ensayo
privados bajo `%LOCALAPPDATA%/FM27Analyzer/SK-01` y los retira al terminar. El sobre JSON
1.x admite metadatos opcionales en el objeto `metadata`. El cierre cuenta las identidades y
observaciones y usa SHA-256 sobre el contenido JSON UTF-8 en orden ordinal de claves.

**Banco local T003.** El manifiesto de Steam identifica FM27 build 26620374. Los metadatos de
la instalación identifican BepInEx 6.0.0-be.791, Il2CppInterop.Runtime 1.5.3 y el
runtime distribuido 6.0.7. El inventario y sus fuentes están en
`%LOCALAPPDATA%/FM27Analyzer/SK-01/banco.json`. Durante el inventario había cero procesos
`fm`. Quedan pendientes la confirmación de copia, perspectiva y corte, la línea base del
juego abierto y la revisión de derechos de uso para el ensayo concreto. La primera lectura real
requiere esas comprobaciones y el control de sesión de la vía que se ensaye.

## Preparar el entorno

- En Windows 11 x64, anotar `dotnet --info`, revisión Git, recursos del equipo,
  compilación exacta de FM27, distribución, idioma y, si aplica, versiones de
  BepInEx, interop y runtime. Partir del BepInEx ya instalado por el propietario
  y comprobar la configuración existente. El ensayo usa una copia de partida y
  conserva la instalación y los guardados durante la captura.
- Preparar una copia con plantilla propia, externos conocidos u ojeados,
  portero, jugador de campo, atributo exacto, intervalo y desconocimiento.
  Preparar una segunda copia con el mismo club y un UID coincidente para
  acreditar ese caso de aislamiento real. Si faltan esas condiciones,
  registrar la omisión y usar un control sintético. Identidades fuera del
  ámbito inicial se usan como control, excluidas de la muestra compartible.
- Fijar copia, fecha, perspectiva, permisos base, casos y límites antes de
  capturar. Seleccionar hasta tres jugadores para el ensayo de viabilidad.
  Tras elegir una vía, fijar una lista de al menos 30 futbolistas antes de
  la primera captura de A-01. Cotejar UID y campos con fichas o vistas de FM27
  del mismo corte que sean independientes de
  la vía bajo ensayo. Conservar las referencias locales autorizadas.
- Anotar el estado visible del juego y metadatos de instalación y guardados
  antes y después, sin abrir los archivos `.fm`. Registrar las operaciones del
  ensayo y una línea base comparable sin captura para atribuir cambios y coste.
  Mantener los datos reales fuera del repositorio.

## Ejecutar la comprobación

Una vez implementados los proyectos, desde la raíz del repositorio:

```powershell
dotnet build FM27Analyzer.slnx -c Release
dotnet test tests/FM27Analyzer.Extraction.Tests/FM27Analyzer.Extraction.Tests.csproj -c Release
```

La compilación normal y las pruebas sintéticas funcionan sin FM27 ni el loader.
Antes de la primera captura real, ejecutar el control sintético de sesión y
permisos. Un cambio de contexto o revisión durante el trabajo debe cancelar
la captura e impedir la publicación del lote pendiente. El productor revalida
el contexto justo antes de publicar.

### Sonda BepInEx T015

Con FM27 cerrado, compilar el proyecto de laboratorio contra las bibliotecas
del BepInEx instalado para Steam 26620374. Copiar únicamente el DLL del
experimento a la carpeta de plugins de ese BepInEx. Conservar el hash para
identificar y retirar el archivo al acabar con el juego cerrado.

```powershell
dotnet build FM27Analyzer.slnx -c Laboratorio "-p:FM27GameRoot=$fm27GameRoot" -warnaserror
Copy-Item -LiteralPath 'experiments/FM27Analyzer.BepInExProbe/bin/Release/net6.0/FM27Analyzer.BepInExProbe.dll' -Destination (Join-Path $fm27GameRoot 'BepInEx/plugins')
```

Comprobar el arranque de `FM27 Analyzer SK-01` en `BepInEx/LogOutput.log`.
Cargar expresamente la copia `SK-01 Valencia 2026-08-01`, verificar
Valencia CF y el corte 1/8/2026 21:45. En «Primer equipo», añadir
«Unique ID» y «Pase» mediante «Insertar columna» y ajustar el ancho del UID.
Dejar la fila fijada a la vista. Ejecutar las peticiones una a una con
PowerShell 7 y el directorio privado del ensayo ya preparado.

```powershell
$sk01Probe = 'experiments/FM27Analyzer.BepInExProbe/Request-VisibleField.ps1'
& $sk01Probe -Uid '67178177' -ConfirmSk01Copy
& $sk01Probe -Uid '67178177' -ConfirmSk01Copy
```

La sonda toma la identidad y el atributo de la tabla de Unity. Comprueba
compilación, fecha y recarga en memoria del proceso. El operador acredita
copia, hora y perspectiva en pantalla. El ensayo sólo permite «Pase» y la
petición caduca tras 15 segundos. Los JSON de cada intento se guardan en
`%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad`. Cotejar UID, fecha y
valor con FM27 y guardar el resultado agregado en `SK-01/bepinex.json`.
Para un control negativo, repetir con la vista sin la tabla de jugadores o
usar `-ExpectedDate '2000-01-01'` en la misma copia y verificar el bloqueo.

### Sonda externa T016 y comparación T017

En Windows x64, compilar Release y ejecutar el filtro `CaptureGuardTests`.
Conservar la referencia de T014 en `SK-01/viabilidad/referencia.json`.
Confirmar en pantalla la copia, el mánager del Valencia, el corte
1/8/2026 21:45, el UID y el estado exacto visible de «Pase». Ajustar
`$sk01Copy` a la ubicación local de la copia. La sonda usa sólo su
tamaño y última modificación.

```powershell
$sk01Game = Get-Process -Name fm
$sk01Copy = Join-Path ([Environment]::GetFolderPath('MyDocuments')) 'Sports Interactive/Football Manager 27/games/SK-01 Valencia 2026-08-01.fm'
$sk01Dll = 'src/FM27Analyzer.Extraction.Poc/bin/Release/net10.0/FM27Analyzer.Extraction.Poc.dll'
$sk01Start = $sk01Game.StartTime.ToUniversalTime().Ticks
dotnet $sk01Dll probe-external --confirm-sk01 $sk01Game.Id $sk01Start $sk01Copy
dotnet $sk01Dll probe-external --confirm-sk01 $sk01Game.Id ($sk01Start + 1) $sk01Copy
```

La primera orden produce un JSON de contraste con dos observaciones y
termina con código 0 si coinciden con la referencia. La segunda debe
terminar con código 2 y sólo un diagnóstico de instancia distinta. El
código 1 indica discrepancia. El presupuesto de lectura es 96 KiB y dos
segundos para localizar y leer el único campo verificado.

Sin cambiar de vista ni corte, repetir las dos peticiones de la sonda
BepInEx. Verificar build, inicio de proceso, contador de cargas, fecha,
UID, campo, valor y estado frente a la referencia. Conservar la
procedencia por observación. Archivar el resultado externo y el control
negativo en `SK-01/lectura-externa.json` y la comparación y vía elegida en
`SK-01/decision-via.json`, ambos con acceso privado del usuario.

### A-01

Con la sonda 0.2.0 de laboratorio cargada en BepInEx, confirmar la copia de SK-01 y
el corte en pantalla. Conservar la referencia prefijada y los controles de
T018 en el directorio privado. Tras una recarga, `Bind` prepara una copia
de la referencia con la nueva vinculación y conserva la huella del original.
Los pasos siguientes escriben artefactos con nombres fijos. Para repetir
todo el ensayo, archivar primero los artefactos de la ejecución anterior.

```powershell
$a01Probe = 'experiments/FM27Analyzer.BepInExProbe/Invoke-A01Page.ps1'
& $a01Probe -Step Bind -ConfirmSk01Copy
```

En Primer equipo, mostrar «Unique ID», «Pase», «Club», «Reflejos» y
«Mejor pos.». Mantener visibles las filas fijadas. En Base de datos de
jugadores, mantener los criterios del contraste, activar «Sólo jugadores
ojeados» y mostrar «Unique ID», «Pase», «Marcaje», «Posición» y «Club».
Para cada vuelta, ejecutar los pasos en este orden y colocarse en la vista
indicada antes de cada petición.

```powershell
# Ejecutar una vez con $round = 1 y otra con $round = 2.
& $a01Probe -Step Squad -Round $round -ConfirmSk01Copy         # Primer equipo
& $a01Probe -Step ScoutedTop -Round $round -ConfirmSk01Copy    # Inicio de ojeados
& $a01Probe -Step ScoutedBottom -Round $round -ConfirmSk01Copy # Final de ojeados
```

Tras repetir el control sintético `BaseVisibilityTests`, confirmar
que se mantuvieron copia, perspectiva y corte, que el control excluido
sigue fuera del filtro y que la captura fue de solo lectura. Desde la
raíz, compilar la consola en Release y ejecutar:

```powershell
& 'experiments/FM27Analyzer.BepInExProbe/Complete-A01.ps1' -ConfirmReadOnlyAndExclusion -SyntheticVisibilityPassed
```

El script reúne las tres páginas de cada vuelta, normaliza las etiquetas
visuales de club, sella los lotes y llama a `verify`. El informe se puede
generar de nuevo con los mismos archivos. El filtro y el desplazamiento
se controlan en pantalla. La sonda utiliza la selección fijada y
compara la huella de la build, fecha, proceso y contador de cargas.

Anotar la revisión y los pasos o el comando exactos de cada vía. Hacer dos
lecturas por vía viable del mismo campo visible, los mismos UID y el mismo
corte. Para A-01, usar la vía elegida, permisos base y dos capturas consecutivas
con los mismos UID y alcance de la referencia independiente. Dejar el juego
en el mismo corte entre capturas.

Preparar el manifiesto de acuerdo con la
[modalidad del ensayo](contracts/captura.md#modalidades-de-a-01-y-resultados-adicionales).
El directorio de ejemplo representa el directorio local de ese ensayo.

```powershell
$sk01Run = Join-Path $env:LOCALAPPDATA 'FM27Analyzer\SK-01\ensayo-a01'
dotnet run --project src/FM27Analyzer.Extraction.Poc/FM27Analyzer.Extraction.Poc.csproj -c Release -- verify --run (Join-Path $sk01Run 'run.json') --out (Join-Path $sk01Run 'informe.json')
```

- **A-01 contrastable.** El manifiesto referencia los dos lotes y el cotejo
  directo de FM27 por separado.
- **A-01 bloqueada.** El manifiesto registra el motivo, el alcance y la clase
  de evidencia disponible. El mismo comando genera un informe con A-01 sin
  acreditar. Se registra la omisión o el fallo que corresponda.
- **Resultados adicionales.** Si existen, indicar en el manifiesto las rutas
  de `capacidades-adicionales.json` y `aislamiento.json`. El informe conserva
  la clase de evidencia y el estado de cada escenario.

Registrar duración de cada captura, memoria pico del capturador y del validador, y el
coste observado dentro de FM27 si aplica, con la misma técnica de medida y línea
base. Usar el mismo hardware, revisión, corte, alcance y método de referencia
al comparar vías. Anotar cancelaciones y resultados parciales.

### Censo de la vista de ojeados

Compilar la solución en configuración `Laboratorio` con .NET SDK
10.0.404 y la ruta de la instalación de FM27. La sonda usa `net6.0`
con el runtime de BepInEx. Cargar el DLL de la sonda 0.3.0 en la
instalación de laboratorio y abrir la copia SK-01.

```powershell
dotnet build FM27Analyzer.slnx -c Laboratorio "-p:FM27GameRoot=$fm27Root" -warnaserror
dotnet test FM27Analyzer.slnx -c Laboratorio --no-build
```

En el corte del Valencia CF de 1/8/2026 a las 21:45, abrir «Base de
datos de jugadores» con las columnas «Unique ID», «Pase», «Club» y
«Posición». En la vista «Copia de Contrato» activar «Sólo jugadores
ojeados». Desactivar los criterios de búsqueda activos y poner «Interés
del jugador» en «No considerado». Comprobar el total en pantalla y
mantener el mismo filtro y orden mientras corre la sonda. En este ensayo
FM27 anunció 356 resultados.

```powershell
$censusProbe = 'experiments/FM27Analyzer.BepInExProbe/Request-ScoutedCensus.ps1'
& $censusProbe -ExpectedCount 356 -ConfirmScoutedView
& $censusProbe -ExpectedCount 356 -ConfirmScoutedView
# Control negativo mientras FM27 sigue mostrando 356 filas.
& $censusProbe -ExpectedCount 355 -ConfirmScoutedView
```

El control negativo debe generar sólo un motivo de bloqueo. Recargar la
misma copia sin cerrar FM27. Volver a aplicar la configuración de la
vista, comprobar el mismo corte y lanzar una tercera petición de 356.
Cotejar UID y Pase de las diez filas ojeadas de T018 y comprobar la
ausencia del control excluido en cada pasada. Para las tres respuestas de
356, comparar el contenido ordenado de `Rows` y comprobar que
`ProcessStartUtcTicks` coincide y `Loads` aumenta tras la recarga.
Reponer los criterios originales y comprobar los metadatos del guardado.

Los archivos de respuesta están en
`%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad`. El ensayo se limita a
500 filas y cancela a los 30 segundos dentro del juego. La confirmación
del operador fija copia, perspectiva y filtro. La sonda comprueba
fecha, contador de carga, vista, total y solapamiento de filas. Un cambio
de filtro que conserve el mismo total y las filas de solapamiento
puede escapar a la detección. [Evidencia del censo](evidencia.md#censo-de-ojeados-y-recarga)

### Aislamiento y recuperación

Con la sonda 0.3.0 en FM27, confirmar la copia y el corte antes de
cada petición. La secuencia del ensayo usa `Valencia2026` (1/8/2026,
21:45), `Solihull` (21/8/2027, 09:00) y `Valencia2025` (7/7/2025,
10:00) en un mismo proceso. El control positivo usa la plantilla del
Valencia con las columnas «Unique ID» y «Pase». Desde Solihull se
accede a la plantilla visitante a través de la ficha de Gayà.

```powershell
$probe = 'experiments/FM27Analyzer.BepInExProbe/Request-VisibleField.ps1'
# Con Solihull abierto, primero el control obsoleto y después el actual.
& $probe -Uid 67178177 -ConfirmSk01Copy -LabCopy Valencia2026
& $probe -Uid 67178177 -ConfirmSk01Copy -LabCopy Solihull
# Con Valencia 2025 abierto, repetir Valencia2026 y pedir Valencia2025.
& $probe -Uid 67178177 -ConfirmSk01Copy -LabCopy Valencia2026
& $probe -Uid 67178177 -ConfirmSk01Copy -LabCopy Valencia2025
# Tras volver a Valencia 2026, invertir las peticiones.
& $probe -Uid 67178177 -ConfirmSk01Copy -LabCopy Valencia2025
& $probe -Uid 67178177 -ConfirmSk01Copy -LabCopy Valencia2026
```

Comprobar que el contexto obsoleto aporta motivo sin UID ni valor.
Las respuestas actuales conservan la referencia visual, el mismo
inicio de proceso y contadores distintos. Los contadores absolutos
dependen de las cargas de esa instancia. Repetir la lectura externa
T016 al volver al corte de 2026. El banco local
`viabilidad/world-isolation/WorldProbe.csproj` permite sujetar una
captura externa hasta la siguiente carga. Con Valencia 2025 activo,
compilarlo y pasar PID, ticks UTC, ruta de su copia y ruta a un archivo
de señal que aún no exista. Enviar `{"UiUids":[67178177],"Players":[]}`
por entrada estándar. Al ver `READY`, volver a la copia de 2026 y
crear el archivo de señal. Tras `RECHECK` se espera código 2 y ausencia
de resultado de captura.

Para el ensayo de recuperación, con una referencia vinculada de A-01
y un lote sellado en ese mismo directorio privado:

```powershell
$run = Join-Path $env:LOCALAPPDATA 'FM27Analyzer/SK-01/ensayo-a01'
$project = 'src/FM27Analyzer.Extraction.Poc/FM27Analyzer.Extraction.Poc.csproj'
$truncated = Join-Path $run 'lote-truncado-recuperacion.json'
[IO.File]::WriteAllText($truncated, '{"content":')
dotnet run --project $project -c Release --no-build -- check-recovery --reference (Join-Path $run 'referencia-revinculada.json') --batch (Join-Path $run 'lote-1.json') --retry $truncated
dotnet test FM27Analyzer.slnx -c Release --no-build --filter 'FullyQualifiedName~SampleRecoveryTests|FullyQualifiedName~CaptureSessionTests'
```

El diagnóstico distingue la primera aceptación, el reenvío y la
recuperación del mismo cierre y fecha tras rechazar el truncado.
Para integrar el aislamiento, copiar el registro en el directorio del
manifiesto manteniendo sus permisos privados y usar `aislamiento.json`
en la propiedad `isolation` de una copia de `run.json`. Ejecutar `verify`
con esa copia. [Resultados del ensayo](evidencia.md#t024t026--recuperación-y-cambio-real-de-carrera)

### Contrastes adicionales y cierre

Para repetir E-04 y E-05 en el banco del Valencia, fijar en pantalla
las referencias de CA y PA del mismo jugador con permiso previo y
advertencia. Confirmar copia, proceso, carga y fecha. Compilar
`%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad/capability-probe/WorldProbe.csproj`
y pasar PID, ticks UTC de inicio y ruta de la copia como argumentos.
Enviar por entrada estándar un único JSON con `UiUids` de la
referencia fijada, `Ability` con `Uid`, `Ca` y `Pa`, y
`ExpectedLoads`. Mantener esa entrada en memoria. El resultado
se limita a clases, recuentos y estado de los campos y permisos.

E-05 requiere inventarios del mismo corte y ámbito. Para declarar un
universo completo, contrastar el total independiente y el conjunto de
UID si existe. El censo de ojeados y la muestra de plantilla aportan
una referencia de subconjunto. E-06 usa la ficha para verificar el
intervalo y el editor oficial como referencia del exacto, con
autorización independiente. Registrar si se usan resultados reales
anteriores, con su informe original y fecha.

```powershell
$run = Join-Path $env:LOCALAPPDATA 'FM27Analyzer/SK-01/ensayo-a01'
$project = 'src/FM27Analyzer.Extraction.Poc/FM27Analyzer.Extraction.Poc.csproj'
dotnet build FM27Analyzer.slnx -c Release -warnaserror
dotnet test FM27Analyzer.slnx -c Release --no-build
dotnet build FM27Analyzer.slnx -c Laboratorio "-p:FM27GameRoot=$fm27Root" -warnaserror
dotnet test FM27Analyzer.slnx -c Laboratorio --no-build
dotnet run --project $project -c Release --no-build -- verify --run (Join-Path $run 'run.json') --out (Join-Path $run 'informe-comun.json')
dotnet run --project $project -c Release --no-build -- verify --run (Join-Path $run 'control-bloqueado.json') --out (Join-Path $run 'control-bloqueado-revalidado-20261124.json')
```

Comparar el informe común con la evidencia por escenario y clase.
Comprobar que el modo bloqueado mantiene A-01 sin acreditar.
[Resultados y huellas](evidencia.md#t027t034--capacidades-adicionales-e-informe-común)

### Atributos visibles y familiaridad

Fijar en `%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad/referencia-prioritarios.json`
una referencia con el contrato de A-01 para jugadores confirmados en el
primer equipo del Valencia. Tomar sólo atributos exactos visibles en sus
fichas. Cada celda usa la escala 1–20 y la misma fuente independiente para
el valor y su visibilidad. La sonda se limita a Steam 26620374 y a la
copia y corte SK-01 del Valencia.

```powershell
$fm = Get-Process -Name fm
$ticks = $fm.StartTime.ToUniversalTime().Ticks
$project = 'src/FM27Analyzer.Extraction.Poc/FM27Analyzer.Extraction.Poc.csproj'
dotnet run --project $project -c Release --no-build -- probe-priority --confirm-sk01 $fm.Id $ticks $copyPath $expectedLoads
```

Usar la ruta local de la copia y el contador de cargas comprobado
en la misma instancia. Verificar los resultados y los recuentos por
grupo. Con permisos base, se esperan cero comprobaciones de
familiaridad numérica. Pasar una carga distinta debe terminar con
código 2 y sin resultado.

Para el contraste de la escala numérica, con permiso independiente, usar
«Editor del juego → Atributos del jugador → Posiciones», en modo
de consulta. Fijar los códigos de puesto y lado observados y pasar a
la misma orden, con `--compare-editor-positions`, un objeto JSON por
stdin que asocie cada UID de la muestra a su mapa de puesto y
valor. Pasar un valor fuera de 0–20 debe bloquear la petición.
La salida no contiene los valores del editor. Las etiquetas de la
ficha se registran por separado como categorías. Para los estados de
ojeo, usar A-01 y comprobar la ficha y la tabla con el filtro activo.
[Resultados del contraste](evidencia.md#t021--atributos-visibles-y-familiaridad-por-posición)

## Resultados esperados

| Caso | Estímulo | Aceptación y registro |
| --- | --- | --- |
| A-01 · E-01/02 | Capturar dos veces la muestra prefijada en el mismo corte y cotejarla en el juego. | Al menos 30 UID únicos verificados entre plantilla y conocidos u ojeados. Acreditar club, una posición y un atributo exacto visible en un portero y un jugador de campo. Contrastar el intervalo y el desconocimiento. Los campos acreditados coinciden con la referencia. Cero duplicados y cambios atribuibles a la captura. |
| Visibilidad base · E-03 | Probar rango, desconocido e identidad excluida con permisos base. Repetir con dos fixtures sintéticas iguales en lo visible y distintas en secretos. | Rango íntegro, estado de conocimiento conservado, CA/PA apagados e identidades fuera del ámbito excluidas de filas y recuentos. En fixtures coinciden muestra y diagnósticos normalizados. Identificar esa evidencia como sintética. |
| CA, PA y exactos ocultos · E-04/06 | Con permiso expreso y advertencia para CA/PA, contrastar cada campo con una referencia independiente lícita en solo lectura. Probar también la revocación. | Acreditación por campo, sin valores especiales en archivos ni diagnósticos. Falta de referencia, capacidad o coincidencia produce «No disponible» con motivo. Al revocar, desaparece el resultado afectado y se conserva el rango observado si sigue autorizado. |
| A-02 · E-05 | Con permiso mundial expreso, sondear el universo cargado con límites fijados. Contrastar con la referencia de la misma población y probar cancelación. | Registrar completos, parciales o indeterminados según la referencia, total, UID y omisiones del contrato. La prueba puede cerrar A-01 aunque el universo completo no se acredite. |
| A-03 · E-07/09 | Recargar, alternar dos carreras reales con club y UID coincidentes, cambiar rama o mánager, repetir un lote y cambiar permiso o conocimiento durante una captura. | Se verifica de nuevo el vínculo y se descarta trabajo obsoleto. Cero cruces y duplicados. Si faltan las copias adecuadas, ejecutar el caso sintético y registrar la acreditación real como pendiente. |
| Fallos · E-08 | Ensayar compilación incompatible, partida cerrada, lote truncado, desconexión y una vía válida independiente cuando proceda. | Diagnóstico de causa y capacidad afectada. Descartar la captura inválida. La última muestra del mismo contexto conserva su fecha y limitación si sigue autorizada. |

Cerrar con los nueve escenarios marcados como aprobados, fallidos u omitidos
y su clase de evidencia. A-01 se acredita conforme a SC-004 de la
[especificación](spec.md#criterios-de-éxito-obligatorio). Entregar el informe
con la matriz de capacidades y sus límites, y la muestra local autorizada si
está disponible.
Si el entorno impide la captura de FM27, conservar el contraste sintético y
la causa del bloqueo como evidencia del trabajo realizado. La prueba real
queda sin acreditar.
