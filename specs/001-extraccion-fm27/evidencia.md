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

## Contraste de CA, PA y Pase en un jugador del primer equipo

El propietario autorizó el 23 de noviembre de 2026 dos lecturas de CA y dos de PA para
José Gayà en la copia del Valencia, tras la advertencia de que muestran la capacidad
exacta fuera del ojeo habitual. En Steam 26620374, corte **2026-08-01 21:45**, ambos campos
coincidieron por separado con las columnas CA y CP de la plantilla. El **Editor del
juego → Atributos del jugador → Atributos clave** mostró los mismos valores en
«Calidad actual» y «Calidad potencial». Se cotejaron esos campos, con escala 1–200, y
se cerró el editor tras consultar la ficha. El programa conservó únicamente el
resultado agregado. [Editor oficial de FM27](https://www.footballmanager.com/es/fm27/funcionalidades/fm27-motor-de-partidos-2d-editor-del-juego-y-mejoras-mas-profundas)

El ensayo externo en C# usó la huella del perfil Steam. Consultó claves del índice de
clubes hasta el ID del Valencia y verificó la relación club/equipo y el UID entre los
miembros del primer equipo. Comprobó instancia, metadatos de la copia, fecha, contador
de cargas y vínculo del jugador con el equipo mediante `CaptureSession`.
`CaptureAuthorization` comprobó CA y PA por separado. El contraste leyó **13.833 bytes
en 12 ms**, incluidos los metadatos y comprobaciones de contexto, con derechos de
consulta y lectura. Con permisos base y el mismo selector, dos lecturas de «Pase»
coincidieron con la ficha, con **13.475 bytes en 15 ms**.
[Perfil de la build](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/profiles/fm27/steam-26620374.toml) ·
[Disposición de campos](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/profiles/fm27/layout.toml)

El código del ensayo y el resumen agregado se conservan en
`%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad/ability-probe` y
`%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad/ca-pa-20261123-151319.json`. Para repetirlo,
compilar `AbilityProbe.csproj` con .NET 10, confirmar copia y corte en FM27 y pasar PID,
ticks UTC de arranque y ruta de la copia. El control de «Pase» usa el argumento `pase` y
la referencia visible por entrada estándar. El contraste de CA/PA usa entrada estándar
para las dos referencias del editor, con el consentimiento específico de la sesión.
El resultado cubre un jugador propio en una sesión. Quedan pendientes el adaptador
de producto, la revocación contrastada en juego, el PA codificado, las lecturas de
BepInEx y la ampliación a otros ámbitos. Las tareas de esos ensayos siguen abiertas.

En el Editor del juego del club, «Mostrar todos los jugadores en la búsqueda» ya estaba
activado. Es una referencia candidata para A-02. El total de esa búsqueda requiere
cotejo con el universo cargado y un contraste aparte para el ojeo conocido.

## Ojeados, jugadores sin informe y universo cargado

El 23 de noviembre de 2026 el propietario autorizó expresamente el ensayo mundial,
los exactos bajo máscara, los atributos ocultos y CA/PA. Se usó la misma copia local,
Steam 26620374, versión en pantalla 27.1.0.2505117 y corte de juego
2026-08-01 21:45. Se añadieron y cotejaron los UID en la búsqueda de FM27.
Los criterios de búsqueda anteriores se repusieron tras medir los recuentos.

| Fuente y alcance | Resultado | Interpretación |
| --- | --- | --- |
| FM27, búsqueda con los tres criterios previos y el interés original | 29 con «Sólo jugadores ojeados» y 461 con el conmutador desactivado | Dos recuentos de la interfaz para los mismos criterios |
| FM27, sin los tres criterios y sin filtro de interés | 356 con «Sólo jugadores ojeados» y 19.836 con el conmutador desactivado | La búsqueda amplia muestra hasta 10.000 filas. Se vieron UID repetidos en esa vista. El total de resultados requiere reconciliación antes de usarlo como denominador de UID. |
| Tabla de jugadores de la carrera en memoria | 19.917 entradas y 19.917 UID únicos en dos recorridos con conjuntos idénticos | 19.887 de clase jugador y 30 de clase jugador y empleado. Cero UID nulos, repetidos o de clases sin clasificar en esta tabla. |
| Muestra independiente de la interfaz | 15 de 15 UID distintos encontrados en memoria | Incluye ojeados, el caso sin informe y filas de la búsqueda amplia. Evidencia parcial por identidad. |

La diferencia entre los dos recuentos amplios es de 81. El resultado de A-02 queda
indeterminado para el total mundial independiente. El total de la búsqueda y el
inventario en memoria usan unidades de recuento que aún deben reconciliarse. Falta
obtener el conjunto de UID de todas las filas o por particiones comparables.

| Caso de contraste | Coincidencias en dos lecturas externas |
| --- | --- |
| Jugador ojeado con informe | CA, PA, Pase, Anticipación, Aceleración, altura y fin del contrato principal. Los atributos visibles se cotejaron en la ficha del mánager y CA/PA en las columnas de la búsqueda. |
| Portero sin informe | CA, PA, valores exactos de Pase, Anticipación, Aceleración y Reflejos, Consistencia, Profesionalidad y familiaridad de portería. Se cotejaron con el editor oficial. También coincidieron altura, fin del contrato principal y fin de cesión con la ficha, y la formación en el club propietario bajo la regla de 15 a 21 años con «Formado en». |
| Referencia BepInEx existente | FM ActionLink volvió a registrar el UID del portero. Coincidió con la columna `Unique ID`. La lectura automatizada de las celdas de ojeo y de los atributos por BepInEx sigue pendiente. |

La ficha del portero mostraba un rango de Pase y personalidad como «Se necesita
ojeo». Se suministraron esos estados de la pantalla al control de política. Al revocar
los permisos especiales en el ensayo, el control canceló la publicación pendiente,
bloqueó CA/PA y los exactos ocultos y conservó el rango y el desconocimiento con
ámbito mundial. El control con permisos base excluyó el caso clasificado como sólo
mundial en la muestra. El lector de memoria aporta el valor exacto y la interfaz
aporta el conocimiento. Sigue pendiente derivar el inventario completo de ojeo
y detectar automáticamente su revisión por mánager.

El ensayo en C#/.NET 10 verificó la huella PE, PID, hora de arranque, metadatos de la
copia, fecha, contador de cargas y límites de la colección. Consultó la tabla
documentada por tipo y UID. La lectura de campos se acotó a los dos UID fijados y se
revalidó contra la política de sesión. En la última ejecución leyó 805.424 bytes en
223 ms y el proceso de ensayo alcanzó 40.972.288 bytes de memoria residente.
Los límites fueron 250.000 entradas, 10 MB leídos y 12 s. Se usaron los derechos
de consulta y lectura de Windows. [Perfil Steam 26620374](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/profiles/fm27/steam-26620374.toml) ·
[Relaciones y formación](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/docs/recon/fm27-nationality.md)

El código del banco y el resumen agregado están en
`%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad/world-probe` y
`%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad/mundo-ojeo-20261123-192018.json`.
Se compila `WorldProbe.csproj` en Release con .NET 10. Recibe PID, ticks UTC del
arranque y ruta de la copia. La referencia se pasa por entrada estándar como un
objeto con `UiUids` y `Players`. Sus campos constan en los tipos `References` y
`Screen`. El JSON del ensayo guarda recuentos y coincidencias, sin valores especiales.
Las 42 pruebas del repositorio pasaron en Release, incluido el caso nuevo de
cancelación ante datos de contexto inválidos. Los ensayos formales y su integración
en el contrato de informes siguen abiertos.
