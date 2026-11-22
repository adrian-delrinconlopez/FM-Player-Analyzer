# Del requisito a la prueba de diseño

**66 de 66 requisitos documentados**, 30 F, 29 C, 4 A y 3 P en el catálogo vigente. Hay 42 contratos de origen y 24 requisitos nuevos o transversales. F/C/A/P reproduce la prioridad propuesta de Fase B. Los estados P y Q siguen sus puertas de decisión. Esta matriz acredita cobertura documental. El resultado de cada prueba se registra cuando se ejecute en su entorno.

El requisito y el tratamiento proceden del [catálogo funcional](../to-be/02-catalogo-funcional.md) y de la [matriz AS-IS → TO-BE](../to-be/18-trazabilidad-as-is-to-be.md). V identifica [superficies posibles](07-screen-design-candidates.md) y C [piezas candidatas](08-component-candidates.md). Las barras entre IDs indican posibles destinos, reutilizaciones o alternativas. La columna de referencia apunta al [benchmark B](02-product-benchmark.md), [atlas R/D/E](03-visual-reference-catalog.md), [decisión DP](../PRODUCT-DECISIONS.md), [cuestión Q](../OPEN-QUESTIONS.md) o [PoC PC](../roadmap/03-riesgos-y-pruebas-de-concepto.md). En una capacidad de infraestructura la referencia principal es el contrato interno. El observable U remite a los [ensayos de UX](11-ux-validation-criteria.md). EQ y NV a los [casos heredados](../testing/01-equivalencia-funcional.md) y los [casos nuevos](../testing/02-casos-de-prueba.md).

