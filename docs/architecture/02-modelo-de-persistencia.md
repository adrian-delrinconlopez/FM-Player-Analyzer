# Persistencia y recuperación en SQLite

Propuesta de una base por carrera/rama lógica identificada, con una tabla Rama dentro de la base de carrera. Un catálogo local pequeño guarda UUID, nombre visible, ruta y última apertura. Configuración reutilizable en biblioteca separada. El contexto de operación enlaza conexión, carrera, rama, revisión y política; cambiar de partida cancela lectores y trabajos anteriores antes de enlazar vistas.

| Conjunto | Claves e índices previstos | Mutación |
|---|---|---|
| Persona y AliasFuente | PK local; UNIQUE(fuente,UID,ámbito de carrera); índice por nombre normalizado e ID | Reconciliar en transacción con revisión de colisiones. Referencias externas con claves foráneas activas en cada conexión. |
| Lote y estado | UNIQUE(sesión,ID de lote), huella de contenido, rama y secuencia confirmada | Publicar sólo al recibir fin con cuenta, fecha, permisos y digest correctos. Duplicado idéntico confirma sin reaplicar. |
| Observación y vigente | Persona, rama, campo, fecha, revisión, procedencia; índice por (rama,persona,campo,fecha) | Guardar cambios autorizados y sello de visto. Ausencia de una columna no borra otra. Reducir lecturas de tabla con proyección vigente. |
| Club/contrato y estadísticas | Índices por temporada, competición, persona, equipo y tramo; clave de fuente para acumulados | Sustituir acumulado de misma fuente y granularidad. Detectar solapes antes de agregar. |
| Perfil, resultado y táctica | Revisión inmutable y huella de cálculo; índice de notas por (revisión,rol,fase,nota,persona) | Activación transaccional. Las versiones referidas por históricos se retienen. |
| Cortes y manifiestos | Carrera, rama, fecha, ámbito, versión del contenido | Referenciar observaciones y pertenencias vigentes, compartiendo versiones idénticas. Un corte fijado es inmutable. |
| Operaciones locales | Lista/escenario, miembro y orden; revisiones de inscripción, fuente y política | Las referencias a UID sobreviven a conciliación y las listas se vuelven a autorizar. |
| Índice de imágenes | Raíz, revisión, tipo, UID y origen relativo | Índice derivado reconstruible. Separarlo del contenido deportivo y de las copias de carrera. |

**Concurrencia.** WAL local, lectores con conexiones de corta duración, escritor serializado y lotes acotados. Preparar una revisión nueva fuera de la proyección publicada y conmutar atómicamente al confirmar. Limitar espera por bloqueo, reintentar operaciones idempotentes y detectar disco lleno. Al reiniciar eliminar sólo preparaciones incompletas de esa sesión y recuperar último cursor confirmado. Filas en staging se someten a la misma política de conocimiento. Limitar duración de lecturas para evitar crecimiento indefinido del WAL.

**Esquema y copias.** Versionar migraciones progresivas y esquema; copia consistente mediante SQLite Backup antes de actualizar, validar integridad y referencias, conservar original hasta confirmar reapertura. Probar restauración en ruta nueva y contraste de recuentos, huellas y cambio confirmado aún en WAL. Migración fallida revierte transacción y conserva diagnóstico local. Purga de revisiones sin referencias sólo después de retención aprobada. [Backup en línea de SQLite](https://www.sqlite.org/backup.html)
