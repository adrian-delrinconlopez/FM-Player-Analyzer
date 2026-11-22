# FM27 Analyzer · UX/UI Design Reference Pack

**Fase B · 22 de noviembre de 2026 · Investigación y propuestas para revisión del propietario.** Este cuaderno acompaña al [TO-BE](../to-be/00-indice-y-vision.md) y a las [especificaciones de UX existentes](../ux-ui/00-vision-visual.md). Permite partir de una tarea deportiva, consultar evidencia visual, elegir entre representaciones y llevar su criterio de aceptación a GitHub Spec Kit.

**Ejemplo.** El titular ficticio Leo Nube tiene un encaje de 58 y Sol Arce de 69 en el mismo puente de fases. El usuario puede justificar los +11 puntos con valores observables, aunque el precio de compra siga indeterminado. La tabla, la ficha y el escenario deben conservar esa distinción. La composición propuesta puede cambiar. [Ver el par y sus referencias](03-visual-reference-catalog.md) · [Recorrer el escenario](10-html-prototype-brief.md)

**Para revisar el estilo**, abre el [tablero de alternativas visuales](15-comparativas-visuales.md). Cada familia reúne ejemplos, señala la diferencia de presentación y propone una tarea para elegir entre ellos.

## Cuatro niveles de autoridad

| Nivel | Qué se conserva y quién decide |
|---|---|
| A · Capacidades funcionales | Los requisitos del catálogo expresan resultados a preservar para su entrega. Las decisiones **confirmadas** proceden del propietario (DP-01…12). Prioridades F/C/A/P, ampliaciones Q-16, términos y demás cuestiones pendientes conservan el estado de sus registros. [Decisiones](../PRODUCT-DECISIONS.md) · [Cuestiones](../OPEN-QUESTIONS.md) |
| B · Propuestas de experiencia | Las superficies V, recorridos y visualizaciones son hipótesis de cómo resolver esas capacidades. Se pueden fusionar, dividir, adaptar o sustituir documentando la justificación y el criterio conservado. |
| C · Candidatos a reutilización | Cada C expresa un problema compartido. Uso repetido, comportamiento común, accesibilidad y coste de mantenimiento justifican una extracción durante la planificación. |
| D · Soluciones validadas e implementadas | VALIDADA exige indicar evidencia, ámbito y aprobación o prueba. IMPLEMENTADA identifica una solución presente en la aplicación con pruebas. El pack entrega investigación y candidatos. Ensayos del mock, PoC y validación de usuario siguen pendientes. |

**Estados de propuesta** · CANDIDATA, RECOMENDADA, VALIDADA, IMPLEMENTADA, SUSTITUIDA y DESCARTADA. Se mantienen separados de los estados de requisitos, antecedentes y decisiones del propietario. El [historial](14-design-decision-log.md) explica cómo actualizar una propuesta sin perder la evidencia previa.

## Ruta de lectura

| Si necesitas… | Documento | Qué encontrarás |
|---|---|---|
| Conocer el método y los límites de acceso | [01 · Metodología](01-research-methodology.md) | Inventario previo de necesidades, fuentes, confianza y herramientas. |
| Elegir qué producto estudiar | [02 · Benchmark](02-product-benchmark.md) | Diez referencias con observación pública, limitación y aplicación propuesta. |
| Abrir una captura o una lámina propia | [03 · Atlas visual](03-visual-reference-catalog.md) · [Manifiesto](references/manifest.json) | 25 imágenes de terceros o del antecedente, 7 láminas propias y 18 referencias sólo enlazadas. |
| Saber qué información presentar | [04 · Problemas de visualización](04-visualization-catalog.md) | Decisión, entrada, alternativa, interacción e incertidumbre por problema. |
| Enlazar decisiones y volver al origen | [05 · Arquitectura de información](05-information-architecture.md) | Ocho recorridos y qué estado viaja o se invalida. |
| Diseñar otra superficie coherente | [06 · Sistema de diseño](06-design-system.md) | Tokens con contraste medido, densidad, estados y adaptación a Windows. |
| Planificar una experiencia | [07 · Pantallas candidatas](07-screen-design-candidates.md) | Ocho áreas prioritarias en profundidad y cobertura de V01–V19. |
| Identificar comportamiento repetido | [08 · Componentes candidatos](08-component-candidates.md) | Dieciocho problemas con opción nativa o composición más sencilla. |
| Contrastar una elección de controles | [09 · Factibilidad WinUI 3](09-winui3-feasibility.md) | Metadatos inspeccionados, controles candidatos y pruebas de rendimiento y accesibilidad. |
| Preparar el prototipo tras la revisión | [10 · Brief HTML](10-html-prototype-brief.md) | Sesiones con datos de Faro, Bruma y Cabo, variantes y resultados esperados. |
| Comprobar una propuesta | [11 · Validación UX](11-ux-validation-criteria.md) | Trece criterios relacionados con EQ, NV, FR, V-A y PoC. |
| Llevar un recorrido a Spec Kit | [12 · Recomendaciones y decisiones pendientes](12-recommendations-and-open-questions.md) | Prioridades de ensayo, responsables y ejemplo de Specify → Converge. |
| Seguir un requisito | [13 · Matriz funcional](13-traceability-to-to-be.md) | 66 requisitos, hito, 42 antecedentes y casos, usuarios, pantallas, componentes y referencias. |
| Saber qué se propuso y por qué | [14 · Historial](14-design-decision-log.md) | Alternativas, madurez, procedimiento de cambio y revisión constitucional 0.3.0 candidata. |
| Revisar dos o tres presentaciones de una tarea | [15 · Tablero de alternativas visuales](15-comparativas-visuales.md) | Comparación por familia con imágenes, preguntas para el mock y carencias de evidencia. |

## Verificación repetible

Ejecutar en la raíz del repositorio con PowerShell, Git y Python con Pillow.

```powershell
python docs/design/verify_pack.py
git diff --check c561252 HEAD
git diff --exit-code c561252 HEAD -- docs/as-is old_version docs/PRODUCT-DECISIONS.md
```

El [verificador](verify_pack.py) coteja requisitos, hitos, tratamientos y casos con las fuentes, resuelve IDs de diseño y enlaces a archivos del pack, abre las imágenes y contrasta dimensiones y SHA-256. Esta edición incluye 66 requisitos, 42 antecedentes, 19 superficies de referencia, 18 piezas candidatas, 50 entradas del manifiesto y 32 archivos de imagen únicos. La revisión visual se hizo abriendo los archivos en hojas de contacto. Las referencias web tienen su alcance de acceso indicado en el benchmark y el manifiesto. [Reproducción de la consulta al SDK](09-winui3-feasibility.md)

En el cierre de la primera edición se resolvieron 152 vínculos locales del pack y otros 89 de los documentos de entrada revisados. En Faro se reprodujeron el delta de +11, el intervalo de +1 a +3 y el saldo hipotético de 2 M€. En Cabo se reprodujeron 1,1 goles/90, P47,9, los 13 resultados del filtro sobre N=24 y la tasa provisional de 0,5. La diferencia frente al commit de partida fue vacía para el AS-IS, el árbol histórico y las decisiones confirmadas.

**Estado de entrega.** Investigación, propuestas, matriz y revisión del borrador terminadas. Quedan para las fases posteriores la revisión del propietario, la implementación y prueba del mock autorizado, las PoC nativas, las mediciones sobre FM27, el cotejo lingüístico por build y la validación con usuarios. La auditoría documental y los resultados sintéticos tienen ese alcance.
