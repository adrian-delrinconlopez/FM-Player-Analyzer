# Inscripción y elegibilidad

## FPA-FUNC-029 · Determinar las reglas y las exenciones

Cada partida puede seleccionar liga y activar UEFA por separado. El país de la persona, el entrenamiento local, la inscripción y el bloqueo son hechos diferentes. La condición de formado en el club implica formado en la federación. Las dos marcas se mantienen a mano. También se marca manualmente extracomunitario, porque el acuerdo de la competición y los pasaportes pueden cambiar el resultado.

| Competición | Versiones | Plazas | Reservadas para formados en federación / club | Exención de plaza | Otra restricción aplicada |
|---|---|---:|---:|---|---|
| Ninguna, Bundesliga | Ambas | Sin lista | — | — | Bloqueo doméstico manual |
| Premier League | Ambas | 25 | 8 / 0 | U21 | — |
| UEFA CL, EL, ECL | Ambas | 25 | 8 / 4 | U21 y formado en club | ≥2 porteros en lista A |
| LaLiga | FM27 | 25 | 0 / 0 | U18 EU | ≤3 extracomunitarios, reglas salariales |
| Serie A | FM27 | 25 | 8 / 4 | — | — |
| Ligue 1, Bundesliga de Austria | FM27 | Sin lista | — | — | Bloqueo doméstico manual |
| Liga Portugal | FM27 | 68 | 10 / 0 | — | Se usa el caso de club con equipo B |
| Super League de Suiza | FM27 | 25 | 8 / 0 | — | ≤10 extracomunitarios en lista, ≤5 en el XI de liga |
| Russian Premier Liga | FM27 | 55 | 8 / 4 | — | Sin cupo específico de extranjeros |
| Ukrainian Premier League | FM27 | 25 | 8 / 4 | U21 | — |
| Süper Lig | FM27 | 28 | 0 / 0 | — | Sin cupo específico de extranjeros |
| Copa doméstica | Ambas, según liga | Sin lista | — | — | Plantilla disponible para copa |

Con tamaño S y reservas H/C se aceptan hasta S−H ajenos a la federación y S−C ajenos al club. Un club con sólo 17 futbolistas no locales puede presentar esos 17 en UEFA. El objetivo configurable de porteros de liga es 2 inicialmente (0…5). UEFA toma el mayor entre 2 y el objetivo. Un portero exento cuenta en liga y queda fuera del recuento de lista A en UEFA.

**U21.** La decisión manual sí/no prevalece. En automático, de 1 a 20 años sí, con 21 o edad desconocida incierto y desde 22 no. Un nuevo dato que aumente la edad hasta 22 o más caduca la decisión manual. Para lista B se usa la formación en club como aproximación al tiempo de permanencia. Un resultado incierto consume plaza en la propuesta.

**U18 EU.** Una decisión manual resuelve edad y nacionalidad conjuntamente. En automático, marca extracomunitaria da no. De 1 a 17 la edad da sí. Con 18 da sí de enero a junio e incierto en el resto. Con 19 da no de julio a diciembre e incierto en el resto. Con fecha desconocida, 18 y 19 son inciertos. Desde 20, no. Edad desconocida, incierto. Para convertir un sí automático en exento hace falta al menos una de las dos nacionalidades reconocida. Una edad ascendente a 20 o más caduca la decisión manual. Códigos aceptados, sin distinguir caja y sin espacios extremos: AUT BEL BUL CRO CYP CZE DEN EST FIN FRA GER GRE HUN IRL ITA LVA LTU LUX MLT NED POL POR ROU SVK SVN ESP SWE ISL LIE NOR.

Ejemplo calculado manualmente. Una jugadora de 18 años, ESP, sin marca extracomunitaria ni decisión manual, está exenta en marzo en LaLiga. En septiembre necesita plaza mientras su situación siga incierta. En UEFA su edad da U21, pero necesita formación en club para lista B. [Reglas y fechas](../../old_version/desktop/src/core/Registration.cpp) · [Pruebas ejecutadas](../../old_version/desktop/src/tests/test_registration.cpp).

## FPA-FUNC-030 · Proponer una lista

