# Especificación funcional · Prueba de extracción de FM27 en solo lectura

**Hito** SK-01 · PoC A, primer contraste A-01 y comprobaciones acotadas A-02/A-03

**Rama de trabajo** `prototype/centro-analisis-fm27`

**Fecha de creación** 2026-11-23

**Estado** Borrador para planificación

**Encargo** Obtener una muestra estructurada de una partida abierta de FM27, contrastarla con el juego y registrar qué datos y ámbitos pueden acreditarse.

## Escenarios de usuario y pruebas *(obligatorio)*

### Historia de usuario 1 · Contrastar una muestra real (prioridad P1)

Como responsable de validar la integración, quiero obtener una muestra autorizada de una copia de partida abierta y cotejar cada campo con una referencia del mismo corte. Así puedo decidir qué datos sirven para el siguiente incremento.

**Prueba independiente** Abrir una copia en una compilación declarada, fijar fecha y perspectiva, y preparar antes de la captura una muestra de al menos 30 futbolistas entre plantilla propia y externos conocidos u ojeados. Incluir portería, campo y casos con dato exacto, rango y desconocimiento. Repetir la captura sin avanzar la partida y comparar ambos resultados.

1. **E-01 · Dada** una instancia de FM27 con la copia abierta, **cuando** el validador inicia el ensayo, **entonces** identifica instancia y compilación y confirma la partida y el corte. Si la identidad automática de la partida es incierta, requiere asociación expresa de esa sesión antes de aceptar la muestra.
2. **E-02 · Dada** la muestra de referencia, **cuando** el validador contrasta las dos capturas con las fichas, vistas y referencias independientes del mismo corte, **entonces** puede cotejar UID, club, atributos, posiciones y familiaridad por jugador y campo. Cada ausencia o discrepancia tiene un estado o motivo. Una categoría posicional visible se conserva como categoría.
3. **E-03 · Dadas** una ficha parcialmente ojeada y una identidad fuera del ámbito inicial, **cuando** se solicita la muestra con permisos base, **entonces** los rangos mantienen sus extremos, los campos desconocidos conservan su estado y la identidad excluida queda fuera de las filas y recuentos. CA y PA permanecen deshabilitados. Dos conjuntos de prueba que sólo difieran en datos ocultos producen la misma salida autorizada.

### Historia de usuario 2 · Comprobar capacidades de acceso adicionales (prioridad P2)

Como responsable de la validación, quiero saber si CA, PA, los atributos ocultos y el universo cargado se pueden acreditar por separado, para distinguir una capacidad demostrada de una limitación de la prueba.

**Prueba independiente** Usar un corte fijado en una copia de partida y contrastar cada permiso con el estado base. Para los valores especiales, preparar una referencia independiente de la vía ensayada cuando esté disponible. Para el universo cargado, intentar contrastar una muestra acotada con una referencia de la misma población.

1. **E-04 · Dada** una solicitud expresa de prueba de CA o PA tras la advertencia, **cuando** se contrasta el campo con una referencia independiente del mismo jugador y corte, **entonces** se registra por separado si se acredita CA o PA. Si falta referencia, capacidad o coincidencia, el campo queda sin acreditar y se registra el motivo. Los valores de prueba quedan fuera del informe compartible.
2. **E-05 · Dada** una prueba mundial autorizada expresamente, **cuando** se contrasta si la vía alcanza identidades fuera del ojeo conocido, **entonces** se informa el ámbito evaluado, los UID únicos autorizados, las omisiones y el denominador si se conoce. La cobertura puede quedar parcial o indeterminada. Ampliar el ámbito conserva los permisos vigentes de atributos y de CA/PA.
3. **E-06 · Dado** un atributo que el juego muestra como rango, **cuando** se prueba o revoca el permiso independiente de atributos ocultos, **entonces** el exacto sólo puede examinarse en un contexto autorizado y con referencia acreditable. Al revocar el permiso se descartan los resultados afectados y sigue disponible el rango observado si continúa autorizado.

### Historia de usuario 3 · Detectar incompatibilidades y descartar capturas inválidas (prioridad P1)

