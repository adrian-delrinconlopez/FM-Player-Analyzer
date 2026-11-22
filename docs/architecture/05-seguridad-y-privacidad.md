# Seguridad, privacidad y control de acceso

El contexto autorizado nace en el escritorio. Cada operación valida usuario local, carrera, sesión, ámbito temporal y revisión de política al entrar y antes de publicar. Si falla se conserva la última revisión permitida con su fecha o se vacía la vista al revocar permisos. Los objetos visibles, cachés y exportadores derivan del mismo servicio. Un nombre de jugador, una celda importada y una respuesta del modelo son datos sin autoridad para cambiar permisos.

| Límite | Comportamiento requerido | Comprobación |
|---|---|---|
| Canal local | Restringir acceso al usuario y a la instancia vinculada, limitar tamaño/colas, rechazar datos de otra sesión | Mensajes con secuencia conflictiva, campo inesperado o carrera cruzada no dejan cambios. |
| Almacenamiento | Directorio por usuario con permisos de Windows. Base deportiva local; minimizar importación de datos ocultos. Copias incluyen revisión y política | Restauración nunca habilita por sí sola CA/PA, la conexión al juego ni IA. |
| Credenciales | Almacén de credenciales de Windows o DPAPI en ámbito del usuario, separar secreto por vía/cuenta | Desconectar elimina el secreto. Revisar paquete, logs, BD, informe de error y copia con un marcador ficticio. |
| ChatGPT | Consentimiento antes de transferir, detalle de categorías y ámbito, alias opcionales, límites, revocación | No enviar fotos, directorios, fichero de carrera, SQL, parámetros privados ni transcripción previa de otra carrera. |
| Importaciones/recursos | Límites de tamaño y tipos, XML sin entidades externas, resolución de rutas confinada, lectura sin ejecución de HTML | Una imagen inválida o un CSV con datos ambiguos deja incidencia local, sin afectar el último estado confirmado. |
| Exportación | Generar Excel o CSV desde la misma consulta autorizada. Escribir texto literal en Excel y neutralizar en CSV el texto que una hoja de cálculo pueda interpretar como fórmula. Conservar tipos numéricos legítimos. | Un nombre que empiece por = queda como texto y ninguna columna ni metadato expone datos restringidos. |
| Diagnóstico | Eventos estructurados con versión, código, duración y recuentos autorizados; rotación propuesta 3 × 2 MiB | El usuario puede revisar el informe antes de compartirlo. Sin telemetría remota por defecto. |

Revocar una autorización invalida trabajos en curso, filtros, cachés y contexto visible de conversación. Los archivos exportados anteriormente están fuera del alcance de revocación. Los términos de retención del servicio remoto se consultan al activar IA. [Datos de API](https://developers.openai.com/api/docs/guides/your-data) · [Política deportiva](../to-be/13-visibilidad-y-juego-limpio.md)
