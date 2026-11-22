# Historial de propuestas de diseño

La aprobación funcional se registra en [DP](../PRODUCT-DECISIONS.md) y las cuestiones que requieren decisión en [Q](../OPEN-QUESTIONS.md). Este historial registra la madurez de una **solución UX** y el motivo de un cambio. La madurez de una lámina, de una tecnología y de un requisito puede ser diferente. Fecha inicial de las entradas: **2026-11-22**.

| ID · estado | Objeto, motivo y evidencia | Alternativa y próxima comprobación |
|---|---|---|
| UX-01 · RECOMENDADA | Referente anclado al entrar desde carencia, REQ-SQUAD-002/004. B01/R06 y estudio D01 muestran dónde pueden perderse el titular y la escala. | Comparador simétrico. V-A1/U03. Q-16 pendiente del propietario. |
| UX-02 · RECOMENDADA | Cálculo de cohorte y filtro de resultados visibles por separado en REQ-STAT-002. B06/B07/B09 y NV-26. | Tabla con barras o tabla con dispersión. Elegir según si se comprende N y la unidad. U08, Q-08/16. |
| UX-03 · CANDIDATA | Alternar fases conservando el UID para REQ-TAC-002. R07/D02 enseñan el desplazamiento. | Dos campos cuando quepan o lista por línea. U06 y PC-06. |
| UX-04 · RECOMENDADA | Rellenar un escenario económico sólo tras adoptar un importe, REQ-DATA-003 y REQ-MARKET-003. D01/D04 y B10. | Tabla antes/después o formulario con conciliación. U05 y Q-16. |
| UX-05 · CANDIDATA | Bandeja de 2–5 y búsqueda global, REQ-SQUAD-004/REQ-UI-001 como contexto. Podrían ahorrar volver a varias listas. | Acción contextual y búsqueda local. Probar volver tras cambiar de carrera y ausencia de filtraciones antes de sugerir ampliaciones. U01/U09/U11. |
| UX-06 · CANDIDATA | Un radar secundario permite inspeccionar forma de una cohorte. La pizza de R03 queda como referencia de investigación, retirada del contenido propuesto en la revisión del 22-11. | Barras para magnitudes y tabla para cifras. Evaluar coste y huecos. Q-16, V-A6 y PC-07. |
| UX-07 · RECOMENDADA | Añadir un contorno informativo a la paleta del sistema. El borde decorativo existente dio 1,60:1 sobre Panel. La alternativa propuesta arroja 4,22:1. | Recursos del sistema en tema de contraste. U11 y V-A3 sobre controles verdaderos. |
| UX-08 · CANDIDATA | La revisión del borrador constitucional hace explícita la evolución de pantallas, visualizaciones y controles en el principio V. | Conservar sólo la delegación general de Gobierno dejaba ambiguo el catálogo anterior. Revisión y ratificación reservadas al propietario. |
| UX-09 · RECOMENDADA | Evaluar la composición por tarea con referencias contrastantes. La ampliación del atlas cubre selección, lectura y recuperación en las superficies V01–V19. | El propietario puede elegir una variante, combinarlas o solicitar otra para el mock. [Tablero y límites](15-comparativas-visuales.md) |

### Revisión constitucional

Archivo afectado: [constitución](../../.specify/memory/constitution.md). La candidata **0.2.0** ya cubría idioma y criterios por capacidad (I), autorización (II), revisión y cobertura (III), independencia y medición (IV), revisión previa al mock y PoC (V), accesibilidad, WinUI y gobierno. La ambigüedad concreta era si una vista o un componente del catálogo adquiría rango de requisito al pasar a Tasks. La [auditoría anterior](../audits/pre-implementation-readiness.md) había señalado la misma tensión en el guion de 19 superficies.

La candidata **0.3.0** añade un párrafo a V. Permite justificar una redistribución, sustituir un gráfico, personalizar un control de plataforma o crear uno propio manteniendo acuerdos, accesibilidad y criterios. El incremento menor refleja ampliación de un principio. Se conservan los encabezados, las demás cláusulas y el estado provisional. En futuros planes, Constitution Check recoge la necesidad, la alternativa y su comprobación. La edición queda hecha en `171790c`. La primera ratificación por el propietario sigue reservada para 1.0.0.

### Cómo actualizar una entrada

Registrar fecha y autor de la decisión, ID, estado anterior y nuevo, capacidad/hito, versión del mock o PoC, fixture, evidencia y límites, variante conservada o sustitutiva, efecto sobre consumidores y pruebas. Si el propietario aprueba con condiciones, transcribir el alcance de la aprobación, la fecha y el criterio pendiente.

- **CANDIDATA** identifica una hipótesis. **RECOMENDADA** identifica la preferida con justificación, conservando al menos una alternativa.
- **VALIDADA** requiere evidencia o aprobación explícita y un ámbito identificado. La revisión de un documento, una captura de tercero o la existencia de una API no validan una experiencia propia.
- **IMPLEMENTADA** requiere la capacidad en la aplicación real con commit y pruebas. **SUSTITUIDA** mantiene el enlace al reemplazo y motivo. **DESCARTADA** conserva el motivo y la evidencia de la decisión.

Un posible registro futuro sería «UX-03 · U06 · comparar dos campos frente a uno conmutado a 150 % · medir identificación del mismo UID, foco y asignación equivalente · registrar resolución del propietario». Si se cambia únicamente la superficie, se actualizan los IDs relacionados en el pack. Si se cambia lo que el producto permite hacer, se actualizan también requisitos, aceptación y decisiones de alcance.
