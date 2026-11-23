---
description: "Tareas de implementación y validación de SK-01"
---

# Tareas · SK-01 · Extracción autorizada de FM27

**Entrada** [Especificación](spec.md) · [Plan](plan.md) · [Investigación](research.md) · [Modelo de datos](data-model.md) · [Contrato de captura](contracts/captura.md) · [Guía de validación](quickstart.md)

**Alcance** PoC A de captura en solo lectura desde una partida FM27 abierta. El MVP corresponde a A-01. A-02 mide el universo cargado y A-03 estudia identidad, aislamiento y permisos. La persistencia de producto, la valoración y la interfaz definitiva corresponden a otros incrementos.

**Formato** `[P]` marca trabajo sobre archivos distintos que puede avanzar una vez completadas sus dependencias. `[USn]` conserva el ID de historia de la especificación. Las rutas de código son relativas al repositorio. `%LOCALAPPDATA%` identifica el directorio del usuario para evidencia local de prueba.

## Fase 1 · Preparación compartida

- [ ] T001 [P] Crear `FM27Analyzer.slnx` y `src/FM27Analyzer.Extraction.Poc/FM27Analyzer.Extraction.Poc.csproj` con C#/.NET 10, nulabilidad y analizadores. La configuración habitual debe compilar sin FM27 ni dependencias del cargador.
- [ ] T002 Incorporar `tests/FM27Analyzer.Extraction.Tests/FM27Analyzer.Extraction.Tests.csproj` a `FM27Analyzer.slnx` como proyecto MSTest. Verificar que restauración, compilación y pruebas se pueden invocar por terminal en Windows 11 x64.
- [ ] T003 [P] Registrar el entorno, la compilación y tienda de FM27, las versiones verificables del BepInEx ya instalado, los derechos que afectan al ensayo y la línea base sin captura en `%LOCALAPPDATA%/FM27Analyzer/SK-01/banco.json`. Identificar la copia y las condiciones de solo lectura sin abrir archivos `.fm`. Si falta un recurso, registrar qué ensayo queda bloqueado. Seguir la [preparación](quickstart.md#preparar-el-entorno) y [R1/R4](research.md).

## Fase 2 · Contrato y reglas compartidas

- [ ] T004 Definir el contexto verificable y la política por sesión en `src/FM27Analyzer.Extraction.Poc/Capture/CaptureContext.cs` y `src/FM27Analyzer.Extraction.Poc/Capture/CapturePolicy.cs`.
  Campos de [data-model.md § Entidades y relaciones](data-model.md#entidades-y-relaciones): `Contexto de captura.edición`, `Contexto de captura.compilación exacta`, `Contexto de captura.distribución`, `Contexto de captura.idioma`, `Contexto de captura.instancia efímera`, `Contexto de captura.sesión`, `Contexto de captura.referencia local a la copia`, `Contexto de captura.identidad de carrera verificada`, `Contexto de captura.confirmación expresa de copia y corte`, `Contexto de captura.rama`, `Contexto de captura.perspectiva`, `Contexto de captura.club dirigido`, `Contexto de captura.fecha del juego`, `Contexto de captura.ámbito`, `Contexto de captura.revisión de política`, `Política efectiva.ámbito base`, `Política efectiva.autorizaciones independientes para mundo`, `Política efectiva.atributos ocultos`, `Política efectiva.CA/PA` y `Política efectiva.revisión por sesión`.
- [ ] T005 Definir el lote y las observaciones con tipos independientes de las vías en `src/FM27Analyzer.Extraction.Poc/Capture/ObservationBatch.cs` y `src/FM27Analyzer.Extraction.Poc/Capture/PlayerObservation.cs`.
  Campos de [data-model.md § Entidades y relaciones](data-model.md#entidades-y-relaciones): `Muestra.identificador de lote`, `Muestra.esquema`, `Muestra.contexto`, `Muestra.vía`, `Muestra.revisión del adaptador`, `Muestra.instante UTC`, `Muestra.límites`, `Muestra.manifiesto de cierre`, `Muestra.identidades autorizadas`, `Muestra.observaciones`, `Muestra.muestra esperada`, `Futbolista.UID de origen`, `Futbolista.tipo de fuente`, `Futbolista.carrera`, `Futbolista.vínculo de club`, `Observación.jugador`, `Observación.clave de campo`, `Observación.calificador`, `Observación.tipo`, `Observación.unidad o escala`, `Observación.valor`, `Observación.estados`, `Observación.procedencia del dato`, `Observación.evidencia de visibilidad`, `Observación.contexto común de la muestra` y `Observación.revisión del catálogo`.
- [ ] T006 [P] Aplicar los permisos efectivos y estados de conocimiento antes de consultar, contar o producir resultados en `src/FM27Analyzer.Extraction.Poc/Capture/CaptureAuthorization.cs`. Usar `Observación.valor` y `Observación.estados` según [data-model.md § Estados y reglas](data-model.md#estados-y-reglas) y FR-005/006.
- [ ] T007 [P] Implementar el intercambio JSON local acotado y la publicación de un lote completo fuera del juego en `src/FM27Analyzer.Extraction.Poc/Capture/BatchExchange.cs`. El consumidor de intercambio sólo recibe la proyección aceptada por la política. Seguir [Entrada y salida](contracts/captura.md#entrada-y-salida).
- [ ] T008 Validar versión, tamaño, identidad, unicidad, manifiesto y permiso vigente antes de aceptar la proyección en `src/FM27Analyzer.Extraction.Poc/Capture/BatchValidation.cs`. Aplicar la [validación en la frontera](contracts/captura.md#validación-en-la-frontera) y las claves de `Muestra.identificador de lote`, `Futbolista.UID de origen`, `Observación.clave de campo` y `Observación.calificador` de [data-model.md § Identidad, publicación y repetición](data-model.md#identidad-publicación-y-repetición).
- [ ] T009 [P] Definir el informe sanitizado por escenario y capacidad, con motivos y evidencia trazable, en `src/FM27Analyzer.Extraction.Poc/Evidence/EvidenceReport.cs` conforme al [contrato](contracts/captura.md#entrada-y-salida) y FR-010.
  Campos de [data-model.md § Entidades y relaciones](data-model.md#entidades-y-relaciones): `Comparación.referencia independiente`, `Comparación.entidad y campo comparables`, `Comparación.esperados`, `Comparación.recuperados`, `Comparación.coincidencias`, `Comparación.discrepancias`, `Comparación.alcance`, `Comparación.duración`, `Comparación.memoria pico` y `Comparación.clase de evidencia`. Usar `Comparación.acreditación de dato` y `Comparación.cobertura` según [data-model.md § Estados y reglas](data-model.md#estados-y-reglas).

**Punto de control** El proyecto y el contrato compartido se pueden comprobar con datos controlados. Los bloqueos del entorno quedan registrados por ensayo.

## Fase 3 · US1 (P1) · Contrastar una muestra real

**Objetivo** Acreditar A-01 con observaciones autorizadas y una referencia independiente del mismo corte.

**Prueba independiente** E-01 a E-03 y SC-001/002/004, más permisos base de SC-003. Dos capturas en una copia real con al menos 30 UID únicos verificados, exacto, rango, desconocimiento y los campos mínimos de portero y jugador de campo. El control sintético de E-03 se identifica como tal. Documentar discrepancias, permisos, cambios atribuibles al ensayo, duración y memoria pico.

### Pruebas

- [ ] T010 [P] [US1] Escribir casos de contrato para la comparación de UID, campos, cortes, procedencia y estados con la referencia y dos lotes en `tests/FM27Analyzer.Extraction.Tests/A01ComparisonTests.cs`. Cubrir los mínimos de SC-001/002 y las discrepancias que impiden acreditar un campo.
- [ ] T011 [P] [US1] Escribir el control de E-03 con dos conjuntos sintéticos que difieran sólo en datos ocultos en `tests/FM27Analyzer.Extraction.Tests/BaseVisibilityTests.cs`. Comparar proyección, recuentos y diagnósticos bajo los permisos base.

### Implementación y contraste

- [ ] T012 [P] [US1] Fijar la muestra de hasta tres futbolistas para viabilidad y la de al menos 30 UID para A-01, y preparar la referencia del juego en `%LOCALAPPDATA%/FM27Analyzer/SK-01/ensayo-a01/referencia.json` con el contexto de E-01 y los casos de SC-001. Si falta la partida o una referencia verificable, anotar el bloqueo en `%LOCALAPPDATA%/FM27Analyzer/SK-01/banco.json`.
- [ ] T013 [US1] Si el BepInEx existente permite cumplir FR-003, preparar sólo el ensayo R1 de instancia, UID y un campo visible en `experiments/FM27Analyzer.BepInExProbe/FM27Analyzer.BepInExProbe.csproj` y `experiments/FM27Analyzer.BepInExProbe/VisibleFieldProbe.cs`, con la configuración de laboratorio en `FM27Analyzer.slnx`. Guardar en `%LOCALAPPDATA%/FM27Analyzer/SK-01/bepinex.json` el contraste o el motivo que impide realizarlo.
- [ ] T014 [US1] Evaluar R2 sobre el mismo corte y campo visible, con acceso de solo lectura y privilegios mínimos, y registrar el resultado en `%LOCALAPPDATA%/FM27Analyzer/SK-01/lectura-externa.json`. Si existe una interpretación verificable y autorizada, acotar el ensayo a ese campo en `src/FM27Analyzer.Extraction.Poc/Capture/ExternalFieldProbe.cs`. Si falta acceso, identidad, correspondencia o autorización, registrar el bloqueo.
- [ ] T015 [US1] Comparar ambos ensayos según R3 y registrar la vía elegida o el bloqueo de A-01 en `%LOCALAPPDATA%/FM27Analyzer/SK-01/decision-via.json`. Limitar la ampliación a una vía que cumpla solo lectura, identidad, coincidencia, visibilidad y repetibilidad.
- [ ] T016 [P] [US1] Implementar el cotejo determinista de los dos lotes con la referencia, con estado y causa por campo, en `src/FM27Analyzer.Extraction.Poc/Evidence/A01Comparison.cs`. Cumplir E-02 y SC-001/002/004.
- [ ] T017 [US1] Conectar `verify --run <manifiesto> --out <informe>` al validador y al cotejo en `src/FM27Analyzer.Extraction.Poc/Program.cs`. Emitir el informe previsto en el contrato y diferenciar entrada inválida de A-01 sin acreditar.
- [ ] T018 [US1] Ampliar sólo la vía seleccionada en `experiments/FM27Analyzer.BepInExProbe/VisibleFieldProbe.cs` o `src/FM27Analyzer.Extraction.Poc/Capture/ExternalFieldProbe.cs` a los campos de FR-004 y la muestra prefijada. Reutilizar el contrato y la política compartidos. Si T015 queda bloqueada, dejar la ampliación pendiente.
- [ ] T019 [US1] Ejecutar E-01 a E-03 siguiendo la [guía](quickstart.md#resultados-esperados) y guardar el manifiesto y el informe en `%LOCALAPPDATA%/FM27Analyzer/SK-01/ensayo-a01/run.json` y `%LOCALAPPDATA%/FM27Analyzer/SK-01/ensayo-a01/informe.json`. Si falta una vía apta o la partida real, consignar el bloqueo y la evidencia sintética por separado.

## Fase 4 · US3 (P1) · Detectar incompatibilidades y descartar capturas inválidas

**Objetivo** Aislar carreras, perspectiva y permisos y diagnosticar fallos sin publicar lotes inválidos.

**Prueba independiente** E-07 a E-09 con muestras controladas. Para acreditar aislamiento real A-03, alternar dos copias que compartan club y UID, probar recarga y registrar los contrastes omitidos. Verificar reenvío, revocación y recuperación con muestras del mismo contexto.

### Pruebas

- [ ] T020 [P] [US3] Cubrir recarga, cambio de carrera, rama o mánager, UID coincidente, revocación y pérdida de conocimiento en `tests/FM27Analyzer.Extraction.Tests/CaptureSessionTests.cs` con los casos E-07/09.
- [ ] T021 [P] [US3] Cubrir lote idéntico, conflicto de ID, compilación incompatible, partida cerrada, truncado, desconexión y recuperación con una vía independiente en `tests/FM27Analyzer.Extraction.Tests/SampleRecoveryTests.cs` según E-07/08.

### Implementación y contraste

- [ ] T022 [US3] Gestionar cambios de contexto o revisión, descartar trabajo en curso y revalidar el vínculo al reanudar en `src/FM27Analyzer.Extraction.Poc/Capture/CaptureSession.cs`. Aplicar `Contexto de captura.sesión` y `Contexto de captura.revisión de política` conforme a [data-model.md § Identidad, publicación y repetición](data-model.md#identidad-publicación-y-repetición).
- [ ] T023 [US3] Incorporar aceptación idempotente, diagnóstico por capacidad y recuperación de una muestra previa aún autorizada en `src/FM27Analyzer.Extraction.Poc/Capture/SampleRecovery.cs`. Aplicar `Muestra.identificador de lote`, `Muestra.manifiesto de cierre` y `Muestra.contexto` de [data-model.md § Identidad, publicación y repetición](data-model.md#identidad-publicación-y-repetición) y FR-009.
- [ ] T024 [US3] Integrar T022/T023 en `src/FM27Analyzer.Extraction.Poc/Program.cs` y registrar E-07 a E-09 en `%LOCALAPPDATA%/FM27Analyzer/SK-01/aislamiento.json`. Repetir los casos aplicables con dos copias si hay una vía apta. Si ello no es posible, registrar la omisión y mantener separada la evidencia sintética.

## Fase 5 · US2 (P2) · Comprobar capacidades de acceso adicionales

**Objetivo** Acreditar por separado CA, PA, exactos ocultos y cobertura del universo cargado, o explicar por qué siguen sin acreditar.

**Prueba independiente** E-04 a E-06 con referencia independiente, permisos específicos y el mismo corte. La cobertura tiene su propio resultado y puede quedar parcial o indeterminada. Los contrastes con datos controlados se ejecutan sin la muestra real de 30 UID.

### Pruebas

- [ ] T025 [P] [US2] Cubrir comparación separada de CA y PA, falta de referencia, revocación y ausencia de valores especiales en resultados persistidos en `tests/FM27Analyzer.Extraction.Tests/SpecialFieldAssessmentTests.cs` según E-04/06.
- [ ] T026 [P] [US2] Cubrir ámbitos, denominadores, correspondencia de conjuntos de UID y clasificación completa, parcial o indeterminada en `tests/FM27Analyzer.Extraction.Tests/WorldCoverageAssessmentTests.cs` según E-05.

### Implementación y contraste

- [ ] T027 [P] [US2] Incorporar el contraste efímero autorizado de CA, PA y exactos ocultos, con motivos de indisponibilidad y salida agregada, en `src/FM27Analyzer.Extraction.Poc/Evidence/SpecialFieldAssessment.cs`. Aplicar `Observación.valor` y `Comparación.acreditación de dato` de [data-model.md § Estados y reglas](data-model.md#estados-y-reglas) y FR-007.
- [ ] T028 [P] [US2] Incorporar evaluación acotada y clasificación de cobertura del mismo universo, preservando los demás permisos y distinguiendo cobertura de identidades y de campos, en `src/FM27Analyzer.Extraction.Poc/Evidence/WorldCoverageAssessment.cs`. Aplicar `Comparación.referencia independiente`, `Comparación.alcance` y `Comparación.cobertura` de [data-model.md § Entidades y relaciones](data-model.md#entidades-y-relaciones) y [Estados y reglas](data-model.md#estados-y-reglas), y FR-008.
- [ ] T029 [US2] Incorporar al informe los resultados opcionales de E-04 a E-06 aportados por el manifiesto en `src/FM27Analyzer.Extraction.Poc/Evidence/EvidenceReport.cs` y `src/FM27Analyzer.Extraction.Poc/Capture/BatchExchange.cs`. Conservar la acreditación propia de A-01 según SC-004.
- [ ] T030 [US2] Ejecutar los contrastes E-04 a E-06 con capacidad, referencias y permisos adecuados. Registrar resultado, alcance, cancelaciones u omisiones en `%LOCALAPPDATA%/FM27Analyzer/SK-01/capacidades-adicionales.json` sin almacenar valores especiales. Una capacidad sin evidencia queda sin acreditar.

## Fase 6 · Cierre y comprobaciones transversales

- [ ] T031 Repetir compilación y pruebas desde `FM27Analyzer.slnx` y [la guía](quickstart.md), verificando una salida de referencia y los casos negativos documentados. Registrar comandos, entorno, resultados y pruebas reales omitidas en `specs/001-extraccion-fm27/evidencia.md`.
- [ ] T032 Consolidar en `specs/001-extraccion-fm27/evidencia.md` la matriz autorizada por campo, vía, compilación y perspectiva, y las decisiones que queden abiertas para SK-03. Actualizar `docs/OPEN-QUESTIONS.md` cuando exista evidencia que cambie una cuestión y `docs/CHANGELOG-TO-BE.md` si se confirma un cambio de comportamiento o alcance. Mantener las capturas reales en su directorio local.

## Dependencias y orden de ejecución

| Bloque | Dependencias |
| --- | --- |
| Preparación | T001 → T002. T003 puede avanzar en paralelo con ambos. |
| Contrato compartido | T001/T002 → T004 → T005 → T006/T007/T009. T008 espera T006 y T007. T003 condiciona los ensayos reales. |
| US1 · A-01 | Tras la fase 2, T010/T011 y T012 pueden avanzar en paralelo cuando estén disponibles sus entradas. T012 → T013 → T014 → T015 → T018. T016 espera T010 y puede avanzar junto al banco y los ensayos de viabilidad. T017 espera T016. T019 necesita T011, T017 y una vía apta según T015/T018 para el contraste real. |
| US3 · A-03 | Tras la fase 2, T020/T021 pueden avanzar con datos controlados. T020 → T022 y T021/T022 → T023. La integración T024 espera T017 y T023. Su contraste real depende de una vía apta y de las dos copias. |
| US2 · A-02 y campos especiales | Tras la fase 2, T025 → T027 y T026 → T028 pueden avanzar con datos controlados. T029 espera T027 y T028. T030 espera T029 y, para sus casos reales, la capacidad y los permisos aplicables. |
| Cierre | T031 reúne resultados, fallos y omisiones de T019/T024/T030. T032 consolida el resultado de T031. |

### Ejemplos de ejecución en paralelo

- **US1** T010 y T011 escriben pruebas en archivos distintos mientras T012 prepara la referencia local. T013 y T014 comparten la partida y conservan su orden.
- **US3** T020 y T021 cubren respectivamente cambios de sesión y recuperación, con datos controlados.
- **US2** T025 y T026 preparan controles independientes. Cuando sus pruebas estén listas, T027 y T028 pueden avanzar sobre servicios distintos.

## Estrategia de implementación

1. Completar la preparación y el contrato compartido. Escribir las pruebas de cada historia antes de su implementación y comprobar que detectan cuando falta el comportamiento previsto.
2. Entregar primero US1. Evaluar ambos ensayos pequeños cuando se den sus condiciones y ampliar únicamente la vía acreditable. Cerrar A-01 con SC-004 o registrar qué falta para acreditarlo.
3. Completar US3 y US2 como incrementos verificables por separado. Publicar el alcance demostrado, incluido el caso en que una capacidad quede sin acreditar.
4. Revisar trazabilidad, resultados y omisiones antes de trasladar decisiones a SK-03. Cada bloque documental o funcional queda asociado a su commit y a una verificación repetible.