Como responsable de la validación, quiero distinguir una muestra parcial legítima de una captura fallida y evitar que se mezclen partidas, perspectivas o permisos.

**Prueba independiente** Ensayar con una copia recargada y con dos contextos que comparten club y UID. Repetir el lote e inducir cambios de contexto e interrupciones. Usar una segunda copia para acreditar el aislamiento con el juego. Los casos con datos controlados se registran como tales.

1. **E-07 · Dados** una recarga y dos contextos con UID coincidentes, **cuando** se cambia de carrera, rama o mánager o se repite el mismo lote, **entonces** el cambio invalida la captura anterior y exige verificar el vínculo vigente. La repetición dentro del mismo contexto conserva un único conjunto de observaciones.
2. **E-08 · Dada** una versión incompatible, una partida cerrada, una lectura truncada o una desconexión, **cuando** se solicita o interrumpe una captura, **entonces** se identifica la causa y la capacidad afectada. La captura inválida se descarta. Si hay una muestra anterior aún autorizada y del mismo contexto, conserva su fecha y se señala como anterior.
3. **E-09 · Dada** una captura en curso, **cuando** se revoca un permiso o se pierde conocimiento de ojeo, **entonces** los resultados pendientes del contexto anterior se descartan y la siguiente salida refleja el permiso y el conocimiento vigentes.

### Casos límite

- **Instancia ambigua.** Con dos instancias candidatas, el validador confirma cuál corresponde al ensayo antes de asociar una partida. Con el juego cerrado o en el menú, se distingue «sin instancia» de «sin partida activa».
- **Identidad conflictiva.** Los homónimos siguen separados. Sin UID verificable, la fila queda en revisión. Si un traspaso o un jugador generado está disponible en la copia, se registra el contraste adicional de identidad o su omisión. Un jugador sin club acreditado se distingue de un vínculo de club desconocido.
- **Cobertura y visibilidad.** Una vista que omite una columna se distingue de una observación explícita de desconocimiento. Un intervalo conserva ambos extremos. Los recuentos publicados se limitan al ámbito autorizado y la falta de denominador se registra como tal.
- **Evidencia discordante.** Una coincidencia de nombre, una etiqueta posicional o un valor obtenido por una vía candidata no bastan para acreditar UID, familiaridad numérica o datos bajo máscara. Una discrepancia abierta impide acreditar el campo afectado.

## Requisitos *(obligatorio)*

### Requisitos funcionales

