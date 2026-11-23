# Evolución del prototipo · 23 de noviembre de 2026

La revisión parte de `c1a3007`. El selector **Dirección visual** de la cabecera aplica Original, A o B a todo el prototipo, incluido el menú lateral, la barra superior y los diálogos. Se conservan la ruta, la carrera y la selección al alternar. La versión previa exacta se conserva en Git y en las capturas tituladas «original». La elección visual corresponde al propietario.

## Direcciones para revisión

| Dirección | Composición | Referencias y alcance |
|---|---|---|
| **A · Football Manager** | Portal oscuro con barra de acciones, navegación lateral más estrecha y agenda deportiva compacta. La ficha utiliza una cabecera de club y una banda de pestañas para pasar de rol a atributos, estadísticas, evolución y contrato. | El portal, la concentración de acciones en cabecera y las tablas densas se estudiaron en capturas oficiales de una compilación de desarrollo de **FM27**. La distribución y los recursos gráficos son propios. [Sports Interactive](https://www.footballmanager.com/fm27/features/fm27-clearer-interface-smoother-navigation) |
| **B · Scouting profesional** | Informe claro con títulos de Georgia, divisores, escala común y prioridades deportivas antes del campo. La ficha dispone la evaluación y su evidencia en dos columnas. El índice lateral, los gráficos y la barra de expediente usan la misma jerarquía. | La densidad de ScoutSheet, la búsqueda contextual de Wyscout y las referencias de comparación de StatsBomb IQ y DataMB guían la lectura. FM Season Hub documenta **FM26** y aporta comparativas y agrupación de atributos. [Referencias del repositorio](../../docs/design/02-product-benchmark.md) · [Atlas](../../docs/design/03-visual-reference-catalog.md) |

En la versión inicial, la panorámica y el campo daban contexto deportivo. La repetición de paneles, el espacio de las cabeceras y el gradiente de la ficha relegaban los datos. A concentra las tareas y mantiene la referencia de club. B elimina la panorámica de la cabecera, da prioridad a la decisión de refuerzo e integra el campo y la tabla en el informe.

Capturas de Edge con Valencia CF y Sol Arce en la misma fecha de juego.

| Resolución | Resumen original | Resumen A | Resumen B | Ficha original | Ficha A | Ficha B |
|---|---|---|---|---|---|---|
| 1920 × 1080 | [Original](review/evolucion/resumen-original-1920x1080.png) | [A](review/evolucion/resumen-fm-1920x1080.png) | [B](review/evolucion/resumen-pro-1920x1080.png) | [Original](review/evolucion/ficha-original-1920x1080.png) | [A](review/evolucion/ficha-fm-1920x1080.png) | [B](review/evolucion/ficha-pro-1920x1080.png) |
| 1366 × 768 | [Original](review/evolucion/resumen-original-1366x768.png) | [A](review/evolucion/resumen-fm-1366x768.png) | [B](review/evolucion/resumen-pro-1366x768.png) | [Original](review/evolucion/ficha-original-1366x768.png) | [A](review/evolucion/ficha-fm-1366x768.png) | [B](review/evolucion/ficha-pro-1366x768.png) |

La evidencia incluye la pizarra con [A](review/evolucion/tactica-fm-1366x768.png) y [B](review/evolucion/tactica-pro-1366x768.png), el [comparador con proyección autorizada](review/evolucion/comparador-proyeccion-1600x900.png) y Ojeo con [A](review/evolucion/ojeo-fm-1920x1080.png) y [B](review/evolucion/ojeo-1920x1080.png). En la revisión visual se redujo el espacio vertical del comparador, se adelantaron los selectores de rol y se corrigió el contraste de los marcadores y los gráficos en B.

## Contratos y casos demostrativos

- **Puesto, fases y perfil.** El puesto gráfico ofrece roles IP y OOP del catálogo AS-IS de FM27. Las equivalencias usan sus códigos de referencia. Ficha, comparador y listas consultan el mismo evaluador de demo. Un delta expresa el jugador comparado menos su referente en pp. El índice combinado de la simulación de refuerzo queda rotulado como índice del XI ponderado al 50 %.
- **Proyección.** En AML y perfil Faro, Leo registra 60 % / 56 % en presente y 62 % / 58 % en el escenario, con margen 2. Sol registra 72 % / 66 % y 84 % / 76 %, con margen 29. Jan puede probar AML con familiaridad presente 11 y futura 20. Su proyección para un rol alejado como portero conserva una nota baja. Mar conserva los intervalos de ojeo y muestra falta de evidencia para la nota. Los desplazamientos de rol, afinidad, perfil y el crecimiento por línea son datos y ajustes didácticos. La transformación CA/PA → atributos futuros requiere validación. [Requisito y alternativas](../../docs/to-be/04-valoraciones-y-perfiles.md#evaluación-presente-y-proyección-por-rol) · [Q-17](../../docs/OPEN-QUESTIONS.md)
- **Juego limpio.** El evaluador exige ambos permisos y un escenario antes de entregar potencial. Si falta un permiso, las columnas de proyección desaparecen de las listas, la comparación y el XLSX. Reabrir una búsqueda tras revocar PA vuelve a un orden permitido. La comparación de rendimiento observado usa su propia cohorte y exportación.
- **Tablas y Excel.** Las listas de jugadores conservan visibilidad y orden de columnas por carrera y vista en el navegador. El botón de restauración recupera sus valores iniciales. Cedidos se consulta mediante disponibilidad en plantilla o cantera. Las listas exportan página, conjunto filtrado o selección filtrada. El comparador exporta su matriz de rol y diferencias o sus tasas observadas, según el origen. Rendimiento exporta su resultado filtrado. El XLSX guarda números, porcentajes y fechas con formato, intervalos como texto y ausencias como celdas vacías. La hoja Contexto documenta los criterios autorizados. Los archivos descargados permanecen bajo control del usuario.
- **Dependencia de exportación.** ExcelJS 4.4.0, distribución compilada de navegador publicada en npm, bajo MIT. Se usa localmente con la licencia junto al archivo. [ExcelJS 4.4.0](https://github.com/exceljs/exceljs/tree/v4.4.0) · [Licencia](vendor/exceljs-LICENSE.txt)

## Repetir las comprobaciones

La suite comprueba los diez recorridos previos, las valoraciones y las descargas, la comparación visual y los 16 destinos del marco en A y B a 1366, 911 y 640 píxeles de ancho efectivo. Para Excel usa openpyxl como lector independiente y comprueba el contenedor, tipos, orden, filtros, hojas y metadatos. La página abre en modo offline y la suite registra excepciones de navegador y solicitudes de red. [Comandos de instalación y pruebas](README.md)

Para regenerar la comparativa con la versión exacta original, extrae `prototype/html` de `c1a3007` a un directorio temporal y pasa la ubicación de su entrada a `review.py --capture --baseline <ruta-al-index-original>`. Las pruebas con el motor real y la validación longitudinal de Q-17 quedan para la aplicación nativa. La Constitución candidata conserva sus principios de autorización y cálculo reproducible.
