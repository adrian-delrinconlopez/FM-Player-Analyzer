# Estrategia de validación

La evidencia se etiqueta como observada en el original (G), ejemplo manual de contrato (M), caso diseñado para el nuevo producto o ejecución real documentada. El registro histórico informa 52 suites aprobadas, con dos casos opcionales de partida real omitidos. La nueva aplicación tendrá su propio registro de ejecución. [Registro original](../as-is/21-ejecucion-de-las-pruebas.md)

| Clase | Referencia | Evidencia exigida para entregar |
|---|---|---|
| Original conservada | Entrada, catálogo FM27, perfil exacto, orden y tolerancia del AS-IS | Resultado equivalente en los casos verificados, con asignación y componentes cuando proceda. |
| Modificada, ampliada o sustituida | Salida previa y delta justificado en la matriz | Dos expectativas cuando cambie el resultado; equivalencia en el subconjunto común cuando se amplíe. |
| Nueva | Fixture determinista, acción, resultado y caso de error | Prueba de dominio, extremo, autorización y recorrido de usuario. |
| Pospuesta/eliminada | Tratamiento y dependencia identificados | Retener caso futuro o comprobar rechazo de edición/fuente excluida. |

Pruebas puras sin UI; contratos entre procesos con fuente simulada; integración SQLite en directorio temporal; UI con datos controlados; banco reproducible de rendimiento y campaña real opcional por build. Separar benchmarks de pruebas de corrección. Inyectar fecha, orden y semilla. Una prueba de cálculo produce la misma cifra desde núcleo, tabla, exportación, ficha y herramienta de explicación.

Por bloque registrar commit, revisión de perfiles, datos de prueba, comandos, ambiente, aprobados, fallidos, omitidos y limitaciones. Usar copia de prueba y lectura desde el plugin para campañas de FM. La integración continua sin juego ejecuta el conjunto sintético. [Equivalencia](01-equivalencia-funcional.md) · [Casos nuevos](02-casos-de-prueba.md)
