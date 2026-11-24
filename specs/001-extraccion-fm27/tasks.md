---
description: "Tareas de implementación y validación de SK-01"
---

# Tareas · SK-01 · Extracción autorizada de FM27

**Entrada** [Especificación](spec.md) · [Plan](plan.md) · [Investigación](research.md) · [Modelo de datos](data-model.md) · [Contrato de captura](contracts/captura.md) · [Guía de validación](quickstart.md)

**Alcance** PoC A de captura en solo lectura desde una partida FM27 abierta. El MVP corresponde a A-01. A-02 mide el universo cargado y A-03 estudia identidad, aislamiento y permisos. La persistencia de producto, la valoración y la interfaz definitiva corresponden a otros incrementos.

**Formato** `[P]` marca trabajo sobre archivos distintos que puede avanzar una vez completadas sus dependencias. `[USn]` conserva el ID de historia de la especificación. Las rutas de código son relativas al repositorio. `%LOCALAPPDATA%` identifica el directorio del usuario para evidencia local de prueba.

## Fase 1 · Preparación compartida

- [X] T001 [P] Crear `FM27Analyzer.slnx` y `src/FM27Analyzer.Extraction.Poc/FM27Analyzer.Extraction.Poc.csproj` con C#/.NET 10, nulabilidad y analizadores. La configuración habitual debe compilar sin FM27 ni dependencias del cargador.
- [X] T002 Incorporar `tests/FM27Analyzer.Extraction.Tests/FM27Analyzer.Extraction.Tests.csproj` a `FM27Analyzer.slnx` como proyecto MSTest. Verificar que restauración, compilación y pruebas se pueden invocar por terminal en Windows 11 x64.
- [X] T003 [P] Registrar el entorno, la compilación y tienda de FM27, las versiones verificables del BepInEx ya instalado, los derechos que afectan al ensayo y la línea base sin captura en `%LOCALAPPDATA%/FM27Analyzer/SK-01/banco.json`. Identificar la copia y las condiciones de solo lectura sin abrir archivos `.fm`. Si falta un recurso, registrar qué ensayo queda bloqueado. Seguir la [preparación](quickstart.md#preparar-el-entorno) y [R1/R4](research.md).

## Fase 2 · Contrato y reglas compartidas

- [X] T004 Definir el contexto verificable y la política por sesión en `src/FM27Analyzer.Extraction.Poc/Capture/CaptureContext.cs` y `src/FM27Analyzer.Extraction.Poc/Capture/CapturePolicy.cs`.
  Campos de [data-model.md § Entidades y relaciones](data-model.md#entidades-y-relaciones): `Contexto de captura.edición`, `Contexto de captura.compilación exacta`, `Contexto de captura.distribución`, `Contexto de captura.idioma`, `Contexto de captura.instancia efímera`, `Contexto de captura.sesión`, `Contexto de captura.referencia local a la copia`, `Contexto de captura.identidad de carrera verificada`, `Contexto de captura.confirmación expresa de copia y corte`, `Contexto de captura.rama`, `Contexto de captura.perspectiva`, `Contexto de captura.club dirigido`, `Contexto de captura.fecha del juego`, `Contexto de captura.ámbito`, `Contexto de captura.revisión de política`, `Política efectiva.ámbito base`, `Política efectiva.autorizaciones independientes para mundo`, `Política efectiva.atributos ocultos`, `Política efectiva.CA/PA` y `Política efectiva.revisión por sesión`.
- [X] T005 Definir el lote y las observaciones con tipos independientes de las vías en `src/FM27Analyzer.Extraction.Poc/Capture/ObservationBatch.cs` y `src/FM27Analyzer.Extraction.Poc/Capture/PlayerObservation.cs`.
  Campos de [data-model.md § Entidades y relaciones](data-model.md#entidades-y-relaciones): `Muestra.identificador de lote`, `Muestra.esquema`, `Muestra.contexto`, `Muestra.vía`, `Muestra.revisión del adaptador`, `Muestra.instante UTC`, `Muestra.límites`, `Muestra.manifiesto de cierre`, `Muestra.identidades autorizadas`, `Muestra.observaciones`, `Futbolista.UID de origen`, `Futbolista.tipo de fuente`, `Futbolista.carrera`, `Futbolista.vínculo de club`, `Observación.jugador`, `Observación.clave de campo`, `Observación.calificador`, `Observación.tipo`, `Observación.unidad o escala`, `Observación.valor`, `Observación.estados`, `Observación.procedencia del dato`, `Observación.evidencia de visibilidad`, `Observación.contexto común de la muestra` y `Observación.revisión del catálogo`.
- [X] T006 [P] Aplicar los permisos efectivos y estados de conocimiento antes de consultar, contar o producir resultados en `src/FM27Analyzer.Extraction.Poc/Capture/CaptureAuthorization.cs`. Usar `Observación.valor` y `Observación.estados` según [data-model.md § Estados y reglas](data-model.md#estados-y-reglas) y FR-005/006.
- [X] T007 [P] Implementar el intercambio JSON local acotado, las modalidades del manifiesto, la lectura de sus referencias a A-02/A-03 y la publicación de un lote completo fuera del juego en `src/FM27Analyzer.Extraction.Poc/Capture/BatchExchange.cs`. El intercambio recibe la proyección autorizada después de que el productor revalide la sesión. Aplicar `Referencia de contraste.contexto verificado del ensayo`, `Referencia de contraste.muestra esperada` y `Referencia de contraste.observaciones de referencia` de [data-model.md § Entidades y relaciones](data-model.md#entidades-y-relaciones) y [Entrada y salida](contracts/captura.md#entrada-y-salida).
- [X] T008 [P] Escribir en `tests/FM27Analyzer.Extraction.Tests/CaptureGuardTests.cs` el control sintético previo a toda captura real. Cubrir sesión válida, cambio de instancia, carrera, rama, perspectiva, corte o revisión durante el trabajo, y contexto incierto antes de leer o publicar. Exigir cancelación y ausencia de publicación del lote pendiente.
- [X] T009 Vincular la captura a la sesión y revisión vigentes en `src/FM27Analyzer.Extraction.Poc/Capture/CaptureSession.cs`. Comprobar el contexto antes de leer, cancelar el trabajo afectado y descartar el lote pendiente si cambia, y validar de nuevo justo antes de publicar. Si el contexto es incierto, registrar el bloqueo. Aplicar `Contexto de captura.sesión`, `Contexto de captura.revisión de política` y `Política efectiva.revisión por sesión` de [data-model.md § Identidad, publicación y repetición](data-model.md#identidad-publicación-y-repetición). Ejecutar T008 con resultado satisfactorio antes de cualquier captura real.
- [X] T010 Validar versión, tamaño, identidad, unicidad, manifiesto y permiso vigente antes de aceptar la proyección en `src/FM27Analyzer.Extraction.Poc/Capture/BatchValidation.cs`. Aplicar la [validación en la frontera](contracts/captura.md#validación-en-la-frontera) y las claves de `Muestra.identificador de lote`, `Futbolista.UID de origen`, `Observación.clave de campo` y `Observación.calificador` de [data-model.md § Identidad, publicación y repetición](data-model.md#identidad-publicación-y-repetición).
- [X] T011 [P] Definir el informe sin valores sensibles en `src/FM27Analyzer.Extraction.Poc/Evidence/EvidenceReport.cs`. Reúne A-01 y la evidencia adicional referenciada, registra los resultados por escenario y clase de evidencia e indica las omisiones y sus motivos según el [contrato](contracts/captura.md#modalidades-de-a-01-y-resultados-adicionales) y FR-010.
  Campos de [data-model.md § Entidades y relaciones](data-model.md#entidades-y-relaciones): `Comparación.referencia independiente`, `Comparación.entidad y campo comparables`, `Comparación.esperados`, `Comparación.recuperados`, `Comparación.coincidencias`, `Comparación.discrepancias`, `Comparación.alcance`, `Comparación.duración`, `Comparación.memoria pico` y `Comparación.clase de evidencia`. Usar `Comparación.acreditación de dato` y `Comparación.cobertura` según [data-model.md § Estados y reglas](data-model.md#estados-y-reglas).

**Punto de control** Completar el contrato y verificar T008 con datos controlados antes de los ensayos reales. Cada adaptador debe aplicar ese control en el punto de captura y publicación. Si carece de contexto verificable o el entorno no está disponible, registrar el ensayo como bloqueado.

## Fase 3 · US1 (P1) · Contrastar una muestra real

**Objetivo** Acreditar A-01 con observaciones autorizadas y una referencia independiente del mismo corte.

**Prueba independiente** E-01 a E-03 y SC-001/002/004, más permisos base de SC-003. Dos capturas en una copia real con al menos 30 UID únicos verificados, exacto, rango, desconocimiento y los campos mínimos de portero y jugador de campo. El control sintético de E-03 se identifica como tal. Documentar discrepancias, permisos, cambios atribuibles al ensayo, duración y memoria pico.

### Pruebas

- [X] T012 [P] [US1] Escribir casos para dos lotes cotejables, ejecución bloqueada sin lotes y entrada malformada en `tests/FM27Analyzer.Extraction.Tests/A01ComparisonTests.cs`. Cubrir discrepancias de UID, campos, corte, procedencia y estados. Comprobar que el informe conserva evidencia A-02/A-03 cuando existe y registra la omisión cuando falta, según SC-001/002/004.
- [X] T013 [P] [US1] Escribir el control de E-03 con dos conjuntos sintéticos que difieran sólo en datos ocultos en `tests/FM27Analyzer.Extraction.Tests/BaseVisibilityTests.cs`. Comparar proyección, recuentos y diagnósticos bajo los permisos base.

### Implementación y contraste

- [X] T014 [P] [US1] Fijar hasta tres futbolistas para los ensayos de viabilidad y preparar la referencia del campo visible en `%LOCALAPPDATA%/FM27Analyzer/SK-01/viabilidad/referencia.json`. Si falta la partida o una referencia verificable, registrar el motivo en `%LOCALAPPDATA%/FM27Analyzer/SK-01/banco.json`.
- [X] T015 [US1] Si el BepInEx existente permite cumplir FR-003, preparar el ensayo R1 en `experiments/FM27Analyzer.BepInExProbe/FM27Analyzer.BepInExProbe.csproj` y `experiments/FM27Analyzer.BepInExProbe/VisibleFieldProbe.cs`, con la configuración de laboratorio en `FM27Analyzer.slnx`. Aplicar T009 en el adaptador según el contrato. Obtener dos lecturas del mismo campo visible, UID y corte, y guardar el contraste o el motivo del bloqueo en `%LOCALAPPDATA%/FM27Analyzer/SK-01/bepinex.json`.
- [X] T016 [US1] Evaluar R2 con el mismo campo visible, UID y corte, en solo lectura y con privilegios mínimos. Si existe una interpretación verificable y autorizada, aplicar T009 y obtener dos lecturas en `src/FM27Analyzer.Extraction.Poc/Capture/ExternalFieldProbe.cs`. Registrar el contraste o la causa del bloqueo en `%LOCALAPPDATA%/FM27Analyzer/SK-01/lectura-externa.json`.
- [X] T017 [US1] Comparar los dos resultados de cada vía viable según R3, conservando valores, estados y procedencia. Registrar la vía elegida o la causa del bloqueo en `%LOCALAPPDATA%/FM27Analyzer/SK-01/decision-via.json`. Limitar la ampliación a una vía que cumpla solo lectura, identidad, coincidencia, visibilidad y repetibilidad.
- [X] T018 [US1] Si T017 acredita una vía apta para ampliar, fijar al menos 30 UID y sus casos de contraste independientes antes de la primera captura de A-01. Guardar la referencia en `%LOCALAPPDATA%/FM27Analyzer/SK-01/ensayo-a01/referencia.json` con el contexto E-01 y los mínimos de SC-001. Si la vía o la referencia faltan, registrar la causa para el manifiesto bloqueado de T022.
- [X] T019 [P] [US1] Implementar el cotejo determinista de los dos lotes con la referencia, con estado y causa por campo, en `src/FM27Analyzer.Extraction.Poc/Evidence/A01Comparison.cs`. Cumplir E-02 y SC-001/002/004.
- [X] T020 [US1] Conectar `verify --run <manifiesto> --out <informe>` al validador en `src/FM27Analyzer.Extraction.Poc/Program.cs`. En modalidad `contrastable`, ejecutar el cotejo. En modalidad `bloqueada`, emitir A-01 sin acreditar, con motivo, alcance y clase de evidencia. Diagnosticar por separado la entrada inválida.
- [ ] T021 [US1] Si T017 selecciona una vía, ampliar esa vía en `experiments/FM27Analyzer.BepInExProbe/VisibleFieldProbe.cs` o `src/FM27Analyzer.Extraction.Poc/Capture/ExternalFieldProbe.cs` a los campos de FR-004 y la muestra de T018. Aplicar el contrato, la política y el control de sesión de T009 antes de aceptar o publicar una captura.
- [X] T022 [US1] Ejecutar E-01 a E-03 cuando estén disponibles la vía apta y la referencia independiente. Guardar el manifiesto y el informe en `%LOCALAPPDATA%/FM27Analyzer/SK-01/ensayo-a01/run.json` y `%LOCALAPPDATA%/FM27Analyzer/SK-01/ensayo-a01/informe.json`. Si faltan entradas para el contraste real, usar modalidad `bloqueada`, consignar el motivo y mantener separada la evidencia sintética.

## Fase 4 · US3 (P1) · Detectar incompatibilidades y descartar capturas inválidas

**Objetivo** Aislar carreras, perspectiva y permisos y diagnosticar fallos sin publicar lotes inválidos.

**Prueba independiente** E-07 a E-09 con muestras controladas. Para acreditar aislamiento real A-03, alternar dos copias que compartan club y UID, probar recarga y registrar los contrastes omitidos. Verificar reenvío, revocación y recuperación con muestras del mismo contexto.

### Pruebas

- [X] T023 [P] [US3] Cubrir recarga, revinculación tras cambiar de carrera, rama o mánager, UID coincidente, revocación y pérdida de conocimiento en `tests/FM27Analyzer.Extraction.Tests/CaptureSessionTests.cs` con E-07/09. Comprobar que el contexto posterior y los resultados recuperados respetan el control compartido de T009.
- [X] T024 [P] [US3] Cubrir lote idéntico, conflicto de ID, compilación incompatible, partida cerrada, truncado, desconexión y recuperación con una vía independiente en `tests/FM27Analyzer.Extraction.Tests/SampleRecoveryTests.cs` según E-07/08.

### Implementación y contraste

- [X] T025 [US3] Incorporar aceptación idempotente, diagnóstico por capacidad y recuperación de una muestra previa aún autorizada en `src/FM27Analyzer.Extraction.Poc/Capture/SampleRecovery.cs`. Aplicar `Muestra.identificador de lote`, `Muestra.manifiesto de cierre` y `Muestra.contexto` de [data-model.md § Identidad, publicación y repetición](data-model.md#identidad-publicación-y-repetición) y FR-009.
- [X] T026 [US3] Integrar T009 y la recuperación T025 en `src/FM27Analyzer.Extraction.Poc/Program.cs`. Registrar E-07 a E-09 por clase de evidencia en `%LOCALAPPDATA%/FM27Analyzer/SK-01/aislamiento.json` para su incorporación al manifiesto. Repetir los casos aplicables con dos copias si hay una vía apta y se ha verificado T008. Si no se puede, registrar la omisión real por separado.

## Fase 5 · US2 (P2) · Comprobar capacidades de acceso adicionales

**Objetivo** Acreditar por separado CA, PA, exactos ocultos y cobertura del universo cargado, o explicar por qué siguen sin acreditar.

**Prueba independiente** E-04 a E-06 con referencia independiente, permisos específicos y el mismo corte. La cobertura tiene su propio resultado y puede quedar parcial o indeterminada. Los contrastes con datos controlados se ejecutan sin la muestra real de 30 UID.

### Pruebas

- [ ] T027 [P] [US2] Cubrir comparación separada de CA y PA, falta de referencia, revocación y ausencia de valores especiales en resultados persistidos en `tests/FM27Analyzer.Extraction.Tests/SpecialFieldAssessmentTests.cs` según E-04/06.
- [ ] T028 [P] [US2] Cubrir ámbitos, denominadores, correspondencia de conjuntos de UID y clasificación completa, parcial o indeterminada en `tests/FM27Analyzer.Extraction.Tests/WorldCoverageAssessmentTests.cs` según E-05.

### Implementación y contraste

- [ ] T029 [P] [US2] Incorporar el contraste efímero autorizado de CA, PA y exactos ocultos, con motivos de indisponibilidad y salida agregada, en `src/FM27Analyzer.Extraction.Poc/Evidence/SpecialFieldAssessment.cs`. Aplicar `Observación.valor` y `Comparación.acreditación de dato` de [data-model.md § Estados y reglas](data-model.md#estados-y-reglas) y FR-007.
- [ ] T030 [P] [US2] Incorporar evaluación acotada y clasificación de cobertura del mismo universo, preservando los demás permisos y distinguiendo cobertura de identidades y de campos, en `src/FM27Analyzer.Extraction.Poc/Evidence/WorldCoverageAssessment.cs`. Aplicar `Comparación.referencia independiente`, `Comparación.alcance` y `Comparación.cobertura` de [data-model.md § Entidades y relaciones](data-model.md#entidades-y-relaciones) y [Estados y reglas](data-model.md#estados-y-reglas), y FR-008.
- [ ] T031 [US2] Adaptar los contrastes T029/T030 a los resultados agregados de E-04 a E-06 en `src/FM27Analyzer.Extraction.Poc/Evidence/EvidenceReport.cs`. Conservar estado y clase de evidencia para el archivo adicional de T032, conforme al [contrato](contracts/captura.md#modalidades-de-a-01-y-resultados-adicionales).
- [ ] T032 [US2] Ejecutar E-04 a E-06 con capacidad, referencias y permisos adecuados, y aplicar T009 a los contrastes reales. Registrar alcance, resultado, cancelaciones u omisiones por clase de evidencia en `%LOCALAPPDATA%/FM27Analyzer/SK-01/capacidades-adicionales.json` para su incorporación al manifiesto. Mantener los valores especiales fuera de los archivos.

## Fase 6 · Cierre y comprobaciones transversales

- [ ] T033 Incorporar al manifiesto `%LOCALAPPDATA%/FM27Analyzer/SK-01/ensayo-a01/run.json` las referencias disponibles de T026/T032 y generar el informe común con `verify`. Repetir compilación y pruebas desde `FM27Analyzer.slnx` y [la guía](quickstart.md), incluida la modalidad bloqueada. Registrar comandos, resultados y omisiones en `specs/001-extraccion-fm27/evidencia.md`.
- [ ] T034 Consolidar en `specs/001-extraccion-fm27/evidencia.md` la matriz autorizada por campo, vía, compilación y perspectiva, y las decisiones que queden abiertas para SK-03. Actualizar `docs/OPEN-QUESTIONS.md` cuando exista evidencia que cambie una cuestión y `docs/CHANGELOG-TO-BE.md` si se confirma un cambio de comportamiento o alcance. Mantener las capturas reales en su directorio local.

## Dependencias y orden de ejecución

| Bloque | Dependencias |
| --- | --- |
| Preparación | T001 → T002. T003 puede avanzar en paralelo con ambos. |
| Contrato compartido | T001/T002 → T004 → T005. T006, T007, T008 y T011 pueden avanzar tras T005. T008 → T009. T010 espera T006, T007 y T009. **Toda captura real depende de T009, con T008 verificada, y de la preparación T003.** |
| US1 · A-01 | Tras la fase 2, T012/T013 y T014 pueden avanzar en paralelo cuando tengan sus entradas. T014 → T015 → T016 → T017. Con una vía apta, T017 → T018 → T021. T019 espera T012 y puede avanzar junto al banco y los ensayos de viabilidad. T020 espera T019. T022 espera T013 y T020, y usa T018/T021 para la modalidad contrastable. Con bloqueo registrado en T017 o T018, T022 sigue por la modalidad bloqueada. |
| US3 · A-03 | Tras la fase 2, T023/T024 pueden avanzar con datos controlados. T025 espera T023/T024. T026 espera T020/T025. Su contraste real depende del control T009 verificado, una vía apta y dos copias. |
| US2 · A-02 y campos especiales | Tras la fase 2, T027 → T029 y T028 → T030 pueden avanzar con datos controlados. T031 espera T029/T030. T032 espera T031 y, para sus casos reales, el control T009 verificado, la capacidad y los permisos aplicables. |
| Cierre | T033 integra resultados, fallos u omisiones de T022/T026/T032 en el informe común. T034 consolida el resultado de T033. |

### Ejemplos de ejecución en paralelo

- **US1** T012 y T013 escriben pruebas en archivos distintos mientras T014 prepara la referencia pequeña. T015 y T016 comparten la partida y conservan su orden.
- **US3** T023 y T024 cubren respectivamente cambios de sesión y recuperación, con datos controlados.
- **US2** T027 y T028 preparan controles independientes. Cuando sus pruebas estén listas, T029 y T030 pueden avanzar sobre servicios distintos.

## Estrategia de implementación

1. Completar la preparación y el contrato compartido. Escribir las pruebas de cada historia antes de su implementación y comprobar que detectan cuando falta el comportamiento previsto.
2. Entregar primero US1. Evaluar ambos ensayos pequeños con el control de sesión verificado. Preparar la referencia de 30 UID tras elegir una vía. Cerrar A-01 con SC-004 o emitir el informe bloqueado.
3. Completar US3 y US2 como incrementos verificables por separado. Publicar el alcance demostrado, incluido el caso en que una capacidad quede sin acreditar.
4. Revisar trazabilidad, resultados y omisiones antes de trasladar decisiones a SK-03. Cada bloque documental o funcional queda asociado a su commit y a una verificación repetible.
