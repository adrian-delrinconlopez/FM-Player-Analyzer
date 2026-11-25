# Constitución de FM27 Analyzer

**Versión 1.0.0 · Ratificada el 2026-11-25 · Última enmienda: 2026-11-25**

FM27 es la única edición admitida. [Decisiones del propietario](../../docs/PRODUCT-DECISIONS.md)

## Principios

### I. Un resultado observable por capacidad

Cada capacidad se entrega como un incremento verificable con su propia especificación.
Esta declara sus dependencias y vincula requisito, hito y recorrido con entrada, resultado
esperado y caso límite. Para comportamientos heredados, identifica el tratamiento del
AS-IS 2.3.1 y la prueba de equivalencia en FM27. El AS-IS permanece congelado.
[Matriz de trazabilidad](../../docs/to-be/18-trazabilidad-as-is-to-be.md)

La documentación y la primera interfaz usan castellano. La interfaz emplea las denominaciones,
los grupos y el orden oficiales de FM27 en castellano donde correspondan. La ficha oficial
requiere cotejo con el juego. Las denominaciones sin verificar se marcan como pendientes.
Los recursos de presentación se preparan para el inglés de la segunda entrega. Los
identificadores técnicos son estables entre idiomas.
[Reglas de localización](../../docs/to-be/16-localizacion-y-nomenclaturas.md)

### II. Autorización antes de usar el dato

La captura y los casos de uso aplican la política de carrera, rama, perspectiva y permiso
vigente antes de consultar, calcular, filtrar, ordenar o contar. La misma política rige
índices, históricos, cachés, exportaciones y herramientas de ChatGPT. El ámbito inicial
abarca la plantilla y los jugadores conocidos u ojeados. El ámbito mundial requiere
activación expresa y cobertura acreditada del universo cargado. Ampliarlo conserva los permisos
de campo. La autorización expresa para atributos ocultos o desconocidos se evalúa por separado
y exige una fuente acreditada para el campo. CA y PA empiezan deshabilitados. Su activación
exige advertencia y consentimiento expreso, y su uso requiere una fuente acreditada. Los
cálculos que dependen de ambos exigen los dos permisos.

Los intervalos observados se conservan. Las identidades fuera del ámbito autorizado quedan
excluidas y los valores desconocidos o restringidos quedan fuera de los cálculos que los
requieren. Los campos y las identidades de visibilidad dudosa se omiten. Cambiar de carrera,
rama, perspectiva o revisión de permisos cancela los trabajos afectados e invalida sus cachés
y resultados. Si dos fuentes sólo difieren en datos ocultos, sus salidas autorizadas deben
coincidir. [Política y prueba de visibilidad](../../docs/to-be/13-visibilidad-y-juego-limpio.md)

### III. Cálculo local reproducible

Cada observación conserva edición, origen, carrera, rama, perspectiva, fechas de captura y
de juego, unidad y revisión de visibilidad. Una carga parcial distingue campo ausente,
desconocido y restringido. El núcleo diferencia datos observados, derivados y estimados.
Ficha, XI y exportación reutilizan el mismo cálculo determinista. Las mismas entradas y
versiones deben producir el mismo resultado. Cada resultado identifica escala, cobertura,
revisiones de datos y perfil, versión de la fórmula e hipótesis aplicadas. Un recálculo
conserva la serie original.

Todo cambio de fórmula explica motivo, resultado anterior y nuevo para la misma entrada, y
efecto histórico. Si sólo hay evidencia manual, se reproduce el ejemplo, se prueba un límite
o una propiedad de la fórmula y se declara hasta dónde alcanza la equivalencia.
[Oráculos y tolerancias FM27](../../docs/testing/01-equivalencia-funcional.md)

### IV. Contratos independientes de la interfaz y del juego

Los adaptadores de captura y de persistencia tienen contratos separados. La captura entrega
observaciones de FM27 y SQLite es la fuente persistente local. Los casos de uso aplican
permisos, invocan al dominio y coordinan la persistencia. El dominio se prueba sin WinUI,
SQLite, FM27 ni red. WinUI y ChatGPT, este último opcional, consumen las proyecciones
autorizadas de los casos de uso y reutilizan sus cálculos. La captura es de solo lectura
y excluye archivos `.fm`. Cada vía candidata, incluida la combinación de BepInEx y lectura
de memoria, exige validar identidad, exactitud y visibilidad por campo y compilación.

