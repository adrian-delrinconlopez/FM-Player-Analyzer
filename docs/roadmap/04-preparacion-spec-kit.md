# Preparación para GitHub Spec Kit

El propietario puede ejecutar los pasos manualmente con Codex o GitHub Copilot. El repositorio conserva las decisiones y los casos de aceptación entre sesiones. Se fija la revisión de Spec Kit al preparar el primer bloque; la sintaxis de comandos puede depender de su versión/integración. No se inicializa Spec Kit en Fase B. [Flujo oficial de GitHub Spec Kit](https://github.com/github/spec-kit)

| Etapa | Insumos de esta fase | Salida a revisar antes de avanzar |
|---|---|---|
| Constitution | Decisiones DP, política de visibilidad, equivalencia, castellano y alcance exclusivo FM27 | Principios verificables y proceso de cambio de decisión. |
| Specify | Un recorrido del catálogo y sus REQ, frontera funcional y estado de evidencia | Historia observable, escenario principal, error, fuera de alcance e IDs de aceptación. |
| Clarify | Cuestiones abiertas y ADR que cambien el resultado de ese bloque | Respuesta del propietario o elección documentada como propuesta pendiente; separar trabajo independiente. |
| Plan | Arquitectura, ADR, versiones acreditadas y resultados PC pertinentes | Dependencias concretas, contrato de datos, casos extremos, migración/invalidez y plan de pruebas. |
| Tasks | Plan aprobado y casos EQ/NV/FR | Tareas pequeñas ordenadas por dependencia, archivos previstos, pruebas, salida y criterio de cierre. |
| Analyze | Constitución, historia, plan, tareas y matriz | Detectar requisitos sin caso, diferencias de escala/permiso, dependencias no validadas y tareas sin objetivo. Resolver hallazgos. |
| Implement | Una tarea acotada, fixture autorizada y rama comprobada | Commit coherente con implementación y sus pruebas, registro de comando, salida y omisiones. |
| Convergencia | Resultados numéricos, integración/UI, matriz y documentos afectados | Verificar extremo a extremo, actualizar estado y cuestiones. El propietario acepta el entregable o registra diferencias. |

**Ejemplo de corte para más adelante.** «REQ-RATING-002, evaluar cobertura de un rol FM27 en una ficha autorizada». Entradas: dos atributos de amplitud 7/3, perfil inmutable. Salidas: caso FR-01 y dos justificaciones de cobertura. Excluir UI y captura real. El agente que integre la ficha reutiliza el caso sin reescribir la fórmula. Evitar que una sola tarea abarque captura, cálculo y UI sin un contrato verificable entre ellas.

El [AGENTS.md](../../AGENTS.md) es la guía persistente común. Las instrucciones de Copilot enlazan al mismo contrato. Para automatización posterior, comprobar IDs y enlaces, oráculos, pruebas de migración, UIA y presupuestos por bloque; conservar una revisión humana de cambios de alcance, licencias, criterios deportivos y publicación.
