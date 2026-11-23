# Política de visibilidad y juego limpio

REQ-VIS-001 a 003, REQ-FM-005, REQ-EXPORT-001 y REQ-AI-002. La política efectiva depende de carrera, rama, mánager/perspectiva, fecha de observación, capacidad certificada, clase de campo y permiso vigente. La política se aplica en cada vía de captura y en la consulta del backend antes de construir claves de orden, índices derivados, agregados, páginas o herramientas. Si se desconoce la legitimidad de un campo o incluso la visibilidad de una persona, se omite. Por defecto el almacenamiento persistente contiene sólo observaciones autorizadas.

**Ámbito mundial en v1.** El ámbito inicial reúne la plantilla y los jugadores conocidos u ojeados. La activación expresa de mundo amplía identidades dentro del universo cargado acreditado. El acceso a valores de atributos ocultos o desconocidos requiere su propio permiso explícito y fuente acreditada por campo. CA/PA conservan su permiso específico. Las tres autorizaciones se evalúan en conjunto antes de consultar o usar un valor. [PoC A y test de permisos](../roadmap/07-poc-y-modelo-v1.md#poc-a--extracción-autorizada-de-fm27) · [Casos v1](../roadmap/06-alcance-v1.md#aceptación-de-v1)

| Circunstancia | Representación permitida | Cálculos y acciones permitidos |
|---|---|---|
| Conocimiento exacto contrastado | Entero de 1 a 20 u otra unidad declarada | Operaciones del catálogo con fecha y fuente. |
| Intervalo observado | Ambos extremos inclusivos, etiqueta «Estimación» para derivados | Clásico usa punto medio, curvas promedian niveles; filtros seguros exigen todo el rango; la UI sigue mostrando el intervalo. |
| Ausente, desconocido o visibilidad dudosa | Motivo general cuando comunicarlo es legítimo. Persona completamente no visible queda fuera del universo de consulta | Sin reconstrucción a partir de datos privados. Evitar exponer recuentos de personas excluidas por completo. |
| CA o PA habilitados y disponibles | Sólo el campo autorizado y con procedencia | Habilitar columna/filtro expresos, exportación sólo si elegida; IA requiere además consentimiento de contexto. |
| Permiso revocado, desconexión o cambio de mánager | Nuevo identificador de política. La última vista válida sólo es reutilizable bajo permisos vigentes | Cancelar solicitudes y descartar proyecciones, conversaciones visibles afectadas, sugerencias, índices y resultados pendientes de versión anterior. |

**Advertencia propuesta.** «CA y PA son valores que Football Manager normalmente oculta. Si los activas, podrás verlos cuando haya una fuente validada. El conocimiento de ojeo del resto de campos seguirá aplicándose». Proponer dos controles independientes, con confirmación al activar y revocación inmediata al desactivar. Sin capacidad validada mostrar su estado y motivo. Excluir los valores especiales de instantáneas, logs y copias exportables de rutina; propuesta inicial de mantenerlos sólo en memoria autorizada. La revocación no permite recuperar archivos que el usuario ya exportó; informar en la acción de exportar.

| Motor | Tratamiento de desconocidos en la aplicación |
|---|---|
| DWRS y puntuaciones por posición | Classic exige datos ponderados observables. Curvas usa K/D. Ponderación alternativa requiere el grupo completo. Sólo el oráculo histórico reproduce el centinela Classic. |
| XI y profundidad | Sin nota utilizable no hay arista por rol. Requisito obligatorio sin certeza produce pendiente; escenario expresamente hipotético puede explorarse por separado. |
| Talento y entrenamiento | Fórmula explicita sustituciones neutras heredadas. Sin rol valorable no hay talento; sin edad no hay prioridad de entrenamiento dependiente de edad. |
| Mercado, edad y economía | No suponer coste, edad, cláusula o salario cero. Identificar moneda, mínimos/máximos y cálculo parcial. Separar seguros/posibles. |
| Estadística e históricos | Excluir pares insuficientes y publicar denominador visible. No interpolar observaciones ni derivar secretos por diferencias entre cohortes. |
| Empleados e inscripción | Falta de atributo ponderado impide nota. Permiso, cupo o salario inciertos quedan indicados en la lista y en el XI. |
| Comparación, exportación y ChatGPT | Reutilizar la misma proyección autorizada. Excel y CSV reflejan sólo las columnas seleccionadas y autorizadas. Se aplica el mismo alcance a los metadatos del archivo. La llamada al modelo sólo recibe campos aprobados y aplicables a esa consulta. |

**Rendimiento observado.** Es una selección de evidencia dentro de la proyección autorizada. Sus consultas, agregados y cohortes usan estadísticas observadas y sus campos de contexto permitidos. Atributos, CA/PA, DWRS y derivados de éstos quedan fuera de la consulta, las sugerencias, la explicación y la exportación en esa vista, incluso si están autorizados en otra. El contexto recuerda qué criterios quedaron suspendidos sin aplicarlos al conjunto activo. Resultados asíncronos y cachés se identifican por revisión de política y vista analítica. Al cambiar cualquiera de ellas se descartan los resultados en curso del contexto anterior. [Uso en ojeo y comparación](09-estadisticas-y-moneyball.md)

**Prueba de indistinguibilidad.** Crear dos fuentes sintéticas iguales en toda observación visible, distintas en PA, personalidad interna, exacto bajo máscara y personas desconocidas. Comparar el contenido normalizado de tabla, ficha, filtro, autocomplete, orden, recuentos, XI, explicación, Excel, CSV, históricos y herramientas. Con permisos normales deben coincidir. Habilitar sólo CA permite que difiera exclusivamente el campo autorizado y sus consultas solicitadas. Un filtro guardado con PA queda suspendido al revocarlo; no devuelve un recuento residual.


**Proyección dependiente de CA y PA.** La capa de datos y casos de uso exige ambos permisos y capacidad acreditada antes de entregar insumos o valorar el escenario de [REQ-RATING-005](04-valoraciones-y-perfiles.md#evaluación-presente-y-proyección-por-rol). Si alguno falta, comunicar indisponibilidad. Retirar sus filtros, diferencias, ordenaciones, agregados, gráficos, exportaciones y metadatos derivados. Revocar invalida consultas y descargas aún en curso, limpia criterios guardados afectados y vuelve a un orden permitido. Las preferencias pueden guardar el identificador de una columna para reutilizarlo con un permiso posterior. Las copias ya descargadas permanecen bajo control del usuario.
