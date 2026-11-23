# Registro de implementación y cobertura

**23 de noviembre de 2026 · Fase 1 del roadmap · REQ-UX-001.** Prototipo para revisión del propietario. Rama `prototype/centro-analisis-fm27`, iniciada con el árbol limpio en `4aad880`. La base contenía el guion y el Design Reference Pack. La primera entrada ejecutable está en [index.html](index.html).

## Qué se puede evaluar

| Tarea y correspondencia TO-BE | Operación disponible en la demo | Evidencia propia |
|---|---|---|
| Resumen, cobertura y prioridad AML · REQ-SQUAD-001/002/003 | Identificar titular, nota, falta de relevo, baja y contrato próximo. Abrir el puesto y conservar al referente hasta la compra hipotética. | [Resumen](review/resumen.png) |
| Plantilla, base, ficha y roles · REQ-DATA-001/002/003, REQ-ROLE-001/002, REQ-RATING-004 | Buscar por nombre o club, seleccionar hasta cinco UID, ordenar, paginar y ajustar columnas. Consultar los 47 atributos por familia, cobertura, contrato, temporada, cortes y nota de cada fase. | [Plantilla](review/plantilla.png) · [Ficha](review/ficha.png) |
| Ojeo y listas · REQ-SCOUT-001/003, REQ-EXPORT-001 | Filtrar por posición, rol, edad, valor y Regate. Guardar criterios, separar posibles y seguros, seguir o retirar personas y copiar una selección a una lista propia. La vista previa exporta las filas filtradas o su selección, también entre páginas. | [Ojeo](review/ojeo.png) · [Seguimiento](review/seguimiento.png) |
| Comparación y operaciones · REQ-SQUAD-004, REQ-MARKET-001/002/003 | Cambiar referente, añadir o retirar personas, cotejar fases, atributos y sueldo. Simular compra, sustitución o venta, revisar relevo y presupuesto, guardar o descartar la operación. | [Comparador](review/comparacion.png) · [Compra](review/operacion.png) |
| Pizarra, XI y profundidad · REQ-TAC-002/003, REQ-SQUAD-001/003 | Alternar 4-3-3 y 4-2-3-1, inspeccionar las dos fases, fijar, intercambiar, arrastrar o elegir desde teclado. Probar baja, buscar relevo, calcular el XI ilustrativo y comparar las dos distribuciones. | [Táctica](review/tactica.png) |
| Desarrollo, entrenamiento e históricos · REQ-DEV-002/003/005, REQ-HIST-002/004 | Elegir joven, atributo y par de fechas. Seguir a Elia en su cesión. Priorizar un atributo de la línea del jugador y guardar un objetivo individual. | [Cantera](review/cantera.png) · [Histórico](review/historicos.png) · [Entrenamiento](review/entrenamiento.png) |
| Estadísticas y Moneyball · REQ-STAT-001/002 | Cambiar métrica, minutos, población y percentil. Inspeccionar puntos y radar, abrir totales, comparar bajo la misma cohorte. Un N inferior a 20 impide publicar percentiles. | [Rendimiento](review/rendimiento.png) |
| Inscripción y empleados · REQ-REG-001/003, REQ-STAFF-001 | Ensayar tres plazas y una reserva en Copa Faro. Revisar una formación desconocida y descartar una propuesta. Cambiar pesos del perfil de Vera Luar y leer la estimación resultante. | [Inscripción](review/inscripcion.png) · [Personal](review/personal.png) |
| Perfiles · REQ-PROFILE-001/002 | Duplicar el perfil didáctico, variar el peso y un punto de la curva, rechazar un tramo decreciente, deshacer y activar la revisión del ejercicio. | [Perfiles](review/perfiles.png) |
| Asistente, preferencias y sesiones · REQ-AI-002, REQ-VIS-001/002, REQ-SAVE-001/002, REQ-FM-004, REQ-ART-001 | Resolver seis consultas locales y abrir su evidencia. Confirmar o revocar CA y PA por separado. Alternar Faro y Bruma, simular cinco estados de fuente, reintentar, cambiar densidad, reindexar el retrato ficticio y reiniciar. | [Asistente](review/asistente.png) · [Configuración](review/configuracion.png) · [Partidas](review/partidas.png) |

El tratamiento heredado y las fórmulas de producción siguen en la [matriz AS-IS → TO-BE](../../docs/to-be/18-trazabilidad-as-is-to-be.md). La tabla identifica las partes observables de los requisitos en esta entrega. La equivalencia matemática con 2.3.1 y la ejecución de sus oráculos corresponden al núcleo nativo.

## Decisiones de composición y datos

