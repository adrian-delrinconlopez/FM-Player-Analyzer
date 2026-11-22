# Constitución de FM27 Analyzer

**Versión 0.3.0 · Candidata a ratificación · Revisada el 2026-11-22**

FM27 es la única edición admitida. [Decisiones del propietario](../../docs/PRODUCT-DECISIONS.md)

## Principios

### I. Un resultado observable por capacidad

Cada especificación vincula requisito, hito y recorrido con entrada, resultado esperado
y caso límite. Para comportamientos heredados, identifica el tratamiento del AS-IS 2.3.1
y la prueba de equivalencia en FM27. El AS-IS permanece congelado. La documentación y la
primera interfaz usan castellano. La interfaz emplea nomenclatura oficial verificada y
recursos localizables para el inglés de la segunda entrega.

### II. Autorización antes de usar el dato

La captura y los casos de uso aplican la política de carrera, rama y perspectiva antes de
consultar, calcular, filtrar, ordenar o contar. La misma política rige históricos, cachés,
exportaciones y herramientas de ChatGPT. CA y PA empiezan deshabilitados. Su activación
exige advertencia y consentimiento expreso. El ojeo y el enmascaramiento conservan sus
propios controles. Cambiar de carrera o revocar permisos invalida los resultados
en curso. Si dos fuentes sólo difieren en datos ocultos, sus salidas autorizadas deben
coincidir. [Política y prueba de visibilidad](../../docs/to-be/13-visibilidad-y-juego-limpio.md)

### III. Cálculo local reproducible

Cada observación conserva edición, origen, carrera, rama, perspectiva, fechas de captura y
de juego, unidad y revisión de visibilidad. Una carga parcial distingue campo ausente,
desconocido y restringido. El núcleo diferencia datos observados, derivados y estimados.
Ficha, XI y exportación reutilizan el mismo cálculo determinista. Cada resultado identifica
escala, cobertura y revisiones de datos y perfil. Un recálculo conserva la serie original.

Todo cambio de fórmula explica motivo, resultado anterior y nuevo para la misma entrada, y
efecto histórico. Si sólo hay evidencia manual, se reproduce el ejemplo, se prueba un límite
o una propiedad de la fórmula y se declara hasta dónde alcanza la equivalencia.
[Oráculos y tolerancias FM27](../../docs/testing/01-equivalencia-funcional.md)

### IV. Contratos independientes de la interfaz y del juego

El dominio se prueba sin WinUI, SQLite, FM27 ni red. Los casos de uso coordinan permisos y
persistencia local en SQLite. WinUI y ChatGPT, este último opcional, consumen sus resultados.
La captura es de solo lectura y excluye archivos `.fm`. Cada vía candidata, incluida la
combinación de BepInEx y lectura de memoria, exige validar identidad, exactitud y visibilidad
por campo y compilación. El plan justifica dependencias y copias relevantes de datos frente
a sus alternativas. Para consultas frecuentes o de más de 100.000 jugadores fija volumen,
métrica y procedimiento de medida.

### V. Entregas verificables

El propietario revisa la Fase B antes del prototipo HTML ficticio y aprueba el prototipo antes
de implementar la UI definitiva. Cada PoC precede a la decisión que depende de ella. Cada
bloque se entrega en un commit con entradas, comandos, resultados y pruebas omitidas. La
evidencia identifica su procedencia documental, sintética o real. Las pruebas con partidas usan copias.

Los catálogos de pantallas, visualizaciones y componentes orientan el diseño. El plan puede
agrupar, dividir o sustituir vistas, adaptar controles existentes o crear uno propio. Registra
la necesidad funcional, técnica o de usabilidad, la alternativa y cómo comprobar el resultado.
Cada cambio preserva los requisitos, las decisiones confirmadas, la accesibilidad y los
criterios de aceptación. La coherencia entre recorridos guía la adaptación del mock a WinUI.

## Ingeniería en .NET y Windows

- **Compilación reproducible.** El escritorio usa C#, .NET 10 LTS, WinUI 3/XAML y Windows App SDK
  compatible en Windows 11 x64. Terminal, VS Code y Visual Studio Enterprise comparten una
  solución `.slnx` que se compila y prueba desde un clon limpio con versiones documentadas.
  Los proyectos C# nuevos activan nulabilidad y analizadores del SDK. Cada supresión se justifica.
  [Análisis de código .NET](https://learn.microsoft.com/es-es/dotnet/fundamentals/code-analysis/overview)
- **Respuesta y consistencia.** Las consultas SQLite y los cálculos largos se ejecutan fuera
  del hilo de UI. Interrumpir un lote preserva el estado confirmado. Reenviar un lote
  confirmado conserva el número de observaciones.
- **Accesibilidad.** Cada recorrido WinUI se completa con teclado y foco visible. Se verifican
  nombres, roles y valores mediante UI Automation. [Accesibilidad en Windows](https://learn.microsoft.com/es-es/windows/apps/design/accessibility/basic-accessibility-information)

## Aplicación en Spec Kit

En `Constitution Check`, el plan vincula principio, decisión de diseño y comprobación.
Al cierre se documentan evidencia y desviaciones del caso, sus consumidores y sus límites.

## Gobierno y revisión

El autor del plan decide detalles reversibles dentro de los contratos aprobados. El propietario
resuelve cambios de alcance y excepciones, y ratifica expresamente la constitución. Las
[cuestiones abiertas](../../docs/OPEN-QUESTIONS.md) conservan su responsable y criterio de cierre.

Cada enmienda identifica motivo, alternativa e impacto en requisitos, planes y pruebas.
Al aprobarla se registran fecha y versión. Los cambios de alcance actualizan las decisiones
y el [registro de cambios](../../docs/CHANGELOG-TO-BE.md). La primera ratificación establece
la versión 1.0.0. Después, un cambio incompatible incrementa la mayor, un principio nuevo
o ampliado la menor y una aclaración la de parche.