- **FR-001 · Detectar el contexto del juego** (E-01, E-08). Identificar la instancia de Football Manager 27, la edición y su compilación, y distinguir instancia ausente, ambigua, incompatible y sesión sin partida. Registrar distribución e idioma cuando se puedan verificar.
- **FR-002 · Confirmar la partida activa** (E-01, E-07). Registrar identificador de carrera si es verificable, rama o estado pendiente de confirmarla, perspectiva del mánager, club dirigido y fecha de juego. Ante identidad incierta, detener la vinculación hasta que el validador confirme expresamente la asociación de la sesión y el corte. Un nombre, club, fecha o UID aislado es evidencia insuficiente.
- **FR-003 · Capturar la muestra en solo lectura** (E-01, E-02, E-08). Obtener observaciones reales de la partida abierta. Mantener intactos el estado del juego, la instalación y sus guardados durante la captura. Los guardados `.fm` quedan fuera de las fuentes de datos.
- **FR-004 · Registrar campos prioritarios** (E-02, E-03). Intentar obtener por jugador UID de origen, club y vínculo identificable cuando exista evidencia, atributos por grupo y estado de máscara, posiciones y lados, familiaridad por posición y estado de pertenencia al ojeo. Conservar las categorías o valores observables de familiaridad con su escala cuando esté acreditada. Mantener los UID de origen con sus prefijos.
- **FR-005 · Preservar valor, conocimiento y procedencia** (E-02, E-03, E-06). Cada observación conserva campo, tipo y unidad o escala cuando proceda, origen, edición y compilación, contexto de carrera, rama, perspectiva, fecha de juego y de captura, y revisión de permisos. Distinguir conocido exacto, conocido como intervalo inclusivo, desconocido para el mánager, ausente en la vista, no disponible en la vía o compilación, y restringido. Para huecos autorizados se informa el estado sin asignar un valor. Se omiten las identidades y campos de visibilidad dudosa.
- **FR-006 · Aplicar el alcance y los permisos efectivos** (E-03, E-04, E-05, E-06, E-09). Aplicar la política antes de consultar o producir filas, recuentos y diagnósticos. El ámbito inicial es plantilla y externos conocidos u ojeados. Mundo, atributos ocultos y CA/PA tienen autorizaciones independientes. CA y PA empiezan deshabilitados y requieren consentimiento expreso tras advertencia. Retirar permisos invalida observaciones y trabajos afectados.
- **FR-007 · Investigar CA/PA y exactos ocultos** (E-04, E-06). Contrastar por separado CA, PA y los campos ocultos ensayados, con permiso de prueba y referencia independiente de la vía evaluada sobre el mismo jugador, corte y contexto. Registrar compatibilidad y concordancia, o el motivo de indisponibilidad. Excluir sus valores especiales de informes compartibles, diagnósticos, copias de evidencia y repositorio. La acreditación de un campo sólo vale para el contexto contrastado.
- **FR-008 · Evaluar el universo cargado** (E-05). Con autorización expresa de prueba, documentar si hay acceso fuera del conjunto conocido, qué grupos cubre y qué evidencia independiente permite estimar cobertura. Comparar totales de la misma población y reconciliar UID cuando exista inventario. «Completa» exige total reconciliado, igualdad de conjuntos si hay inventario por UID y cero omisiones o duplicados conocidos. «Parcial» exige un subconjunto contrastado con un límite de alcance verificable. Si la evidencia disponible no establece la cobertura ni permite acotarla, el resultado es «indeterminada». La aptitud de cada campo se acredita por separado.
- **FR-009 · Detectar fallos y preservar aislamiento** (E-07, E-08, E-09). Diagnosticar incompatibilidad de compilación o capacidad, dato contradictorio, captura truncada, desconexión y cambios de partida o permisos. Descartar capturas inválidas sin duplicarlas al reintentar ni mezclar observaciones de distintos contextos. La recuperación sólo reutiliza una muestra del mismo contexto y todavía autorizada, fechada y distinguible de una captura nueva.
- **FR-010 · Entregar evidencia reproducible** (E-02, E-04, E-05, E-07, E-08). Entregar una muestra estructurada autorizada y un informe de contraste. Por campo o familia, vía, compilación y perspectiva, el informe presenta fuente de referencia, esperados y recuperados, coincidencias, discrepancias, denominador si existe, permiso efectivo, duración, memoria pico y estado acreditado, parcial o sin acreditar. Registrar entradas, pasos o comandos, resultados, fallos y omisiones, con el tipo de evidencia identificado como real, sintética o de terceros.

### Entidades clave

- **Sesión de prueba.** Instancia, compilación, copia y asociación de carrera, rama, mánager, fecha del juego y política efectiva.
- **Muestra de jugadores.** Conjunto acotado de identidades autorizadas elegido antes de la captura, con UID de origen y club vinculado cuando se verifica. Las identidades excluidas son controles de la prueba y quedan fuera de la muestra compartible.
- **Observación de campo.** Valor o estado de conocimiento de un jugador, asociado a su unidad o escala, contexto y procedencia.
- **Resultado de acreditación.** Comparación de una capacidad con una referencia independiente de la vía ensayada, con alcance, concordancia, limitaciones y estado.

### Trazabilidad

| Bloque | Requisitos TO-BE y antecedente AS-IS 2.3.1 | Pruebas de referencia |
| --- | --- | --- |
| Captura, contexto y compatibilidad | REQ-SCOPE-001, REQ-FM-001/002/004, REQ-DATA-002. FPA-FUNC-025/026, sustituidas, y FPA-FUNC-002, modificada. | PC-01/02, A-01/A-03, EQ-002/025/026, NV-01/02/05 y criterios de identidad de V1-01 |
| Muestra y observaciones | REQ-DATA-001 y REQ-DATA-004 como contraste complementario. FPA-FUNC-001, ampliada, y FPA-FUNC-024, modificada. | A-01, EQ-001 y EQ-024 como antecedente del contraste tabular |
| Ojeo, permisos y cobertura | REQ-FM-005, REQ-VIS-001 a 003 y ámbito de REQ-SCOUT-001. FPA-FUNC-016, modificada, y FPA-FUNC-025, sustituida. | PC-03, A-01 a A-03, NV-03/04 y criterios de visibilidad de V1-02 |

