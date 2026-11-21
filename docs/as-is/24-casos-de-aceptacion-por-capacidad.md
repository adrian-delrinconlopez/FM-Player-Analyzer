# Casos de aceptación por capacidad

Cada fila identifica una operación que se puede contrastar usando únicamente esta carpeta. El enlace del ID abre su contrato, incluida la versión a la que se aplica. Cuando el escenario suministra notas por rol, se parte de esas notas. Los casos de importación y los oráculos permiten comprobar por separado cómo se obtienen. Salvo los cambios indicados, se aplican los ajustes iniciales y sólo intervienen las personas y restricciones descritas.

**G** indica **GOLDEN MASTER VERIFICADO** en el escenario enlazado. **M** indica **EJEMPLO CALCULADO MANUALMENTE** a partir de las reglas del contrato. Una suite que ejercite ese dominio no convierte un escenario manual en una ejecución observada. La comparación pendiente con una carrera real de FM27 se describe en [P01](14-casos-de-prueba-de-referencia.md#p01).

## Ficha, roles y valoraciones

| Contrato | Entrada y operación | Salida esperada | Evidencia del caso |
|---|---|---|---|
| [FPA-FUNC-001](02-modelo-funcional-de-datos.md#fpa-func-001) | En una ficha FM24 con UID 42, importar la fila inicial de M02, con Pac=12-14 y estatura «170 cm». | Pace=(12,14), media 13 y estatura 170. El UID sigue siendo el texto 42. | [M · M02](14-casos-de-prueba-de-referencia.md#m02) |
| [FPA-FUNC-002](02-modelo-funcional-de-datos.md#fpa-func-002) | Existe UID 437 para Ana con una observación. Llega r-437 con el mismo nombre recortado. No existe otra ficha con el prefijo. | Una sola Ana bajo r-437 conserva el identificador local y la observación. | M |
| [FPA-FUNC-003](02-modelo-funcional-de-datos.md#fpa-func-003) | Usar el nombre de club «Club» y el texto monetario «€1,5K». | Valor 0, sueldo semanal 1 500 y agente libre falso. | [G · G14](14-casos-de-prueba-de-referencia.md#g14) |
| [FPA-FUNC-004](04-posiciones-y-roles.md#fpa-func-004) | Asignar los roles iniciales a una ficha nueva FM24 con posición «D/WB (R), ST». | Quedan las posiciones D (R), WB (R) y ST (C). Los roles son la unión sin duplicados de sus tres listas del capítulo 16. | M |
| [FPA-FUNC-005](04-posiciones-y-roles.md#fpa-func-005) | Atributo key 14–16 del rol, otro preferable a 8, personalidad desconocida, opción de atributos globales apagada y demás atributos desconocidos. | Una fortaleza por el key de media 15. El preferable queda fuera de ambas listas. | M |
| [FPA-FUNC-006](03-valoraciones-y-formulas.md#fpa-func-006) | Usar los pesos Classic iniciales y los 47 atributos exactos a 20. | Normalizada 100 para todos los roles iniciales de ambos juegos. G01 enlaza además las absolutas por rol. | [G · G01](14-casos-de-prueba-de-referencia.md#g01) |
| [FPA-FUNC-007](03-valoraciones-y-formulas.md#fpa-func-007) | FM27, perfil inicial harvestgreen22 y 47 atributos a 12. Evaluar AM-IP y AM-OOP. | Normalizadas a una decimal 70,1 y 70,3, respectivamente. | [G · G02](14-casos-de-prueba-de-referencia.md#g02) |
| [FPA-FUNC-008](03-valoraciones-y-formulas.md#fpa-func-008) | FM27, rol BGK, perfil inicial y fichas Uniforme12, Gradiente e Inverso del oráculo de curvas. | Normalizadas a una decimal 57,9, 54,0 y 46,0 en ese orden. | [G · G02](14-casos-de-prueba-de-referencia.md#g02) |

## Táctica, plantilla y selección

| Contrato | Entrada y operación | Salida esperada | Evidencia del caso |
|---|---|---|---|
| [FPA-FUNC-009](05-tacticas-y-optimizacion.md#fpa-func-009) | FM24, cuatro puestos, nueve jugadores, pesos unitarios y primaria de Aguilera como en G03. | Suma de titulares 290 y de equipo B 279. G03 identifica los ocho elegidos por puesto. | [G · G03](14-casos-de-prueba-de-referencia.md#g03) |
| [FPA-FUNC-010](05-tacticas-y-optimizacion.md#fpa-func-010) | FM27, dos puestos en ambas fases, Boey en DR y Olise 90/55 frente a Gnabry 80/75 en AMR, según G04. | Con cuota IP 50 % es titular Gnabry. Al 70 % entra Olise. El titular de cada puesto aparece en las dos fases. | [G · G04](14-casos-de-prueba-de-referencia.md#g04) |
| [FPA-FUNC-011](05-tacticas-y-optimizacion.md#fpa-func-011) | Una fase, un puesto, pesos unitarios, titular 80 ausente y único suplente legal 71. Sin restricciones adicionales. | Un plan, entra el suplente, puesto cubierto, total 71 y pérdida 9. | M |
| [FPA-FUNC-012](06-analisis-de-plantilla.md#fpa-func-012) | A y B ya asignados. Sólo queda un jugador sin primaria con DWRS positivos en los tres roles distintos de la táctica. Máximo de roles de profundidad 2. | Es la opción de profundidad en los tres roles. El tercer rol utiliza la regla de reserva al agotarse las alternativas. | M |
| [FPA-FUNC-013](06-analisis-de-plantilla.md#fpa-func-013) | MC titular a 50 con DMC como alternativa a 80, otro titular DMC a 75, desplazamiento mínimo 10 y caída desactivada. | Un aviso en MC, mejor destino DMC y gravedad 30. | [G · G05](14-casos-de-prueba-de-referencia.md#g05) |
| [FPA-FUNC-014](06-analisis-de-plantilla.md#fpa-func-014) | Táctica con los once valores de G05, cuatro puestos con dos candidatos y siete con uno. | 11 ocupados, media 71,5, mediana 71, profundidad media 1,36 y siete puestos con cobertura frágil. | [G · G05](14-casos-de-prueba-de-referencia.md#g05) |
| [FPA-FUNC-015](06-analisis-de-plantilla.md#fpa-func-015) | Veinte jugadores de campo de 80 a 61 y dos porteros de 90 y 88. Sin táctica ni convocatoria previa, solicitar diez con objetivo de tres porteros. | Dos porteros y ocho de campo. Se completa el grupo aunque falte un portero. | [G · G10](14-casos-de-prueba-de-referencia.md#g10) |

## Mercado y desarrollo

| Contrato | Entrada y operación | Salida esperada | Evidencia del caso |
|---|---|---|---|
| [FPA-FUNC-016](07-scouting-y-mercado.md#fpa-func-016) | Dos candidatos y umbrales Pace≥13 y Passing≥10. Ada tiene medias 13 y 0, Bea tiene 13 y 11. Resto como en M03. | Sólo mostrar umbrales devuelve las dos filas con 1/2 y 2/2. Exigirlos devuelve únicamente a Bea. | [M · M03](14-casos-de-prueba-de-referencia.md#m03) |
| [FPA-FUNC-017](07-scouting-y-mercado.md#fpa-func-017) | Mejor propio del rol a 74. Dos externos activos, 20 años y valor 5 M cada uno, con 74 y 76. Filtros iniciales. | Una propuesta para ese rol, el de 76, con ventaja de 2. | M |
| [FPA-FUNC-018](07-scouting-y-mercado.md#fpa-func-018) | Defensa de 80, polivalente a 78 en defensa y 79 en medio, medio de 70. Añadir el único candidato, defensa de 77, en el escenario de G06. | Titulares de 228 a 236 (+8). Recolocación del polivalente y ganancia de 70 en el equipo B. | [G · G06](14-casos-de-prueba-de-referencia.md#g06) |
| [FPA-FUNC-019](07-scouting-y-mercado.md#fpa-func-019) | Tres excedentes de campo de 16, 17 y 21 años, fuera de todos los equipos y profundidad. Talento 45 para los dos jóvenes, ajustes iniciales. | El de 16 permanece sin sugerencia, el de 17 se propone para cesión y el de 21 para venta. | M |
| [FPA-FUNC-020](08-desarrollo-y-entrenamiento.md#fpa-func-020) | Mejor rol 70, edad 18, tope 24, Determination y Work Rate a 15, Model Citizen. | Talento 87,5. | [G · G07](14-casos-de-prueba-de-referencia.md#g07) |
| [FPA-FUNC-021](08-desarrollo-y-entrenamiento.md#fpa-func-021) | Perfil de curvas con un solo atributo activo de peso 1, base cero y forma lineal. Edad 22, atributo a 10, banda explosiva de peso 1 y factor de edad 2/3. | Ganancia 100/19. Prioridad 200/57, aproximadamente 3,50877. | M |
| [FPA-FUNC-022](08-desarrollo-y-entrenamiento.md#fpa-func-022) | Jugadora de campo. Únicas prioridades positivas Pace=2 y Acceleration=1. Evaluar el método Quickness de Attributes. | Puntuación del método 3. Su homónimo de Injury Rehab queda excluido del plan. | M |

## Importaciones e históricos

| Contrato | Entrada y operación | Salida esperada | Evidencia del caso |
|---|---|---|---|
| [FPA-FUNC-023](09-historicos.md#fpa-func-023) | Mismo jugador seguido y demás campos constantes. En enero Pace=10. En febrero se observa 12 el día 5, vuelve a 10 el día 20 y luego llega una carga fechada el día 10 con 13. | El histórico conserva enero y febrero visto hasta el día 20, sin instantánea de cambio de febrero. La ficha actual puede pasar a 13 con la última carga. | M |
| [FPA-FUNC-024](11-importacion-y-visibilidad.md#fpa-func-024) | Partida FM24 vacía. Cargar las dos filas de G11 como CSV o HTML y pedir asignación y recálculo. | Cada formato crea dos personas y 17 valoraciones. Quitar Pac deja nueve columnas y provoca rechazo. | [G · G11](14-casos-de-prueba-de-referencia.md#g11) |
| [FPA-FUNC-025](11-importacion-y-visibilidad.md#fpa-func-025) | Registro decodificado de Ada con UID 13, conocimiento 62 y atributos 12. Ejecutar con máscara activa y desactivada. | Las salidas de 61 campos figuran en G12. Con máscara hay 19 rangos y sin máscara ninguno. | [G · G12](14-casos-de-prueba-de-referencia.md#g12) |
| [FPA-FUNC-026](11-importacion-y-visibilidad.md#fpa-func-026) | Seguimiento activo de Career.fm. Aparece Career (v02).fm más reciente, estable en dos comprobaciones, con antigüedad suficiente y sin diálogo ni trabajo activo. | Se importa esa versión y se guarda su marca temporal al terminar con éxito. Las siguientes comprobaciones con la misma marca no disparan otra importación. | M |
| [FPA-FUNC-027](09-historicos.md#fpa-func-027) | Misma persona y rol con tres sesiones: 50, 60 por recálculo, 62 por importación. | Serie ajustada [60,60,62], cambio deportivo +2 y reajuste +10. | [G · G08](14-casos-de-prueba-de-referencia.md#g08) |
| [FPA-FUNC-028](09-historicos.md#fpa-func-028) | Externo de 36 años, club distinto del propio, último sello 3 y contador actual 5. Umbrales iniciales. | Antigüedad 2, obsoleto y retirado por inferencia. La ficha sigue almacenada. | M |

## Inscripción y configuración

| Contrato | Entrada y operación | Salida esperada | Evidencia del caso |
|---|---|---|---|
| [FPA-FUNC-029](10-inscripcion-y-elegibilidad.md#fpa-func-029) | LaLiga FM27, jugador de 18 años, ESP, sin decisión manual ni marca extracomunitaria. Comparar marzo y septiembre. | Exención automática en marzo. En septiembre la decisión queda incierta y la propuesta cuenta una plaza. | M |
| [FPA-FUNC-030](10-inscripcion-y-elegibilidad.md#fpa-func-030) | G09, LaLiga con presupuesto semanal 100 000 y cinco candidatos. Primero sin promesa y luego con Star Player para star. | Primero entran pricey, cheap y unknown por 100 000. Después star y unknown por 60 000. | [G · G09](14-casos-de-prueba-de-referencia.md#g09) |
| [FPA-FUNC-031](10-inscripcion-y-elegibilidad.md#fpa-func-031) | Competición con lista. Dos adultos del primer equipo sin exención, únicos candidatos para dos puestos distintos, DWRS 80 y 60, sólo el de 80 inscrito. | La lista activa deja un puesto ocupado con media 80. Al vaciar la lista ambos vuelven a ser elegibles y la media es 70. | M |
| [FPA-FUNC-032](12-configuracion-y-reglas-globales.md#fpa-func-032) | Crear partida copiando ajustes de otra con táctica personal, club propio definido, un jugador y seguimiento activo de un archivo FM27. | La nueva dispone de la táctica. Se inicia sin ese jugador, sin la asignación de club de origen y con el seguimiento desactivado. | M |
| [FPA-FUNC-033](12-configuracion-y-reglas-globales.md#fpa-func-033) | Duplicar un perfil y cambiar únicamente su nombre y descripción, conservando juego, roles y parámetros de cálculo. | El duplicado tiene ID nuevo y conserva la revisión de fórmula. Cambiar después un punto válido de la forma de Pace sí altera esa revisión. | M |
| [FPA-FUNC-034](12-configuracion-y-reglas-globales.md#fpa-func-034) | Exportación FM27 de Control Press · 4-2-3-1. Cambiar sólo el rol del STC con balón por PFB y, en otro ensayo, por NIX sin definición. | El primer archivo pasa la validación. En el segundo se informa del rol desconocido NIX. | [G · G13](14-casos-de-prueba-de-referencia.md#g13) |
| [FPA-FUNC-035](12-configuracion-y-reglas-globales.md#fpa-func-035) | Persona con rol manual X y posición cuyos predeterminados producen sólo Y. Ejecutar primero completar y después restablecer. | Después de completar tiene X e Y. Después de restablecer queda sólo Y y, si X era la primaria, se limpia. | M |

## Operaciones individuales y ajustes

| Contrato | Entrada y operación | Salida esperada | Evidencia del caso |
|---|---|---|---|
| [FPA-FUNC-036](23-operaciones-con-fichas-y-selecciones.md#fpa-func-036) | En una comparación de campo con el rol asignado, Acceleration=12–16 y Pace desconocido. | La media de velocidad vale (14+0)/2=7. El ausente sigue en el denominador. | M |
| [FPA-FUNC-037](23-operaciones-con-fichas-y-selecciones.md#fpa-func-037) | Selección configurada con dos convocados, edades 19 y desconocida, mejor nota del rol 70. Los dos únicos elegibles externos al grupo tienen 20 años y 72/74 en el rol. | Dos convocados, edad media 19,0 y una sugerencia para ese rol con 74. La sugerencia no cambia la convocatoria. | M |
| [FPA-FUNC-038](23-operaciones-con-fichas-y-selecciones.md#fpa-func-038) | Exportar una sección de la matriz con una sola fila «López, Ana», en un sistema con separador de campos coma. Ocultar la columna Sueldo que ya pertenece al modelo. | Se conserva Sueldo, el nombre se emite como «"López, Ana"», el fichero empieza con BOM UTF-8 y acaba con LF. | M |
| [FPA-FUNC-039](12-configuracion-y-reglas-globales.md#fpa-func-039) | Perfil de entrenamiento v1 con una única banda llamada Prueba, Pace, pico 60, bloqueo 30 y peso 8. | Pace pertenece a Prueba con pico 50, bloqueo 50 y peso 5. Los atributos que quedan fuera de la banda no tienen prioridad de entrenamiento. | M |
| [FPA-FUNC-040](23-operaciones-con-fichas-y-selecciones.md#fpa-func-040) | Club A tiene los UID 1 y 2. Importar una actualización de plantilla que sólo contiene 1. El 2 tenía marca de venta. Confirmar su salida con el destino inicial. | El UID 2 pasa a FrA y se limpian venta, cesión y destino previsto. Si se cancela la conciliación sigue en A. | M |
| [FPA-FUNC-041](11-importacion-y-visibilidad.md#fpa-func-041) | Carpeta de vistas inicialmente vacía y escribible. Instalar sólo el grupo de vistas FM27 desde los recursos distribuidos. | Se copian los tres archivos Standard de PlayerDatabase, Shortlist y Squad. Sus nombres, tamaños y huellas están en FPA-FUNC-041. | M |
| [FPA-FUNC-042](12-configuracion-y-reglas-globales.md#fpa-func-042) | Editar una curva pasando los veinte puntos p(i)=50+(i−1), i=1…20. Después introducir p(10)=47 manteniendo los otros puntos. | Primera serie aceptada con base 50, peso 19 y forma (i−1)/19. La segunda es decreciente entre niveles 9 y 10, se conserva la última serie válida. | M |
