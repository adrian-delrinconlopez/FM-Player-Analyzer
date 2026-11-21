# Arquitectura de escritorio propuesta

DP-02/03/04/06 fijan las tecnologías principales. Se propone un monolito modular en el escritorio, un plugin mínimo en el proceso de FM27 y, exclusivamente cuando el usuario lo solicita, acceso al servicio oficial de OpenAI. El juego puede cerrarse sin perder la capacidad de consultar la última observación.

| Límite | Responsabilidad y dependencias permitidas | Contrato que se prueba |
|---|---|---|
| Dominio y análisis | Modelos de rango, identidad, unidades, perfiles, puntuaciones, optimizador, inscripción, scouting y estadística. C# puro sin dependencias de UI, red o almacenamiento. | Entradas autorizadas e inmutables → resultado, cobertura y explicación deterministas. |
| Aplicación | Casos de uso, política de visibilidad, carrera activa, cancelación y coordinación de transacciones. | La misma consulta alimenta tabla, exportación y herramientas. El consumidor no puede aportar sus propios permisos. |
| Infraestructura | SQLite, adaptador de importación, canal local, miniaturas, registro y cliente OpenAI. Implementa los puertos necesarios. | Proyecciones paginadas, persistencia atómica y pruebas de error con dobles de fuente. |
| Presentación | WinUI/XAML y modelos de vista por pantalla. Traduce códigos y unidades, conserva selección por ID, expone comandos. | Ninguna fórmula deportiva en XAML, convertidores ni ordenadores locales de tabla. |
| Plugin | Sólo observar campos y contexto permitidos para esa compilación, formar lotes y responder a cancelación. | El hilo del juego se libera entre lecturas acotadas; sin dependencia de WinUI, SQLite, IA o .NET 10 de escritorio. |
| Pruebas | Núcleo, contratos, integración con disco temporal, interfaz y benchmarks en proyectos de prueba. | El núcleo y el protocolo se validan sin instalar FM ni iniciar la interfaz. |

**Organización propuesta.** Un proyecto para núcleo y casos de uso con espacios de nombres por dominio, otro para infraestructura, otro para escritorio, proyecto aislado del plugin y proyectos de pruebas. Compartir un esquema de mensajes con fixtures comunes entre runtimes. Separar un ensamblado únicamente si hay dependencia, ciclo o necesidad de despliegue medible. Composición explícita desde el arranque. Sin servidor de base de datos, bus distribuido ni modelo de IA residente.

**Recorrido de una sincronización.** Plugin → validación de sesión y conocimiento → área temporal → transacción de la carrera → invalidación de proyecciones por revisión → actualización del resumen visible. Una lectura de tabla sigue el sentido inverso hasta una consulta de aplicación filtrada. El asistente invoca ese mismo servicio sin adquirir una conexión SQL. [Persistencia](02-modelo-de-persistencia.md) · [Contratos](03-integraciones-y-contratos.md) · [Decisiones abiertas](09-decisiones-tecnicas-pendientes.md)
