# ADR 002 · Una base por carrera y cortes incrementales

**PROPUESTA.** Elegir una base SQLite por carrera, ramas explícitas dentro de la base y manifiestos por cierre. El catálogo de instalación guarda las referencias a carreras. Esto facilita copias y reduce el riesgo de combinar UID de dos partidas. El coste es gestionar más archivos y conexiones al cambiar de carrera.

Una base única simplificaría búsquedas globales que no forman parte del producto. Una copia total por sincronización aumenta el coste incluso cuando nada cambia. Las versiones de observación compartidas implican controlar integridad y retención.

**Aceptación.** Dos partidas homónimas se restauran de forma independiente, 100 lotes idénticos no duplican hechos y un cierre sigue siendo reproducible después de cambiar la plantilla. Revisar con copia WAL y datos de 150.000 personas.
