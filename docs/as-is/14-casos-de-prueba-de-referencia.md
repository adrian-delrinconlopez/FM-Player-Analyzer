# Casos de prueba de referencia

Las entradas son datos funcionales. Cuando un ejemplo empieza por notas por rol, se suministran esas notas directamente. Para probar el recorrido desde atributos, están los dos oráculos y las reglas de importación. Una afirmación sobre un conjunto sin orden no prescribe el orden de sus miembros. Todos los casos G remiten a resultados del baseline ejecutado. Los casos M derivan de las fórmulas. Los P requieren evidencia adicional.

## G01 · Valoración Classic de ambos juegos

**GOLDEN MASTER VERIFICADO.** El [oráculo Classic](20-oraculo-dwrs-classic.md) contiene las 4.608 parejas (absoluta, normalizada), la construcción de los 16 jugadores y las dos configuraciones para todos los roles. Probar los niveles fijos, las distribuciones distintas por atributo, intervalos, faltantes parciales y ficha vacía. La ficha de valores 1 tiene normalizada 0 en todos los roles iniciales. La de valores 20 tiene 100. La totalmente vacía puede caer por debajo de cero. La selección táctica aplica después sus propios criterios de nota positiva. [Ejecución de la comparación](../../old_version/desktop/src/tests/test_classicreference.cpp).

## G02 · Curvas en FM27 y portería

**GOLDEN MASTER VERIFICADO.** El [oráculo de las curvas](22-oraculo-de-curvas-fm27.md) especifica tres fichas completas, el perfil y las 237 normalizadas emitidas por el evaluador del original a una decimal. Se incluyen todos los roles IP y OOP. La ficha de valores 12 ofrece una comprobación de que una distribución constante de atributos no obliga a que portería y campo den el mismo DWRS. Para intervalos, cobertura y valores ausentes, usar también M01 y las pruebas ejecutadas de curvas y de instantáneas. [Evaluador de referencia](../../old_version/desktop/src/tools/ratingcheck_cli.cpp) · [Pruebas de instantáneas](../../old_version/desktop/src/tests/test_ratingsnapshot.cpp).

## G03 · La asignación del equipo B desempata al XI en una fase

**GOLDEN MASTER VERIFICADO.** Táctica FM24, puestos ordenados DCL/DCR con BPD-D y DMCL/DMCR con DM-S. Factores de selección unitarios. Orden de entrada igual al de la tabla. Un guion significa que no existe nota ni asignación a ese rol.

| Jugador | Posición | BPD-D | DM-S | Primaria |
|---|---|---:|---:|---|
| Casas | D (C), DM | 73 | 71 | — |
| Jafari | D (RC), DM | 72 | 73 | — |
| Kerche | D (C) | 72 | — | — |
| Dumont | D (RC) | 70 | — | — |
| Aguilera | D (C), DM | 68 | 67 | BPD-D |
| Vlcek | D (L), DM | — | 72 | — |
| Tani | DM, M (C) | — | 71 | — |
| Junio | DM, M (C) | — | 70 | — |
| Hughes | D (C), DM | 43 | 49 | — |

**Salida.** Centrales A, Casas y Kerche. Medios A, Jafari y Vlcek. Suma A=290. Centrales B, Aguilera y Dumont. Medios B, Junio y Tani. Suma B=279. Hughes queda disponible para profundidad y Aguilera queda excluido como opción de DM-S por su primaria. En puestos gemelos, esta prueba compara los conjuntos de nombres. [Caso versatilePlayerStartsWhereTheBTeamLosesLeast](../../old_version/desktop/src/tests/test_squadbuilder.cpp).

## G04 · El mismo once en dos fases

**GOLDEN MASTER VERIFICADO.** Puestos AMR y DR en ambas fases. Para AMR, Olise con 90 en IF y 55 en TW, Gnabry con 80/75. Ambos son AM (R). Para DR, Boey D (R) con 70 en FB y 72 en PFB. Sin otras compatibilidades, sin pesos adicionales. Con 50 % IP resulta Gnabry en AMR en ambas fases, 77,5, y Boey en DR. Olise ocupa AMR en B y DR en B queda vacío. Con 70 % IP es titular Olise (79,5 frente a 78,5). En otro caso, si X sólo admite AML en IP y MR en OOP, e Y sólo AMR en IP y ML en OOP, con 70 IP y 60 OOP para ambos, la solución completa cruza los lados. [Casos tradesBallSkillForDefensiveWork y playersMayCrossSidesBetweenLineUps](../../old_version/desktop/src/tests/test_twophase.cpp).

