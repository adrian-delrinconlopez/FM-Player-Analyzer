# Plan de implementación · SK-01 · Extracción autorizada de FM27

**Rama Git** `prototype/centro-analisis-fm27` · **Funcionalidad Spec Kit** `001-extraccion-fm27`

**Fecha** 2026-11-23

**Entrada** [Especificación activa](spec.md)

## Resumen

Acreditar, para una compilación exacta de FM27, qué datos se pueden observar en
solo lectura y con qué límites. Partir del BepInEx ya instalado por el propietario
y comparar su alcance con un ensayo de lectura externa en C# a pequeña escala.
Ampliar a los 30 UID verificados de A-01 sólo la vía que permita reproducir el
contraste con identidad, procedencia y permisos. Evaluar CA, PA, exactos ocultos,
cobertura mundial y aislamiento real como resultados propios. El plan define un validador de consola
y un contrato de observaciones para reutilizar en SK-03. [Investigación](research.md)

## Contexto técnico

| Aspecto | Decisión o estado |
| --- | --- |
| Plataforma | Windows 11 x64 y una compilación de FM27 identificada por ejecución. Registrar distribución, idioma y perspectiva. |
| Lenguaje y versiones | C# con .NET 10 LTS para validador y prueba externa. El TFM del ensayo BepInEx depende de loader, interop y runtime de la combinación que se valide. Fijar versiones reproducibles al preparar el banco. [Política de soporte de .NET](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) |
| Dependencias | Biblioteca estándar para el validador. Referencias del juego y BepInEx confinadas al ensayo dentro del proceso. Se eligen tras la prueba inicial y la revisión de procedencia y licencias. |
| Almacenamiento | Lote de evidencia autorizado en archivo local JSON UTF-8 versionado, acotado y fuera del directorio del juego. Comparación experimental de valores especiales en memoria de sesión. SQLite de producto corresponde a SK-03. |
| Pruebas | `dotnet test` con un proyecto MSTest para contrato, política y recuperación con fixtures. Contraste real repetible desde consola con el juego abierto y copias de prueba. |
| Rendimiento | Medir duración y memoria pico por captura comparable, con línea base sin captura y el mismo límite de muestra. Fijar presupuesto de tiempo y tamaño de la prueba pequeña antes de ejecutarla. Ante el límite, cancelar y registrar la cobertura obtenida. |
| Frontera | Lectura exclusiva y local. Los binarios, estado del juego, instalación y guardados se preservan durante la captura. Los archivos `.fm` quedan fuera de la lectura. El incremento entrega evidencia de extracción, previo a la implementación de valoración, DWRS, optimizador táctico, WinUI 3 definitivo y persistencia de producto. |

## Constitution Check

Revisión de entrada y reevaluación tras el diseño con la Constitución candidata 0.4.0.
Los controles de ejecución siguen abiertos hasta obtener evidencia.

| Principio | Revisión de entrada | Reevaluación de diseño y comprobación |
| --- | --- | --- |
| I · Resultado observable | SK-01, A-01 a A-03 y trazabilidad de la especificación delimitan la prueba. | El protocolo distingue el mínimo A-01 de la acreditación adicional. Cotejar UID, etiquetas verificadas y estados contra el juego. [Guía de validación](quickstart.md) |
| II · Autorización antes de usar el dato | DP-05/16 rigen las dos vías. | Antes de cualquier ensayo real, verificar el control de sesión y permisos con un caso sintético de cambio de contexto. Aplicar ámbito y permiso antes de consultar o contar, cancelar el trabajo afectado por cambios y revalidar antes de publicar. Probar intervalo, revocación y salida indistinguible. [Contrato](contracts/captura.md) |
| III · Reproducibilidad | Cada observación debe tener fuente, tipo, escala y contexto. | Comparar dos lotes del mismo corte. Mantener valores, estados y procedencia al normalizar sólo metadatos variables legítimos. [Modelo de datos](data-model.md) |
| IV · Contratos independientes | Separar captura, validación y presentación. | El validador y sus pruebas funcionan sin el juego ni el loader. El método sólo cruza la frontera mediante observaciones acotadas. Comparar duración y memoria. [Investigación](research.md) |
| V · Entrega verificable | Evidencia sintética, documental y real identificadas. | Registrar versiones, pasos, comandos, discrepancias y omisiones. El banco real usa copias. WinUI definitivo conserva su fase y su aprobación propia. [Guía de validación](quickstart.md) |

