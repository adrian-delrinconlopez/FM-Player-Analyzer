# Validación de interfaz y accesibilidad

Aplicar el [guion visual](../ux-ui/06-criterios-de-aceptacion-visual.md) al prototipo. La interfaz nativa repite los recorridos con datos del núcleo mediante UI Automation. Capturas a escala complementan las aserciones sobre valores y acciones.

| Superficie | Acción | Comprobación |
|---|---|---|
| Menú y tabla | Navegar con teclado, filtrar, ordenar, abrir tercera fila y volver | Foco, consulta, UID y cantidad persisten; las páginas no duplican filas. |
| Campo | Asignar a Ada por selector y por arrastre | Mismo UID/puesto; conflicto de bloqueo anunciado y ambas fases accesibles. |
| Gráfico | Comparar con la tabla, activar contraste alto y retirar un dato | Unidad y N anunciados. El hueco se conserva, sin caer a cero. |
| Configuración | Cancelar/aceptar PA y revocar durante lectura del comparador | Foco vuelve, ninguna columna antes de aceptar, revocación limpia también el árbol accesible. |
| Diseño adaptable | Repetir escalas y tamaños previstos con texto largo e imagen ausente | Sin acciones recortadas, advertencias interpretables sin color. |
| Localización | Recorrer recursos es-ES y prueba con inglés, manteniendo misma fixture | Cero claves sin resolver; orden cotejado en ficha, nombres personales iguales, mismos resultados. |

Registrar lector de pantalla, control, nombre accesible, estado y acción. La aprobación visual identifica versiones del catálogo lingüístico y de la fixture. La regresión FR-08 compara el resultado de interfaz con el del núcleo.
