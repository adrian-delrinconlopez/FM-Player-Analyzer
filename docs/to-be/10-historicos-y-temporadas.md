# Históricos, instantáneas y temporadas

REQ-HIST-001 a 004. Una sincronización registra lo recibido y actualiza el estado consultable; una instantánea identifica qué conjunto de observaciones constituye un corte deportivo. Ambas conservan fecha real, fecha del juego, carrera, rama y revisión de visibilidad. Sin fecha de juego se registra la carga y se pide fecharla antes de usarla en series.

| Desencadenante | Registro | Política temporal propuesta |
|---|---|---|
| Lote confirmado | Procedencia, huella y diferencias autorizadas por persona/campo | El mismo lote se reconoce por identidad y contenido. Un reenvío no crea un segundo punto. |
| Observación fechada | Cambios de atributos, minutos, club, contrato y notas de seguidos | Conservar estados y sello de última observación. Ofrecer cortes mensuales del último dato visto; marca de visto aunque coincida con el mes previo. |
| Cierre de temporada o corte manual | Manifiesto y referencias a versiones existentes | Cierre inmutable con ámbito (club/lista/base visible), calendario, participantes y cobertura. Dos cierres con idénticos datos pueden compartir contenido. |
| Cambio de pesos | Revisión nueva y recálculo identificado | Series originales accesibles; superponer recalculadas sólo si existen entradas autorizadas y con etiqueta de revisión. |
| Transferencia o cambio de mánager | Nuevo tramo de pertenencia/perspectiva | Preservar identidad y fecha. En comparativas distinguir presentes en ambos cortes, altas y bajas. |

Fecha anterior al último estado publicado: ofrecer nueva rama para una recarga anterior o importación histórica dirigida. Jamás sustituir el presente de manera silenciosa. Para igual fecha y fuentes en conflicto, conservar procedencias y advertir antes de preferir una fuente. El ejemplo enero 10 → febrero 12 → febrero 10 produce cierres iguales con distinta fecha de observación. Medir cambios de desarrollo con el ajuste por recálculos del AS-IS en la vista compatible. [Reglas temporales del original](../as-is/09-historicos.md)

**Retención propuesta.** Seguir inicialmente plantilla, cantera, cedidos y listas del usuario; ampliar por selección explícita. Mantener las últimas tres temporadas a granularidad mensual, cierres fijados y referencias de perfil mientras exista algún resultado. Para anteriores ofrecer consolidación al cierre anual mostrando cuántos cortes se perderán y una copia restaurable antes de aplicar. No rellenar huecos históricos con datos actuales. La comparación entre temporadas muestra cambios de edad media y masa salarial con N conocido, altas, bajas, cambio de liga y jóvenes promovidos según su pertenencia en ambos cortes.
