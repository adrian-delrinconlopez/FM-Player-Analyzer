# Recomendaciones y decisiones que siguen abiertas

### Qué probar primero

1. **La referencia del puesto debe viajar.** Desde AML se necesita saber a quién se reemplaza, en qué fases y con qué revisión. Probar el par contextual Leo/Sol y una tabla simétrica con la misma información. Elegir según la capacidad del participante de explicar el +11, el rango y el coste pendiente. El usuario puede preservar ese resultado con otra agrupación de pantallas. [D01 y D04](03-visual-reference-catalog.md) · [Q-16](../OPEN-QUESTIONS.md)
2. **Mostrar la población junto al número.** Un candidato de 180 minutos puede tener una tasa útil para inspección y muestra insuficiente para clasificación. Para buscar, tabla y dispersión enlazadas por jugador. Para estudiar forma, radar secundario con tabla. Probar con Cabo cambiando por separado la cohorte y el filtro de resultados. [Caso NV-26](../testing/02-casos-de-prueba.md) · [B06/B07/B09](02-product-benchmark.md)
3. **Probar la asignación por teclado antes de pulir el campo.** Un campo con dos fases aporta orientación cuando el usuario sigue a un mismo UID. Una lista de puestos permite descubrir por qué la fijación falla. Ambas representaciones pueden operar sobre la misma decisión del núcleo. [D02](03-visual-reference-catalog.md) · [PC-06](../roadmap/03-riesgos-y-pruebas-de-concepto.md)
4. **Separar el paso observado del hipotético.** Las dos cifras del sueldo y las dos fechas de un cierre se parecen visualmente. Mantener en el encabezado el rol de la cifra, su fecha, escala y unidad. En detalle, aportes y origen. Ensayar la distinción sin ayuda del color ni de un tooltip. [D03/D05](03-visual-reference-catalog.md) · [Sistema de diseño](06-design-system.md)

### Qué decisión falta, quién la resuelve y con qué evidencia

| Cuestión vigente | Recomendación concreta | Responsable y evidencia que permitiría decidir |
|---|---|---|
| Q-13 y Q-16 · alcance del guion | Probar refuerzo, desglose económico y rendimiento observado en los destinos actuales. El primer recorrido puede validarse antes de profundizar en las superficies secundarias. | Propietario. Acordar tareas, variantes y cobertura antes del HTML, registrar ratificación de cada cambio de alcance por separado. |
| Q-01 y Q-04 · nota parcial y permisos especiales | Comparar cómo se lee «Sin nota» con cobertura insuficiente. Ensayar cancelación y revocación de PA en Faro, con CA todavía apagada. | Propietario y análisis. Revisar Classic 55/32 y prueba de indistinguibilidad. PC-03 acredita visibilidad por campo, con independencia de la disposición visual. |
| Q-05 · castellano oficial | Usar claves estables, marcar términos pendientes y permitir etiquetas analíticas provisionales en el mock. | Revisión lingüística/propietario. Evidencia en la misma edición, idioma y compilación con nombre, abreviatura, grupo y orden. |
| Q-08 · cohorte | Mantener 450 min, N=20 y ±2 años como parámetros de ensayo editables, con ámbito visible. | Propietario deportivo. Repetir con dos poblaciones, distintos minutos y una banda estrecha. Comparar cuándo empieza a variar la decisión. |
| Q-10 y Q-12 · gráficos, tabla y recursos | Contrastar lista virtualizada, renderer y marcadores nativos con sus alternativas sólo al resolver la primera capacidad consumidora. | Arquitectura y recursos. PC-04/05/06/07/08 con .NET 10, tabla equivalente, selección, recuperación de memoria y casos de fotos homónimas. |
| Q-02/03/15 y Q-09 · captura y asistente | Mantener origen y capacidad visibles por campo. En chat simulado probar la comprensión del consentimiento y el enlace local. | Integración y propietario. PC-01/02/03/09, evidencia por build/carrera y verificación del flujo oficial de acceso. |
| Q-06 y Q-14 · derechos y propuestas pospuestas | Mantener por enlace las referencias sin permiso claro. Posponer calor espacial, perfiles predictivos y gestión nacional hasta reabrir su requisito. | Propietario con responsables de datos/derechos. Fuente, licencia y caso observable identificados para cada elemento. |

La cola de decisiones funcionales continúa en el [registro abierto](../OPEN-QUESTIONS.md). Búsqueda global, bandeja persistente, alternancia de fases y densidad por ventana son hipótesis reversibles de diseño. Un motor nuevo de similitud, una predicción de precio o convertir pizza en compromiso funcional requieren decisión de alcance.

### Cómo utilizar el pack en Spec Kit

Ejemplo con REQ-SQUAD-004. En **Specify**, redactar el resultado observable: desde el puesto de Leo se puede comparar el mismo rol en Sol, ver ambas fases y volver al referente. Identificar dependencia de Q-16. Conservar como caso límite una vacante o un titular sin nota. Adjuntar NV-25 y EQ-036 y enlazar a la fila correspondiente de la [matriz](13-traceability-to-to-be.md).

En **Plan**, elegir par anclado o tabla transpuesta según el mock aprobado, especificar proyección autorizada, regla de comparabilidad, invalidación, foco y prueba de teclado. En **Tasks**, dividir por resultados: obtener el par con escala y revisión, presentar el delta o causa de ausencia, restaurar selección por ID y verificar a 150 %. **Analyze** coteja los REQ, aceptación y estados de datos. **Implement** y **Converge** comprueban el resultado y enlazan commit/prueba antes de marcar IMPLEMENTADA. El nombre de un control o el número de destinos no sustituye a la prueba. [Flujo local](../roadmap/04-preparacion-spec-kit.md)
