# Modelo funcional de datos

## FPA-FUNC-001 · Ficha del futbolista

En ambas versiones la partida mantiene una ficha actual por UID. El identificador local enlaza las observaciones históricas incluso después de corregir el UID. La persona puede pertenecer al primer equipo, al segundo o a otro club. La shortlist y la convocatoria son pertenencias independientes. No se ha identificado una ficha ni un motor de valoración de empleados en la aplicación de escritorio. Las columnas de relaciones con empleados que un exportador puede incluir no generan empleados. [Contrato del jugador](../../old_version/desktop/src/core/Player.h) · [Entradas admitidas e ignoradas](../../old_version/desktop/src/core/ImportSchema.cpp).

| Dato conceptual | Tipo y ausencia | Fuente y conversión | Utilización |
|---|---|---|---|
| ID local / UID | Entero positivo al guardar / texto no vacío | ID local asignado, UID externo conservado con prefijo cuando proceda | Relaciones internas / conciliación |
| Nombre y club actual | Texto, club puede faltar | Exportación o partida, edición de ficha. Un club puede cambiar de nombre conservando su ID de juego | Búsqueda, pertenencia a equipo, salidas |
| Edad | Entero en años, 0 desconocido | Exportación o fecha de nacimiento frente a fecha de juego | Talento, inscripción, entrenamiento, mercado |
| Nacionalidad principal y segunda | Códigos de texto, vacío posible | Exportación normalizada, lectura directa asigna país principal por ID | Filtros, selección, exención juvenil |
| Posiciones / naturales | Texto original / conjuntos, pueden estar vacíos | Texto, competencias de partida y anulación manual. Distingue «manual vacío» de «sin anulación» | Elegibilidad y bonificación |
| Lado preferido | Vacío, Left, Right | Manual | Desempate y aviso por lado incorrecto |
| Pie izquierdo, derecho y preferido | Texto, vacío posible | Etiquetas importadas o derivadas. Escala de fuerza ordinal de 1 (Very Weak) a 6 (Very Strong) | Comparación. No decide el lado central preferido |
| Estatura | Texto y entero cm, 0 desconocido | Quitar «cm», recortar espacios, convertir entero. Se conserva el original | Perfil y comparación |
| Atributos visibles | 47 pares enteros (lo, hi), 1…20 o (0,0) | Entero n→(n,n), intervalo inclusivo a-b. Vacío, guion, límites inválidos o ilegibles →(0,0) al importar | DWRS, diagnóstico, entrenamiento, talento, comparación. Una fila por atributo y versión en el [diccionario](17-parametros-y-atributos.md) |
| Personalidad / Media Handling | Texto, puede faltar | Exportación o etiqueta reconstruida bajo condiciones de conocimiento. Una personalidad vacía no borra otra existente | Clase good/neutral/bad, talento, filtro e informe. Media Handling es descriptivo |
| Agreed Playing Time | Texto, vacío equivale a None al seleccionar | Contrato vigente o ajuste del usuario | Multiplicador por categoría, prioridad con presupuesto salarial |
| Sueldo / valor | Texto con moneda y periodo / texto y valor numérico | Ver FPA-FUNC-003 | Filtros, inscritos, comparaciones, economía de plantilla |
| Calificación media | Real, 0 sin dato | Rating, Av Rat o estadística de temporada. Admite coma o punto decimal | Perfil, histórico y aviso de forma |
| Roles asignados / primario / entrenamiento | Lista de códigos, dos códigos opcionales | Unión automática por posición o edición, primaria y entrenamiento manuales | Valoraciones calculadas, restricción de selección, objetivo de entrenamiento |
| Selección nacional / shortlist / lesión larga | Booleanos iniciales falsos | Mantenidos en el analizador | Convocatoria, seguimiento, exclusiones deportivas |
| En venta / en cesión / nuevo club | Dos booleanos y texto opcional | Decisión del usuario. En transferencias un destino no vacío diferente mueve al jugador al guardar | Planificación de salidas |
| Inscripción | Booleanos y estados automáticos/sí/no | Formación local/club, no UE, listas, bloqueos, exención de mínimo salarial, U21 y juventud de liga manuales | Capítulo 10. El exportador puede incluir Reg sin alimentar estas marcas |
| Última observación | Entero ≥0, 0 sin sello histórico | Contador vigente de cargas completas | Obsolescencia y retiro inferido |
| DWRS, talento, ganancias | Reales derivados por jugador, rol, perfil o escenario | No son atributos físicos almacenados en FM | Selección y recomendaciones. CA y PA no son campos de la ficha |

La fecha, origen, perfil y entradas de una valoración son hechos históricos distintos de los campos editables. Cada táctica asigna roles a puestos de una o dos fases. Una convocatoria y una lista de competición contienen referencias al jugador de esa partida. Las preferencias globales de arranque no participan en su identidad.

## FPA-FUNC-002 · Identidad y conciliación

Primero descartar UID vacío, ante duplicado exacto en la carga gana la última fila. Comparar el nombre recortado con el anterior. Una diferencia para el mismo UID produce un aviso de cambio de identidad, pero por sí sola no crea otra persona. La coincidencia de nombre sin relación entre UID tampoco autoriza fusión. Cuando llega X y existe r-X con el mismo nombre, usar r-X. Si además existía X del mismo nombre, fusionarlo. Cuando llega r-X y sólo existe X del mismo nombre, renombrar la ficha conservando su ID local. Con ambos presentes, gana r-X. X frente a r-X con nombres distintos mantiene las identidades y puede producir aviso.

La fusión rellena roles, primario, lado, tiempo acordado y rol de entrenamiento vacíos, conserva naturales manuales prioritarias y combina selección, shortlist y lesión larga por OR. Los campos clásicos de inscripción y las dos extensiones posteriores se heredan por grupos si el grupo superviviente está vacío. El histórico DWRS conserva el destino en colisiones jugador–rol–instante, y la última valoración se reconstruye tras fusionar. En observaciones del mismo periodo prevalece la fecha de juego posterior. Ejemplo, X=437 y r-437 para «A. Ruiz» en el mismo club pasan a una sola ficha r-437 cuando la nueva carga activa la conciliación. «A. Ruiz» con 438 sigue siendo independiente. [Conciliación](../../old_version/desktop/src/core/HtmlImporter.cpp) · [Fusión persistente](../../old_version/desktop/src/core/Database.cpp).

## FPA-FUNC-003 · Valor, salario y agente libre

El valor se expresa en unidades de la moneda indicada, sin conversión entre monedas en las exportaciones. «Not for Sale» sin distinguir mayúsculas vale 2 000 000 000. Tomar el límite inferior del rango separado por « - », quitar €, £ o $, aplicar M=10⁶ o K=10³ (mayúsculas) y convertir a real. Entrada vacía o inválida vale 0. El detector de agente libre exige club vacío, guion, raya «–», etiqueta FrA o un cero **explícito** en valor. Un texto ilegible y un cero monetario pueden producir el mismo número sin producir la misma clasificación.

El sueldo semanal admite K/M sin distinguir caja, separador de miles con coma y coma decimal. Tomar el extremo inferior, convertir unidades, dividir por 52 para p/a o p/y, por 52/12 para p/m y por 1 para p/w o sin periodo. Inválido o negativo → desconocido (−1 antes de la regla de inscripción). Ejemplo, «€1,5K p/m» → 346,153846… por semana. «£500K - £800K» → 500 000 de valor, sin mezclarlo con euros. [Conversión](../../old_version/desktop/src/core/Utils.cpp) · [Pruebas de importes](../../old_version/desktop/src/tests/test_utils.cpp).
