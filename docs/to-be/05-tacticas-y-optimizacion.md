# Tácticas y optimización

REQ-TAC-001 a 004 y REQ-ROLE-003. Una definición tiene edición FM27, ID/revisión, colección ordenada de puestos por fase, posición admitida, rol y fase del rol. Cada instancia guarda cuota IP, competición/lista aplicable, equipos, indisponibilidades y fijaciones (UID, fase, puesto). Duplicar o editar un escenario preserva el resultado base.

| Problema | Objetivo y restricciones | Resultado |
|---|---|---|
| Una fase | Maximizar lexicográficamente plazas A cubiertas, suma A cuantizada, plazas B cubiertas y suma B cuantizada, con un UID a lo sumo una vez. | A y B simultáneos, vacantes explícitas. Parámetros de aptitud posicional y bono natural con inicial 1. |
| Dos fases | Cadena de capacidad uno puesto IP → persona → puesto OOP. Maximizar cobertura y después total ponderado de fase. A se elige primero, B con los restantes. | Misma lista de titulares en ambas fases. Incluso con peso de fase cero hay que ser elegible en ella. |
| Ajuste de puentes | Conservar el grupo y bloqueos. Tolerancia inicial 0,1 para la suma del equipo. Priorizar preferencias laterales, distancia geométrica y suma cruda en el orden heredado. | Mostrar puesto inicial/final y desplazamiento. Aclarar si terminó por presupuesto de búsqueda. |
| Restricción manual | Primaria, posición, rol, disponibilidad, cupo e inscripción activos se evalúan antes de elegir. Fijaciones contradictorias devuelven puestos y causa. | El escenario anterior sigue disponible al cancelar o fallar el recálculo. |

En dos fases la nota individual conjunta es p·DWRS(IP)+(1−p)·DWRS(OOP). La utilidad de selección incorpora los factores posicionales y cuantización del original. La nota del rol se conserva en su escala. Para reproducibilidad se fijan orden de enumeración, redondeo, coste entero, orden de aristas, podas, límites de grupo y presupuesto de refinamiento. La garantía de asignación sin cupos adicionales no se extiende al refinamiento acotado. [Contrato matemático completo](../as-is/05-tacticas-y-optimizacion.md) · [Puestos y fases](../as-is/16-tablas-de-roles-y-tacticas.md)

**Operaciones.** Crear y alternar 4-3-3, 4-2-3-1 u otra táctica legal. Elegir rol por fase, fijar por arrastre o por selector de teclado, simular baja, comparar dos tácticas sobre el mismo corte y ver movimientos y huecos. Una nota no disponible carece de arista de selección. Si faltan datos de elegibilidad obligatoria, mostrar plaza pendiente o simular expresamente una hipótesis identificada. Un cambio de rol invalida sólo planes que lo consumen. Selección para el plan de entrenamiento y lista de posibles fichajes usan la misma definición de fase.

**Propuestas deportivas.** Pierna hábil conocida, rasgos observados, instrucciones de equipo e individuales pueden documentarse como condiciones cualitativas con evidencia. Señalar, por ejemplo, extremo izquierdo de pie derecho y regate suficiente para el criterio elegido. Se pospone un índice de sinergia o complementariedad hasta disponer de modelo y contraste. La simulación de fichajes usa el motor de XI con el mismo perfil activo en ambos escenarios. [Mercado](07-scouting-y-mercado.md)
