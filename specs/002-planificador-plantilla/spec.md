# Especificación del Planificador de plantilla

- **Rama de trabajo** `prototype/centro-analisis-fm27`
- **Creada** 26 de noviembre de 2026
- **Estado** Borrador funcional · prioridad v2
- **Encargo** Planificar manualmente la profundidad del primer equipo en una formación con posesión y detectar jugadores con pocas opciones en el plan. [DP-22](../../docs/PRODUCT-DECISIONS.md) · [REQ-SQUAD-006](../../docs/to-be/02-catalogo-funcional.md)

## Escenarios de usuario y pruebas

### Historia 1 · Montar la profundidad sobre el campo (P1)

Como mánager, quiero partir de mi formación vacía y colocar a mis jugadores donde contaría con ellos, para decidir la jerarquía de cada puesto.

**Valor.** Permite explorar la composición de la plantilla con mi criterio deportivo.

**Prueba independiente.** Usar una formación con dos puestos y una lista de primer equipo, filial y juvenil, sin cargar ninguna valoración.

1. **Dada** una formación con posesión, **al** crear el plan, **se** dibujan sus puestos y roles con cero jugadores y sin valoración de primera opción.
2. **Dado** el plan vacío, **al** arrastrar a un jugador del primer equipo a dos puestos, **se** añade una vez en cada puesto. Confirmar que añadir, mover la prioridad y quitar también se pueden hacer con teclado.
3. **Dados** tres jugadores en un puesto, **al** abrirlo, adelantar a un jugador y retirar a otro, **se** actualizan el orden manual y el número de asignados en ese puesto. Los demás puestos conservan sus asignaciones. El jugador retirado vuelve a estar disponible para añadir a ese puesto.
4. **Dados** los filtros de filial y juvenil apagados, **al** encender sólo el de filial, **se** incorporan sus jugadores autorizados a la lista de origen. Apagar el filtro conserva sus asignaciones ya hechas.

### Historia 2 · Detectar quién tiene pocas opciones (P1)

Como mánager, quiero ver en cuántos puestos he colocado a cada jugador del primer equipo y cuál es su mejor prioridad, para revisar inclusiones, rotación y posibles salidas.

**Valor.** Distingue polivalencia de prioridad real dentro de la planificación.

**Prueba independiente.** Abrir un plan de prueba con Álex cuarto en mediapunta y quinto en mediocentro, y Bruno sin asignaciones.

1. **Dada** esa jerarquía, **al** consultar el resumen del primer equipo, **se** muestran Álex con dos puestos y mejor prioridad 4.º, y Bruno con cero puestos y «Sin puesto en el plan». La lista permite ordenar por ambos indicadores.
2. **Dado** Álex en dos puestos, **al** adelantarlo a segundo en mediocentro, **se** muestran dos puestos y mejor prioridad 2.º. Quitarlo de ese puesto deja uno y mejor prioridad 4.º.
3. **Dado** un orden manual guardado, **al** ordenar temporalmente el detalle por estrellas, porcentaje u otra columna autorizada, **se** conserva la prioridad manual que usa el resumen.

### Historia 3 · Consultar calidad por puesto y rol (P2)

Como mánager, quiero consultar la valoración actual y potencial de cada opción en el mismo rol para entender por qué podría cambiar la jerarquía.

**Valor.** Pone el dato deportivo al lado de la decisión manual y permite identificar ausencias de datos.

**Prueba independiente.** Abrir un puesto de un plan de prueba con una ficha autorizada y resultados de valoración conocidos. Probar permiso base, CA activo, ambos activos y revocación.

1. **Dado** un puesto con una primera opción valorable, **al** inspeccionarlo, **se** ven las estrellas actuales y potenciales disponibles de sus jugadores, los porcentajes disponibles del rol y su escenario. El campo resume el número de asignados y las estrellas actuales disponibles de la primera opción manual.
2. **Dado** un jugador con sólo CA habilitada y acreditada, **al** abrir su fila, **se** puede consultar CA. PA y los derivados que necesitan ambos permisos quedan excluidos. Con ambos permisos y proyección acreditada se muestran los resultados disponibles. Al revocar cualquiera se retiran los datos y los derivados afectados, incluido cualquier orden temporal dependiente.
3. **Dadas** dos fuentes que difieren sólo en CA/PA restringidas, **con** permisos base, **se** obtiene el mismo listado autorizado, recuento, orden y diagnóstico.

### Casos límite

- Dos puestos de la formación pueden compartir denominación o rol. Cada uno conserva una lista propia y su identidad de puesto.
- Un jugador puede cubrir varios puestos. Se cuenta una vez por puesto concreto y una vez por fila del resumen del primer equipo. Un jugador de la filial o el juvenil puede desplazar en un puesto a uno del primer equipo y contar en ese orden.
- Una asignación con familiaridad baja, desconocida o sin valoración se puede anotar como elección manual, con su estado visible. La prioridad expresa la intención del mánager.
- Si cambia el contexto de carrera, club, perspectiva o revisión de táctica, se abre el ámbito correspondiente. El plan anterior sigue asociado a su contexto. Una sincronización parcial conserva las decisiones manuales; las identidades cuya visibilidad se pierda quedan fuera de toda salida y métrica del ámbito vigente.

## Requisitos

### Requisitos funcionales