## G05 · Equipo, carencias y cobertura

**GOLDEN MASTER VERIFICADO.** Once ocupado con notas 60,65,66,68,70,71,72,75,78,80,82. Cuatro puestos tienen dos candidatos y siete tienen uno. Resultado: 11 ocupados, siete con un solo candidato, profundidad 15/11 → 1,36, mediana 71 y media 787/11 → 71,5. Si se elimina el único portero quedan 10 ocupados y GK sin cobertura. Con grupo vacío, mediana interna −1, profundidad 0 y 11 puestos sin cobertura. [Casos de comparación táctica](../../old_version/desktop/src/tests/test_tacticexplorer.cpp).

**GOLDEN MASTER VERIFICADO.** Con jugador A de DWRS 50 en su puesto MC y 80 para DMC, DMC ocupado por B de 75, umbral de desplazamiento 10, caída desactivada y sin penalización de lado aplicable, se produce un único aviso en MC con mejor destino DMC y gravedad 30. El aviso no afirma que una permuta sea legal o que el XI vaya a sumar 30. [Caso displacementPointsAtTheBetterSlot](../../old_version/desktop/src/tests/test_gapanalysis.cpp).

## G06 · Distinguir una nota de mercado y una mejora de plantilla

**GOLDEN MASTER VERIFICADO.** Primer equipo con dos centrales BPD de 70 y 62, dos DM de 74 y 73 y un AMC de 71. Mercado: central 75, DM 76, AMC 72 y DM 60, cada uno compatible sólo con los puestos de su posición. Los cinco titulares suman 350. El central de 75 aporta +13 al XI, el mediapunta aporta +1 y el medio de 60 no obtiene opción. En otra entrada, central puro 80, polivalente central 78/medio 79 y medio puro 70. Fichar un central 77 lleva la suma de 228 a 236, +8, pues el polivalente cambia de puesto. El medio de 70 pasa a B y aporta +70 a ese equipo. [Casos ranksPositionsByMeasuredGain y countsKnockOnEffects](../../old_version/desktop/src/tests/test_signingadvisor.cpp).

**GOLDEN MASTER VERIFICADO.** A un candidato con notas IF=90 IP y PFB=50 OOP se le calcula, como indicador de búsqueda, 70 con cuota IP 50 % y 80 al 75 %. Con un único criterio PFB y cuota IP 100 %, la suma de pesos sería cero y se obtiene la media simple 50. Con un criterio sólo posicional la salida de este indicador es −1. La comprobación de compatibilidad posicional es otra operación. [Caso combinedValueWeighsThePhases](../../old_version/desktop/src/tests/test_scoutsearch.cpp).

## G07 · Juventud y entrenamiento

**GOLDEN MASTER VERIFICADO.** Mejor nota 70, edad 18, tope 24, Determination y Work Rate 15, Model Citizen → 87,5. Cambiar a nota 60, edad 26, mentales 5 y 7, Slack → 49. El caso de atributo en intervalo con nota 50, edad y tope 24, Determination 12–15, Work Rate 20 y personalidad neutra devuelve 53,375. [Casos del talento](../../old_version/desktop/src/tests/test_talentengine.cpp).

**GOLDEN MASTER VERIFICADO.** Campo FM24, AM (R), 47 atributos a 10, perfil Classic histórico, WR-S y bandas de inicio. A los 19, Pace o Acceleration encabezan los atributos y Quickness los métodos. Con Pace a 20, Pace no aparece entre los atributos entrenables. A los 30 sólo aparecen atributos mentales en la lista de entrenables y Determination se puede señalar por mentoría. Ejercitar también la vía de curvas con meseta mediante M01. [Casos de entrenamiento](../../old_version/desktop/src/tests/test_trainingadvice.cpp).

## G08 · Tiempo de juego y fechas de observación

**GOLDEN MASTER VERIFICADO.** Misma persona y rol con sesiones sucesivas de 50, 60 y 62. La segunda está marcada como recálculo. El resultado deportivo es +2, la serie ajustada es [60,60,62] y el reajuste aislado es +10. Si el 60 del segundo día se registra a las 20:00 y el 62 a las 20:04, pertenecen a la misma sesión y el crecimiento deportivo sigue siendo +2. [Caso recalcsRebaseInsteadOfCounting](../../old_version/desktop/src/tests/test_development.cpp).

