# Contrato de captura y contraste · SK-01

## Interfaz

El adaptador entrega un lote conforme al [modelo de datos](../data-model.md).
El validador recibe el manifiesto de ejecución, contrasta lote y referencia,
y emite un informe local de evidencia. Ambos lados usan el mismo contrato,
aunque se ejecuten con runtimes distintos. Intercambio de archivos JSON UTF-8
acotados dentro de un directorio de trabajo accesible sólo al usuario de
prueba, fuera del directorio del juego.

## Entrada y salida

| Artefacto | Contenido mínimo |
| --- | --- |
| Manifiesto de ejecución | Versión del contrato, identificador del ensayo, contexto confirmado por el validador, referencia a la muestra esperada y a los dos lotes de A-01, revisión y ámbito de permisos, presupuestos de tamaño, tiempo y muestra, y observaciones de ejecución. El operador fija referencias y límites antes de capturar. |
| Lote de captura | Versión mayor y menor, identificador de lote, contexto, vía y revisión, instante UTC, alcance total o parcial declarado para el lote, jugadores y observaciones. El cierre declara número de jugadores y observaciones y una huella de integridad del contenido ordenado. |
| Referencia de contraste | Contexto y registro de la fuente independiente por observación. Lista previa de UID esperados y casos a cotejar, con valor o estado autorizado, unidad o escala y evidencia de la fecha y perspectiva. El contraste de valores especiales se hace dentro de la sesión autorizada y sólo aporta resultados agregados al manifiesto. |
| Informe | Resultado de E-01 a E-09 por escenario y clase de evidencia. Matriz por familia o campo, vía, compilación y perspectiva con semántica, unidad, alcance, esperados, recuperados, concordancia, discrepancias, denominador verificable, duración, memoria y estado. Indica causas, omisiones, comandos y límites. |

Un lote parcial de la vista inicial puede ser válido para A-01. El número de
filas del lote verifica integridad, y sólo una referencia independiente permite
usar un total como denominador de cobertura. La proyección autorizada determina
los valores, identificadores y recuentos publicables. Un dato especial queda
fuera de estos archivos, aunque su ensayo haya sido satisfactorio.

## Validación en la frontera

1. Antes de leer, el adaptador comprueba identidad de instancia y contexto,
   versión, alcance y política efectiva. Consulta únicamente los campos e
   identidades autorizados. Si la fuente carece de límites verificables
   o de evidencia de conocimiento, la capacidad afectada queda sin acreditar.
2. El adaptador revalida sesión, corte y política antes de publicar un lote.
   Sólo la proyección autorizada puede llegar a un archivo, incluidos
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

`verify --run <manifiesto> --out <informe>` valida el intercambio y calcula los
resultados a partir de las capturas y la referencia. Comunica si A-01 se
acredita, queda sin acreditar con motivos o recibe una entrada inválida. Los
resultados adicionales se informan individualmente. El informe y las
salidas de error omiten valores sensibles, identificadores fuera del ámbito,
fragmentos de archivos y contenidos de memoria. La ejecución sólo acredita la
evidencia que el operador puede reproducir en el entorno declarado.