El plan justifica dependencias y copias relevantes de datos frente a sus alternativas.
Las consultas habituales de listados usan proyecciones paginadas y los lotes y cachés
tienen límites de memoria. Para operaciones frecuentes o de más de 100.000 registros,
el plan fija volumen, métricas de latencia y memoria, y un procedimiento de medida
reproducible. [Protocolo de rendimiento](../../docs/testing/03-validacion-rendimiento.md)

### V. Entregas verificables

El propietario revisa la Fase B antes del prototipo HTML ficticio y registra su aprobación
antes de implementar la UI definitiva. El prototipo sirve de referencia para validar recorridos
e interacciones y su código queda reservado a la demostración. La UI de producción se implementa
en WinUI 3/XAML a partir de los requisitos y criterios aprobados. Cada PoC precede a la decisión
que depende de ella.

Cada incremento exige pruebas automatizadas del comportamiento y de los contratos que afecta,
incluidos sus casos límite. El núcleo y los contratos se validan con datos controlados y sin
iniciar el juego. Cada bloque se entrega en un commit con entradas, comandos, resultados y
pruebas omitidas. La evidencia identifica su procedencia documental, sintética o real.
Las pruebas con partidas usan copias.
[Estrategia de validación](../../docs/testing/00-estrategia-de-validacion.md)

Los catálogos de pantallas, visualizaciones y componentes orientan el diseño. El plan puede
agrupar, dividir o sustituir vistas, adaptar controles existentes o crear uno propio. Registra
la necesidad funcional, técnica o de usabilidad, la alternativa y cómo comprobar el resultado.
Cada cambio preserva los requisitos, las decisiones confirmadas, la política de visibilidad,
la accesibilidad y los criterios de aceptación. La coherencia entre recorridos guía la
evolución del diseño en WinUI.

## Ingeniería en .NET y Windows

- **Compilación reproducible.** El escritorio usa C#, .NET 10 LTS, WinUI 3 nativo con XAML
  y Windows App SDK compatible en Windows 11 x64. Terminal, VS Code y Visual Studio
  Enterprise comparten una solución `.slnx` que se compila y prueba desde un clon limpio
  con versiones documentadas. Los proyectos C# nuevos activan nulabilidad y analizadores
  del SDK. Cada supresión se justifica.
  [Análisis de código .NET](https://learn.microsoft.com/es-es/dotnet/fundamentals/code-analysis/overview)
- **Respuesta y consistencia.** Las consultas SQLite y los cálculos largos se ejecutan fuera
  del hilo de UI. Interrumpir un lote preserva el estado confirmado. Reenviar un lote
  confirmado conserva el número de observaciones.
- **Accesibilidad.** Cada recorrido WinUI se completa con teclado y foco visible. Se verifican
  nombres, roles y valores mediante UI Automation. [Accesibilidad en Windows](https://learn.microsoft.com/es-es/windows/apps/design/accessibility/basic-accessibility-information)

## Aplicación en Spec Kit

En `Constitution Check`, el plan vincula principio, decisión de diseño y comprobación, dentro
del alcance aprobado para la entrega. La revisión de cierre comprueba el cumplimiento con la
evidencia del caso, de sus consumidores y de sus límites, y registra las desviaciones.

## Gobierno y revisión

El autor del plan decide detalles reversibles dentro de los contratos aprobados. El propietario
resuelve cambios de alcance y excepciones, y ratifica expresamente la constitución. Las
[cuestiones abiertas](../../docs/OPEN-QUESTIONS.md) conservan su responsable y criterio de cierre.

Cada enmienda identifica motivo, alternativa e impacto en requisitos, planes y pruebas.
Al aprobarla se registran fecha y versión. Los cambios de alcance actualizan las decisiones
y el [registro de cambios](../../docs/CHANGELOG-TO-BE.md). La primera ratificación establece
la versión 1.0.0. Después, un cambio incompatible incrementa la mayor, un principio nuevo
o ampliado la menor y una aclaración la de parche.

**Versión**: 1.0.0 | **Ratificada**: 2026-11-25 | **Última enmienda**: 2026-11-25
