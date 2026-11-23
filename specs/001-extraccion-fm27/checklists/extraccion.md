# Lista de revisión de requisitos · Extracción autorizada de FM27

**Propósito** Revisar la claridad y suficiencia de los requisitos de SK-01 antes de desglosar las tareas.

**Creada** 2026-11-23

**Funcionalidad** [Especificación SK-01](../spec.md) · [Plan](../plan.md) · [Modelo de datos](../data-model.md) · [Contrato de captura](../contracts/captura.md) · [Investigación](../research.md) · [Guía de validación](../quickstart.md)

**Revisión** Profundidad estándar. Destinada a quien revise la especificación y el plan.

**Responsabilidad** La persona revisora marca `[x]` cuando considera satisfecho el criterio de calidad del requisito escrito. Cada marca se refiere a la revisión documental.

## Alcance y criterios de aceptación

- [ ] CHK001 ¿El criterio de cierre de A-01 define qué futbolistas cuentan para el mínimo de 30 UID únicos y qué evidencia se admite para verificar su identidad? [Medición · Espec. SC-001, SC-004 y Casos límite]
- [ ] CHK002 ¿Se precisan los campos mínimos que deben acreditarse en portería y campo y los estados de ojeo que debe cubrir la muestra? [Completitud · Espec. FR-004, SC-001 y E-02/03]
- [ ] CHK003 ¿Queda claro cuáles son los resultados obligatorios de A-01 y cómo se consignan los contrastes adicionales de CA/PA, atributos ocultos, mundo y aislamiento real entre carreras? [Consistencia · Espec. SC-003/004 y E-04 a E-07]
- [ ] CHK004 ¿La especificación y el plan delimitan el incremento de extracción y distinguen la evidencia de una partida abierta del contraste tabular complementario? [Alcance · Espec. FR-003 y Supuestos y límites · Plan, Contexto técnico]

## Identidad, semántica y procedencia

- [ ] CHK005 ¿Se especifica qué identifica la instancia, la compilación exacta, la carrera, la rama, el corte y la perspectiva, y qué evidencia permite asociar la sesión si falta un identificador de carrera verificable? [Claridad · Espec. FR-001/002 y E-01]
- [ ] CHK006 ¿Se define el ámbito de unicidad del UID y el tratamiento de prefijos, homónimos y filas con identidad pendiente o conflictiva? [Cobertura · Espec. FR-004, Entidades clave y Casos límite]
- [ ] CHK007 ¿Se distinguen la existencia del dato, la disponibilidad en la vía, el conocimiento del mánager y la autorización de uso? [Claridad · Espec. FR-005 · Modelo de datos, Estados y reglas]
- [ ] CHK008 ¿Se define qué conservar de los intervalos y de la familiaridad posicional cuando sólo consta una categoría o la escala numérica sigue sin acreditar? [Claridad · Espec. FR-004/005 y E-02/03]
- [ ] CHK009 ¿El contrato conserva el origen del dato y el de la evidencia de visibilidad cuando difieren, junto con el contexto, las fechas y la revisión de permisos? [Consistencia · Espec. FR-005 · Modelo de datos, Entidades y relaciones]

## Solo lectura, visibilidad y cobertura

- [ ] CHK010 ¿La regla de solo lectura delimita juego, instalación y guardados, excluye la lectura de archivos `.fm` y establece cómo atribuir posibles cambios a la captura? [Medición · Espec. FR-003 y SC-002 · Guía, Preparar el entorno]
- [ ] CHK011 ¿Los requisitos definen el ámbito base, las autorizaciones independientes y el estado inicial deshabilitado de CA y PA, con advertencia y consentimiento expreso? [Completitud · Espec. FR-006 y E-03 a E-06 · DP-05/16]
- [ ] CHK012 ¿Está claro qué puede aparecer en la muestra, los recuentos, los diagnósticos y el informe cuando hay datos restringidos o identidades de visibilidad dudosa? [Cobertura · Espec. FR-005/006 y SC-003 · Contrato, Validación en la frontera]
- [ ] CHK013 ¿Se define cómo se acreditan CA y PA por separado con una referencia independiente del mismo jugador y corte, y cuándo se declara «No disponible»? [Medición · Espec. FR-007 y E-04]
- [ ] CHK014 ¿Se definen las condiciones de cobertura completa, parcial o indeterminada para el universo cargado, con denominador independiente y reconciliación de UID cuando haya inventario comparable? [Medición · Espec. FR-008 y E-05]

## Excepciones, recuperación y compatibilidad

- [ ] CHK015 ¿Se distinguen una vista parcial válida, una captura truncada o desconectada y una incompatibilidad que sólo afecta a ciertas familias de campos? [Cobertura · Espec. FR-009 y E-08 · Contrato, Validación en la frontera]
- [ ] CHK016 ¿Se especifica qué evidencia se invalida tras una recarga, un cambio de contexto o de permisos, una pérdida de conocimiento de ojeo o una actualización del juego? [Cobertura · Espec. FR-001/006/009 y E-07/09 · Investigación R4]
- [ ] CHK017 ¿Se precisa cuándo puede conservarse una muestra anterior autorizada y cómo se distingue de una muestra nueva? [Claridad · Espec. FR-009 y E-08]
- [ ] CHK018 ¿El contrato distingue el reenvío idéntico del conflicto de contenido con un mismo identificador y fija el efecto de ambos sobre las observaciones aceptadas? [Claridad · Espec. FR-009 y E-07 · Modelo de datos, Identidad, publicación y repetición]

## Evidencia, dependencias y límites del ensayo

- [ ] CHK019 ¿Se fija qué elementos del contexto, los valores y los estados deben coincidir entre las dos capturas y qué metadatos pueden variar? [Medición · Espec. SC-002 · Contrato, Validación en la frontera]
- [ ] CHK020 ¿El plan indica qué límites se fijan antes de cada ensayo, qué medidas de duración y memoria se registran y cómo reconocer un resultado comparable o cancelado? [Medición · Espec. FR-010 y SC-002 · Plan, Contexto técnico · Guía, Ejecutar la comprobación]
- [ ] CHK021 ¿El informe requerido distingue evidencia real, sintética y de terceros, y explica omisiones, discrepancias y límites por campo, vía y compilación? [Trazabilidad · Espec. FR-010 y SC-004 · Contrato, Entrada y salida]
- [ ] CHK022 ¿El plan separa los supuestos sobre la instalación existente, la compatibilidad y los derechos de uso de los resultados observados, y documenta qué condiciones permiten ampliar sólo una vía al mínimo de A-01? [Dependencias y supuestos · Plan, Secuencia de ejecución prevista · Investigación R1 a R4]

## Notas

- Dejar pendiente cualquier criterio que requiera aclaración o corrección. Registrar los hallazgos junto a su ID y enlazar el requisito afectado.
- `$speckit-implement` consulta el estado de las casillas. La evaluación corresponde a la persona revisora.
- [Lista general de calidad](requirements.md) tiene su propio ciclo de revisión mediante `$speckit-specify` y `$speckit-clarify`.