- **FR-001.** Crear un plan vacío desde una formación con posesión, con un puesto por plaza concreta y un rol con posesión identificado en cada plaza. Abrir cada puesto desde el campo y ver el recuento de asignados y la calidad actual de la primera opción cuando esté disponible.
- **FR-002.** Mostrar el primer equipo como lista de origen, con controles independientes para incluir filial y juvenil. Añadir mediante arrastre o una acción de teclado equivalente. El mismo jugador puede estar en varios puestos con una única asignación en cada uno.
- **FR-003.** Consultar cada lista por puesto, cambiar el orden manual, retirar entradas y volver a incorporarlas. Distinguir «Prioridad en el plan» del orden visual por columnas.
- **FR-004.** Mostrar a todo jugador autorizado del primer equipo cuántos puestos distintos ocupa y la mejor prioridad, definida como el menor ordinal de sus asignaciones tras aplicar la visibilidad vigente. Para cero puestos, mostrar «Sin puesto en el plan» y prioridad ausente. Actualizar los indicadores tras añadir, mover o retirar una entrada y al cambiar la población autorizada. Son señales de posible falta de minutos.
- **FR-005.** Mostrar las estrellas actuales y potenciales disponibles y los porcentajes del rol para el mismo contexto de puesto, perfil, corte y fase. Identificar procedencia, cobertura y el escenario de familiaridad. Para la columna de potencial máximo del rol, usar la proyección condicionada con familiaridad 20 de REQ-RATING-005 cuando esté acreditada. Etiquetar las estrellas derivadas de Analyzer como estimación; el tratamiento de estrellas observadas y su comparabilidad se decide en Q-19.
- **FR-006.** Aplicar los permisos independientes de CA y PA antes de consultar o mostrar campos. Exigir ambos permisos y proyección acreditada para los derivados que los usen. Cuando falte cobertura, fuente o permiso, la valoración correspondiente queda indisponible y el orden manual sigue utilizable con las identidades autorizadas.
- **FR-007.** Conservar el plan manual y su prioridad al volver a abrir su ámbito de carrera, rama, club, perspectiva y revisión táctica. Guardar las decisiones locales. Volver a evaluar resultados por corte, perfil y permiso vigente. Indicar el contexto de cualquier plan abierto.

### Entidades principales

- **Plan de plantilla.** Ámbito de carrera, rama, club, perspectiva y revisión táctica. Identifica una formación con posesión y sus puestos ordenables.
- **Puesto planificado.** Plaza concreta de la formación, posición y rol con posesión. Contiene una secuencia manual de referencias a jugadores autorizados.
- **Asignación manual.** Referencia a jugador, puesto concreto y prioridad en el plan. Un jugador puede aparecer en varias secuencias.
- **Resumen individual.** Número de puestos y mejor prioridad de un jugador del primer equipo. Derivado de las asignaciones autorizadas vigentes.
- **Valoración contextual.** Estrellas o porcentajes actuales y potenciales disponibles para una asignación, con fuente, perfil, cobertura, corte y escenario.

## Criterios de éxito

### Resultados medibles

- **SC-001.** En una formación de 11 plazas, el plan nuevo muestra 11 puestos vacíos. Añadir el mismo jugador dos veces al mismo puesto mantiene una sola entrada. Añadirlo a un segundo puesto aumenta su recuento a dos.
- **SC-002.** El caso 4.º/5.º arroja dos puestos y 4.º como mejor prioridad. Cambiar un ordinal a 2.º, quitar la asignación y recargar reproducen 2.º, 4.º y el último orden manual respectivamente. Un jugador con cero puestos aparece en el resumen.
- **SC-003.** Añadir, reordenar, quitar e inspeccionar se pueden completar con teclado en las cuatro pruebas del recorrido. Ordenar temporalmente por calidad conserva ambos resultados manuales del caso SC-002.
- **SC-004.** Las cuatro combinaciones de permisos CA/PA muestran sólo los datos autorizados. Ambas fuentes gemelas coinciden con permisos base. Al revocar cualquiera de los permisos desaparecen las valoraciones dependientes antes de mostrar otra salida. Un cambio de ámbito conserva el aislamiento.

## Supuestos y dependencias

- Se prioriza para v2. El primer alcance usa con posesión y la formación elegida. El plan es local y manual. El XI automático, los minutos efectivos y las operaciones sobre plantilla siguen sus propios recorridos.
- Se proponen dos filtros independientes para filial y juvenil, conservación de asignaciones al ocultar el origen, recuento de plazas concretas y tarjeta con estrellas actuales de la primera opción. Son valores iniciales de diseño revisables antes de planificar.
- Se propone una ordenación visual temporal para consultar notas manteniendo la jerarquía manual como dato del plan. Una sincronización parcial conserva las decisiones pendientes de revisión y un cambio de revisión táctica conserva el plan anterior en su ámbito.
- Conjunto de dependencias de v1. Plantillas y permisos de SK-03, valoración actual y potencial de SK-04/05, y catálogo, formación y puestos de SK-07. Cada nota contextual depende de fuentes, permisos y cobertura acreditados. El plan manual puede ensayarse con datos controlados.
- Los porcentajes iniciales usan el motor de Analyzer acreditado. Genie Scout es una referencia de experiencia para mostrar calidad actual y máxima por rol. La equivalencia numérica y la incorporación de recursos de terceros quedan sujetas a evidencia reproducible y derechos. [PoC B](../../docs/roadmap/07-poc-y-modelo-v1.md#evidencia-para-poc-b) · [Genie Scout](https://geniescout.com/features/)
- El caso transversal de aceptación es [NV-30](../../docs/testing/02-casos-de-prueba.md). La planificación debe decidir la escala visual de estrellas y comprobar si existe fuente observada comparable para el rol exacto. [Q-19](../../docs/OPEN-QUESTIONS.md)