| Requisito | Hito | Antecedente y tratamiento | Necesidad UX | Superficie candidata | Piezas posibles | Referencias | Criterio UX | Caso TO-BE |
|---|---|---|---|---|---|---|---|---|
| REQ-DATA-001 | F | FPA-FUNC-001 · AMPLIADA | Distinguir valor medido, rango y hueco en el corte elegido. | V06/V07 | C06/C14 | R05/D05 | U02 | EQ-001 |
| REQ-DATA-002 | F | FPA-FUNC-002 · MODIFICADA | Resolver dos homónimos antes de reunir sus históricos. | V02/V06 | C01/C18 | R05, DP-03 | U01 | EQ-002 |
| REQ-DATA-003 | F | FPA-FUNC-003 · MODIFICADA | Reconocer divisa, periodicidad y texto de importe ilegible. | V06/V11 | C10 | B10/D01 | U05 | EQ-003 |
| REQ-ROLE-001 | F | FPA-FUNC-004 · CONSERVADA | Ver qué roles son elegibles en cada posición y fase. | V08/V09 | C07/C08 | R07/R09 | U06 | EQ-004 |
| REQ-ROLE-002 | F | FPA-FUNC-005 · MODIFICADA | Identificar la observación que explica una fortaleza, también en portería. | V06/V08 | C06/C07 | R05/R09 | U02 | EQ-005 |
| REQ-RATING-001 | F | FPA-FUNC-006 · MODIFICADA | Entender por qué Classic puede quedar sin nota deportiva. | V06/V08 | C07/C14 | R09, Q-01 | U02 | EQ-006 |
| REQ-RATING-002 | F | FPA-FUNC-007 · CONSERVADA | Leer peso observado y cómo se valoró cada entero de un rango. | V08/V17 | C06/C07 | R09/D05 | U02 | EQ-007 |
| REQ-RATING-003 | F | FPA-FUNC-008 · CONSERVADA | Separar base de portero y diferencial de marcas por fase. | V08/V06 | C07 | R09 | U02 | EQ-008 |
| REQ-TAC-001 | F | FPA-FUNC-009 · CONSERVADA | Leer quién ocupa A/B y por qué queda una plaza vacía en una fase. | V10/V09 | C08/C09 | R07 | U06 | EQ-009 |
| REQ-TAC-002 | F | FPA-FUNC-010 · CONSERVADA | Seguir los mismos titulares entre fases al variar la cuota IP. | V09/V10 | C08/C07 | R07/D02 | U06 | EQ-010 |
| REQ-TAC-003 | C | FPA-FUNC-011 · CONSERVADA | Medir pérdida tras fijar o quitar un jugador en un escenario. | V10/V11 | C08/C09 | R07/D02 | U06 | EQ-011 |
| REQ-SQUAD-001 | C | FPA-FUNC-012 · CONSERVADA | Distinguir un suplente real de uno reclamado por varios puestos. | V10/V03 | C09/C03 | R04/R07 | U06 | EQ-012 |
| REQ-SQUAD-002 | F | FPA-FUNC-013 · AMPLIADA | Pasar de la causa de una carencia al puesto y su referente. | V01/V10/V05 | C09/C04 | B01/D04 | U03 | EQ-013, NV-25 |
| REQ-SQUAD-003 | C | FPA-FUNC-014 · AMPLIADA | Comparar dos tácticas en el mismo corte, con media y profundidad. | V10/V09 | C09/C02 | R04/R07 | U06 | EQ-014 |
| REQ-NAT-001 | P | FPA-FUNC-015 · POSPUESTA | Al reabrirse, revisar nacionalidad visible, elegibilidad y cupo de porteros. | V16/V10 (futuro) | C16/C09 | B03, Q-14 | U13 | EQ-015 |
| REQ-SCOUT-001 | F | FPA-FUNC-016 · MODIFICADA | Separar candidatos seguros y posibles en rol o puente. | V05/V09 | C04/C03 | B01/B05 | U03/U04 | EQ-016, NV-25 |
| REQ-SCOUT-002 | C | FPA-FUNC-017 · CONSERVADA | Identificar la ventaja sobre el mejor propio por rol, con coste comparable. | V05/V07 | C05/C10 | B01/D01 | U03/U05 | EQ-017 |
| REQ-MARKET-001 | C | FPA-FUNC-018 · AMPLIADA | Examinar efecto de un fichaje y recolocaciones de A y B. | V11/V10 | C10/C09 | D04/R07 | U05/U06 | EQ-018, NV-25 |
| REQ-MARKET-002 | C | FPA-FUNC-019 · CONSERVADA | Revisar la propuesta local de cesión o venta de un excedente. | V11/V04 | C10/C11 | B03/B10 | U05/U07 | EQ-019 |
| REQ-DEV-001 | C | FPA-FUNC-020 · MODIFICADA | Entender qué factores de talento provienen de observación o sustitución neutral. | V12/V04 | C07/C14 | R08 | U02/U07 | EQ-020 |
| REQ-DEV-002 | C | FPA-FUNC-021 · MODIFICADA | Elegir atributo por ganancia marginal y reconocer la ausencia de edad. | V12/V08 | C07/C06 | R08/B04 | U07 | EQ-021 |
| REQ-DEV-003 | C | FPA-FUNC-022 · CONSERVADA | Consultar método entrenable y contribuciones para el rol objetivo. | V12/V06 | C07/C14 | R05/R08 | U07 | EQ-022 |
| REQ-HIST-001 | C | FPA-FUNC-023 · MODIFICADA | Distinguir corte anterior, rama nueva y presente al retroceder de fecha. | V15/V02 | C11/C02 | D03, DP-03 | U01/U07 | EQ-023 |
| REQ-DATA-004 | F | FPA-FUNC-024 · MODIFICADA | Revisar mapa de cabeceras y rango antes de confirmar importación visible. | V02/V03 | C18/C03 | B03/D05 | U01/U12 | EQ-024 |
| REQ-FM-001 | F | FPA-FUNC-025 · SUSTITUIDA | Identificar la fuente autorizada de un corte y su disponibilidad. | V02/V06 | C02/C18 | DP-04, D05 | U01/U09 | EQ-025 |
| REQ-FM-002 | F | FPA-FUNC-026 · SUSTITUIDA | Reconocer progreso y resultado de reintentar un lote confirmado. | V02 | C18/C02 | DP-03/DP-04 | U12 | EQ-026 |
| REQ-HIST-002 | C | FPA-FUNC-027 · AMPLIADA | Separar reajuste matemático y evolución deportiva original. | V15/V12 | C11/C07 | R08/D03 | U07 | EQ-027 |
| REQ-HIST-003 | C | FPA-FUNC-028 · MODIFICADA | Revisar antigüedad y evidencia de una retirada inferida. | V15/V04 | C11/C14 | R08, DP-05 | U07 | EQ-028 |
| REQ-REG-001 | C | FPA-FUNC-029 · AMPLIADA | Reconocer cupo incierto y procedencia de regla por competición. | V16/V10 | C16/C14 | B03, Q-11 | U06/U13 | EQ-029 |
| REQ-REG-002 | C | FPA-FUNC-030 · MODIFICADA | Leer plazas y subtotal económico cuando falta sueldo. | V16/V11 | C16/C10 | B03/B10 | U05/U06 | EQ-030 |
| REQ-REG-003 | C | FPA-FUNC-031 · CONSERVADA | Revisar cómo cambia el XI al aplicar o vaciar la lista local. | V16/V10 | C16/C08 | B03/R07 | U06 | EQ-031 |
| REQ-SAVE-001 | F | FPA-FUNC-032 · AMPLIADA | Copiar ajustes de otra carrera conservando ámbito e identidad. | V02/V19 | C02/C15 | DP-03 | U01/U12 | EQ-032 |
| REQ-PROFILE-001 | F | FPA-FUNC-033 · AMPLIADA | Distinguir duplicado, revisión y activación de un perfil. | V17/V08 | C17/C07 | B04, FR-07 | U02/U13 | EQ-033 |
| REQ-TAC-004 | C | FPA-FUNC-034 · CONSERVADA | Validar una táctica importada y alternar definiciones conservando la activa. | V09/V19 | C08/C18 | R07/D02 | U06/U13 | EQ-034 |
| REQ-ROLE-003 | C | FPA-FUNC-035 · CONSERVADA | Distinguir completar asignaciones manuales y restablecer las de catálogo. | V08/V09 | C07/C08 | R09/R07 | U06 | EQ-035 |
| REQ-SQUAD-004 | F | FPA-FUNC-036 · MODIFICADA | Comparar 2–5 personas, cobertura del área y anotación manual. | V07/V06 | C05/C06 | R06/D01 | U02/U03 | EQ-036, NV-25 |
| REQ-NAT-002 | P | FPA-FUNC-037 · POSPUESTA | Al reabrirse, distinguir comparar candidatos nacionales de convocarlos. | V07/V16 (futuro) | C05/C16 | B03, Q-14 | U13 | EQ-037 |
| REQ-EXPORT-001 | F | FPA-FUNC-038 · MODIFICADA | Saber qué filas, columnas y orden se exportan en Excel o CSV. | V03/V05 | C03/C18 | B05/R02, DP-12 | U04/U09 | EQ-038 |
| REQ-DEV-004 | C | FPA-FUNC-039 · CONSERVADA | Comprobar límites de una banda antes de activar su revisión. | V12/V17 | C17/C07 | R08 | U07/U13 | EQ-039 |
| REQ-SQUAD-005 | C | FPA-FUNC-040 · MODIFICADA | Revisar una salida sólo con evidencia de cobertura completa. | V02/V03 | C18/C03 | B03/D05 | U01/U12 | EQ-040 |
| REQ-FM-003 | P | FPA-FUNC-041 · POSPUESTA | Si se reabre, conocer procedencia y disponibilidad de cinco archivos .fmf. | V02/V19 (futuro) | C18 | Q-14, B03 | U13 | EQ-041 |
| REQ-PROFILE-002 | C | FPA-FUNC-042 · CONSERVADA | Detectar punto de curva inválido y deshacer sin afectar la activa. | V17 | C17/C07 | B03/B04 | U02/U13 | EQ-042 |
| REQ-SCOPE-001 | F | Transversal/nueva | Completar análisis local de FM27 con el juego o la red desconectados. | V02/V03/V10 | C02/C18 | DP-01/DP-06 | U01/U12 | NV-01 |
| REQ-SAVE-002 | F | Transversal/nueva | Identificar carrera y mánager antes de sincronizar dos UID iguales. | V02 | C02/C01 | DP-03 | U01 | NV-02 |
| REQ-VIS-001 | F | Transversal/nueva | Obtener la misma proyección autorizada al ordenar, exportar o explicar. | Transversal, V03/V05/V18 | C03/C04/C14 | DP-05/D05 | U09 | NV-03, NV-26 |
| REQ-VIS-002 | F | Transversal/nueva | Entender aviso, alcance efectivo y revocación de CA/PA. | V19/V07 | C15/C02 | DP-05, Q-04 | U09 | NV-04 |
| REQ-FM-004 | F | Transversal/nueva | Entender por qué una vía no actualiza sus datos y otras siguen disponibles. | V02/V01 | C18/C02 | DP-04, Q-15 | U12 | NV-05 |
| REQ-PREF-001 | C | Transversal/nueva | Distinguir orden por filosofía del DWRS y el XI de referencia. | V19/V05 | C15/C07 | R09, DP-11 | U02/U03 | NV-06 |
| REQ-RATING-004 | C | Transversal/nueva | Leer escala 1–20 del perfil alternativo por posición. | V08/V17 | C07/C17 | B04/E09 | U02 | NV-07 |
| REQ-SCOUT-003 | C | Transversal/nueva | Guardar criterio o lista, revalidar y recuperar contexto. | V05/V07 | C04/C05 | B01/B05 | U04/U09 | NV-08 |
| REQ-MARKET-003 | C | Transversal/nueva | Adoptar una oferta explícita y distinguirla de sueldo y valor FM. | V11/V07 | C10/C05 | B10/D01 | U05 | NV-09, NV-27 |
| REQ-STAFF-001 | A | Transversal/nueva | Valorar funciones de personal con categoría y escala conocidas. | V13/V03 | C03/C07 | B04/E09 | U13 | NV-10 |
| REQ-DEV-005 | C | Transversal/nueva | Seguir al mismo cedido entre destinos y reconocer última observación. | V04/V12 | C01/C11 | R08/D03 | U07 | NV-11 |
| REQ-STAT-001 | C | Transversal/nueva | Distinguir total, tasa, muestra y ausencia de denominador. | V14/V07 | C12/C03 | B07/B09 | U08 | NV-12 |
| REQ-STAT-002 | A | Transversal/nueva | Seleccionar rendimiento frente a coste en ojeo conservando cohorte. | V14/V05/V07 | C12/C04 | B01/B06/E05/D06 | U08/U09 | NV-13, NV-26 |
| REQ-HIST-004 | C | Transversal/nueva | Explicar diferencia de cierres separando cambios de población. | V15/V03 | C11/C02 | B02/D03 | U07 | NV-14 |
| REQ-AI-001 | A | Transversal/nueva | Elegir contexto, consentimiento y retorno al análisis local. | V18/V19 | C13/C15 | DP-06 | U09/U10 | NV-15 |
| REQ-AI-002 | A | Transversal/nueva | Seguir la recomendación hasta la evidencia local de la misma revisión. | V18/V08 | C14/C07 | B05, DP-06 | U10 | NV-16 |
| REQ-ART-001 | C | Transversal/nueva | Reconocer persona o club aun con retrato ausente o ambiguo. | V06/V19 | C01/C18 | R03, DP-07 | U11/U12 | NV-17 |
| REQ-I18N-001 | F | Transversal/nueva | Leer la ficha en orden oficial y conservar identidad al cambiar cultura. | V06/V19 | C06/C15 | DP-08, Q-05 | U02/U11 | NV-18 |
| REQ-UI-001 | F | Transversal/nueva | Completar el recorrido con teclado, texto ampliado y alto contraste. | Transversal, V03/V09 | C03/C08 | E10/D05 | U11 | NV-19 |
| REQ-UX-001 | F | Transversal/nueva | Ensayar decisiones con fixture local tras revisión del propietario. | Transversal, V01/V07 | C02/C05 | D01/D04, DP-09 | U03/U11 | NV-20 |
| REQ-PERF-001 | F | Transversal/nueva | Cambiar filtro o selección con memoria acotada y salida vigente. | V05/V03 | C03/C18 | R02, PC-05 | U12 | NV-21 |
| REQ-TECH-001 | F | Transversal/nueva | Reproducir las pruebas de presentación en el mismo escritorio compilado. | Transversal, sin vista propia | C03 como muestra de prueba | DP-02/DP-10, PC-04/10 | U11/U12 | NV-22 |
| REQ-OPS-001 | C | Transversal/nueva | Distinguir copia, restauración y fallo sin perder último estado confirmado. | V02/V19 | C18/C02 | DP-03 | U12 | NV-23 |
| REQ-TERM-001 | F | Transversal/nueva | Comprobar oficialidad de cada etiqueta y mantener ID, fase y orden. | V06/V08/V19 | C06/C07 | DP-08, Q-05 | U02/U11 | NV-24 |

Para proponer otra vista, mantener el ID del requisito, el caso y la frontera de datos. Añadir la alternativa visual y su evidencia en el [registro](14-design-decision-log.md). Las filas de exportación, visibilidad y localización también se aplican a las vistas que consumen el resultado, aunque sólo aparezcan ejemplos representativos.
