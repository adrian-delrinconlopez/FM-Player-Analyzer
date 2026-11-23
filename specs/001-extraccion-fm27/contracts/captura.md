# Contrato de captura y contraste · SK-01

## Interfaz

El adaptador entrega un lote conforme al [modelo de datos](../data-model.md).
El validador recibe el manifiesto de ejecución y emite un informe local de
evidencia. Si A-01 es contrastable, coteja los lotes con la referencia. Ambos
lados usan el mismo contrato, aunque se ejecuten con runtimes distintos.
Intercambio de archivos JSON UTF-8
acotados dentro de un directorio de trabajo accesible sólo al usuario de
prueba, fuera del directorio del juego.

## Entrada y salida

| Artefacto | Contenido mínimo |
| --- | --- |
| Manifiesto de ejecución | Versión del contrato, identificador del ensayo, modalidad de A-01, contexto y permisos que se hayan podido verificar, presupuestos de tamaño, tiempo y muestra cuando correspondan y observaciones de ejecución. El operador fija referencias y límites antes de capturar. |
| Lote de captura | Versión mayor y menor, identificador de lote, contexto, vía y revisión, instante UTC, alcance total o parcial declarado para el lote, jugadores y observaciones. El cierre declara número de jugadores y observaciones y una huella de integridad del contenido ordenado. |
| Referencia de contraste | Artefacto independiente del lote, con contexto, lista previa de UID esperados y observaciones de referencia. Incluye la fuente por observación, valor o estado autorizado, unidad o escala y evidencia de fecha y perspectiva. El manifiesto la referencia por separado de los lotes. |
| Informe | Resultado de E-01 a E-09 por escenario y clase de evidencia. Matriz por familia o campo, vía, compilación y perspectiva con semántica, unidad, alcance, esperados, recuperados, concordancia, discrepancias, denominador verificable, duración, memoria y estado. Indica causas, omisiones, comandos y límites. |

Un lote parcial de la vista inicial puede ser válido para A-01. El número de
filas del lote verifica integridad, y sólo una referencia independiente permite
usar un total como denominador de cobertura. La proyección autorizada determina
los valores, identificadores y recuentos publicables. Un dato especial queda
fuera de estos archivos, aunque su ensayo haya sido satisfactorio.

### Modalidades de A-01 y resultados adicionales

| Modalidad | Entradas y resultado |
| --- | --- |
| `contrastable` | Exige contexto y permisos verificados, dos lotes y referencia independiente fijada antes de la primera captura y registrada en el manifiesto. El validador aplica SC-001 a SC-004 y registra los escenarios aprobados, fallidos u omitidos. |
| `bloqueada` | Exige el motivo, el alcance afectado, el estado de los escenarios afectados (fallido u omitido) y la clase de la evidencia disponible. Puede referenciar un lote íntegro y aún autorizado o resultados sintéticos como evidencia parcial. Genera un informe con A-01 «sin acreditar» aun cuando falten lotes, referencia o contexto del juego. |

Un manifiesto malformado o con campos obligatorios ausentes según su modalidad
se diagnostica como entrada inválida. Un manifiesto `contrastable` que
referencie un lote ausente, truncado o incompatible se diagnostica como inválido.

El manifiesto puede referenciar `capacidades-adicionales.json` (E-04 a E-06)
y `aislamiento.json` (E-07 a E-09). Cada resultado identifica escenario,
estado (aprobado, fallido u omitido), clase de evidencia, contexto y causa o
limitación cuando corresponda. El validador conserva el resultado por clase de
evidencia y registra como omitidos los escenarios sin evidencia. La comparación
de CA/PA y exactos ocultos se hace dentro de la sesión autorizada y sólo aporta
resultados agregados. Los resultados adicionales conservan su propia acreditación
con independencia del resultado de A-01.

## Validación en la frontera

1. Antes de leer, el adaptador comprueba identidad de instancia y contexto,
   versión, alcance y política efectiva. Consulta únicamente los campos e
   identidades autorizados. Si la fuente carece de límites verificables
   o de evidencia de conocimiento, la capacidad afectada queda sin acreditar.
2. El adaptador vincula cada captura al contexto y revisión de permisos vigentes.
   Si cambian la instancia, la carrera, la rama, la perspectiva, el corte o el
   permiso, cancela el trabajo afectado y descarta el lote pendiente. Justo
   antes de publicar comprueba de nuevo que contexto y revisión siguen vigentes. Si no
   puede confirmarlos, el ensayo queda bloqueado. Sólo la proyección
   autorizada puede llegar a un archivo, incluidos
   temporales. La publicación de un archivo completo se hace fuera del juego,
   tras terminar la escritura y confirmar el manifiesto.
3. El validador comprueba versión mayor, tamaño, contexto exacto, política
   aplicable, tipos, intervalos, UID, unicidad y manifiesto. Sólo los metadatos
   opcionales de una versión menor compatible se pueden ignorar. Un campo
   desconocido queda fuera de la proyección. Un lote truncado, desfasado o
   incompatible se descarta. La huella sirve como comprobación de integridad.
4. Repetir un lote idéntico produce un solo conjunto de observaciones. El mismo
   ID con contenido distinto se rechaza como conflicto. Para comparar las dos
   capturas de A-01, normalizar orden y metadatos de la ejecución, y mantener
   contexto, valores, estados y procedencia.
5. Registrar causa y campo o vía afectados. Un fallback sólo entrega la
   muestra anterior del mismo contexto si sigue autorizada y conserva su
   fecha. Las capacidades válidas independientes se contrastan por separado.

## Uso previsto del validador

`verify --run <manifiesto> --out <informe>` valida el intercambio según la
modalidad y reúne la evidencia de E-01 a E-09. Una ejecución bloqueada produce
un informe con causa y alcance. Una entrada inválida produce un diagnóstico.
Los resultados adicionales se informan individualmente. El informe y las
salidas de error omiten valores sensibles, identificadores fuera del ámbito,
fragmentos de archivos y contenidos de memoria. La ejecución sólo acredita la
evidencia que el operador puede reproducir en el entorno declarado.