Se necesita club propio y una competición con lista. Reunir club y segundo equipo configurado, excluyendo retirados. Usar las dos tácticas favoritas existentes, sin repetición. En su ausencia tomar la primera táctica disponible en el orden de favoritas. Por cada jugador conservar su mejor categoría entre las tácticas (XI, B, profundidad, reserva), y su mejor DWRS. Orden estable por categoría y después DWRS descendente. Entradas adicionales: portería, exención, bloqueo por competición, salario, tiempo prometido y las marcas anteriores.

1. Separar exentos primero. Apartar a los bloqueados de los demás. Para completar la lista, considerar en orden a quienes pasan todas las restricciones.
2. Si hay presupuesto, intentar primero Star Player, Important Player, Regular Starter y First-Choice Goalkeeper. Después completar el mínimo de porteros alcanzable. Por último recorrer todo el ranking.
3. Para explicar un rechazo aplicar esta precedencia: sueldo por debajo del mínimo, lista llena, máximo de ajenos a la federación, máximo de ajenos al club, máximo de extracomunitarios, presupuesto. Un mínimo M>0 impide el alta con sueldo semanal conocido <0,995 M salvo exención manual. El desconocido aporta cero al coste y pasa el mínimo. Presupuesto semanal B>0 admite hasta B+0,01. Mes equivale a 52/12 semanas y año a 52. Los ajustes salariales sólo afectan a LaLiga. Ambos parten de cero.
4. Si hay presupuesto, intentar hasta 200 mejoras locales de cero o una salida y una o dos entradas. No sale un titular ni alguien con tiempo prometido. Mantener plazas, cupos, presupuesto y al menos el mínimo entre los porteros alcanzados inicialmente y el objetivo. La utilidad de cada alta es mejor DWRS + 1000/300/100/0 según categoría. Aplicar el intercambio con mayor ganancia estricta superior al margen 0,000001. Detener al agotarse las mejoras. No garantiza el óptimo global.
5. Devolver inscritos en orden de ranking, exentos, descartes con razón, porteros, cupos, gasto y plazas vacías. Dos personas exentas no añaden dos plazas al tamaño máximo.

El caso ejecutado de LaLiga de [los ejemplos de referencia](14-casos-de-prueba-de-referencia.md) demuestra que cambiar sólo el tiempo prometido puede reducir la lista y dejar parte del presupuesto sin gastar. [Clasificación, propuesta y refinamiento](../../old_version/desktop/src/core/Registration.cpp) · [Selección del grupo](../../old_version/desktop/src/app/pages/RegistrationPage.cpp).

## FPA-FUNC-031 · Comprobar, confirmar y aplicar la lista

El usuario puede cambiar los inscritos propuestos y confirmar las marcas por jugador y competición. Comprobar recuento total, ajenos a federación y club, extracomunitarios, porteros, infracciones del mínimo salarial y suma semanal. Se pide confirmación adicional por exceso de tamaño, de ajenos a federación o club, de presupuesto o por sueldo insuficiente. El exceso de extracomunitarios y la falta de porteros sí aparecen en la comprobación, pero por sí solos no disparan esa confirmación adicional.

Una competición con lista empieza a filtrar el XI cuando existe **al menos un inscrito** en el conjunto de primer y segundo equipo. El primer equipo puede participar por inscripción o exención. El segundo sólo por inscripción. Guardar todos los inscritos a falso vuelve a la situación de lista vacía. En liga sin lista, el bloqueo doméstico manual excluye del XI de liga y de la vista general asociada. En copa se puede alinear al bloqueado. El límite suizo del XI se aplica en modo liga. El coste por táctica compara la media de DWRS de titulares ocupados antes y después de aplicar la lista. Una alineación vacía tiene media cero. Ese denominador puede hacer subir la media incluso habiendo perdido un titular.

Ejemplo calculado manualmente. Con primer equipo de dos adultos de DWRS 80 y 60 en puestos diferentes, y sólo el primero inscrito, la media pasa de 70 a 80, quedando un puesto vacío. Una lista totalmente vacía devuelve a ambos adultos al conjunto de selección. [Elegibilidad y cálculo del coste](../../old_version/desktop/src/core/Registration.cpp) · [Confirmación](../../old_version/desktop/src/app/pages/RegistrationPage.cpp) · [XI por competición](../../old_version/desktop/src/app/pages/BestXiPage.cpp).
