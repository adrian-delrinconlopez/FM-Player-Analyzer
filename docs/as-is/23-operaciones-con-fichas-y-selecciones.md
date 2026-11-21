# Operaciones sobre fichas, matrices y selecciones

<a id="fpa-func-036"></a>

## FPA-FUNC-036 · Comparar y editar fichas

**Comparación, FM24 y FM27.** Elegir un rol del catálogo o de una táctica (ambas fases en FM27). Exigir que el jugador tenga ese rol asignado. El conjunto puede ser club propio, selección guardada o todos. Elegir hasta cinco personas. Presentar identidad y atributos de cada una, roles y notas, y las siguientes medias. Una categoría sin atributos vale cero. Para n nombres de atributo, sumar sus medias (lo+hi)/2 y dividir por n. Los ausentes aportan cero al numerador y permanecen en el denominador. Determina las familias la condición de **portero del rol elegido**.

| Área | Atributos canónicos |
|---|---|
| Velocidad | Acceleration, Pace |
| Finalización | Finishing, Long Shots |
| Pase | Passing, Crossing, Vision |
| Regate | Dribbling, First Touch, Flair |
| Defensa | Tackling, Marking, Positioning |
| Físico | Strength, Stamina, Balance |
| Mental de campo | Work Rate, Determination, Teamwork, Decisions |
| Paradas de portería | Reflexes, One vs One, Handling, Agility |
| Juego aéreo de portería | Aerial Reach, Command of Area, Jumping Reach |
| Distribución de portería | Kicking, Throwing, Passing, Vision |
| Salidas de portería | Rushing Out (Tendency), Acceleration, Pace |
| Mental de portería | Composure, Concentration, Decisions, Anticipation |

La lista de roles de cada persona ordena establemente las asignaciones por DWRS descendente (ausente toma −1). Presenta los cuatro primeros y deja disponible la lista completa. El segundo grupo de medias usa los tres primeros niveles de campo o portería del [modelo Classic](03-valoraciones-y-formulas.md), más key y preferable del rol. Ejemplo: Acceleration=12–16 y Pace ausente producen (14+0)/2=7 en velocidad. La comparación no modifica el perfil DWRS ni las notas almacenadas. [Cálculo y filtros](../../old_version/desktop/src/app/pages/PlayerComparisonPage.cpp).

**Edición.** Cambiar club cuando el nuevo texto no esté vacío. Si el jugador pertenecía al primer club, se pueden modificar tiempo acordado, naturales manuales (incluida una selección expresamente vacía), primaria entre roles asignados y lado preferido. Las ediciones de inscripción sólo afectan a las marcas aplicables. Guardar cambios de ficha e inscripción conjuntamente y refrescar tras éxito. Una corrección no equivale a una nueva importación completa. La ficha permite además refrescar desde una fila de exportación y consultar histórico, rol y entrenamiento. Ejemplo: desactivar selección manual de naturales deja que se vuelvan a usar las naturales disponibles de la partida. [Edición](../../old_version/desktop/src/app/pages/EditPlayerPage.cpp) · [Consulta individual](../../old_version/desktop/src/app/pages/PlayerProfilePage.cpp).

<a id="fpa-func-037"></a>

## FPA-FUNC-037 · Gestionar manualmente la selección y ver mejoras por rol

Configurar identidad del equipo nacional, código y límite de edad. El grupo guardado se edita añadiendo o retirando UID. Una persona guardada que deja de cumplir edad o nacionalidad sigue siendo localizable para poder quitarla. El cálculo de la propuesta automática con tamaño solicitado está en [convocatoria](06-analisis-de-plantilla.md). La matriz permite filtrar personalidad y ocultar inactivos. Distingue grupo marcado y candidatos elegibles aún sin convocar. El filtro por táctica selecciona los roles a mostrar. La asignación manual guardada también alimenta el XI nacional.

