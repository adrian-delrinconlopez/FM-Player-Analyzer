# Alcance y requisitos globales

El usuario dirige un club en FM27. Al abrir una carrera ve su club, fecha del juego, origen y última sincronización válida. Puede pasar de una alerta a sus evidencias, simular una decisión y volver a la plantilla real. Las decisiones deportivas registradas son locales. Una carrera admite varios clubes dirigidos a lo largo del tiempo y mantiene el contexto temporal del cambio.

| Pregunta | Resultado que admite la aplicación | Límite de interpretación |
|---|---|---|
| ¿Qué nivel tiene? | Vector de atributos conocidos y puntuación por posición con perfil identificado | La calidad general depende de la referencia elegida. CA es un dato oculto opcional. |
| ¿Dónde encaja? | DWRS por rol/fase con cobertura, contribuciones y fecha | Una normalizada del rol no mide probabilidad de victoria. |
| ¿Mejora el equipo? | Diferencia entre dos alineaciones comparables con las mismas restricciones | Una vacante y una recolocación se distinguen de la mejora individual. |
| ¿Responde a mi filosofía? | Reglas activas, coincidencias e incógnitas | Sólo influye cuando se aplica explícitamente el criterio de preferencia. |
| ¿Conviene económicamente? | Precio, coste salarial, ingresos y saldo por horizonte | Sin precio, moneda o periodo comparables la conclusión queda indeterminada. |

**Alcance de versión.** F, C, A y P ordenan el roadmap global por primera funcional, primera completa, ampliaciones y evolución posterior. Para la primera entrega del producto rige la [matriz funcional v1](../roadmap/06-alcance-v1.md) de DP-15/16. Incluye la proyección potencial, el diagnóstico de la táctica y la simulación de refuerzos. La disponibilidad con datos reales depende de la evidencia de las PoC.

**Gobierno.** CONFIRMADA identifica requisitos del propietario; PROPUESTA señala la alternativa recomendada; PENDIENTE DE VALIDACIÓN exige evidencia; POSPUESTA sitúa el trabajo fuera de la entrega indicada; DESCARTADA documenta una exclusión. El [catálogo](02-catalogo-funcional.md) expresa obligaciones verificables para cuando se apruebe su entrega. Los tratamientos de la [matriz](18-trazabilidad-as-is-to-be.md) clasifican el efecto sobre el legado; no conceden aprobación al diseño. [Decisiones](../PRODUCT-DECISIONS.md) · [Cuestiones abiertas](../OPEN-QUESTIONS.md)