[Catálogo TO-BE](../../docs/to-be/02-catalogo-funcional.md) · [Trazabilidad AS-IS → TO-BE](../../docs/to-be/18-trazabilidad-as-is-to-be.md) · [Protocolos de PoC A](../../docs/roadmap/07-poc-y-modelo-v1.md#poc-a--extracción-autorizada-de-fm27) · [Aceptación de v1](../../docs/roadmap/06-alcance-v1.md#aceptación-de-v1)

## Criterios de éxito *(obligatorio)*

### Resultados medibles

- **SC-001 · Muestra real contrastada.** Recuperar y verificar los UID de al menos 30 futbolistas únicos de plantilla y externos conocidos u ojeados en una compilación declarada. Como mínimo, acreditar el vínculo de club, una posición y un atributo exacto visible de un portero y de un jugador de campo de la muestra. Contrastar también los casos de intervalo y desconocimiento. Cada campo acreditado tiene cero discrepancias pendientes y cada hueco de la muestra tiene un estado o motivo. La familiaridad, incluida su escala, se acredita sólo al nivel demostrado.
- **SC-002 · Repetibilidad y solo lectura.** Dos capturas en el mismo corte y contexto producen las mismas identidades, valores autorizados y estados, descontando metadatos que cambian en cada ejecución. Hay cero duplicados, cero cruces entre contextos y cero cambios en el juego, instalación o guardados atribuibles a la captura. Registrar duración y memoria pico de las dos capturas.
- **SC-003 · Visibilidad comprobada.** Con permisos base, cero CA/PA, exactos bajo máscara, identidades excluidas o recuentos que las revelen en las salidas. Con permisos de prueba, registrar un resultado separado para CA, PA, exactos ocultos y exploración mundial. La revocación deja cero resultados obsoletos de los permisos afectados.
- **SC-004 · Fallos y límites explicados.** Documentar el resultado de cada escenario E-01 a E-09 como aprobado, fallido u omitido, con causa, alcance y clase de evidencia. Un caso omitido deja su capacidad sin acreditar. El cierre útil de A-01 exige la muestra real contrastada, la repetición en el mismo corte y la comprobación de solo lectura y permisos base. La disponibilidad de CA/PA y la cobertura mundial completa se evalúan como resultados adicionales.

## Supuestos y límites

- El primer ensayo usa una compilación de FM27 para Windows 11 x64, una copia de partida abierta por el validador y una referencia tomada en el mismo corte y perspectiva. Un segundo contexto sirve para probar aislamiento. Los resultados acreditan únicamente las combinaciones contrastadas de compilación, distribución, idioma, vía y perspectiva.
- La selección y comparación de vías de captura pertenece a la planificación. A-01 requiere evidencia de captura de la partida abierta. Una vista tabular visible puede servir de referencia o complemento. La evidencia de terceros orienta el ensayo y se etiqueta como tal. [Investigación de mecanismos](../../docs/to-be/12-integracion-fm27.md) · [Cuestiones abiertas Q-02/03/04/15](../../docs/OPEN-QUESTIONS.md)
- El incremento entrega evidencia y una muestra estructurada. La interfaz WinUI 3, la valoración, la proyección, el importador tabular de producto y la persistencia de carrera corresponden a otros incrementos. [Alcance y orden de Spec Kit](../../docs/roadmap/06-alcance-v1.md#incrementos-dependencias-y-orden-de-spec-kit)
- Se aplica como referencia la Constitución candidata 0.4.0. La acreditación requiere ejecutar la PoC. Este documento define la prueba y las condiciones de cierre. [Constitución](../../.specify/memory/constitution.md)