Con selección configurada y personas convocadas, el resumen devuelve número de convocados y edad media con una decimal, sólo de edades positivas. Por puesto toma la nota ocupada de A, la de B y todas las de profundidad para ese rol. Calcula mínimo, máximo y media aritmética de esa lista. Los pesos APT están desactivados. Para sugerir una mejora, por cada rol de la táctica (dos fases en FM27) toma la mejor valoración entre los convocados, mínimo de referencia cero. Busca entre no convocados disponibles con edad positiva dentro del filtro y con nota en ese rol. Gana el primero que mantiene el máximo estrictamente superior a esa referencia. El mismo jugador puede aparecer en más de un rol. Ejemplo: dos convocados con 19 y edad ausente dan edad media 19,0. Para un rol con máximo 70, dos elegibles con 72 y 74 producen una sugerencia de 74 aunque el número de convocados ya sea 23. [Grupo manual](../../old_version/desktop/src/app/pages/NationalSquadSelectionPage.cpp) · [Matriz](../../old_version/desktop/src/app/pages/NationalSquadMatrixPage.cpp) · [Indicadores y mejora](../../old_version/desktop/src/app/pages/NationalDashboardPage.cpp).

<a id="fpa-func-038"></a>

## FPA-FUNC-038 · Exportar una matriz

Partir de una sección de la matriz, aplicar sus filtros y ordenación vigentes. Emitir una fila de cabeceras y una por jugador en ese orden, con todas las columnas definidas para el modelo de esa sección y los valores de presentación. Codificación UTF-8 con BOM. Si el separador decimal del sistema es coma usar punto y coma, en otro caso coma. Si un campo contiene el separador, comillas o salto de línea, envolverlo en comillas y duplicar cada comilla interna. Un conjunto sin resultados conserva las cabeceras. Con separador coma y celda López, Ana, la celda se escribe **"López, Ana"**. La salida depende de la selección de roles, filtros y configuración de presentación. [Exportación del conjunto filtrado](../../old_version/desktop/src/app/pages/SquadMatrixPage.cpp) · [Filas y cabeceras](../../old_version/desktop/src/app/widgets/PlayerTableModel.cpp) · [Escape](../../old_version/desktop/src/core/Utils.cpp).

### Columnas que forman el CSV

Las columnas se construyen por sección en el orden siguiente. Las cabeceras de texto se traducen al idioma activo. Las funciones que ocultan columnas ya construidas para la consulta conservan esas columnas en el CSV. El archivo escribe los tres bytes `EF BB BF`, usa LF entre filas y termina en LF. Las celdas exportadas son las que se presentan a la persona. Por ejemplo, para el precio se usa el texto monetario original aunque la ordenación use un número.

| Orden | Club | Ojeo |
|---|---|---|
| 1. Identidad | Nombre, edad | Estrella de shortlist (★ o ☆), nombre, edad. Si está obsoleto o retirado, el nombre lleva el prefijo «⚠ » |
| 2. Talento | — | Si se activó talento, su puntuación redondeada al entero más cercano, mitad alejándose de cero |
| 3. Búsqueda | — | Si hay requisitos, valor combinado y, si hay referente, diferencia. A continuación roles elegidos como foco o requeridos, diferencia con el mejor propio si procede, cuenta de umbrales cumplidos y una columna por umbral activo en el orden del usuario |
| 4. Datos generales | Posición y, cuando alguna ficha de la partida tiene personalidad conocida, personalidad | Las mismas y luego club, valor de traspaso y antigüedad |
| 5. Contrato y detalle | Sueldo. Con detalle adicional se añaden pie izquierdo, pie derecho, estatura y valor de traspaso | Sueldo. Con detalle adicional se añaden pie izquierdo, pie derecho y estatura |
| 6. Atributos de talento | — | Sólo con talento activo, Determination y Work Rate con las medias truncadas a entero |
| 7. Roles restantes | Una columna por cada rol de la selección | Una columna por rol restante. Cada rol solicitado tiene columna aun si no figuraba en la selección inicial |

Con táctica elegida se utilizan sus roles sin duplicados, en FM27 los de IP preceden a los de OOP. Sin táctica se usa el catálogo completo ordenado por fase. El rol presenta «–» cuando falta la nota. El contador de umbrales usa «cumplidos/total». En la columna de cada umbral se presenta el punto medio con hasta tres cifras significativas según la configuración regional, o «–» si vale cero. Los dos atributos de talento de la fila 6 tienen una regla de presentación diferente y pueden truncar un medio punto.

