# Catálogo de componentes

Usar controles de plataforma para interacción y dibujo propio cuando el deporte lo requiera. Cada componente acepta una proyección con ID, unidad, fuente y estado; expone eventos por ID. El cálculo deportivo pertenece al dominio. PC-05/06/07 comparan alternativas antes de aprobar dependencias.

| Componente | Interacción y semántica | Implementación WinUI propuesta |
|---|---|---|
| Tabla avanzada | Orden estable, multiselección de hasta 5 para comparar, columnas visibles, filtros por rango, selección por ID y recuento autorizado | ListView virtualizado con encabezado y Grid por fila. Evaluar TableView según canal y medición; lector de pantalla anuncia encabezado, fila, valor y orden. |
| Tarjeta de jugador | Identidad, club, puesto, fecha, estado y acceso a ficha | Plantilla XAML con botón y nombre accesible. Foto bajo demanda y sustituto con iniciales. |
| Atributo y DWRS | Rango o exacto, cobertura, escala, revisión y causas al pulsar | Texto, barra opcional y Flyout. Mostrar «Sin nota» accesible y referencia local al cálculo. |
| Comparador | Columnas de personas fijadas y filas por atributo, señalan comparabilidad y huecos | Grid/ListView con encabezados de jugadores y acción de retirar; al estrechar, selector del par activo. |
| Radar de detalle | Radios con percentiles de una cohorte, detalle por eje y diferencias de muestra | Candidato LiveCharts o dibujo Win2D/XAML; tabla alternativa accesible. Un dato ausente rompe el polígono. |
| Barras y líneas | Unidad y escala común, origen cero para magnitudes de barra, discontinuidades cuando falta una fecha | Biblioteca medida o dibujo por series; selector de intervalo, tabla y lista de valores focalizables. |
| Dispersión, distribución y percentiles | Seleccionar punto/intervalo y filtrar personas. Incluir N, minutos, dirección y fuente | Win2D para nubes densas o biblioteca si cumple PC-07; tabla equivalente y agrupación anotada. |
| Mapa de calor | Rejilla de frecuencia, normalización, orientación y leyenda; sin coordenadas, mensaje de indisponibilidad | Win2D o rectángulos XAML con lista descriptiva; pospuesto para datos reales. |
| Campo y posiciones | Geometría normalizada, líneas y puestos con nombres accesibles. Marcador circular con dorsal/rol, foco, bloqueo y lesión | Canvas/Grid para fondo y botones XAML para jugadores, Composition sólo si se mide beneficio. |
| Arrastre y fases | Arrastrar jugador a puesto válido, previsualizar destino y causa si se rechaza. Alternativa «Asignar» con teclado | Arrastre nativo y SelectorBar para IP/OOP; dos campos lado a lado con ancho suficiente, alternancia preserva UID seleccionado. |
| Escudos, fotos y banderas | Imagen contextual por tipo/UID; rotura usa iniciales/símbolo y descripción | Image con DecodePixelWidth/Height y caché limitada, tamaño reservado para evitar saltos. |
| Incertidumbre y alertas | Procedencia, fecha, magnitud de rango, lesión, sin permiso o dato antiguo sin revelar secretos | TextBlock con icono y etiqueta, InfoBar para bloqueo recuperable y ProgressRing para trabajo cancelable. |
| Conversación | Mensaje, evidencia enlazable, contexto a compartir, estado parcial y cancelación | Lista virtualizada de mensajes, editor y botones. Respuesta de texto con referencias locales validadas. |
| Filtros avanzados | Criterios reutilizables de posición, edad, atributos, rangos y coste; limpiar restaura consulta | Panel con ComboBox/NumberBox, flyout accesible y resumen de filtros activos; campos autorizados por el backend. |

Pruebas comunes: cada selección se hace con ratón y teclado, el foco sobrevive a una actualización, el lector anuncia rangos y ausencias, el tema de contraste conserva diferencias y cerrar el módulo libera sus suscripciones e imágenes. [Evaluación de dependencias](../architecture/06-bibliotecas-y-componentes.md)
