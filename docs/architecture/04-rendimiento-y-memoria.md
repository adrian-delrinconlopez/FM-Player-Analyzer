# Rendimiento y memoria

REQ-PERF-001. Se propone consultar SQLite por páginas de 100 filas, con orden estable (clave elegida, ID) y cursor ligado a consulta, revisión y política. Índices en carrera/rama, club, fecha, rol y claves frecuentes; comprobar cada consulta con EXPLAIN QUERY PLAN y datos sesgados. Columnas por lista blanca y parámetros enlazados para filtros. La ficha carga atributos e histórico al abrir y los gráficos piden series acotadas. Para navegación profunda usar paginación por clave y descartar cursores cuando cambie el filtro.

| Recurso | Presupuesto inicial propuesto | Comportamiento al superar el límite |
|---|---|---|
| Tabla / resultados | Hasta 5 páginas de 100 filas por vista activa; cargar siguiente página al acercarse al borde | Expulsar la página más lejana y conservar selección por ID. Sin colección completa paralela en el modelo de vista. |
| Filtros | Agrupar pulsaciones durante 150 ms, un resultado vigente por vista | Cancelar trabajo anterior y validar generación antes de publicar. |
| Sincronización | Páginas de hasta 500 personas y 1 MiB por marco, con contrapresión | Reducir tamaño al primero de los límites, confirmar avance fuera del hilo del juego; cancelar de forma cooperativa. |
| Miniaturas | 64 MiB decodificados, 256 MiB de miniaturas en disco por instalación | LRU (expulsar menos usadas), cargar al tamaño visible; revocar referencias y liberar superficies al cerrar. |
| Cálculo | Como inicio, un trabajo pesado por carrera y concurrencia máxima de dos trabajadores de análisis | Priorizar interacción y permitir cancelar. Publicar sólo la versión de entradas que originó la petición. |
| Dispersión y líneas | Pantalla muestra una muestra determinista o agregación por píxeles/celdas documentada cuando exceda el presupuesto gráfico | Calcular percentiles sobre la cohorte completa autorizada; indicar puntos representados y total. |

SQLite puede ejecutar de forma síncrona los métodos asíncronos del proveedor; usar trabajador dedicado y consultas cortas cancelables. Evitar cargar fotos originales en una lista y mantener suscripciones después de salir de pantalla. Comparar caché fría/caliente, medir memoria privada y GPU, CPU del juego por separado, latencia y tiempos de fotograma. Publicar tamaño de datos, hardware y p95 antes de aprobar cualquier umbral. [Plan de medida](../testing/03-validacion-rendimiento.md) · [Comportamiento de Microsoft.Data.Sqlite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/async)