Para la diferencia por rol usar la mayor nota de un jugador del primer club sin lesión de larga duración. Para la combinada usar la referencia de la búsqueda, si existe. Primero redondear la nota y la referencia por separado a la precisión DWRS configurada (0…2 decimales, mitad alejándose de cero). Restarlas y presentar el signo positivo si corresponde. Con referente 70,44 y candidato 70,46, a una decimal resulta +0,1 y a cero decimales resulta 0. Cambiar la precisión puede cambiar si la comparación señala una mejora. [Selección y cálculo de las columnas](../../old_version/desktop/src/app/pages/SquadMatrixPage.cpp#L502) · [Orden de roles](../../old_version/desktop/src/core/Definitions.cpp#L313) · [Precisión](../../old_version/desktop/src/core/DwrsDisplay.cpp#L36) · [Salida del CSV](../../old_version/desktop/src/app/widgets/PlayerTableModel.cpp#L186).

<a id="fpa-func-040"></a>

## FPA-FUNC-040 · Conciliar salidas después de una carga de plantilla

En una importación declarada de plantilla, comparar los UID afectados con **ambos** clubes configurados tras guardar. Presentar los miembros de esos clubes que faltaron. Inicialmente se proponen como salidas a agente libre (FrA). El usuario puede indicar que se quedaron, otro destino o Retired. Cancelar deja intactos los clubes de los ausentes. Al aceptar, para cada salida cuyo destino difiere del club actual guardar nuevo club, borrar marca de venta, de cesión y destino pendiente. Un destino vacío adopta FrA. Guardar el conjunto de salidas y refrescar fichas. En particular una persona ausente de un fichero de ojeo no se convierte por ese hecho en agente libre. La detección es posterior a importar las filas y es independiente del cálculo cuantitativo de carga completa de antigüedad. [Comparación y resolución](../../old_version/desktop/src/app/pages/DashboardPage.cpp) · [Efectos de una salida](../../old_version/desktop/src/core/PlayerStatus.cpp).

## Relación entre operaciones

| Entrada o acción | Hecho que persiste | Operaciones dependientes |
|---|---|---|
| Importar exportación de plantilla | Ficha actual por UID, fecha/carga y observaciones si proceden | Asignar roles, recalcular DWRS, conciliar ausentes |
| Guardar nuevas naturales o primaria | Preferencia del jugador | Nuevo XI, B, inscripción propuesta y simulación de fichajes |
| Guardar rol de entrenamiento | Elección de la persona | Plan y ficha usan la elección si aún aparece entre sus opciones |
| Cambiar de perfil de valoración | Fórmula y selección efectiva en esa partida | Recalcular notas, registrar reajuste, reevaluar alineación, mercado y ganancias marginales |
| Guardar una convocatoria | Miembros de selección | XI, referencia para nuevas propuestas y mejores externos por rol |
| Confirmar la lista de liga | Marca por persona en esa liga | Conjunto elegible de XI en liga y coste de la inscripción |
| Ocultar un jugador por antigüedad | La consulta lo excluye al aplicar la regla | Conserva su identidad, ficha e históricos, puede reaparecer al refrescar |
| Guardar un destino de traslado | Club de la ficha y limpieza de marcas | La siguiente formación del grupo de club/mercado ya refleja el cambio |

### Marcas de la persona

Venta, cesión y shortlist son marcas que se pueden cambiar para una ficha existente. Lesión de larga duración es una decisión guardada para la planificación de un club. Se diferencia de una exclusión puntual en una convocatoria. Los cambios de inscripción actualizan las marcas de liga o UEFA por separado. Por ejemplo, activar shortlist conserva al externo no retirado en consultas que normalmente lo ocultarían por antigüedad, y amplía el ámbito de las observaciones del juego. [Acciones por UID](../../old_version/desktop/src/app/PlayerActions.cpp) · [Reglas de actividad](../../old_version/desktop/src/core/PlayerStatus.cpp).
