# Contratos entre adaptadores y aplicación

Propuesta de canal local Named Pipes restringido al usuario de Windows y a la sesión enlazada. El escritorio recibe mensajes del plugin con autenticación local de instancia; el usuario ve y acepta el vínculo a la carrera. Protocolo de sólo observación, con lista cerrada de tipos y capacidades. Usar marco con longitud limitada y JSON UTF-8 versionado (System.Text.Json donde el runtime lo permita). Un esquema común y casos de ida/vuelta verifican ambos runtimes. [Canalizaciones de .NET](https://learn.microsoft.com/en-us/dotnet/standard/io/pipe-operations)

| Mensaje | Campos esenciales | Resultado o error |
|---|---|---|
| Saludo / capacidades | versión mayor/menor, compilaciones, distribución, perspectiva, capacidades y versión de catálogo | Rechazar mayor incompatible; ignorar campos opcionales de una menor compatible. Ningún campo desconocido pasa a consulta sin autorización. |
| Vinculación | ID de sesión, evidencia de carrera, club y fecha visibles, rama local seleccionada | Una identidad incierta exige asociación; cambiar de partida invalida el enlace. |
| Inicio de lote | ID único, secuencia, fecha de juego, origen/vista, alcance completo/parcial, revisión de política | Reservar staging sólo para un enlace vigente. Una vista de ojeo es parcial por defecto. |
| Página | ID lote, ordinal, filas con UID/campo/tipo/unidad y estado de observación | Validar tamaño, tipos, rango, UID, orden y capacidades. Duplicada con igual huella se reconoce. Diferente contenido con mismo ID es conflicto. |
| Fin / confirmación | Total de páginas y filas, hash de contenido ordenado | Comparar manifiesto, identidad/política de inicio y actuales; confirmar sólo tras commit durable. |
| Cancelación / error | Lote, código estable, reintentable, último confirmado | Descartar staging, conservar última vista válida. Reanudar desde último lote confirmado o pedir relectura completa acotada. |

Ejemplo conceptual de fila: sesión S1, lote L8, UID textual 42, campo attr.pace, tipo intervalo, mínimo 12, máximo 14, visibilidad de vista, fecha de juego 2027-09-14. Un lote cancelado entre dos páginas no cambia el 12–14 visible de la revisión previa. Un total de cero filas sólo significa vacío en el ámbito declarado.

**Alternativas.** CSV/HTML autorizado como importación manual con fecha y ámbito elegidos. Archivo local de intercambio versionado como alternativa de transporte si las restricciones del juego impiden usar pipes; aplicar idéntico contrato de entrada. Descartar un servidor HTTP accesible por red para la sincronización inicial. El cliente OpenAI recibe únicamente DTO de herramientas ya proyectados por aplicación. [Herramientas y errores](../to-be/14-asistente-chatgpt.md)
