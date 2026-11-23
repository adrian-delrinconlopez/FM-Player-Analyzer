# Investigación técnica · SK-01

**Estado** Investigación documental. Compatibilidad, exactitud y cobertura de FM27
pendientes de validación empírica.

## R1 · Ensayo pequeño dentro del proceso

**Decisión** Partir de BepInEx ya instalado en el FM27 del propietario y comprobar
si su revisión y configuración sirven para un ensayo en una copia de partida
conforme a FR-003. Verificar carga, identidad de la instancia y un campo visible
en hasta tres jugadores de un mismo corte. Incluir el UID contrastable como
vínculo. Registrar revisión del loader, interop, runtime, tienda, build de FM27
e idioma. El TFM del ensayo depende de esa combinación.

**Motivo** El proyecto FM View Exporter fijado por TO-BE usa `net6.0`, referencias
IL2CPP del juego y BepInEx BE 755 en su proyecto. Es evidencia de terceros para
listas visibles, no una versión seleccionada para SK-01 ni prueba de acceso a CA/PA,
familiaridad numérica o universo cargado. La guía de BepInEx instala el loader
en el directorio del juego y documenta archivos generados en el primer arranque.
El punto de partida es la configuración existente. Comparar contra el mismo
entorno sin captura y documentar los efectos. Los binarios, la instalación y
los guardados se preservan durante la captura. Si el ensayo incumple FR-003
o provoca cambios de instalación atribuibles a la captura, registrar el bloqueo
y continuar con la vía externa si cumple sus propias condiciones.
[Investigación TO-BE](../../docs/to-be/12-integracion-fm27.md) ·
[Proyecto del exportador en la revisión citada](https://raw.githubusercontent.com/ElectroHugin/FM-View-Exporter/2ed5898f6e3b8597b6b8ec5401f41d16b961cdfd/src/FMViewExporter/FMViewExporter.csproj) ·
[Guía de BepInEx para IL2CPP](https://docs.bepinex.dev/master/articles/user_guide/installation/unity_il2cpp.html)

**Alternativas consideradas** Empezar por una comparación tabular visible cuando
la prueba dentro del proceso esté bloqueada. Sirve como referencia de control,
con independencia de la vía bajo ensayo. El contraste tabular por sí solo
queda como evidencia complementaria para A-01.

## R2 · Ensayo externo en C#

**Decisión** Evaluar acceso de consulta y lectura con privilegios mínimos a la
instancia propia de laboratorio seleccionada, como usuario estándar. Intentar un
campo visible autorizado de la misma muestra, sólo si existe una interpretación
documentada y verificable por build e identidad. Si falta esa interpretación,
hay una denegación o no se puede limitar la lectura al dato autorizado, detener el
ensayo y registrar el motivo. Acotar el tiempo, alcance y memoria antes de ejecutar.

**Motivo** El acceso al proceso permite plantear una prueba de lectura, pero no
acredita que se pueda identificar una observación ni interpretar el conocimiento
del mánager. La prueba evita permisos de escritura, elevación, alteración de
controles de acceso, barridos y volcados de memoria. La disponibilidad de CA/PA y
la semántica de familiaridad quedan pendientes de contraste independiente.
[Derechos de acceso a procesos, Microsoft Learn](https://learn.microsoft.com/en-us/windows/win32/procthread/process-security-and-access-rights) ·
[Investigación TO-BE](../../docs/to-be/12-integracion-fm27.md)

**Alternativas consideradas** Ampliar sólo la vía BepInEx si supera el contraste y
cubre el mínimo de A-01. Considerar un complemento externo para un campo concreto
si demuestra valor y se puede vincular al mismo UID, carrera, rama, corte,
perspectiva y permiso. Si las dos vías quedan bloqueadas, registrar el resultado,
la referencia tabular disponible y qué falta para reintentar.

## R3 · Selección y transporte

**Decisión** Pasar de la prueba pequeña a A-01 con una sola vía. Exigir identidad
verificada, coincidencia con la referencia, prueba de visibilidad, repetibilidad y
solo lectura. Comparar el coste de preparación, la duración, la memoria y la
dependencia de la compilación si más de una supera la puerta. La vía y el reparto
por campo se eligen con evidencia. El contrato común se intercambia como un lote
local acotado en JSON UTF-8 versionado. Para una fuente en el mismo proceso del
validador puede usarse el mismo modelo en memoria.

**Motivo** Un archivo local de evidencia autorizada basta para un contraste bajo
demanda. Permite validar el intercambio entre runtimes sin distribuir referencias
de FM27 con el validador. Aísla la evolución de un adaptador y reduce el trabajo
para incorporar el contrato a SK-03.
[Contratos de arquitectura](../../docs/architecture/03-integraciones-y-contratos.md) ·
[Alcance de v1](../../docs/roadmap/06-alcance-v1.md#incrementos-dependencias-y-orden-de-spec-kit)

**Alternativas consideradas** Named Pipes para sincronización de producto y SQLite
para persistencia de carreras, ambos aplazados a la planificación de SK-03. Un
contrato en un ensamblado compartido sólo se evaluará si los runtimes compatibles
lo justifican. La PoC usa el formato y las mismas muestras sintéticas como frontera.

## R4 · Versiones, derechos y actualizaciones

**Decisión** Fijar la revisión de herramientas tras el primer contraste aceptado,
y registrar versión y procedencia de cada binario, licencia y dependencias transitivas.
Revisar los términos de la distribución de FM27 y los derechos sobre interop y datos
antes de usar artefactos de terceros. Los proyectos comunitarios se usan como
referencias técnicas. El repositorio recibe código propio, fixtures sintéticas y
evidencia agregada autorizada. Los binarios del juego, los ensamblados generados de
interop y los datos especiales se mantienen fuera del repositorio y de las entregas.

**Motivo** Un cambio de tienda, build o semántica puede dejar inválida una
acreditación. Los términos del juego y las licencias de cada artefacto tienen
alcances distintos. El fichero de licencia del repositorio de BepInEx es LGPL 2.1,
y el inventario local señala GPLv3 para FM View Exporter y FMLibreEditor. La
declaración de la licencia de un repositorio es un punto de partida para revisar
la revisión, el paquete y el uso concretos. El aviso de Steam de FM27 remite a las
condiciones de SEGA y por sí solo no resuelve si el entorno admite los ensayos.
[Inventario de componentes](../../docs/architecture/06-bibliotecas-y-componentes.md) ·
[Licencia del repositorio BepInEx](https://raw.githubusercontent.com/BepInEx/BepInEx/master/LICENSE) ·
[Aviso de Steam de FM27](https://store.steampowered.com/eula/4530370_eula_0)

**Alternativas consideradas** Reutilizar un lector, plugin, referencias o tablas
de terceros sólo tras resolver Q-06 y revisar condiciones del artefacto concreto.
Conservar el último resultado aún autorizado con su fecha. Una actualización
requiere volver a contrastar los campos afectados antes de considerarlos
compatibles. [Q-02, Q-03, Q-04, Q-06 y Q-15](../../docs/OPEN-QUESTIONS.md)

## Decisiones que necesitan ejecución

| Incógnita | Evidencia necesaria | Salida si no se acredita |
| --- | --- | --- |
| Build, distribución, loader, interop y runtime viables | Arranque y un campo visible repetido en el entorno de prueba | Vía bloqueada para esa combinación |
| UID estable y vínculo de carrera | Misma identidad tras repetir y recargar. Dos copias para acreditar aislamiento real | Asociación expresa por sesión y corte si es verificable. Identidades dudosas en revisión |
| Máscara, familiaridad y exactos ocultos | Ficha o vista comparable. Escala numérica u ocultos sólo con referencia independiente y permiso aplicable | Categoría o intervalo comprobados. Campos o nivel de precisión pendientes |
| CA y PA | Coincidencia por campo, mismo jugador y corte, autorización y referencia independiente | «No disponible» por campo con motivo |
| Cobertura de mundo | Referencia independiente de la misma población cargada. Reconciliar total y, si existe, conjunto de UID | Parcial si el subconjunto tiene límites verificables. Indeterminada si la evidencia no permite acotarla |