## G09 · Presupuesto y promesa de tiempo de juego

**GOLDEN MASTER VERIFICADO.** LaLiga, todos de edad 25, no porteros, categoría profundidad, en este orden con DWRS 100,99,98,97,96. Tope semanal 100.000, mínimo 5.000 y objetivo de porteros 0. Las fichas son star (60.000 por semana), pricey (50.000 por semana), cheap (2.600.000 por año), youth (1.000 por semana) y unknown (sin salario). Sin tiempo prometido se inscribe a pricey, cheap y unknown. Coste 100.000. Motivos de los descartes: star excede presupuesto, youth queda debajo del mínimo. Cambiar únicamente star a Star Player produce star y unknown por 60.000. Añadir bargain de 40.000 y nota 95 completa la lista con star, unknown, bargain por 100.000. [Caso wageRules](../../old_version/desktop/src/tests/test_registration.cpp).

## G10 · Visibilidad y selección nacional

**GOLDEN MASTER VERIFICADO.** Carrera masculina, Home FC, 14-09-2026. Jugadores con atributo uniforme: propio UID 10 a 10, famoso UID 11 con reputación 8.000 a 15, externo UID 12 con conocimiento 95 a 14, UID 13 con 62 a 12, UID 14 con 40 a 11 y UID 15 sin conocimiento a 9. Añadir agente libre no observado 16, filial 17 alineado por el club propio y una jugadora famosa 18. Con máscara, el propio, el famoso y el bien ojeado tienen cero intervalos. El de 62 tiene 19 intervalos. El de 40 tiene los 47 con anchos 2…4, el desconocido con club tiene los 47 con anchos 4…7. La lista para histórico por conocimiento incluye sólo 12 y 13. El sueldo propio de 10.000 libras semanales aparece como €12K p/w. La exportación del analizador puede por ello incluir fichas externas aun sin informe. [Caso visibility](../../old_version/desktop/src/tests/test_fm27save.cpp).

**GOLDEN MASTER VERIFICADO.** Grupo nacional con dos porteros puntuados 90 y 88 y veinte M (C) de 80 a 61. Sin táctica ni convocatoria previa, pedir diez con tres porteros devuelve diez personas, dos porteros y ocho de campo. [Caso tooFewGoalkeepersFillsWithOutfield](../../old_version/desktop/src/tests/test_nationalcallup.cpp).

## M01 · Cobertura y curva

**EJEMPLO CALCULADO MANUALMENTE.** Dos atributos activos con pesos 7 y 3, factores 1, bases 2 y 5 y cuotas de rol cero. Con el de peso 7 a 20 y el otro ausente, cobertura 70 %, absoluto 14 y normalizada 100. Intercambiar el conocido da cobertura 30 % y ausencia de nota. Con un solo atributo de peso 1, forma escalón que vale 0 hasta nivel 10 y 1 desde 11, observar 9–11 da normalizada 100/3 y elevar ambos extremos a 10–12 da 200/3. En la meseta 12–14, incrementar en uno no mejora la nota. [Regla de cálculo](03-valoraciones-y-formulas.md).

## M02 · Una importación parcial y una edición posterior

**EJEMPLO CALCULADO MANUALMENTE.** FM24. Primera carga con diez cabeceras UID, Name, Age, Club, Nat, Position, Personality, Wage, Height, Pac, y fila 42, Ana, 19, A, ESP, M (C), Balanced, vacío, 170 cm, 12-14. Segunda carga de la misma persona con diez cabeceras que sustituyen Pac por otro campo opcional y Age=20. Sigue con Pace 12–14. Una tercera carga con Pac presente y guion lo deja desconocido. En una actualización individual el UID objetivo se conserva incluso si difiere el UID de la fila. [Entrada y actualización](11-importacion-y-visibilidad.md) · [Diccionario de cabeceras](19-contrato-de-tablas-de-entrada.md).

## P01 · Sesión real de FM27

**PENDIENTE DE VERIFICACIÓN.** Para comprobar equivalencia con la información que realmente posee un mánager se necesitan una copia de partida con fecha y club conocidos y un exportado visible para el mismo mánager e instante. Emparejar por UID y comparar miembros, intervalos, personalidad, valor, sueldo y naturales en distintos estados de conocimiento. El éxito en datos sintéticos verifica el comportamiento del analizador, pero deja abierta esa comparación contra el juego. [Límites de evidencia](15-trazabilidad-y-lagunas.md).
