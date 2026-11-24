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

**Estado en ese corte del ensayo.** T014 quedó con una referencia de pantalla fijada
para contrastar dos lecturas de «Pase» por vía y elegir si ampliar.
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

## T015 · Lectura de un atributo visible mediante BepInEx

La sonda propia se cargó en BepInEx 6.0.0-be.791 e Il2CppInterop 1.5.3
en FM27 Steam 26620374 con el runtime de juego .NET 6.0.7. Se volvió a cargar
la copia `SK-01 Valencia 2026-08-01`, con el Valencia CF a 1/8/2026 21:45,
y se añadió «Pase» junto a «Unique ID» en Primer equipo. La ficha y la fila de
José Gayà coincidieron con las dos lecturas de la sonda.

| Lectura | UID | «Pase» | Fecha | Duración del recorrido |
| --- | --- | --- | --- | --- |
| FM27, ficha y fila identificada en pantalla | 67178177 | 13 | 2026-08-01 | |
| BepInEx, primera petición | 67178177 | 13 | 2026-08-01 | 23,5 ms |
| BepInEx, segunda petición | 67178177 | 13 | 2026-08-01 | 10,6 ms |

Ambas peticiones vieron el mismo proceso y contador de cargas igual a 1. Se
leyó de la tabla UI Toolkit el texto de las celdas UID y «Pase». Se comprobaron
la huella de la build, la fecha y el contador de cargas antes de leer y antes de
publicar. La copia, hora de juego y perspectiva se confirmaron en la interfaz.
El tamaño y la fecha de modificación de la copia coincidieron antes y después.
Durante el ensayo se reinició FM27 para cargar el DLL de laboratorio y se
ajustaron las columnas de la vista. Se preservaron los metadatos del guardado.
[Estructura de la tabla en FM View Exporter](https://github.com/ElectroHugin/FM-View-Exporter/blob/2ed5898f6e3b8597b6b8ec5401f41d16b961cdfd/src/FMViewExporter/Export/PlayerTable.cs) ·
[Perfil Steam 26620374](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/profiles/fm27/steam-26620374.toml)

Dos intentos de la primera revisión devolvieron «Límite de recorrido visual».
La revisión contrastada evitó entrar en el contenido de las demás celdas.
El menú sin carrera, una fecha incorrecta con la copia cargada y un UID
ausente produjeron diagnósticos con UID y valor nulos. El tiempo medido
corresponde a la operación dentro de Unity. Quedan por medir memoria atribuible
y coste de fotograma con una línea base equivalente. La muestra sólo acredita
un jugador propio y un atributo exacto en una vista. Falta probar el recorrido
de ojeados y la lectura de rangos, desconocimiento y exclusión en ese ámbito.

El registro agregado está en `%LOCALAPPDATA%/FM27Analyzer/SK-01/bepinex.json`.
Se completó T015. La compilación Release y Laboratorio terminó con cero
advertencias y errores y pasaron las 42 pruebas de Release. T016 requiere
incorporar y repetir el contraste externo mediante su adaptador de la PoC.
T017 decidirá qué vía ampliar a la muestra de al menos 30 UID de A-01.
[Guía para repetir T015](quickstart.md#sonda-bepinex-t015) · [Tareas](tasks.md)

## T016 y T017 · Comparación de vías en la misma sesión

Se incorporó la sonda externa acotada a la consola .NET 10. El punto de
entrada `probe-external --confirm-sk01` exige la referencia previa del
primer equipo y la confirmación de copia, perspectiva y corte. La
sonda verificó la imagen cargada de la build Steam 26620374, el proceso,
la fecha, el contador de cargas y la relación UID, equipo y club antes
de usar la proyección del campo. Usó derechos mínimos de consulta y
lectura y un presupuesto de 96 KiB y dos segundos. El control con un
instante de arranque distinto se bloqueó con código 2 y sólo un
diagnóstico. [Derechos de acceso a procesos](https://learn.microsoft.com/en-us/windows/win32/procthread/process-security-and-access-rights) ·
[Perfil Steam 26620374](https://raw.githubusercontent.com/tozmoz/FMLibreEditor/v0.5.3/profiles/fm27/steam-26620374.toml)

| Vía | Observación y estado | Evidencia de visibilidad | Medida |
| --- | --- | --- | --- |
| FM27, ficha y Primer equipo | UID 67178177, «Pase» 13, exacto en escala 1–20 | Pantalla del mánager del Valencia en SK-01 | Referencia independiente |
| Memoria externa | Dos observaciones de 13 para el mismo UID, exacto visible | Confirmación previa del operador en FM27. El dato se lee con el perfil de memoria. | 12,1 ms para localización, comprobaciones y lecturas antes de publicar. 13.431 bytes leídos y 38.113.280 bytes de memoria residente pico del proceso externo. |
| BepInEx | Dos observaciones de 13 para el mismo UID, exacto visible | Texto de celdas «Unique ID» y «Pase» en la tabla de UI Toolkit de FM27 | 10,4 y 11,1 ms de recorrido en dos peticiones. |

Las dos vías observaron la misma instancia, contador de cargas 1 y fecha
1/8/2026, con hora 21:45 confirmada en pantalla. La copia conservó
43.957.632 bytes y la misma fecha de modificación. Se midió el proceso de
FM27 en reposo y antes y después de cada par. El intervalo de referencia
duró 2,03 s. El conjunto residente del juego varió de 3.288.088.576 a
3.288.285.184 bytes en reposo. Se mantuvo en 3.288.285.184 bytes durante
la lectura externa y la medida final del par BepInEx fue 3.289.915.392
bytes. Las ventanas son distintas y el muestreo de FM27 no mide el pico
atribuible al plugin ni el tiempo de fotograma. Los tiempos internos
miden operaciones distintas.

T017 selecciona BepInEx para ampliar A-01. En esta prueba la tabla
proporciona a la vez el valor exacto y su visibilidad. Para la vía
externa se acreditó el valor, con visibilidad contrastada por separado.
Quedan por probar en BepInEx los rangos, el desconocimiento, la exclusión
y el recorrido de varias páginas. Continúa abierta la elección por
familia para el producto y para los campos con permisos especiales.

La evidencia privada se conserva como `SK-01/lectura-externa.json` y
`SK-01/decision-via.json` bajo `%LOCALAPPDATA%/FM27Analyzer`. Release
compiló sin advertencias ni errores. Pasaron los 14 controles de sesión
y las 42 pruebas de la batería. [Reproducción de T016 y T017](quickstart.md#sonda-externa-t016-y-comparación-t017)

## T018 · Referencia independiente para A-01

Se fijó el 23 de noviembre de 2026 a las 19:57, hora de Madrid, en el
mismo corte de juego y con política base. Se transcribieron 20 filas de
Primer equipo y 10 de «Sólo jugadores ojeados», antes de capturar lotes
de A-01. En ojeo se eligieron nueve filas de la primera página y una del
final. La búsqueda mostraba 29 resultados con ese conmutador activo,
los tres criterios previos y el filtro de interés original. Al desactivarlo
volvió a mostrar 461 con los mismos criterios.

| Caso | Referencia de FM27 |
| --- | --- |
| Portero de la muestra | Stole Dimitrievski, Valencia CF, posición POR, Pase 10 y Reflejos 16. Identidad y Pase en la tabla, y club, posición y atributos cotejados en ficha. |
| Jugador de campo de la muestra | José Gayà, Valencia CF, mejor posición CR (I) y Pase 13. La ficha confirma el lado izquierdo y muestra DF/CR/ME/MP (I). |
| Intervalo visible | Pase 2–7 en Tomáš Hašler y 13–16 en Reda Belahyane. Se conservan ambos extremos. |
| Desconocimiento explícito | «Marcaje» de Tomáš Hašler aparece con guion en la ficha y su personalidad muestra «Se necesita ojeo». La referencia registra Marcaje como desconocido y valor nulo. |
| Exclusión | Un jugador sin informe aparecía en la búsqueda con el conmutador desactivado. Se comprobó su ausencia al recorrer las 29 filas con «Sólo jugadores ojeados». El estímulo queda en el registro privado de controles. |

El contrato C# aceptó el JSON y selló un lote de comprobación en memoria
con 30 UID únicos y 34 observaciones. Las 34 observaciones corresponden
a 30 de Pase, dos posiciones, un caso de Reflejos y un estado de
desconocimiento. Fuera de los dos casos de Primer equipo con ficha cotejada,
sus filas acreditan pertenencia a la plantilla visible. El club de
contrato de esas filas queda sin afirmar.

La referencia está en `SK-01/ensayo-a01/referencia.json` con SHA-256
`9EE8711965DABB0483E6EBE5B1E160392FEED79E355571EF569938164ED0B6A5`.
`controles-de-pantalla.json` conserva los casos de comprobación en el
mismo directorio privado. La operación ajustó las columnas de la vista de
ojeo, repuso el conmutador original y conservó los metadatos de la copia.
A-01 queda pendiente de dos capturas independientes de la referencia y
su cotejo mediante `verify`. [Guía del ensayo](quickstart.md#a-01)

## A-01 · Captura real repetida y cotejo

El 23 de noviembre de 2026 se reinició FM27 para cargar la sonda 0.2.0
y se volvió a abrir la copia SK-01 del Valencia en Steam 26620374.
La referencia original mantuvo su huella. Su copia de trabajo actualizó
la vinculación y se fijó antes del primer lote. Las seis peticiones
usaron la misma instancia, contador de cargas y fecha 1/8/2026. En
pantalla se mantuvieron las 21:45 y la perspectiva del Valencia CF.
Cada vuelta leyó tres páginas bajo permisos base y devolvió 20, 9 y 1
identidades. La navegación y el filtro de ojeados se controlaron desde FM27.

| Cotejo contra T018 | Coincidencias en ambos lotes | Discrepancias |
| --- | ---: | ---: |
| UID y vínculo de club | 30 de 30 | 0 |
| Pase exacto o en intervalo | 30 de 30 | 0 |
| Mejor posición | 2 de 2 | 0 |
| Reflejos | 1 de 1 | 0 |
| Marcaje desconocido | 1 de 1 | 0 |

Las 30 identidades por lote se limitaron a la muestra autorizada.
Se cotejó el vínculo de club en las 12 filas con referencia de club.
El resto conserva el vínculo sin verificar. El alcance de ojeo procede
de la vista «Sólo jugadores ojeados» confirmada por el operador. Los
dos rangos conservaron ambos límites y el Marcaje desconocido se
publicó con valor nulo. El jugador de control y los campos CA/PA
quedaron fuera de los lotes y de sus recuentos. El control sintético de
igualdad ante cambios en los datos ocultos pasó por separado. El primer
intento de sellado bloqueó etiquetas de formato en los nombres de club.
Se normalizaron las etiquetas de presentación y se volvieron a cotejar
los mismos registros crudos de las seis peticiones.

| Lote | Tiempo de recorrido en Unity | Tiempo de extremo a extremo | Pico residente acumulado de FM27 |
| --- | ---: | ---: | ---: |
| 1 | 63,1 ms | 2 min 46,4 s | 3.216.834.560 bytes |
| 2 | 35,6 ms | 35,9 s | 3.366.006.784 bytes |

El tiempo de extremo a extremo incluye navegación manual y, en el
primer lote, preparación de columnas de ojeo. El pico de FM27 incluye
el juego y el cargador. Sigue pendiente aislar el coste de la sonda por
fotograma con una línea base equivalente. La captura preservó la fecha
del juego y los metadatos del guardado de 43.957.632 bytes. Se repuso
el filtro inicial y quedaron las nuevas columnas para repetir el ensayo.

`verify` acreditó A-01 con E-01 a E-03 reales y E-03 sintético. Una
segunda ejecución produjo la misma huella de informe. El control
bloqueado devolvió un informe con los nueve escenarios y A-01 sin
acreditar. Un manifiesto truncado terminó con código 2 y sin informe.
Se guardaron `run.json`, `informe.json`, los dos lotes y los controles en
`%LOCALAPPDATA%/FM27Analyzer/SK-01/ensayo-a01`. El SHA-256 del informe
es `386AF1E457A6359397EB2037006D9E10E8E7711E11D10CD841DE0CCFCDE5E08E`.
E-04 a E-09 figuran como omitidos en ese informe hasta integrar la
evidencia de sus tareas. T021 queda abierta para completar los grupos
de atributos, posiciones con familiaridad y acreditación de alcance
más allá de la selección fijada. [Reproducción de A-01](quickstart.md#a-01)

## Censo de ojeados y recarga

El 24 de noviembre de 2026 se recorrió la tabla de ojeados de la copia
SK-01 con la sonda BepInEx 0.3.0 en Steam 26620374. Se mantuvieron el
Valencia CF y el corte de 1/8/2026 a las 21:45. Se confirmó el nombre
de la copia en el diálogo «Guardar partida como» y se canceló el diálogo
sin guardar. La copia conservó sus 43.957.632 bytes y su fecha de
modificación UTC `2026-11-23T13:51:14.0444918Z`.

| Pasada | Total mostrado y UID únicos leídos | Pase exacto / intervalo / desconocido | Páginas | Cargas | Tiempo total / lectura acumulada |
| --- | ---: | ---: | ---: | ---: | ---: |
| Tres criterios e interés original | 29 / 29 | 26 / 3 / 0 | 2 | 1 | 0,41 s / 8,72 ms |
| Sólo ojeados, primera | 356 / 356 | 242 / 114 / 0 | 29 | 1 | 7,68 s / 53,19 ms |
| Sólo ojeados, segunda | 356 / 356 | 242 / 114 / 0 | 29 | 1 | 5,01 s / 39,78 ms |
| Sólo ojeados, tras recargar la copia | 356 / 356 | 242 / 114 / 0 | 29 | 2 | 6,72 s / 56,78 ms |

Las cuatro pasadas coincidieron con los diez casos de la referencia
visual T018 y excluyeron el UID de control. Las tres pasadas de 356
coincidieron en los UID, su orden, el texto de Pase y el estado de cada
celda. Se conservó el inicio de proceso y se registró el cambio del
contador de cargas. Pedir 355 filas cuando FM27 anunciaba 356 produjo
«No se pudo comprobar el total anunciado por FM27», sin publicar filas.
Se restauraron los tres criterios, el interés original y el conmutador
de ojeados desactivado. FM27 volvió a mostrar los 461 resultados iniciales.

La sonda lee únicamente UID y Pase y desplaza temporalmente la tabla
virtualizada. Los tiempos de lectura son acumulados y queda pendiente
medir el impacto por fotograma. El registro de BepInEx avisó de que
Il2CppInterop omitió el registro de un método privado con parámetro
gestionado. Las cuatro capturas y el control negativo funcionaron. La
confirmación manual de filtro y perspectiva limita el ensayo a esa
vista. Quedan pendientes otras vías de conocimiento del mánager, los
demás campos y la cancelación real al cambiar de carrera o permisos
durante una captura.

Los agregados y las huellas de las respuestas están en
`%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad/ojeados-20261124.json`,
SHA-256 `EF1C8701DA596199B642C95E15D8618F558DD3E11F0A3C838CF837E9EA18B989`.
Compilaron Release y Laboratorio con cero errores y advertencias. Pasaron
50 pruebas MSTest en cada configuración. El resultado complementa A-01
y aporta evidencia parcial a E-07. T021 y T026 siguen abiertas.
[Reproducción del censo](quickstart.md#censo-de-la-vista-de-ojeados)

### Referencia visual de táctica y finanzas

En el mismo corte, «Tácticas → Dibujo táctico» mostró 11 puestos con
roles por cada fase de la táctica activa `VALENCIA-4231`. Se observaron
cuatro cambios de puesto entre «Con posesión» y «Sin posesión». Los dos
mediapuntas de banda pasaban a medios de banda y uno de los dos
centrocampistas centrales, a mediapunta central. Javi Guerra cambiaba
de ME (C) a MC y los laterales conservaban el lado con rol distinto.
La tabla combinada mostró el rol en ambas fases y el XI. Es una
referencia visual para cotejar una captura futura de puestos y roles por
fase. Falta vincular las filas de la lectura por UID.

Portal y Contratación coincidieron en el presupuesto disponible de
fichajes de 3,97 M € y el disponible de sueldos de 392 mil € por mes.
En el Portal, el gasto de fichajes de la temporada era 6,57 M € y el de
sueldos, 5,11 M € por mes. La interfaz permite distinguir saldo
disponible, gasto y periodo. Queda por cotejar una captura de esos
importes y su escala con una fuente independiente.

## T023 · Aislamiento con datos controlados

Se probaron seis casos de E-07 y E-09 con fixtures MSTest. En los
cuatro casos de E-07 se mantiene el club y el UID. Una recarga o el
cambio de carrera, rama o mánager cancela la sesión anterior, impide
publicar un lote preparado y bloquea la lectura de su lote previo. Con
la vinculación nueva, el intercambio rechaza ese lote previo y admite
un lote nuevo que conserva la nueva procedencia.

En los dos casos de E-09 se eleva la revisión durante la lectura. Se
cancela el trabajo pendiente. La nueva sesión rechaza la comparación de
CA al revocarla y conserva Pase mientras siga visible. Tras perder
conocimiento de Pase, la nueva sesión devuelve el estado desconocido
sin consultar el valor. La comparación de CA sigue disponible en ese
segundo caso porque conserva su permiso.

```powershell
dotnet build FM27Analyzer.slnx -c Release -warnaserror
dotnet test FM27Analyzer.slnx -c Release --no-build --filter FullyQualifiedName~CaptureSessionTests
dotnet test FM27Analyzer.slnx -c Release --no-build
dotnet build FM27Analyzer.slnx -c Laboratorio "-p:FM27GameRoot=$fm27Root" -warnaserror
dotnet test FM27Analyzer.slnx -c Laboratorio --no-build
```

Pasaron los seis casos dirigidos y 56 de 56 pruebas totales en cada
configuración. Ambas compilaciones terminaron con cero advertencias y
errores. El inventario de copias SK-01 encontró una única copia de
prueba. La recarga real documentada arriba aporta evidencia de un
cambio de contador en la misma carrera. La alternancia de dos carreras,
la revocación en FM27 y la recuperación de T025 siguen pendientes del
resto de A-03.

## T024–T026 · Recuperación y cambio real de carrera

El 24 de noviembre de 2026, FM27 Steam 26620374 convirtió dos
guardados de FM26 con su función de carga. `Solihull-Inverted` se abrió
desde la nube y `Rebuild Valencia` desde el directorio local de FM26.
Se guardaron como `SK-01 Solihull 2027-08-21` y `SK-01 Valencia 2025-07-07`
en la lista local de FM27. Las sondas consultaron el juego cargado.
La copia inicial del Valencia mantuvo 43.957.632 bytes y su fecha de
modificación. Al terminar se dejó abierta en su corte y se verificaron
los 461 resultados de la búsqueda original.

| Carga en el mismo proceso | Copia y perspectiva | Comprobación |
| ---: | --- | --- |
| 2 | Valencia CF, 1/8/2026 21:45 | Referencia inicial y censo de ojeados. |
| 3 | Solihull Moors, 21/8/2027 09:00 | Gayà conservaba el UID `67178177` y su club Valencia. La ficha y BepInEx coincidieron en su Pase visible de 11–15. BepInEx y memoria externa rechazaron solicitudes del Valencia 2026. |
| 4 | Valencia, 7/7/2025 10:00 | Segundo Valencia dirigido, en otra carrera, con el mismo UID en su plantilla. La pantalla y BepInEx coincidieron en Pase 13. La solicitud del Valencia 2026 quedó bloqueada. |
| 5 | Valencia CF, 1/8/2026 21:45 | Una captura iniciada en la carga 4 se canceló antes de publicar. Una petición del Valencia 2025 también se bloqueó. En una sesión nueva, BepInEx coincidió con Pase 13 y Reflejos 2, y la vía externa volvió a cotejar Pase dos veces. |

Las tres respuestas de BepInEx fuera de contexto omitieron UID y valor.
En la prueba en curso, la sonda externa registró `READY 2025-07-07 4`
antes de cargar la otra copia y `RECHECK 2026-08-01 5` justo antes de
publicar. `CaptureSession` canceló la publicación y el ejecutable de
prueba terminó con código 2. Se fijó copia y mánager en pantalla en cada
vinculación. Queda por automatizar esa identificación cuando dos
carreras coinciden también en su fecha o el mánager cambia sin recarga.

El control sintético de recuperación cubrió reenvío, conflicto de ID,
build incompatible, partida cerrada, truncado, desconexión y otra vía
con procedencia propia. Un reenvío idéntico no agrega observaciones. La
recuperación revalida los permisos, conserva la fecha original y se
bloquea si el contexto deja de ser verificable. En consola se reenvió
un lote real y se reintentó con un JSON truncado de forma controlada.
El reintento recuperó la misma huella y fecha de captura del lote
original. Los fallos de build, cierre y desconexión se comprobaron con
fixtures. La revocación de CA/PA se había contrastado con un cambio de
política simulado sobre lecturas reales. Sigue pendiente comprobar una
pérdida de conocimiento originada en FM27 durante una captura.

El registro privado `SK-01/aislamiento.json` incorpora E-07 a E-09 por
clase. `verify` acreditó A-01 con esos seis resultados incorporados a una
copia del manifiesto. El control bloqueado devolvió A-01 sin acreditar.
Pasaron **62 pruebas** en Release y Laboratorio, con cero advertencias
y errores. Las huellas de `aislamiento.json` y del informe son
`D1A85653668B8486C276684D89E575B97594F11248A1755B3F375BF3536BAFF8` y
`074817696C64BB3DADC9998B7CF91FAC71D6ECD9850E94F7076F7B4EC6A81F01`.
El inventario de respuestas y la traza se conservan en
`SK-01/viabilidad/aislamiento-observado-20261124.json`.
[Reproducción de aislamiento y recuperación](quickstart.md#aislamiento-y-recuperación)

### Conciliación entre BepInEx y memoria

En el Valencia de 2026, dos recorridos externos recuperaron el mismo
conjunto de 19.917 UID. Estaban presentes los 356 UID del censo BepInEx
de ojeados y los 20 UID de plantilla fijados en T018. Las 376 referencias
eran distintas y se comprobó la identidad por UID. En Solihull, dos
lecturas externas dieron 90.687 UID distintos y un conjunto idéntico.
Se comprobó independientemente un UID de esa carrera, el de Gayà.
Ambas pruebas consultaron sólo identidades. Los recuentos de memoria
se mantienen como observados en su colección. La cobertura mundial
sigue indeterminada porque falta una referencia del mismo universo.

El cruce se hizo con el `WorldProbe.dll` del banco privado y `Players`
vacío. El ensayo del Valencia leyó 797.500 bytes en 190 ms. Se guardaron
los agregados en `SK-01/viabilidad/mundo-cruce-ojeados-20261124.json`
con SHA-256 `50EA457B2E462D1356ADEE42E6B6E20BE8E9CE8786D23946C2CA5586A3A22BA3`
y en `SK-01/viabilidad/aislamiento-solihull-memoria.json`.

## T027–T034 · Capacidades adicionales e informe común

El 24 de noviembre de 2026 el banco privado se vinculó a la misma
instancia de FM27 Steam 26620374 y a la carga 5 del Valencia, en
2026-08-01 21:45. El lector externo verificó el PID, el inicio de
proceso, la huella PE, el corte, el contador de cargas, los límites de
la tabla y los metadatos de la copia antes de emitir resultados. El
repositorio conserva el cotejo y la producción de métricas en C#.

| Escenario | Referencia y vía | Resultado y límite |
| --- | --- | --- |
| E-04 · CA | Columnas de plantilla comprobadas con el editor oficial, mismo UID, corte y carga. Memoria externa. | Dos lecturas concordantes en un jugador propio, escala 1–200. Al revocar el permiso en el banco se canceló un resultado en curso y se rechazaron nuevas consultas. |
| E-04 · PA | Misma referencia, campo independiente. Memoria externa. | Dos lecturas concordantes en el mismo jugador, escala 1–200. La revisión revocada impidió una nueva consulta. El PA codificado y las demás compilaciones quedan sin acreditar. |
| E-05 · Identidades de plantilla y ojeo | Censo BepInEx de 356 ojeados y referencia visual de 20 jugadores de plantilla. Memoria externa. | **376 de 376** UID conciliados. Es cobertura parcial de identidades con un subconjunto verificable. |
| E-05 · Universo cargado | Tabla de jugadores en memoria. Comparación previa con las 19.836 filas anunciadas en la búsqueda. | Dos lecturas idénticas de 19.917 UID, 19.887 jugadores y 30 personas con doble rol. Diferencia de 81 entre recuentos de poblaciones aún sin reconciliar. Referencia mundial indeterminada y cobertura de atributos evaluada por separado. |
| E-06 · Atributos bajo máscara y ocultos | Editor oficial y ficha del portero sin informe del ensayo del 23/11. Memoria externa, evidencia histórica del mismo corte y build. | Dos coincidencias por campo en Pase, Anticipación, Aceleración, Reflejos, Consistencia y Profesionalidad. El banco revocó el exacto y mantuvo intervalo y desconocimiento. |

Los valores especiales se compararon en memoria y los archivos
conservan coincidencias, métricas y motivos. El cambio de política se
aplicó en la sonda al releer el proceso real. La observación directa de
una pérdida de conocimiento originada en FM27 durante una captura
sigue omitida en E-09. La repetición visual del ensayo E-06 quedó
bloqueada por el entorno de control de ventanas, que resolvía Node
18.12.1 con requisito mínimo 22.22.0. Se incorporó la evidencia real
anterior y se identificó su fecha por separado.

El informe común usa el manifiesto `ensayo-a01/run.json` y las
referencias privadas `aislamiento.json` y
`capacidades-adicionales.json`. Se archivó el manifiesto original
como `run-a01-sin-anexos.json`. El contraste común volvió a acreditar
A-01 y reunió los nueve escenarios por clase. E-08 y E-09 reales
figuran como omitidos con su causa. E-05 declara la identidad parcial
y la cobertura global indeterminada. Pasaron 74 pruebas en Release y
Laboratorio, con cero advertencias y errores, y se repitió el control
bloqueado. [Comandos y criterios](quickstart.md#contrastes-adicionales-y-cierre)

| Artefacto privado bajo `%LOCALAPPDATA%/FM27Analyzer/SK-01` | SHA-256 |
| --- | --- |
| `viabilidad/capacidades-en-vivo-20261124.json` | `C92F6F91FD1E87724EA8167045F7EE5114A78F6C505FEFEBDEE9F872FE983A2C` |
| `capacidades-adicionales.json` | `19C74F0B85A3E2BF73826C29EF850649E455310504DBD24B08AA8BC2ACAE20BE` |
| `ensayo-a01/informe-comun.json` | `A19B8825F9700C0659CA7C70C53791FA05866DFAD7837B08558CB2381FD0EBA3` |

### Traspaso técnico

La evidencia disponible permite planificar SK-03 sobre el contrato de
captura inicial de Steam 26620374 en castellano, con perspectiva y
corte confirmados manualmente. Los permisos especiales se mantienen
independientes. El modo mundial completo queda condicionado a una
referencia conciliada. [Matriz de vías y campos](matriz-fuentes-to-be.md)

La aceptación de la UI definitiva y los ensayos de plataforma
conservan sus propias puertas. [Alcance y dependencias](../../docs/roadmap/06-alcance-v1.md) ·
[PoC A y PoC B](../../docs/roadmap/07-poc-y-modelo-v1.md)

## T021 · Atributos visibles y familiaridad por posición

El 24 de noviembre de 2026, en Steam 26620374, carga 5 del Valencia
CF a 1/8/2026 21:45, se fijó una referencia independiente con las fichas
de José Gayà, Arnau Martínez y Stole Dimitrievski. Los tres UID
pertenecen a la muestra T018. Se confirmó club y UID en la plantilla,
y se cotejaron los atributos exactos publicados en cada ficha. El
adaptador externo validó la copia y la estructura de FM27, comprobó
cada vínculo de equipo antes y después de leer y selló la proyección
visible con el contrato de captura.

| Grupo | Observaciones | Coincidencias con FM27 en ambas lecturas |
| --- | ---: | ---: |
| Técnica | 23 | 23 |
| Balón parado | 10 | 10 |
| Mental | 42 | 42 |
| Físico | 24 | 24 |
| Portería | 11 | 11 |
| **Total** | **110** | **110** |

Para familiaridad se usó «Editor del juego → Atributos del jugador →
Posiciones» en solo lectura. Se cotejaron las 14 posiciones de cada
jugador de campo y 10 del portero, con **38 de 38** coincidencias en
dos lecturas. Se verificaron los lados del defensa, carrilero,
centrocampista y mediapunta con los calificadores del perfil. La
referencia cubrió valores observados de 1 a 20 dentro de la escala
declarada de 0 a 20. El puesto adicional del perfil de terceros carece
de referencia en la pantalla ensayada y queda excluido.

La familiaridad numérica procede del editor y exige permiso de prueba.
La ficha del mánager aporta una representación posicional y sus
etiquetas. Se observó, por ejemplo, que la ficha de Arnau no enumera
todos los puestos para los que el editor muestra un valor. La
equivalencia entre etiquetas, colores y números sigue sin acreditar.
En la ejecución con permisos base se repitieron las 110 observaciones y
se hicieron cero lecturas de la escala posicional. Los controles de
carga incorrecta y referencia fuera de escala terminaron con código 2
y sin resultado. La prueba anterior T016 conservó su coincidencia.

Para ojeo se volvió a comprobar el filtro y la ficha de Tomáš Hašler.
La ficha mostró rangos técnicos, mentales y físicos y el guion de
desconocimiento. Se mantienen las capturas A-01 de BepInEx como fuente
de los estados verificados de Pase y Marcaje. Queda por ampliar la
captura automatizada de las demás celdas del ojeo. No se extrapola a
otros jugadores, tiendas o compilaciones la validación numérica de
esta muestra. Se cerró el editor sin cambiar campos, se repuso la
búsqueda original de 461 resultados y se preservaron los metadatos del
guardado.

La captura combinada de la muestra leyó 50.673 bytes en 148,4 ms y
alcanzó un pico residente de 41.664.512 bytes en la sonda. Los números
del editor entraron por stdin y el informe sólo guardó recuentos por
posición. Las tres salidas están en `%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad`.

| Artefacto | SHA-256 |
| --- | --- |
| `referencia-prioritarios.json` | `BF29C809584D0F737CE6E80E8B1EC702B4B552F614AA6C004331EA6232268A06` |
| `prioritarios-en-vivo-20261124.json` | `D9C5292E8EF63A0357B6719A622A25393335B208B1CA01444A9CAD1785148BC0` |
| `prioritarios-base-20261124.json` | `611F4E009CAA35994FB63D329F68C21BACFDF928B9B5A7E93B4433B02DA3D6A3` |

T021 aporta una fuente contrastada para los grupos prioritarios y
para la familiaridad experimental de campo y portería. PoC B conserva
la validación del uso de esa escala y de la proyección de atributos.
El inventario mundial y las demás familias del TO-BE conservan los
límites de su matriz. [Reproducción](quickstart.md#atributos-visibles-y-familiaridad) ·
[Layout contrastado](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/profiles/fm27/layout.toml) ·
[Matriz de fuentes](matriz-fuentes-to-be.md)
