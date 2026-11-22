<!-- Impacto de sincronización: plantilla inicial → borrador 0.1.0. Principios I–V. Las plantillas oficiales conservan su contenido. Revisión y ratificación a cargo del propietario. -->

# Constitución de FM27 Analyzer

**Borrador 0.1.0 para discusión · 22 de noviembre de 2026 · ratificación pendiente.**

Esta constitución guía la redacción de especificaciones, planes y tareas. Las [decisiones del propietario](../../docs/PRODUCT-DECISIONS.md) fijan el alcance. Las elecciones pendientes están en el [registro de cuestiones](../../docs/OPEN-QUESTIONS.md). La aprobación de este texto corresponde al propietario.

## I. Un contrato observable por capacidad

Cada especificación identifica los requisitos, el recorrido del usuario, la entrada, el resultado observable y al menos una condición límite relevante. Si la capacidad procede del AS-IS 2.3.1, enlaza su tratamiento y su caso de equivalencia FM27. Si cambia una fórmula, documenta expectativa anterior, expectativa nueva, motivo y repercusión histórica. La edición admitida es FM27. Las especificaciones y la primera interfaz se redactan en castellano.

## II. La autorización forma parte del dato

La observación identifica carrera, perspectiva, fecha de juego, fuente, unidad y estado de conocimiento. Una carga parcial conserva la distinción entre un campo ausente y uno observado como desconocido. Sólo se publican lotes atribuibles a la carrera activa. El backend aplica el permiso antes de filtrar, ordenar, calcular, exportar o seleccionar contexto para IA. CA y PA empiezan deshabilitados. Ante un cambio de carrera o de permiso, una respuesta en curso sólo se publica si su contexto sigue vigente. La aceptación de un flujo con datos incluye un caso de aislamiento o revocación cuando corresponda.

## III. Cálculo deportivo explicable y reproducible

El núcleo determinista recibe entradas autorizadas y devuelve resultados con escala, cobertura y revisión. El mismo cálculo alimenta la ficha, el XI y la explicación. Cada plan identifica qué oráculo, fixture o invariante permitirá detectar una regresión. Los históricos conservan la revisión de datos y de perfil que produjo cada resultado. Un cambio de pesos puede generar una serie recalculada identificable. El asistente ChatGPT es opcional y explica resultados locales mediante herramientas acotadas.

## IV. Arquitectura proporcional a la decisión

El destino es Windows 11 x64 con C#, .NET 10 LTS, WinUI 3/XAML y SQLite local. La solución .slnx y los proyectos serán comunes a CLI, VS Code y Visual Studio Enterprise. El adaptador de lectura BepInEx tiene su propio contrato de compatibilidad. Un plan que añada una dependencia o una copia importante de datos explica su necesidad, alternativa, coste y prueba. En consultas frecuentes o sobre más de 100.000 jugadores, declara volumen, métrica y procedimiento de medida. Una capacidad observada en otro proyecto sigue pendiente hasta contrastarla en nuestro entorno.

## V. Entregas que el propietario pueda revisar

Mantener separadas especificación, planificación y ejecución. El prototipo HTML local con datos ficticios sigue a la revisión de Fase B y su aprobación precede a la UI definitiva. Para cada bloque, registrar evidencia obtenida, supuestos y pruebas omitidas. Una comprobación documental, una fixture sintética y una ejecución con FM27 tienen alcances distintos. Las PoC se sitúan antes de comprometer la capacidad que depende de ellas.

## Aplicación en Spec Kit

En el apartado «Constitution Check» del plan, indicar principio aplicable, diseño que lo satisface y comprobación prevista. Por ejemplo, al diseñar una consulta por rol, el principio II se comprueba variando únicamente PA con el permiso apagado: la clasificación y el número de resultados deben coincidir. En la revisión del plan y antes de cerrar la capacidad, anotar toda divergencia, impacto y decisión pendiente. Las fases de aclaración y análisis pueden realizarse manualmente con el propietario.

## Gobierno y revisión

El propietario ratifica y modifica los principios. Para una enmienda, proponer texto, razón, alternativa y requisitos, planes o pruebas afectados. Al ratificar, registrar fecha y versión. Como propuesta de versionado, 1.0.0 será la primera versión ratificada; después, un cambio incompatible incrementa la mayor, un principio nuevo la menor y una aclaración la de parche. Los cambios de alcance aprobados se reflejan también en las decisiones de producto y en su registro de cambios.

**Preguntas para la primera revisión.** ¿Qué metadatos mínimos debe conservar toda observación histórica, incluso cuando la fuente deja de estar disponible? ¿Qué nivel de evidencia debe exigirse para una fórmula modificada si el original sólo aporta un caso manual? ¿Qué decisiones de diseño puede cerrar el autor de un plan y cuáles quieres reservarte?
