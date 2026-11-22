# Especificación del prototipo HTML interactivo

**REQ-UX-001 · entregable posterior a la revisión de Fase B.** Abrir un archivo de entrada local con CSS, JavaScript clásico y datos incluidos, sin instalación ni servidor. Bloquear toda necesidad de red, fuentes web, analítica, API, SQLite, juego o plugin. Usar elementos de escritorio trasladables a WinUI. Todas las personas, clubes, presupuestos y estadísticas del guion son ficticios. Identificar el entorno como «Partida de demostración».

## Estado inicial y escenarios

Carrera Faro, Club Faro, mánager Vega, 14-09-2027, EUR y sueldo semanal, presupuesto 6 M€, formación 4-3-3, IP 50 %, castellano, tema oscuro y CA/PA apagados. Segunda carrera Bruma, 01-03-2028, mánager Alba, vacía salvo UID 1 de otra persona («Luz Tena»). Alternar carreras conserva sus planes y ajustes durante la sesión. Recargar inicia los valores anteriores; «Reiniciar demostración» permite repetir cada recorrido. No pedir claves, rutas reales ni credenciales. Usar iniciales y escudos geométricos ficticios.

**Plantilla de Faro.** Las claves siguientes definen selección y navegación. A es apto, L lesionado, C cedido fuera. Las notas son valores de escenario predefinidos para su puesto principal en ambas fases. Mostrar la leyenda «Valoración de ejemplo»; el prototipo valida interacciones y distinción de métricas.

| UID | Nombre | Edad | Puestos admitidos | Estado | Nota de ejemplo IP/OOP |
|---|---|---:|---|---|---|
| 1 | Noa Mar | 27 | GK | A | 68/70 |
| 2 | Teo Linde | 24 | DR | A | 66/70 |
| 3 | Nil Roa | 25 | DCR | A | 65/72 |
| 4 | Ivo Sael | 26 | DCL | A | 64/73 |
| 5 | Jon Vela | 22 | DL | A | 67/66 |
| 6 | Uri Paz | 23 | DM, MC | A | 68/74 |
| 7 | Pol Aire | 22 | MC, DM | A | 70/69 |
| 8 | Ada Neral | 19 | MC, AMC | A | 73/65 |
| 9 | Lía Sanzal | 20 | AMR, MR | A | 72/64 |
| 10 | Leo Nube | 21 | AML, ML | A | 60/56 |
| 11 | Rui Bel | 26 | ST | A | 74/62 |
| 12 | Isa Luar | 18 | GK | A, cantera | 54/55 |
| 13 | Dani Lago | 20 | DCR, DCL | L, hasta 30-09 | 64/69 |
| 14 | Kai Sena | 17 | MC, AMC | A, cantera | 59/57 |
| 15 | Elia Rul | 19 | DCL | C, Club Brezo | 62/65 |

**Mercado.** En la Liga del Estuario, UID 21 Sol Arce, AML/ML, 19 años, pie derecho, Regate 14–16, Pase 13, valor FM 4 M€, sueldo vigente 8.000 €/semana, nota 72/66; UID 22 Mar Olmo, AML, 20 años, pie izquierdo, Regate 12–14, Pase sin observar, valor FM 5 M€, sueldo desconocido y sin nota; UID 23 Jan Silo, ST, 26 años, Regate 11, Pase 12, valor FM 8 M€, sueldo vigente 15.000 €/semana, nota 70/60. Los tres pertenecen a Club Salina. Para Sol hay CA ficticia 122 y PA 151 sólo disponibles al habilitar cada permiso de demostración. Los precios observados de los tres quedan desconocidos. Los importes del escenario empiezan vacíos.

**Resto de datos deterministas.** Para los UID 1–15 asignar pie derecho a impares e izquierdo a pares. En el orden de 47 atributos del anexo 17, índice a desde 0, nivel = 8 + resto de (UID + 3a) entre 10. Para los tres candidatos, los atributos no indicados quedan desconocidos. En plantilla fijar precio en cientos de miles de euros igual al UID, sueldo semanal 1.000 + 200 × UID y fin de contrato el 30-06-2029 excepto Leo Nube (30-06-2028). Dos cierres históricos de ejemplo, 30-06-2026 y 30-06-2027: Leo pasa de Regate 11 a 13 y de nota conjunta 54 a 58; en el presente su Regate es 13. Elia está en Faro en el primer cierre y cedida en el segundo, mismo UID. Para los demás usar el valor actual como observación constante y mostrar sólo fechas registradas. Un empleado ficticio, E1 Vera Luar, preparadora, tiene dos categorías visibles «Good» y «Elite» con pesos 2 y 1 en el perfil de muestra del capítulo de desarrollo.

