# Alcance y requisitos globales

El usuario dirige un club en FM27. Al abrir una carrera ve su club, fecha del juego, origen y última sincronización válida. Puede pasar de una alerta a sus evidencias, simular una decisión y volver a la plantilla real. Las decisiones deportivas registradas son locales. Una carrera admite varios clubes dirigidos a lo largo del tiempo y mantiene el contexto temporal del cambio.

| Pregunta | Resultado que admite la aplicación | Límite de interpretación |
|---|---|---|
| ¿Qué nivel tiene? | Vector de atributos conocidos y puntuación por posición con perfil identificado | La calidad general depende de la referencia elegida. CA es un dato oculto opcional. |
| ¿Dónde encaja? | DWRS por rol/fase con cobertura, contribuciones y fecha | Una normalizada del rol no mide probabilidad de victoria. |
| ¿Mejora el equipo? | Diferencia entre dos alineaciones comparables con las mismas restricciones | Una vacante y una recolocación se distinguen de la mejora individual. |
| ¿Responde a mi filosofía? | Reglas activas, coincidencias e incógnitas | Sólo influye cuando se aplica explícitamente el criterio de preferencia. |
| ¿Conviene económicamente? | Precio, coste salarial, ingresos y saldo por horizonte | Sin precio, moneda o periodo comparables la conclusión queda indeterminada. |

**Alcance de versión.** F es primera versión funcional, C primera completa, A ampliaciones y P evolución posterior. La prioridad de cada ID es una PROPUESTA. F permite conectar o importar lo visible, identificar la partida, valorar, comparar y planificar el XI. C cierra el ciclo de temporada. Ninguna capacidad dependiente de datos reales pasa a disponible por el mero hecho de tener una pantalla diseñada.

**Gobierno.** CONFIRMADA identifica requisitos del propietario; PROPUESTA señala la alternativa recomendada; PENDIENTE DE VALIDACIÓN exige evidencia; POSPUESTA sitúa el trabajo fuera de la entrega indicada; DESCARTADA documenta una exclusión. El [catálogo](02-catalogo-funcional.md) expresa obligaciones verificables para cuando se apruebe su entrega. Los tratamientos de la [matriz](18-trazabilidad-as-is-to-be.md) clasifican el efecto sobre el legado; no conceden aprobación al diseño. [Decisiones](../PRODUCT-DECISIONS.md) · [Cuestiones abiertas](../OPEN-QUESTIONS.md)
