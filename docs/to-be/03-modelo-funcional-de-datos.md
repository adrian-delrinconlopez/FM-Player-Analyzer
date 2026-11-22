# Modelo funcional de datos

REQ-DATA-001 a 004, REQ-SAVE-001/002 y REQ-HIST-001. La unidad de trabajo es una carrera activa y un punto de observación. El jugador puede cambiar de club, la persona conserva su identidad y el mánager puede cambiar de empleo. Un nombre de club, una ruta, el nombre del mánager o una fecha por sí solos no identifican una carrera.

| Entidad y grano | Clave de negocio o vínculo | Datos y reglas |
|---|---|---|
| Carrera y rama temporal | UUID local y asociación de fuente confirmada | Edición, mánager, club dirigido por periodo, fecha dentro del juego, inicio de temporada. Retroceso de calendario abre revisión de rama. |
| Sesión y lote | Carrera, sesión efímera, secuencia, huella | Compilación del juego/plugin, protocolo, alcance visible, conjunto parcial/completo, fecha real y fecha del juego. |
| Persona / alias | ID local inmutable y (carrera, tipo de fuente, UID textual) | Futbolista, empleado o ambos. Mantener prefijos de generados. La colisión deja dos registros en revisión hasta conciliar. |
| Pertenencia y contrato | Persona, club, intervalo de vigencia | Club propietario y club de cesión distintos. Sueldo vigente con moneda y periodo, valor de FM y precio observado por separado, tipo, fecha y fuente del importe, fin y opciones. Fecha final ausente equivale a desconocida. |
| Observación | Persona, campo, instante, fuente y perspectiva del mánager | Exacto, intervalo inclusivo, desconocido, no autorizado o no aplicable. Igualdad y orden se definen por tipo y unidad. Distinguir observado, manual y derivado. |
| Definiciones | Edición, clave, revisión | Atributo, puesto, rol, fase, competición, temporada, catálogo lingüístico y regla de inscripción. La presentación usa claves estables. |
| Plan | Carrera, ID local, revisión | Táctica, puestos, bloqueo, lista de seguimiento, selección, simulación y preferencias. Referencia a jugadores por ID local. |
| Resultado | Persona o plantilla, cohorte, revisión de entradas y cálculo | Perfil y huella, método, cobertura, causas, fecha y ámbito autorizado. Se invalida al cambiar una dependencia. |
| Estadística | Persona × competición × temporada × equipo × tramo o partido | Minutos, apariciones, numeradores y denominadores. Registrar si la fila es acumulada o aditiva. |
| Recurso | Raíz local, tipo e identificador FM | El índice de rutas es independiente de la persona. Resolver tras confirmar el espacio de ID. |

**Conciliación.** Primero coincide el UID dentro de la misma carrera. Si aparecen dos UID incompatibles se propone un enlace indicando por qué. La aceptación reasigna observaciones, roles, seguimiento y escenarios en una transacción. Conflictos de datos con igual fecha conservan ambas procedencias y un valor efectivo documentado. Repetir la misma conciliación tiene un solo efecto. La edición local permite corregir fuente, club, estado o dato visible con autor y fecha. No eleva permisos de consulta.

**Carga parcial.** Un campo ausente de la proyección indica que esa vista no lo observó. Un campo explícitamente desconocido sí actualiza el conocimiento actual. Al cambiar de perspectiva, prevalece el permiso actual incluso sobre históricos. La [política de visibilidad](13-visibilidad-y-juego-limpio.md) determina qué información puede salir de cada consulta.