El diseño cumple estas comprobaciones de entrada. La selección de vía y la
acreditación de campos se resuelven con los ensayos y sus puertas de salida.
[Constitución](../../.specify/memory/constitution.md)

## Estructura del proyecto

### Documentación de esta funcionalidad

```text
specs/001-extraccion-fm27/
├── spec.md
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
└── contracts/
    └── captura.md
```

### Código fuente previsto

```text
FM27Analyzer.slnx
src/FM27Analyzer.Extraction.Poc/
tests/FM27Analyzer.Extraction.Tests/
experiments/FM27Analyzer.BepInExProbe/
```

**Decisión de estructura** Una solución para terminal, VS Code y Visual Studio
Enterprise. Consola C#/.NET 10 con validación y comparación deterministas y un
proyecto de pruebas. El ensayo externo puede ser un módulo acotado de la consola
si la investigación lo justifica. El ensayo BepInEx se crea sólo si supera la
comprobación de entorno. Su proyecto queda aislado en una configuración de
laboratorio de la misma solución, con dependencias locales. La configuración
habitual compila el validador y las pruebas sintéticas desde un clon limpio.
El contrato se reutiliza en SK-03 sin trasladar referencias del juego a los casos de uso.

## Secuencia de ejecución prevista

| Paso | Trabajo y criterio de salida |
| --- | --- |
| 1 · Banco de prueba | Preparar copia, corte, referencia directa del juego y una selección previa de hasta tres jugadores para comparar las vías. Inventariar el BepInEx ya instalado y fijar alcance y restricciones antes de capturar. Si un campo carece de referencia independiente, queda sin acreditar. |
| 2 · Ensayos de viabilidad | Tras verificar con datos sintéticos la comprobación y cancelación por cambio de contexto, hacer dos lecturas por vía viable del mismo campo visible, UID y corte, sin ampliar permisos. Detener una vía ante incompatibilidad, identidad dudosa, acceso indebido, falta de correspondencia verificable o cambios atribuibles al ensayo. Registrar también ensayos bloqueados. |
| 3 · Ampliar una vía | Elegir la vía con evidencia suficiente. Fijar entonces la referencia independiente de al menos 30 UID y hacer dos capturas A-01 sobre ese conjunto. Si ninguna pasa, emitir un informe de ejecución bloqueada con A-01 sin acreditar. Una combinación sólo se considera para una carencia concreta y atribuye procedencia por campo. |
| 4 · Contrastes adicionales | Verificar A-03 con fixtures y dos copias reales cuando estén disponibles. Sondear A-02 bajo permiso expreso y límites predeclarados. Contrastar CA, PA y exactos ocultos por separado con permiso y referencias aplicables. Registrar evidencia y omisiones por escenario. |
| 5 · Cierre y traspaso | Reunir A-01 y la evidencia adicional disponible de A-02/A-03 en el informe común, con matriz de capacidades por familia, vía, compilación y perspectiva. Registrar las decisiones resultantes para Q-02/03/04/15 y el futuro SK-03. |

El [protocolo de PoC A](../../docs/roadmap/07-poc-y-modelo-v1.md#poc-a--extracción-autorizada-de-fm27)
define PC-01 a PC-03. El [corte v1](../../docs/roadmap/06-alcance-v1.md) separa
SK-01 de la persistencia y del modelo de valoración. Se conserva el tratamiento
del AS-IS de la [trazabilidad de la especificación](spec.md#trazabilidad).

## Complejidad

El validador, el lote local y el adaptador que supere la prueba pequeña cubren el
incremento. La única separación de proyectos específica de un proveedor responde
a la dependencia del runtime de BepInEx y del juego.