- Mantuvimos Segoe UI, fondo oscuro, acento menta, referencia azul, estados acompañados de texto y navegación adaptable. El resumen incorpora una panorámica original de estadio, generada para esta entrega. Escudo, iniciales, marcador, campo e iconos tienen geometría propia. Las capturas de terceros permanecen en el pack documental.
- El menú agrupa las tareas en Club, Mercado, Análisis y Sistema. La ficha, el comparador y el entrenamiento se abren desde una persona. Profundidad y comparación táctica comparten pizarra. El asistente ocupa un destino completo, accesible desde la cabecera, para que sus respuestas y enlaces quepan al ampliar texto.
- La carrera Faro conserva las identidades, fechas y valores del guion. Los 96 registros adicionales amplían el volumen de tabla. Once titulares disponen además de un parte ficticio de tres jornadas de Liga del Estuario 2027/28. La cohorte del Cabo conserva sus 24 delanteros elegibles al umbral inicial y un caso de 180 minutos. Cada población muestra su propia temporada.
- El catálogo local conserva las claves y el orden de los 47 atributos de referencia. Regate y Pase siguen el texto del guion. Las demás etiquetas de atributo muestran la referencia del catálogo. La ficha identifica la denominación pendiente de cotejo oficial en FM27.
- HTML, CSS y scripts clásicos permiten abrir la carpeta por `file:` y reproducirla sin instalación. Los cálculos de muestra, los recursos y el estado de ambas carreras residen en la máquina del usuario. Las listas y los escenarios duran hasta recargar o reiniciar.

## Validación reproducible

Entrada de prueba en [tests/review.py](tests/review.py). Comandos de instalación, comprobación estática y ejecución en el [README](README.md). Entorno de revisión con Windows, Python 3.11, Playwright 1.65.2, Microsoft Edge 159.0.4402.13 y Node 18.12.1 para comprobar sintaxis.

| Caso | Resultado observado |
|---|---|
| V-A1 / U03 y U05 | Leo 58 → Sol 69, diferencia +11. El Regate pasa de 13 a 14–16. Adoptar 4 M€ deja 2 M€ antes de gastos. Otra moneda deja el saldo indeterminado. El retorno recupera AML y el foco. |
| V-A4 y V-A5 / U04 y U06 | La búsqueda segura devuelve Sol y la posible añade Mar. Funcionan selección, listas, retorno por UID y vista previa de 139 personas. La lesión impide asignar a Dani. La baja de Noa ofrece a Isa. El arrastre repone a Noa. Cambiar la formación con Ada fijada permite resolver el conflicto. |
| U07 y U13 | Leo registra +2 en Regate y +4 en nota entre cierres. Cambiar el intervalo, guardar entrenamiento y consultar la cesión de Elia conservan la identidad. La reserva desconocida queda pendiente. El perfil didáctico cambia de 14 a 13 y rechaza el punto 47. Vera obtiene 15,3/20. |
| V-A6 / U08 | El ordinal 12 tiene 1,1 goles/90 y P47,9. El umbral 47 devuelve 13 personas y mantiene N=24. Los 180 minutos producen 0,5 provisional. El mínimo de 901 minutos deja N=0. La comparación conserva la cohorte y la métrica. |
| V-A7 y V-A8 / U01, U09 y U10 | Cancelar la activación conserva el permiso apagado. Aceptar sólo PA muestra el valor ficticio de Sol y mantiene su intervalo. Revocar retira el campo y el criterio guardado. La exportación completa vuelve a coincidir con la anterior al permiso. Bruma muestra a Luz Tena bajo UID 1, con sus propias listas y conversación. |
| V-A2 a V-A4 / U11 | Se recorrieron 16 destinos del marco en siete combinaciones de resolución y escala. No se detectó desplazamiento horizontal de la ventana. Se comprobó teclado en búsqueda, ficha, campo y diálogo, el modo de colores forzados y seis parejas de contraste de texto. |
| U12 | Apertura directa offline, cinco estados de fuente, reintento y reinicio. Las diez pruebas completaron sus recorridos sin excepción JavaScript ni solicitud HTTP registrada. |

Las capturas están en [review](review/). Incluyen las vistas de decisión y las variantes del resumen en 1366 × 768 y 1920 × 1080 al 100 % y 150 %, 1600 × 900 al 100 %, 2560 × 1440 al 200 % y una prueba adicional en 1280 × 960 al 200 %. La escala se reprodujo mediante el ancho y alto CSS efectivos. Los PNG de escala ampliada tienen ese tamaño efectivo.

La inspección visual llevó a separar los valores de fase y a representar el extremo incierto de Regate con tramado. Una repetición desde un archivo Git limpio detectó un cierre de diálogo tardío al reabrir permisos rápidamente con Intro. El manejador comprueba la apertura vigente y devuelve el foco al invocador de la ruta correspondiente. Se repitió el recorrido de teclado doce veces.

## Límite de la revisión

El XI, las notas de rol, el objetivo de entrenamiento, la regla de inscripción, el perfil de dos atributos y el conocimiento de ojeo se limitan a estos escenarios. La exportación termina en la vista previa. El asistente responde al guion local. La aplicación definitiva tendrá sus propias pruebas de persistencia, rendimiento, lector de pantalla, captura del juego, exportación de ficheros y equivalencia de cálculo.

**Pendiente del propietario · V-A9.** Recorrer AML → Ojeo → Comparador → Operaciones e identificar el titular, la causa de la incertidumbre y la hipótesis necesaria para comprometer presupuesto. Conviene repetir la asignación táctica en el equipo y con la escala de Windows habituales antes de aprobar el paso a WinUI.
