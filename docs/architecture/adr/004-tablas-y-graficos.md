# ADR 004 · Seleccionar controles por medición

**PROPUESTA.** Comenzar la evaluación con ListView virtualizado, plantilla de fila y consulta paginada. Para el campo usar elementos nativos navegables; evaluar Win2D para fondos y dibujo denso. Comparar radar/dispersión de LiveCharts con dibujo propio sobre la misma serie.

Un único control de terceros puede ahorrar implementación pero añade dependencias, recursos nativos y restricciones de accesibilidad. Un gráfico propio requiere pruebas de ejes, selección, teclado y ciclo de vida de GPU. Mantener siempre el acceso tabular a los valores.

**Aceptación.** Registrar p95, memoria tras navegación repetida, tabla accesible y acción equivalente con teclado. Seleccionar la variante con menor coste de mantenimiento que cumpla el presupuesto aprobado en PC-05/06/07.
