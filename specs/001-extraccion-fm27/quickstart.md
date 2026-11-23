# Guía de validación · SK-01

**Estado** Procedimiento previsto para la implementación. Los proyectos, comandos
del validador y capturas del juego se generarán en esa fase.

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
  capturar. Seleccionar hasta tres jugadores para el ensayo de viabilidad y
  una lista previa de al menos 30 futbolistas para A-01. Cotejar UID y campos
  con fichas o vistas de FM27 del mismo corte que sean independientes de
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
El procedimiento de captura del adaptador que pase la prueba pequeña se anota
con su revisión y sus pasos o comando exactos. Las dos vías candidatas se
comparan en el mismo corte y con el mismo campo visible. Para A-01, usar la
vía elegida, permisos base y dos capturas consecutivas con los mismos UID y
alcance. Dejar el juego en el mismo corte entre capturas.

Con los lotes y la referencia local preparados según el
[contrato](contracts/captura.md), ejecutar el comando de contraste previsto.
El directorio de ejemplo representa el directorio local de ese ensayo:

```powershell
$sk01Run = Join-Path $env:LOCALAPPDATA 'FM27Analyzer\SK-01\ensayo-a01'
dotnet run --project src/FM27Analyzer.Extraction.Poc/FM27Analyzer.Extraction.Poc.csproj -c Release -- verify --run (Join-Path $sk01Run 'run.json') --out (Join-Path $sk01Run 'informe.json')
```

El manifiesto referencia los dos lotes y el cotejo directo de FM27. Registrar
duración de cada captura, memoria pico del capturador y del validador, y el
coste observado dentro de FM27 si aplica, con la misma técnica de medida y línea
base. Usar el mismo hardware, revisión, corte, alcance y método de referencia
al comparar vías. Anotar cancelaciones y resultados parciales.

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
[especificación](spec.md#criterios-de-éxito-obligatorio). Entregar una muestra
local autorizada y el informe con la matriz de capacidades y sus límites.
Si el entorno impide la captura de FM27, conservar el contraste sintético y
la causa del bloqueo como evidencia del trabajo realizado. La prueba real
queda sin acreditar.
