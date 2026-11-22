# Cómo se hizo la investigación

**Corte documental del 22 de noviembre de 2026.** Punto de partida limpio en `chore/preparar-spec-kit`, commit `c5612520302d5caf1960bd976f06e92d107133d5`. El trabajo responde a decisiones de un mánager de FM27. La pregunta guía fue qué información necesita para justificar una elección y volver a comprobarla después.

## Inventario previo a elegir gráficos

| Pregunta del mánager | Evidencia mínima y origen en el repositorio | Error de diseño que permitiría una mala decisión |
|---|---|---|
| ¿Quién juega hoy y quién cubre una baja? | Puesto en cada fase, disponibilidad, inscripción y XI A/B de la misma revisión. [Táctica](../to-be/05-tacticas-y-optimizacion.md) | Contar dos veces un UID o recomendar un suplente no elegible. |
| ¿En qué supera el candidato a mi titular? | Rol, escala, observación, curva y peso conocido. [Valoraciones](../to-be/04-valoraciones-y-perfiles.md) | Interpretar un hueco de ojeo como debilidad deportiva. |
| ¿Puedo asumir el cambio? | Importe observado, hipótesis aceptada, salario, moneda y horizonte. [Mercado](../to-be/07-scouting-y-mercado.md) | Usar el valor de FM como precio negociado. |
| ¿Produce más con una muestra comparable? | Numerador, minutos, cohorte y sentido por métrica. [Estadística](../to-be/09-estadisticas-y-moneyball.md) | Mejorar el percentil estrechando el filtro de resultados. |
| ¿Ha cambiado el futbolista o el cálculo? | UID, cierres, club en cada corte y revisión. [Históricos](../to-be/10-historicos-y-temporadas.md) | Unir dos perfiles matemáticos en una serie deportiva. |
| ¿Qué puedo compartir o recuperar? | Identidad de carrera y revisión del permiso antes de consultar, ordenar o explicar. [Visibilidad](../to-be/13-visibilidad-y-juego-limpio.md) | Recuperar de una caché un dato ya revocado. |

Se contrastaron el [índice funcional](../to-be/00-indice-y-vision.md), los [42 antecedentes y sus tratamientos](../to-be/18-trazabilidad-as-is-to-be.md), los [casos del original](../as-is/24-casos-de-aceptacion-por-capacidad.md), la [arquitectura](../architecture/00-arquitectura-general.md), los [ADR de controles](../architecture/adr/004-tablas-y-graficos.md), los [criterios de interfaz](../testing/04-validacion-interfaz.md), los [hitos](../roadmap/00-roadmap-general.md), la [auditoría anterior](../audits/pre-implementation-readiness.md) y la [Constitución](../../.specify/memory/constitution.md). En `docs/ux-ui` se encontraron especificaciones y un guion textual. En el inventario versionado, las capturas anteriores pertenecen a `old_version/docs/screenshots`; no apareció un mock visual de la aplicación nueva anterior a este estudio.

## Procedimiento y confianza

1. Para cada tarea, identificar REQ, hito, antecedente y salida observable. Una decisión DP tiene autoridad sobre una analogía externa. Q-04, Q-08, Q-13 y Q-16 afectan al diseño que se someterá a revisión.
2. Examinar las diez referencias pedidas por la misma secuencia de trabajo: formar una población, elegir una persona, identificar el referente, explicar el valor y recuperar el contexto. Registrar qué se pudo ver y qué describe su autor. Una guía con imágenes acredita el ejemplo mostrado en su fecha.
3. Seleccionar una muestra pequeña de imágenes que permita discriminar alternativas. Conservar originales de prensa identificados y rutas del material histórico presente. Utilizar enlace y descripción si no está claro el permiso para conservar una copia. Separar las láminas propias y sus cifras ficticias.
4. Contrastar una lectura directa con una alternativa para la misma decisión. Anotar ventajas, coste de interacción, barreras de acceso, disponibilidad del dato y cómo decidir entre ambas en el mock o en una PoC.
5. Vincular cada fila del catálogo funcional con al menos un criterio existente. Examinar también operaciones sin pantalla propia, elementos pospuestos, cambios de carrera y permisos durante una consulta.

**Observación visual** significa que se abrió la imagen. **Documentado por el autor** significa que una fuente primaria describe la función. **Inferencia de diseño** es nuestra interpretación de su posible utilidad para FM27. Confianza alta en un rasgo visible o descrito con ejemplo concreto, media en una descripción parcial o histórica para la que falta acceso actual. El grado se refiere al hallazgo y al periodo citado. No hubo sesiones con usuarios, ejecución de los productos de terceros ni acceso a cuentas deportivas privadas.

## Herramientas y límites

Se usaron búsqueda y apertura web para las fuentes públicas, curl para tres imágenes del kit y Python/Pillow para decodificar, comparar dimensiones, inspeccionar una hoja de contacto y optimizar las láminas propias. SHA-256 identifica los originales locales. Edge y Chrome están instalados y admiten captura de páginas públicas. La navegación automatizada está disponible en la sesión; Playwright no se resolvió en Python, Node ni como ejecutable en PATH. Se prefirió la captura oficial identificada al volver a fotografiarla. Dos consultas HTTP directas a ScoutSheet y DataMB devolvieron 403. Se usaron las páginas públicas documentadas por el buscador y se mantuvieron enlaces. No se instalaron dependencias.

Para WinUI se consultaron Gallery, Learn, los paquetes publicados y la documentación XML del SDK candidato. No hay proyecto de escritorio restaurado de FM27 Analyzer ni `winapp` resoluble en PATH. La [factibilidad](09-winui3-feasibility.md) delimita lo que se comprobó y las pruebas que requieren ejecución. El [índice](00-index.md) recoge el procedimiento de auditoría local.