**Cohorte estadística independiente.** Liga del Cabo, temporada 2026/27, Club Junco, 24 delanteros ficticios con UID 101–124 y nombre «Atleta del Cabo» seguido de su ordinal k de 1 a 24. Edad 22, minutos 900, 12 apariciones, goles k−1, remates 3k, salario semanal 1.000 + 100k EUR. Métrica goles/90 = (k−1)/10. Con un mínimo de 450 min forman la cohorte de análisis. Para k=12 se muestran 1,1 goles/90 y percentil 47,9 con un decimal. Añadir a la misma población el UID 125, «Atleta del Cabo 25», de 22 años, 180 min, 2 apariciones y 1 gol. Su tasa 0,5 aparece como provisional en detalle y queda fuera del percentil y de la dispersión. El mínimo ≥901 deja la cohorte vacía. Un jugador de Faro sin minutos queda fuera de ella. Mostrar denominadores y permitir inspeccionar el punto desde la tabla. Probar N insuficiente con un subconjunto explícito de menos de 20 personas.

## Contrato de interacción de la demostración

| Flujo | Acciones y efecto observable |
|---|---|
| Navegación | Recorrer las tareas acordadas en la revisión de Fase B. Las siete familias y V01–V19 son su distribución de referencia. Si se agrupan destinos, registrar dónde se completa cada tarea. Volver de una ficha restaura filtros, selección y foco. La ruta activa y la carrera siguen visibles. [Sesiones y variantes propuestas](../design/10-html-prototype-brief.md) |
| Plantilla y ojeo | Por defecto mostrar 15 personas vinculadas a Faro, incluidas cantera y cesión etiquetadas. Desde la carencia AML de Leo abrir «Buscar refuerzo» y conservar AML/ML, W-IP/WM-OOP, táctica y titular. En la búsqueda general sólo por posición usar AML, edad ≤21, valor FM ≤6 M€ y Regate mínimo 14. «Sólo seguros» da Sol e «Incluir posibles» añade Mar. Guardar filtro y lista, salir y recuperar ambos. |
| Comparar | Desde el resultado contextual, comparar a Sol abre Leo y Sol con notas IP/OOP, diferencia de puesto de +11 y sueldo semanal. Abrir atributos muestra Regate 13 frente a 14–16 (diferencia de +1 a +3). Añadir Mar muestra su Pase como «Sin observar». Volver restaura el puesto y el foco. |
| Táctica | En 4-3-3 asignar 1–11 según la tabla, 6 en DM y 7/8 en MC. En 4-2-3-1 situar 6/7 en doble pivote y 8 en AMC. Para la demostración OOP conserva UID y representa los extremos más retrasados y el AMC cercano a MC. Cambio de fase conserva selección. |
| Fijación y profundidad | Fijar a Ada en su puesto compatible, intentar asignar al lesionado y ver causa. El selector de teclado equivale al arrastre. Si el cambio de formación invalida la fijación, señalarla y ofrecer quitarla antes del cálculo. Alternativa disponible en portería = Isa; puesto sin reemplazo marcado vacante. |
| Simular | Llevar Leo y Sol desde el comparador al escenario AML. Sustituir a Leo por Sol manteniendo el resto. A 50 % IP la contribución pasa de 58 a 69 y el XI gana +11. El saldo de compra está indeterminado hasta introducir o adoptar expresamente 4 M€ como precio hipotético. Quedan 2 M€ antes de gastos, con el sueldo ofertado sin determinar. Guardar escenario o descartar. La plantilla real conserva a Leo. |
| Rendimiento observado | En ojeo, preparar un criterio de atributos y cambiar a «Rendimiento observado». El criterio se muestra suspendido. Seleccionar Liga del Cabo, 2026/27, delanteros y goles/90, y filtrar por percentil ≥47. Salen los ordinales 12 a 24 y la cohorte sigue siendo N=24. Comparar dos de ellos conserva la métrica y la cohorte. Retirar el filtro relativo permite consultar los 180 min del UID 125. Volver a encaje recupera el criterio autorizado. |
| Entrenamiento | Abrir Leo, alternar los dos cierres, observar +2 en Regate y +4 en nota. Ejemplo pedagógico separado con atributo lineal: mejora 100/19, banda con factor 2/3, prioridad 200/57. |
| Visibilidad | Activar PA y cancelar el aviso deja la vista inicial. Aceptar permite ver PA 151 de Sol, CA sigue apagada, el rango 14–16 permanece. Desactivar retira PA de tabla, comparador, filtro, exportación simulada y respuesta simulada. |
| Conversación | «¿Qué posición refuerzo?» devuelve respuesta preescrita que enlaza AML, Leo, Sol y +11. Preguntar por un campo restringido devuelve explicación del permiso. Estado simulado indicado en cabecera; ninguna llamada externa. |
| Casos de fallo | Selector de estado de demostración en gestión de partidas: «Cargando», «Fuente incompatible», «Sin datos», «Lista parcial» y «Disponible». Incompatible conserva último corte con fecha, parcial no propone salidas y reintentar vuelve al guion. |
| Persistencia de la sesión | Cambiar a Bruma muestra Luz Tena bajo UID 1 y ningún filtro ni chat de Faro. Volver restaura el escenario de Faro. Reiniciar devuelve permisos apagados y selección inicial. |

