# Asistente deportivo con ChatGPT

REQ-AI-001/002. El usuario elige una carrera, abre una consulta contextual y ve qué categorías de datos visibles saldrán del equipo. El motor determinista calcula notas, escenarios, cohortes y discrepancias. El asistente devuelve una interpretación en castellano con fecha, tipo de indicador y enlaces locales a sus evidencias. Se puede cancelar y consultar los mismos resultados sin conversación.

## Acceso y consumo

OpenAI documenta **Sign in with ChatGPT** para clientes abiertos/locales elegibles: autorización OAuth en navegador con PKCE, registro del cliente, identificador opaco y estable de instalación, alcances de inferencia y consentimiento de cuenta/espacio. El catálogo de modelos depende de esa cuenta. Las peticiones elegibles usan Responses API pública, transmisión y almacenamiento remoto de respuesta desactivado. El permiso de identidad por sí solo no concede inferencia. Verificar elegibilidad de distribución, límites, soporte de herramientas y revocación en PC-09 antes de ofrecer esta vía. [Flujo oficial](https://developers.openai.com/siwc/token-sharing-open-source) · [Modelos e inferencia](https://developers.openai.com/siwc/token-sharing-open-source/models-and-inference)

Como alternativa propuesta, clave API propia introducida por el usuario para su organización y modelo autorizado. Guardar sólo en almacén protegido de Windows, borrable al desconectar; ninguna clave compartida en el paquete. Facturación de API y suscripción se gestionan por separado. Puede haber créditos promocionales para planes elegibles y créditos/uso limitado en el flujo anterior. Mostrar vía de pago/plan, modelo disponible, consumo devuelto y tope local configurable. Una cuota agotada requiere decisión del usuario para cambiar a otra vía con coste. [Guía de API](https://developers.openai.com/api/docs/quickstart) · [Facturación](https://help.openai.com/en/articles/9039756-managing-billing-for-chatgpt-and-the-api-platform) · [Créditos elegibles](https://developers.openai.com/api/docs/guides/chatgpt-api-credits)

## Herramientas de lectura propuestas

Todas reciben un contexto **inyectado por la aplicación** de sesión, carrera, rama, fecha, revisión analítica y política. El modelo sólo propone argumentos tipados con identificadores locales previamente visibles. Validar esquema y permisos en cada llamada y antes de transmitir respuesta. Salida común: evidencia local, unidad, población, fecha, cobertura, revisión y motivo si falta. Máximo propuesto de 20 filas por herramienta y 5 llamadas por turno. [Contrato oficial de herramientas](https://developers.openai.com/api/docs/guides/function-calling)

| Herramienta | Argumentos controlados | Resultado y pregunta que resuelve |
|---|---|---|
| carencias | táctica, competición | Puestos débiles con titular, alternativa, caída y causa. ¿Dónde reforzar? |
| candidatos | rol/fase, presupuesto, moneda, filtros permitidos | Candidatos con mejora, coste y cobertura. ¿A quién puedo fichar o reemplazar? |
| explicarValoracion | uno o dos ID, rol, revisión autorizada | Factores y contribuciones, con escala y campos ausentes. ¿Por qué difieren? |
| simularPlantilla | escenario local efímero, ID de alta/baja, táctica | XI y presupuesto antes/después, plazas vacías y restricciones. ¿Qué pasaría si vendo o ficho? |
| desarrollo | jugador o lista juvenil, rol objetivo, dos fechas | Cambio deportivo separado del recálculo, talento y ganancia marginal. ¿A quién priorizo y qué entreno? |
| compararTemporadas | dos cierres de la misma carrera, ámbito | Plantilla común, entradas y salidas, variación con N. ¿Cómo ha evolucionado? |
| encajeTactico | táctica y selección acotada | Asignación IP/OOP, alternativas y referencias a las notas; distingue preferencia personal. |

Sin acceso SQL, rutas, procesos del juego, credenciales ni comandos de escritura. Proteger contra instrucciones incrustadas en nombres y notas tratándolas como datos. Ante pregunta sobre PA desactivada responder que la política no permite consultarla, sin sugerencias indirectas. En 401 solicitar reconexión, en 429 indicar límite y reintento permitido, en interrupción etiquetar respuesta incompleta. Propuesta: historial de conversación en memoria por carrera; silenciar nombres mediante alias opcionales en el contexto enviado. Mostrar qué recibe el proveedor, retención aplicable y opción de revocar. La opción store=false no elimina por sí sola cualquier registro del proveedor. [Controles de datos del proveedor](https://developers.openai.com/api/docs/guides/your-data)
