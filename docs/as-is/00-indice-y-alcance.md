# FM Player Analyzer 2.3.1 · Especificación funcional AS-IS

**Referencia de producto.** Aplicación de escritorio de FM Player Analyzer para FM24 y FM27, versión **2.3.1** del **20 de noviembre de 2026**. Se documenta qué hace el analizador bajo unas entradas y ajustes dados. Esta carpeta reúne los contratos, parámetros, ejemplos y resultados necesarios para estudiar ese comportamiento sin conocer el lenguaje del original.

| Identidad | Valor |
|---|---|
| Repositorio de referencia | [ElectroHugin/FM-Player-Analyzer, revisión fijada](https://github.com/ElectroHugin/FM-Player-Analyzer/tree/126c97007946604b4e83bb6c089b473442581753) |
| Commit de producto analizado | 126c97007946604b4e83bb6c089b473442581753 |
| Base local con la reorganización ya aplicada | c16e2b800dd63137f0c77074cd693d739dc75da6 |
| Comprobación de correspondencia | El árbol de fuentes de escritorio coincide en ambas revisiones, objeto Git 67a3c07cd3432ec8583389ad2f5a26d7b045f1f9. El árbol histórico coincide, objeto 865d4025aa6d51090b0b0c2d0c223a6019365c31 |
| Cambio propio de 2.3.1 | Restablece la lectura de la tabla de clubes de FM27 27.1 y mantiene soporte para 27.0. [Notas de versión](../../CHANGELOG.md) |
| Idioma | Español para la especificación. Identificadores y nombres originales cuando la denominación española oficial no está verificada |
| Fecha de verificación | 21 de noviembre de 2026 |
| Entrega | 42 capacidades identificadas. 16 capítulos, del 00 al 15, y ocho anexos, del 16 al 23 |

## Cómo leer esta carpeta

El [catálogo funcional](01-catalogo-funcional.md) sirve de entrada por tarea. Sus identificadores son estables aunque luego cambie la organización de un documento. Cada enlace lleva a la regla que gobierna una salida y a los parámetros de los que depende. Para comparar resultados, fijar versión de FM, partida, perfil de valoración y sus pesos, orden de entrada y opciones de la operación. La [matriz de cobertura](15-trazabilidad-y-lagunas.md) indica la fuerza de la evidencia y los límites para una reimplementación independiente.

| Capítulo | Qué contiene |
|---|---|
| [01 · Catálogo funcional](01-catalogo-funcional.md) | Problema resuelto, entrada al contrato, disponibilidad, salida y dependencias por ID |
| [02 · Modelo funcional de datos](02-modelo-funcional-de-datos.md) | Persona, identidad, campos, valores importados, derivados y editables |
| [03 · Valoraciones y fórmulas](03-valoraciones-y-formulas.md) | Classic, perfiles por curvas, peso conocido, redondeo y porteros |
| [04 · Posiciones y roles](04-posiciones-y-roles.md) | Compatibilidad por puesto, asignación, valoraciones comparables y diagnóstico |
| [05 · Tácticas y optimización](05-tacticas-y-optimizacion.md) | Objetivos, prioridades, restricciones, dos fases, fijaciones y sustituciones |
| [06 · Análisis de plantilla](06-analisis-de-plantilla.md) | Profundidad, categorías, diferencias, métricas y convocatoria nacional automática |
| [07 · Scouting y mercado](07-scouting-y-mercado.md) | Filtros, comparación directa, alta simulada, venta y cesión |
| [08 · Desarrollo y entrenamiento](08-desarrollo-y-entrenamiento.md) | Talento, ganancia por punto, bandas, métodos y elección de rol |
| [09 · Históricos](09-historicos.md) | Estado mensual, fecha de juego, fecha de carga, reajustes y antigüedad |
| [10 · Inscripción y elegibilidad](10-inscripcion-y-elegibilidad.md) | Competiciones, exención, formación, salarios y aplicación de una lista |
| [11 · Importación y visibilidad](11-importacion-y-visibilidad.md) | Tablas, conciliación, extracción FM27, máscara y seguimiento de archivo |
| [12 · Configuración y reglas globales](12-configuracion-y-reglas-globales.md) | Partidas, parámetros, límites de edición, perfiles, roles y tácticas intercambiables |
| [13 · Casos límite y errores](13-casos-limite-y-errores.md) | Entradas problemáticas, resultado observable y estado que persiste |
| [14 · Casos de prueba de referencia](14-casos-de-prueba-de-referencia.md) | Entradas reproducibles, cálculo razonado y salida esperada por escenario |
| [15 · Trazabilidad y lagunas](15-trazabilidad-y-lagunas.md) | Matriz de evidencia, discrepancias con documentación anterior y auditoría de autosuficiencia |

| Anexo | Qué permite reconstruir o comprobar |
|---|---|
| [16 · Roles y tácticas](16-tablas-de-roles-y-tacticas.md) | 65 roles FM24 y 79 FM27, marcas por atributo, compatibilidad, tácticas incluidas, plantillas, fases y personalidades |
| [17 · Parámetros y atributos](17-parametros-y-atributos.md) | Los 47 atributos en orden canónico, sus cabeceras, bandas, mediciones de FM27 y métodos |
| [18 · Visibilidad FM27](18-parametros-de-visibilidad.md) | Prioridades de rango, identificadores, personalidad, países de partida y campos de contrato |
| [19 · Contrato de tablas de entrada](19-contrato-de-tablas-de-entrada.md) | Cabeceras escalares, conocidas sin efecto, alias y conversión de nombres de países |
| [20 · Oráculo numérico Classic](20-oraculo-dwrs-classic.md) | 4.608 pares de absoluta y normalizada con sus entradas y tolerancia |
| [21 · Registro de pruebas](21-ejecucion-de-las-pruebas.md) | Las 52 suites, casos incluidos, resultados y dos omisiones por falta de partida real |
| [22 · Oráculo de curvas FM27](22-oraculo-de-curvas-fm27.md) | 237 salidas del evaluador original para comprobar las 79 variantes de rol |
| [23 · Operaciones con fichas y selecciones](23-operaciones-con-fichas-y-selecciones.md) | Comparación, edición, matriz nacional, exportación y salidas tras actualizar la plantilla |

## Recorridos de lectura

- **De una tabla de FM a una alineación.** Usar 19 y 11 para construir las fichas, 02 para conciliarlas, 04 y 16 para sus roles, 03 y 17 para calcular sus notas, y 05 y 10 para decidir quién ocupa cada puesto.
- **De un puesto débil a un posible fichaje.** Calcular el XI, determinar el aviso del 06 y comparar los filtros y la simulación del 07. La nota del rol, el déficit local y la mejora del once completo responden a preguntas diferentes.
- **De una segunda observación a una decisión de entrenamiento.** Usar 09 para separar subida y recálculo, 08 para calcular ganancia marginal al perfil activo y 12 para interpretar la banda de desarrollo.
- **De una partida FM27 al dato conocido por el usuario.** Usar 11 y 18 antes de calcular DWRS. La misma persona puede producir nota exacta, nota dependiente de intervalos o ausencia de valoración según la fuente, el perfil y el conocimiento. El capítulo 11 también identifica los cinco archivos .fmf distribuidos para el propio juego.

## Convenciones y límites del baseline

IP y OOP designan la fase con y sin posesión. A es el XI titular y B el equipo alternativo. UID identifica la persona en el ámbito de la partida. Una celda vacía, una cantidad cero, una etiqueta no clasificada y una valoración sin cobertura son estados distintos. La coma se usa como separador decimal en el texto español. Las tablas de oráculos utilizan punto decimal para admitir comparación automática.

**GOLDEN MASTER VERIFICADO** identifica una salida contrastada con el original ejecutado a la precisión declarada. **EJEMPLO CALCULADO MANUALMENTE** identifica un resultado derivado de las reglas sin atribuírselo a la ejecución del escenario. **PENDIENTE DE VERIFICACIÓN** y **NO VERIFICADO** identifican la falta concreta de evidencia. Un enlace a código en un capítulo identifica la fuente de la afirmación. Para interpretar los contratos de producto, las referencias numéricas y las limitaciones bastan los documentos enlazados dentro de esta carpeta.

## Registro de trabajo y estado

| Bloque | Resultado |
|---|---|
| Baseline | Versión, revisión upstream y equivalencia de los árboles locales fijadas antes de extraer parámetros |
| Inventario | 42 ID estables trazados. Se incluyen selección nacional, edición y comparación, exportación y diagnósticos secundarios |
| Modelos y parámetros | Fórmulas y catálogos extraídos separando juego, perfil, campo/portería y fase |
| Flujos y persistencia | Reglas de selección, mercado, seguimiento, histórico, inscripción y configuración contrastadas con sus consumidores |
| Ejecución | 52/52 suites aprobadas. En el desglose, 580 registros aprobados (104 de preparación/cierre), 2 casos opcionales omitidos y 0 fallidos |
| Auditoría independiente del texto | Reconstrucción aritmética a partir de los Markdown: 4.608 casos Classic y 237 resultados de curvas dentro de sus tolerancias |
| Límites abiertos | Partidas reales, interoperabilidad binaria, datos históricos particulares, desempates exhaustivos y terminología española oficial. El [dictamen final](15-trazabilidad-y-lagunas.md) precisa el impacto de cada uno |