Pantallas secundarias deben permitir una acción funcional sobre datos de la demostración: revisar cupo/pendiente en inscripción, comparar empleado con su perfil, duplicar un perfil de ejemplo y rechazar una curva decreciente, abrir el detalle de un cierre o elegir gráfico de cohorte. Los módulos sin evidencia de juego se representan mediante datos ficticios o un estado de capacidad claramente señalado. [Criterios de revisión](06-criterios-de-aceptacion-visual.md) · [Pantallas](03-catalogo-de-pantallas.md) · [Orden de los atributos del generador ficticio](../as-is/17-parametros-y-atributos.md)
## Detalles de los recorridos secundarios

Estas convenciones completan los datos de demostración para que se puedan reproducir las pantallas secundarias.

| Vista | Datos, operación y resultado esperados |
|---|---|
| Fases | En 4-3-3 IP, orden de líneas desde portería: 1 / 2,3,4,5 / 6 / 7,8 / 9,11,10. En OOP: 1 / 2,3,4,5 / 6 / 9,7,8,10 / 11. En 4-2-3-1 IP: 1 / 2,3,4,5 / 6,7 / 9,8,10 / 11. En OOP: 1 / 2,3,4,5 / 9,6,7,10 / 8 / 11. Conservar los UID al alternar. Los cambios relativos entre líneas son posiciones del escenario, sin inferencia de instrucciones del juego. |
| Roles | Elegir el rol específico de fase del catálogo FM27 para el puesto; en el escenario principal usar GK-IP/OOP para 1, FB-IP/OOP para 2 y 5, CB-IP/OOP para 3 y 4, DM-IP/OOP para 6 o 7 en DM y CM-IP/OOP para 6, 7 u 8 cuando juegan en MC, AM-IP/OOP para 8 en AMC, W-IP para AML/AMR y WM-OOP para los extremos retrasados a ML/MR y CFD-IP/OOP para 11. Las notas IP/OOP de muestra siguen al UID en los puestos admitidos. |
| Dispersión y radar | Para k=1–24, fijar asistencias = resto de k entre 5, pases acertados = 100 + 2k, pases intentados = 150 + 3k, entradas ganadas = 24 − k y faltas = resto de k entre 4. Abrir con goles/90 frente a EUR/semana. El radar de detalle añade remates/90, asistencias/90, acierto de pase, entradas ganadas/90 y faltas/90 (menor es mejor). Cambiar de gráfico conserva persona, cohorte y tabla de valores. |
| Histórico | Comparar los dos cierres para Leo y Elia. Rotular los puntos como observaciones de demostración. En el selector de fichas comparar sólo a personas con observación en ese corte; sin registro, mostrar «Sin observación en esta fecha». |
| Inscripción | Competición ficticia Copa Faro con tope de tres plazas y una reservada a formado en club. Seleccionar los UID 1, 7 y 8; sólo 8 tiene formación en club confirmada. La propuesta tiene 3/3 plazas y la reserva cubierta. Cambiar 8 por 10, cuya formación se desconoce, deja la reserva sin acreditar. Aplicar o descartar afecta únicamente a esta competición. |
| Editor | Perfil didáctico lineal, dos atributos con pesos 1 y 2, jugador de ejemplo a 10 y 16. Duplicar y cambiar a pesos 1 y 1 produce 13 frente a 14 sobre 20. En el editor de curvas, aceptar la secuencia creciente 50…69, rechazar el punto 10 cambiado a 47 y poder deshacer. El perfil activo conserva la revisión anterior hasta activar el borrador. |
| Empleados y gráficos locales | E1 muestra estimación 15,3/20 con el desglose de ambas categorías. En Configuración, «Elegir carpeta de ejemplo» alterna entre retrato ficticio disponible para UID 1 y sin retrato para UID 12. Reindexar no abre el sistema de archivos. |
| Conversación y exportación | Ofrecer consultas predefinidas de refuerzo, desarrollo y explicación. Para texto libre ajeno al guion indicar «Consulta fuera de esta demostración». La exportación simulada muestra Excel como formato preferido y CSV como alternativa. La vista previa reproduce encabezados, filas y columnas autorizadas para el formato elegido. La simulación se valida con esa vista previa. |
