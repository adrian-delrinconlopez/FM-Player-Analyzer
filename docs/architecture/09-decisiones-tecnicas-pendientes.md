# Registro de decisiones técnicas

Las decisiones de producto cerradas están en [PRODUCT-DECISIONS](../PRODUCT-DECISIONS.md). Los ADR siguientes son propuestas que se pueden contrastar individualmente. Resolver cada uno con fecha, evidencia, quién aprueba, consecuencias y revisiones afectadas. Los contratos de datos dependen de la evidencia por compilación del juego.

| ADR | Decisión propuesta | Evidencia para cerrar | Salida si falla |
|---|---|---|---|
| [001](adr/001-escritorio-y-plugin.md) | Aislar runtimes y negociar por canal local | PC-01, 02, 04 y 10 | Vista tabular autorizada y edición manual de planes mientras se investiga. |
| [002](adr/002-partidas-y-observaciones.md) | SQLite por carrera, histórico incremental | PC-02 y pruebas de WAL/retención y volumen | Mantener staging y medir alternativas de índices/granularidad. |
| [003](adr/003-proyeccion-autorizada.md) | Política común antes de cálculo, consulta y exportación | PC-03 y comparación de no inferencia | Bloquear la capacidad o proyección sin evidencia. |
| [004](adr/004-tablas-y-graficos.md) | Virtualización nativa y dibujo bajo demanda | PC-05, 06 y 07 con misma fixture | Alternativa nativa accesible y tabla numérica. |

También requieren decisión la raíz gráfica y tipos de mapa (PC-08), vía oficial de acceso a ChatGPT (PC-09), derechos de distribución, versiones concretas y firma de paquetes. [Cuestiones y responsables](../OPEN-QUESTIONS.md) · [Pruebas de concepto](../roadmap/03-riesgos-y-pruebas-de-concepto.md)
