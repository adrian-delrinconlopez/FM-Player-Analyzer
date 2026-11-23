# Evidencia de SK-01 · Comprobación preliminar

El 23 de noviembre de 2026 se repitieron las pruebas de Setup y Foundational en Windows 11 x64 con
FM27 abierto. La revisión de código fue `f864e2d`, el SDK fue .NET 10.0.404 y la instalación del juego
correspondía a Steam build 26620374. Se preparó en FM27 una copia local del Valencia CF con **Guardar
partida como → Local**, en el corte del 1 de agosto de 2026 a las 21:45. La evidencia de la ficha
y los identificadores observados se guardaron fuera de Git.

## Resultados

| Comprobación | Resultado | Evidencia |
| --- | --- | --- |
| Base de SK-01 | Compilación Release con cero errores y advertencias. Pasaron las 41 pruebas, incluidas las de cancelación y publicación del lote. | Sintética |
| Lectura externa de control | `OpenProcess` concedió consulta limitada y lectura sin elevación. En un proceso auxiliar con un búfer de cuatro bytes conocido, `ReadProcessMemory` devolvió los cuatro bytes previstos dos veces. | Sintética |
| Lectura externa en FM27 | Dos lecturas de dos bytes en la base del módulo principal coincidieron con la firma de `fm.exe` en disco. El tamaño y la fecha de modificación de la copia coincidieron antes y después. | Real, sobre la imagen del ejecutable |
| BepInEx | `LogOutput.log` registró el arranque completo de BepInEx 6.0.0-be.791 en esta ejecución. FM ActionLink - FMSH 2.2.3 registró la fecha del juego, que coincidió con el corte visible, y un identificador al abrir la ficha. La procedencia que declara el plugin para la fecha es memoria. El identificador de cabecera queda pendiente de cotejo independiente. | Arranque observado y resultados declarados por un plugin de terceros ya instalado |
| Referencia del campo | Se fijó «Pase» como primer candidato. Su valor exacto se observó en la ficha de un jugador del primer equipo, en la sección Técnico. Queda pendiente fijar el UID por una vía independiente de los dos ensayos. | Real, observación directa de la pantalla |

## Qué se puede ensayar por cada vía

| Vía | Campos candidatos y alcance de la evidencia |
| --- | --- |
| BepInEx | Empezar por UID y un atributo exacto de una ficha visible. Si coinciden, contrastar club, posición y lado, categorías de familiaridad, intervalos y desconocimiento en las vistas autorizadas. FM View Exporter documenta exportación de filas y columnas de plantilla, búsqueda y ojeo. Es una referencia de terceros para vistas. [FM View Exporter](https://github.com/ElectroHugin/FM-View-Exporter) |
| Lectura externa | Empezar por el mismo UID y atributo. El perfil publicado de FMLibreEditor v0.5.3 identifica esta build de Steam y sirve para estudiar una ruta acotada al campo. Cualquier campo adicional requiere comprobar por separado su interpretación, UID, corte y visibilidad en este equipo. [Publicación de FMLibreEditor v0.5.3](https://github.com/tozmoz/FMLibreEditor/releases/tag/v0.5.3) |

CA, PA, exactos ocultos y cobertura mundial siguen pendientes de sus contrastes y permisos específicos.
A-01 sigue sin acreditar. Las dos lecturas de la firma del ejecutable prueban el transporte de bytes en
esta instalación. Falta una correspondencia comprobada entre los bytes y un dato del futbolista.

## Reproducción y siguiente ensayo

1. Repetir `dotnet build FM27Analyzer.slnx -c Release -warnaserror` y
   `dotnet test FM27Analyzer.slnx -c Release --no-build`. Consultar el arranque en
   `BepInEx/LogOutput.log` y cotejar la compilación en el manifiesto de Steam.
2. Para repetir el control externo, seleccionar la instancia `fm` de FM27, obtener la base de
   `MainModule` y solicitar `PROCESS_QUERY_LIMITED_INFORMATION | PROCESS_VM_READ` con
   `OpenProcess`. Leer dos veces dos bytes desde la base y compararlos con los dos primeros
   bytes de `fm.exe`. Cerrar el handle al terminar. [Derechos de acceso al proceso](https://learn.microsoft.com/en-us/windows/win32/procthread/process-security-and-access-rights) · [ReadProcessMemory](https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-readprocessmemory)
3. Para T014–T017, fijar UID y «Pase» desde FM27 en la copia, con la fecha y perspectiva observadas.
   Aplicar T009 en cada adaptador y hacer dos lecturas del mismo campo y UID antes de cambiar de
   pantalla, partida, fecha o permisos. Registrar vía, revisión, valor o estado, procedencia,
   coincidencia, duración y memoria respecto a una línea base comparable. Revisar las condiciones
   del proveedor antes de incorporar un componente externo. [Guía de validación](quickstart.md) · [Aviso de Steam](https://store.steampowered.com/eula/4530370_eula_0)

La instantánea de un segundo en esta sesión sólo sirve como control del banco. La medición de coste
del capturador queda para las pruebas de campo. Los registros locales de esta comprobación son
`%LOCALAPPDATA%/FM27Analyzer/SK-01/preflight-20261123-144915.json` y
`%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad-valencia-20260801.json`.

## Contraste tras recarga con la interfaz predeterminada

Se verificó una nueva instancia de FM27 y el guardado local abierto desde el diálogo de
guardado, que se canceló. La tabla Primer equipo mostró el UID del jugador fijado y su ficha
volvió a mostrar «Pase» con el mismo valor exacto. El UID coincidió con el identificador
registrado por FM ActionLink - FMSH 2.2.3. Esta coincidencia acredita el UID emitido por
ese plugin para la ficha observada. El plugin registró tres mensajes coincidentes en
la sesión. La muestra independiente de un UID y un campo se fijó antes del ensayo de
«Pase» en `%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad/referencia.json`.

**Memoria externa.** Se compararon tamaño de imagen y marca PE de `game_plugin.dll` con
el perfil publicado para la build, y se leyeron dos veces los cuatro bytes del campo
de fecha documentado. Ambas lecturas devolvieron **2026-08-01**, igual que la pantalla.
Se comprobó identidad del proceso tras cada intento. El total de lectura fue de
96 bytes incluidos los de la huella, sin leer registros de jugadores. El tamaño
y la fecha de modificación de la copia coincidieron antes y después. Este control
aporta evidencia de fecha, con hora observada desde la pantalla. La comprobación
completa de sesión de T009 queda para el adaptador, incluido detectar una recarga
que conserve la fecha. La evidencia local está en
`%LOCALAPPDATA%/FM27Analyzer/SK-01/contraste-20261123-150320.json`.

Para reproducir, comprobar copia y fecha en FM27, localizar el módulo cargado,
comparar su huella PE con el perfil fijado y aplicar la codificación de la fecha
documentada por la fuente. Leer dos veces con los derechos indicados arriba.
El perfil identifica la ubicación de la fecha. Su código de localización referencia
el formato de ordinal de día y año. [Perfil Steam 26620374](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/profiles/fm27/steam-26620374.toml) ·
[Decodificación](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/crates/fmlibre-core/src/locate.rs)

**Estado de las tareas.** T014 cuenta con una referencia de pantalla fijada. T015–T017
siguen abiertas para contrastar dos lecturas de «Pase» por vía y elegir si ampliar.
La compilación Release volvió a terminar con cero advertencias y errores y pasaron
las 41 pruebas sintéticas. El inventario de fuentes para las entregas completas
se mantiene en la [matriz TO-BE](matriz-fuentes-to-be.md).
