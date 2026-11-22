<!-- Impacto de sincronización · 0.1.0 → 0.2.0
I–V conservan su objeto. II explicita la revocación y III la evidencia parcial.
IV distingue contratos y vías candidatas. V concreta reproducción y accesibilidad.
Se añade Ingeniería en .NET y Windows. Las preguntas iniciales se integran en III y Gobierno.
Pendiente de ratificación por el propietario. -->

# Constitución de FM27 Analyzer

**Versión 0.2.0 · Candidata a ratificación · Revisada el 2026-11-22**

Las [decisiones del propietario](../../docs/PRODUCT-DECISIONS.md) fijan el alcance.
Esta constitución establece cómo diseñar, probar y aceptar cada capacidad.

## Principios

### I. Un resultado observable por capacidad

Cada especificación vincula el requisito y su hito con un recorrido, una entrada, un
resultado esperado y un caso límite. Para comportamientos heredados, identifica el tratamiento
del AS-IS 2.3.1 y su prueba de equivalencia en FM27. El AS-IS permanece congelado. La
documentación y la primera interfaz usan castellano. La interfaz conserva la nomenclatura
oficial verificada y prepara los recursos para el inglés de la segunda entrega.

### II. La visibilidad se comprueba antes de usar el dato

La captura y los casos de uso aplican la política vigente de carrera, rama y perspectiva
antes de consultar, calcular, ordenar o contar. Históricos, cachés, exportaciones y
herramientas de ChatGPT obedecen la misma política. CA y PA requieren activación expresa
con advertencia y respetan los límites de ojeo y enmascaramiento de los demás campos.
Al cambiar de carrera o de permisos se invalidan los resultados afectados, incluidos los
que siguen en curso. Si dos fuentes sólo difieren en datos ocultos, todas las salidas
autorizadas deben coincidir. [Política y prueba de visibilidad](../../docs/to-be/13-visibilidad-y-juego-limpio.md)

### III. Un cálculo local que pueda reproducirse

Cada observación conserva edición, origen, carrera, rama, perspectiva, fechas de captura y
de juego, unidad y revisión de visibilidad. Una carga parcial distingue ausencia, valor
desconocido y restricción. El núcleo distingue lo observado de lo derivado o estimado.
Con las mismas entradas y revisión de perfil produce el mismo resultado, escala y cobertura
para ficha, XI y exportación. Un recálculo conserva la serie original e identifica la nueva.

Todo cambio de fórmula documenta motivo, resultado anterior y esperado para la misma entrada,
y efecto en los históricos. Si sólo existe un ejemplo manual, se reproduce ese caso, se añade
una prueba de límite o de una propiedad de la fórmula y se declara el alcance de la equivalencia.
[Oráculos y tolerancias FM27](../../docs/testing/01-equivalencia-funcional.md)

### IV. Contratos independientes de la interfaz y del juego

El dominio se prueba sin WinUI, SQLite, FM27 ni red. Los casos de uso coordinan permisos y
persistencia. La presentación y el cliente opcional de ChatGPT consumen sus resultados.
La captura es de solo lectura y excluye los archivos `.fm`. Habilitar BepInEx, lectura de
memoria o su combinación para un campo exige validar identidad, exactitud y visibilidad
en la compilación del juego. Cada dependencia o copia relevante de datos se justifica por
su coste y una alternativa. En consultas frecuentes o de más de 100.000 jugadores se
documentan volumen, métrica y procedimiento de medida.

### V. Entregas verificables por el propietario

El propietario revisa la Fase B antes del prototipo HTML ficticio y aprueba el prototipo
antes de implementar la UI definitiva. Cada PoC precede a la decisión que depende de ella.
Cada bloque se entrega en un commit con entradas, comandos, resultados y pruebas omitidas.
La evidencia identifica su procedencia documental, sintética o real. Las pruebas con partidas
utilizan copias.

## Ingeniería en .NET y Windows

- **Compilación.** El escritorio usa C#, .NET 10 LTS y WinUI 3/XAML con Windows App SDK
  compatible en Windows 11 x64. Desde un clon limpio, terminal, VS Code y Visual Studio
  Enterprise comparten solución `.slnx`, proyectos y versiones de herramientas y paquetes.
  El código C# nuevo activa la nulabilidad y los analizadores del SDK. Toda supresión
  identifica el diagnóstico y justifica su alcance. [Análisis de código .NET](https://learn.microsoft.com/es-es/dotnet/fundamentals/code-analysis/overview)
- **Respuesta y consistencia.** Las consultas SQLite y los cálculos largos se ejecutan fuera
  del hilo de UI. Un lote se confirma de forma atómica y su reenvío conserva el resultado.
  La prueba de integración cubre interrupción y reintento.
- **Accesibilidad.** Cada recorrido WinUI se puede completar con teclado y conserva un foco
  identificable. La prueba comprueba los nombres, roles y valores de los controles mediante
  UI Automation. [Accesibilidad en Windows](https://learn.microsoft.com/es-es/windows/apps/design/accessibility/basic-accessibility-information)

## Aplicación en Spec Kit

En `Constitution Check`, el plan vincula cada principio aplicable con una decisión de diseño
y una comprobación. Al cierre se contrasta la evidencia con lo previsto y se registran las
desviaciones. Se valida primero el caso afectado y después sus consumidores y límites.

## Gobierno y revisión

Quien redacta el plan decide los detalles reversibles dentro de los contratos aprobados.
El propietario resuelve los cambios de alcance, las excepciones y la ratificación expresa.
Las [cuestiones abiertas](../../docs/OPEN-QUESTIONS.md) conservan su responsable y criterio
de cierre. Toda enmienda identifica motivo, alternativa y requisitos, planes o pruebas
afectados. Al aprobarla se registran fecha y versión y, si cambia el alcance, se actualizan
las decisiones y el [registro de cambios](../../docs/CHANGELOG-TO-BE.md).

La primera ratificación establece la versión 1.0.0. Después, un cambio incompatible incrementa
la versión mayor, un principio nuevo o ampliado la menor y una aclaración la de parche.
