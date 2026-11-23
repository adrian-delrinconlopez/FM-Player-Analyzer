# Fuentes y cobertura del TO-BE · FM27

Inventario del 23 de noviembre de 2026. Abarca los 70 requisitos del [catálogo
funcional](../../docs/to-be/02-catalogo-funcional.md), incluidas las entregas posteriores a v1.
La acreditación de la captura se refiere a Windows 11 x64, Steam 26620374 y al corte
documentado en [evidencia de SK-01](evidencia.md). Las vías de la tabla son candidatas.
La v1 y las entregas posteriores siguen el [roadmap aprobado](../../docs/roadmap/06-alcance-v1.md).

**Evidencia local hasta hoy.** La fecha del juego coincidió en dos lecturas externas
y con lo visible en FM27. El UID visible de un jugador coincidió con la salida del
plugin de terceros cargado mediante BepInEx. Un ensayo local de memoria externa
repitió «Pase», CA y PA para ese UID y coincidió con la ficha y el editor oficial.
Quedan pendientes el adaptador mantenido y las pruebas de alcance y revocación.

| Requisitos | Qué necesita el producto | Fuente y forma de obtenerlo | Verificación o límite |
| --- | --- | --- | --- |
| REQ-DATA-001, REQ-DATA-002, REQ-FM-001, REQ-DATA-004 | UID textual, club, nombre, rol de persona, edad, altura, atributos de campo y portería, posiciones, lados, pierna, estado y origen | Plantilla/ficha/vistas visibles por BepInEx, CSV/HTML de la misma vista como entrada manual. El perfil de memoria describe UID, identidad y atributos. | UID e identidad cotejados para un jugador. «Pase» coincide en dos lecturas externas. Probar portero, jugador generado, persona con doble rol y cesiones antes de extender. Conservar prefijos y validar tipos por vista e idioma. [Columnas publicadas](https://github.com/ElectroHugin/FM-View-Exporter/blob/2ed5898f6e3b8597b6b8ec5401f41d16b961cdfd/docs/columns/fm27.csv) · [Layout publicado](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/profiles/fm27/layout.toml) |
| REQ-FM-002, REQ-FM-004, REQ-SAVE-002 | Build, idioma, fecha, carrera, rama, perspectiva, recarga, lote y compatibilidad | Manifiesto Steam, UI y fecha de memoria. Perfil de terceros describe un contador de cargas. Asociación expresa de copia y corte al inicio, con UUID de producto para carrera y rama. El lote, sus límites y su huella son locales. | Fecha de memoria repetida y copia confirmada. La hora viene de la UI. Validar detección de dos recargas, cambio de mánager y dos carreras con UID coincidente. |
| REQ-VIS-001, REQ-FM-005, REQ-SCOUT-001, REQ-SCOUT-003 | Universo conocido, máscaras, informes, listas y filtros | Vistas de búsqueda, ojeo y selección de FM27 como evidencia por mánager. Extraer por vista paginada por UID. Guardar filtros y listas de producto localmente. Para el universo mundial, el perfil externo tiene una tabla de jugadores cargados. | Las vistas y la tabla mundial necesitan reconciliaciones distintas. Validar rango, exacto, desconocido y jugador que queda fuera de la vista. La sección de ensayos detalla ambos procedimientos. |
| REQ-VIS-002, REQ-VIS-003, REQ-RATING-005 | CA, PA y exactos ocultos autorizados. Proyección puntual de atributos y encaje | Memoria de entidades de jugador documentada. Hay un ejemplo de terceros dentro de BepInEx. Comparación temporal en memoria con permisos específicos. Proyección de potencial calculada localmente por PoC B. | CA y PA cotejados con el editor oficial para un jugador propio. PA codificado y exactos ocultos pendientes. La proyección además requiere los dos permisos, modelo validado y escenario declarado. |
| REQ-DATA-003, REQ-SCOUT-002, REQ-MARKET-001, REQ-MARKET-002, REQ-MARKET-003 | Valor FM, precio observado, moneda, sueldo, contrato, cuotas de cesión, presupuesto, gastos y movimientos | Vistas de ficha, contrato, fichajes, cesiones y finanzas propias. Candidatos de memoria para contrato, cláusulas y cesión. Importes adoptados, tipos de cambio aplicados y simulaciones son inputs o resultados locales. | Conservar texto, rango, unidad y periodo. Fijar tipo de importe y perspectiva. Validar moneda y periodicidad, distinguir precio pedido / oferta / operación efectiva y presupuesto total / disponible antes de calcular. [Contratos](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/docs/recon/fm27-contracts.md) · [Cesiones](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/docs/recon/fm27-loans.md) |
| REQ-ROLE-001, REQ-ROLE-002, REQ-ROLE-003, REQ-RATING-001, REQ-RATING-002, REQ-RATING-003, REQ-RATING-004, REQ-RATING-006, REQ-PROFILE-001, REQ-PROFILE-002 | Catálogo, atributos y familiaridad observada, pesos, curvas, DWRS y escenario a 20 | Catálogo cotejado en UI. Memoria contiene candidatos a escalas de posición. Pesos, perfiles, Classic, DWRS y escenario a 20 se versionan y calculan en Analyzer. | Contrastar semántica y escala de familiaridad por posición y lado. Categoría visible y número en memoria requieren evidencia de equivalencia. Validar fórmulas con oráculos y cobertura de atributos. |
| REQ-TAC-001, REQ-TAC-002, REQ-TAC-003, REQ-TAC-004 | Táctica, fases, puestos, roles, instrucciones, fijaciones y XI | Definición inicial local según catálogo. Pantalla táctica para cotejo. Táctica activa y roles IP/OOP documentados por terceros en memoria. Asignación y simulación deterministas en Analyzer. | El TO-BE inicial admite táctica local. Para leer la del juego, fijar mánager, equipo, fase, selección, puestos e instrucciones y probar cambios. [Investigación de táctica](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/docs/recon/fm27-tactics-and-value.md) |
| REQ-SQUAD-001, REQ-SQUAD-002, REQ-SQUAD-003, REQ-SQUAD-004, REQ-SQUAD-005 | Plantilla, filiales, cantera, cedidos, profundidad, huecos, comparación y salidas | Vistas explícitas de cada equipo y de cedidos. Relación persona-equipo/club candidata en memoria. Diagnóstico, comparación, sugerencias y simulación derivadas de observaciones y táctica local. | Reconciliar propiedad, destino, rol y roster completo por equipo. Ausencias parciales no acreditan salida, lesión, curación o cambio de contrato. |
| REQ-NAT-001, REQ-NAT-002, REQ-REG-001, REQ-REG-002, REQ-REG-003 | Nacionalidades, formación, cupos, exenciones, elegibilidad real y listas | Pantallas de elegibilidad y reglas por temporada/carrera. Columnas visibles de nacionalidad y formación. Datos de selecciones, nacionalidades y cantera descritos por terceros. Reglas y listas aceptadas se versionan localmente. | Nacionalidad, formación y elegibilidad son campos distintos. Acreditar por competición y temporada. Permisos de trabajo y excepciones siguen pendientes de una fuente comparable. [Nacionalidades y formación](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/docs/recon/fm27-nationality.md) |
| REQ-DEV-001, REQ-DEV-002, REQ-DEV-003, REQ-DEV-004, REQ-DEV-005, REQ-STAFF-001 | Edad, personalidad, minutos, rol objetivo, evolución, carga y entrenamiento, empleados | Perfil/player history, pantallas de informes y entrenamiento y vistas de plantilla, cedidos y cuerpo técnico. Memoria documenta gestores de entrenamiento y datos de empleados. Fórmulas de talento, bandas y plan de entrenamiento son locales. | Probar que un dato del historial corresponde a la granularidad y escala requeridas. Personalidad es la etiqueta observada. Los ojeadores y empleados tienen escalas y permisos propios. [Empleados](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/docs/recon/fm27-staff.md) · [Perfil de la build](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/profiles/fm27/steam-26620374.toml) |
| REQ-STAT-001, REQ-STAT-002 | Apariciones, minutos, acciones, tasas, xG, costes y cohortes para Moneyball | Vistas estadísticas y columnas publicadas. CSV/HTML como entrada complementaria. Captura en bloque por competición, temporada y equipo; tasas, percentiles y cohortes calculados localmente. | Validar acumulados frente a tramos, numeradores y denominadores, competición y equipo en traspasos. Coordenadas de acciones y granularidad partido a partido siguen sin fuente acreditada. [Columnas estadísticas](https://github.com/ElectroHugin/FM-View-Exporter/blob/2ed5898f6e3b8597b6b8ec5401f41d16b961cdfd/docs/columns/fm27.csv) |
| REQ-HIST-001, REQ-HIST-002, REQ-HIST-003, REQ-HIST-004 | Series, cambios de club/atributos, ramas, cierres y tendencias | Crear instantáneas locales a partir de observaciones fechadas y revisiones de cálculo. Contrastar pertenencia y hechos históricos visibles si se incorporan. | Cobertura empieza donde hay cortes. Comparar cohorte y revisiones. Una retirada inferida se etiqueta y se puede revertir. |
| REQ-ART-001, REQ-I18N-001, REQ-TERM-001, REQ-FM-003 | Retratos, escudos, catálogos de términos y recursos `.fmf` candidatos | Ruta graphics elegida y resolución local por ID. Etiquetas contrastadas en pantallas FM27 por idioma. Archivos `.fmf` sólo si se aprueban sus derechos y utilidad. | Sustituto visual ante falta de asociación. Los catálogos comparan orden, idioma y fase. Los recursos `.fmf` no aportan observaciones de la carrera. |
| REQ-SCOPE-001, REQ-SAVE-001, REQ-PREF-001, REQ-EXPORT-001, REQ-UI-001, REQ-UX-001, REQ-PERF-001, REQ-TECH-001, REQ-OPS-001 | Perfiles de carrera, preferencias, filtros, Excel, UI, diagnósticos, copias y rendimiento | SQLite, ajustes, cálculos y proyecciones autorizadas de Analyzer. Herramientas de desarrollo y fixtures para comprobar UI, operaciones y rendimiento. | Validar aislamiento, cancelación, revocación y exportaciones sobre los mismos contratos. Probar volumen con fixtures mientras se mide el universo realmente cargado. |
| REQ-AI-001, REQ-AI-002 | Autenticación, herramientas autorizadas y referencias a las explicaciones | ChatGPT opcional y casos de uso de Analyzer. | Las herramientas aplican la proyección de la carrera y la política actual. La analítica local funciona sin ChatGPT. |

## Ensayos de acceso a jugadores

### CA y PA

La vía externa candidata lee dos campos de la entidad ya verificada por UID. La documentación
publicada describe CA como entero de 16 bits sin signo de 1 a 200 y PA como entero de 16 bits
con signo. Los códigos de PA negativos requieren interpretación antes de exponer una cifra
o proyectar un jugador. La vía BepInEx tiene un precedente que decodifica memoria y cambia la
presentación en pantalla. Sus pruebas publicadas corresponden a otras builds y el proyecto
documenta una revisión de BepInEx distinta. Sirve de referencia técnica de terceros.
[Layout de jugadores](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/profiles/fm27/layout.toml) ·
[FM27 Ability Numbers](https://github.com/da3gal/fm27-ability-numbers)

Para E-04, fijar un UID autorizado y una referencia independiente del mismo corte. La
comparación local ya usó «Calidad actual» y «Calidad potencial» del editor oficial del juego
para un jugador propio, con dos coincidencias por campo.
Solicitar consentimiento expreso tras advertencia. Leer CA y PA por separado y repetir cada lectura.
Las estrellas de la ficha se conservan como estrellas y el contraste espera números en la misma
escala. El informe guarda coincidencias y motivos sin los valores. Si dos herramientas comparten
la interpretación del mismo lector, hace falta otra referencia para acreditar el campo.

### Todo lo conocido u ojeado

Preparar una vista de búsqueda u ojeo vinculada al mánager y al corte, con el total que declara
FM27, los filtros efectivos y UID por fila. Una lista de seguimiento es una selección local de
jugadores. El universo conocido puede abarcar más vistas. Leer cada vista candidata mediante
BepInEx, conservar identidad, orden de paginación y celdas exactas, en rango o desconocidas.
Si hay solapes, reconciliar por UID y contexto, y registrar en qué vista se acreditó cada campo.

Para probar cobertura, comparar el total visible con los UID únicos y revisar omisiones y
duplicados. Para probar conocimiento, contrastar plantilla, conocido, ojeado parcial, desconocido
y fuera de ámbito. En una combinación de vías, la evidencia de pantalla determina qué se puede
consultar y con qué precisión. Hay descripciones externas del conocimiento que usan archivos
`.fm`; DP-04 excluye esa fuente para SK-01. [Exportación de vistas](https://github.com/ElectroHugin/FM-View-Exporter/blob/2ed5898f6e3b8597b6b8ec5401f41d16b961cdfd/README.md) ·
[Ejemplo documental de otro mecanismo](https://github.com/Napas/fm-mcp#fog-of-war)

### Todos los jugadores de la base cargada

El perfil de Steam 26620374 documenta un par de límites de la colección de jugadores
en memoria. Es el candidato para E-05. El universo que se evalúa es el de la carrera
cargada en ese corte. Con autorización mundial expresa, probar primero sólo identidades,
por bloques acotados y con cancelación al cambiar contexto. Reconciliar UID únicos y
total con una referencia independiente del mismo universo, incluyendo agentes libres,
generados y personas con doble rol si aparecen. La vista de ojeo conocido tiene su propio
alcance. Si falta una referencia comparable, dejar indeterminada la cobertura o acreditar
el subconjunto comprobable. El editor oficial del club ofrece «Mostrar todos los jugadores
en la búsqueda». En la copia de prueba ya estaba activo y se puede evaluar como referencia
candidata para el universo mundial. Ampliar el universo mantiene los permisos de campo.
[Perfil de Steam 26620374](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/profiles/fm27/steam-26620374.toml) ·
[Escenarios y criterios](spec.md)

### Primer atributo de SK-01

La referencia tiene un UID confirmado en pantalla, «Pase» y un corte fijado antes del ensayo.
El ensayo externo encontró un selector por las claves de club y los miembros del primer
equipo. Verificó el vínculo, UID y corte y obtuvo dos coincidencias de «Pase» con la ficha.
En BepInEx, el primer candidato es el texto de la ficha o la celda «Pase» de una fila ya
identificada por «Unique ID». T015–T017 reúnen las dos lecturas por vía y evalúan si procede
ampliar a A-01. [Lector de referencia](https://github.com/tozmoz/FMLibreEditor/blob/v0.5.3/crates/fmlibre-core/src/players.rs) · [Tareas](tasks.md)

## Cierre de viabilidad antes de la aplicación definitiva

El propietario pide aclarar la obtención de datos de todo el TO-BE antes de empezar
la interfaz y el backend de producto. La PoC se puede ampliar con los contrastes que
falten. Cada familia deberá tener una fuente contrastada, una alternativa manual o local
aceptada por el propietario, o una limitación con decisión explícita sobre su alcance.
Las referencias de terceros orientan los ensayos. El cierre registra el resultado
obtenido en FM27 y el tratamiento aprobado para los huecos.

| Orden | Qué falta decidir | Prueba mínima y evidencia de salida |
| --- | --- | --- |
| 1 | Vía inicial y contexto fiable | Terminar T015–T017 y A-01. Comparar campo visible, UID, permiso, exactitud, tiempo y memoria por vía. Ejecutar A-03 con recarga, cambio de perspectiva y dos carreras con UID coincidente. |
| 2 | Ojeados, mundo y campos especiales | Contrastar exacto, rango, desconocido y exclusión en ojeo. Acreditar la referencia independiente para el universo mundial, o delimitar su cobertura. Ensayar CA y PA por separado y exactos ocultos con sus permisos y revocación. Registrar si hay una fuente viable para familiaridad numérica y para el modelo de proyección. |
| 3 | Datos de las entregas posteriores | Ampliar el banco con un caso por familia crítica: contrato y cesión con moneda y periodicidad, táctica IP/OOP, formación y regla de inscripción de una competición, empleado por función, entrenamiento/juveniles y estadísticas con minutos por competición/temporada. Para cada caso, cotejar pantalla y lectura o cerrar una alternativa. Usar dos cortes para históricos y desarrollo. |
| 4 | Decisiones de alcance | Revisar la matriz con cobertura, huecos, degradación, coste y dependencia de la build. Enlazar cada decisión a sus requisitos. Las familias que se derivan de datos o dependen de configuración local cierran con un contrato de entrada y un oráculo reproducible. |

Esta secuencia conserva los criterios de SK-01. Los ensayos nuevos de familias posteriores
se concretarán cuando la evidencia indique qué vía puede reutilizarse y qué familia
necesita un contraste propio. [Plan de SK-01](plan.md) ·
[Cuestiones abiertas del producto](../../docs/OPEN-QUESTIONS.md)
