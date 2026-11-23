# Casos de aceptación nuevos y transversales

Ejecutar con datos controlados del [prototipo](../ux-ui/05-especificacion-prototipo-html.md) o del [banco de rendimiento](03-validacion-rendimiento.md). Probar el núcleo directamente sin depender del HTML.

Los [casos V1-01 a V1-10](../roadmap/06-alcance-v1.md#aceptación-de-v1) especifican los recorridos de entrega y enlazan a las pruebas de equivalencia, casos nuevos y fronteras reutilizados. El contraste con FM27 real sigue el [protocolo A/B](../roadmap/07-poc-y-modelo-v1.md).

| Caso | Requisito | Entrada, acción y resultado esperado |
|---|---|---|
| NV-01 | REQ-SCOPE-001 | Un lote FM24 se rechaza antes de persistir. Sin red se completa plantilla → rol → XI. |
| NV-02 | REQ-SAVE-002 | Dos carreras del mismo club con UID 42 conservan dos personas y dos históricos independientes. |
| NV-03 | REQ-VIS-001 | Cambiar sólo valores secretos deja idénticos filas, totales, orden, herramientas y contenido de Excel y CSV autorizados. |
| NV-04 | REQ-VIS-002 | Activar sin capacidad mantiene «No disponible»; desactivar cancela consultas y descarta proyecciones anteriores. |
| NV-05 | REQ-FM-004 | En una captura híbrida simulada, una vía queda incompatible tras un cambio de compilación. Se conserva el último dato autorizado de sus campos con fecha y motivo. La otra vía sólo actualiza sus campos independientes y validados. |
| NV-06 | REQ-PREF-001 | Cambiar la prioridad de juventud altera el orden por preferencia, manteniendo DWRS e XI de referencia. |
| NV-07 | REQ-RATING-004 | Dos atributos exactos 10 y 16 con pesos 1 y 2 dan 14/20; si falta uno no hay nota. |
| NV-08 | REQ-SCOUT-003 | Recuperar una búsqueda de otra sesión conserva sus parámetros y revalida cada campo autorizado. |
| NV-09 | REQ-MARKET-003 | Venta a 8 M y reemplazo a 5 M muestran +3 M brutos de caja antes de salarios o gastos. |
| NV-10 | REQ-STAFF-001 | «Good» y «Elite» con pesos 2 y 1 dan estimación 46/3 sobre 20; palabra desconocida impide valorar. |
| NV-11 | REQ-DEV-005 | Un cambio de club conserva la serie del UID; sin última observación figura su antigüedad. |
| NV-12 | REQ-STAT-001 | 5 goles en 450 min dan 1 por 90; sin minutos válidos no aparece punto. |
| NV-13 | REQ-STAT-002 | Cambiar liga recalcula N y percentiles; una celda insuficiente muestra la causa en tabla y gráfico. |
| NV-14 | REQ-HIST-004 | Venta de un jugador produce una baja, conserva su pasado y cambia el denominador del nuevo cierre. |
| NV-15 | REQ-AI-001 | Sin credencial se mantiene toda la analítica local. Un 429 conserva resultados y explica el límite. |
| NV-16 | REQ-AI-002 | Una consulta sobre un refuerzo cita revisión, rol, coste y fecha; solicitudes ajenas al permiso no devuelven campos. |
| NV-17 | REQ-ART-001 | Dos homónimos tienen fotos distintas; archivo ausente usa iniciales y reindexar actualiza sólo cambios. |
| NV-18 | REQ-I18N-001 | Cambiar cultura de prueba conserva orden canónico, ID y puntuación; término sin fuente conserva estado pendiente. |
| NV-19 | REQ-UI-001 | Se consulta una ficha y se bloquea un puesto sólo con teclado al 150 %. |
| NV-20 | REQ-UX-001 | Sin conexión se completan los recorridos de aceptación visual y hay cero peticiones remotas. |
| NV-21 | REQ-PERF-001 | Con 150.000 filas la lista materializa como máximo la ventana especificada y descarta resultados de filtro obsoletos. |
| NV-22 | REQ-TECH-001 | Desde un clon limpio, misma revisión, pruebas y versión de artefactos en ambos IDE y terminal. |
| NV-23 | REQ-OPS-001 | Una copia mientras hay cambios confirmados en WAL se restaura con esos cambios y con integridad referencial. |
| NV-24 | REQ-TERM-001 | Se puede remontar cada etiqueta marcada oficial a edición, idioma, vista y evidencia. |
| NV-25 | REQ-SQUAD-002, REQ-SQUAD-004, REQ-SCOUT-001, REQ-MARKET-001 | Desde AML con Leo y puente W-IP/WM-OOP abrir candidatos, comparar a Sol y simular. Mismo contexto, titular y delta de +11 (58 → 69 a 50 % IP). En un caso separado exigir 65 en cada fase: candidato 80/60 queda fuera aunque la media sea 70. Con vacante o titular sin nota se omite el delta. Si cambia la revisión, revisar la referencia antes de simular otra vez. |
| NV-26 | REQ-STAT-002, REQ-VIS-001 | En la cohorte ficticia del Cabo, k=12 da 1,1 goles/90 y percentil 47,9 con N=24. El filtro percentil ≥47 devuelve k=12 a 24. Un jugador de 180 min y 1 gol muestra ese total y 0,5 provisional en detalle, fuera de la clasificación. Cambiar atributos autorizados, DWRS o PA conserva el resultado estadístico, orden y exportación. Cambiar de vista suspende y revalida criterios y descarta solicitudes anteriores. |
| NV-27 | REQ-MARKET-003 | Sol tiene valor FM 4 M€ y sueldo vigente de 8.000 €/semana, precio observado ausente y presupuesto 6 M€. Antes de adoptar 4 M€ como compra hipotética, el saldo de compra queda indeterminado. Tras adoptarlo, quedan 2 M€ antes de gastos. El sueldo vigente no rellena el ofertado. Repetir con moneda distinta deja indeterminado el importe sin conversión. |
| NV-28 | REQ-SQUAD-004, REQ-RATING-005, REQ-VIS-001 | Cambiar puesto, ambos roles y perfil con dos o más jugadores. Comprobar deltas positivos, negativos y empate. Con un rol, contrastar familiaridad presente y 20 hipotético, márgenes dispares y ausencia de evidencia. Quitar un permiso y repetir con dos fuentes que sólo difieren en secretos. |
| NV-29 | REQ-UI-001, REQ-EXPORT-001 | Personalizar y reordenar columnas, recargar y restaurar. Ordenar numéricamente por calidad de rol en ambos sentidos, con ausentes al final. Exportar página, selección y conjunto filtrado y abrir el XLSX. Comprobar números, porcentajes, metadatos, ausentes y la retirada de derivados y órdenes tras revocación. |


## Fronteras y regresiones

| Caso | Preparación y acción | Resultado |
|---|---|---|
| FR-01 | Curvas lineales de amplitud 7 y 3. Conocido a 20 sólo el primero y después sólo el segundo. | Cobertura 70 %, normalizada 100. Con 30 %, sin nota. |
| FR-02 | Curva 0 hasta 10 y 1 desde 11. Observar 9–11 y después extremos invertidos. | Contribución media 1/3 y después intervalo inválido. |
| FR-03 | Elegible sólo en IP y cuota IP=100 %. Fijarlo sin posición OOP. | Falta arista OOP, causa visible y ningún titular duplicado. |
| FR-04 | Variar sólo secretos en dos fuentes gemelas y revocar un permiso a mitad de consulta. | Mismo orden, recuentos, resultados y explicaciones autorizados. Incluir búsqueda de refuerzo y Moneyball con sus exportaciones. |
| FR-05 | Lote de dos páginas; cortar tras la primera, reabrir y reintentar dos veces. | Sin cambios parciales; un lote y un conjunto de hechos al confirmar. |
| FR-06 | Cero goles en 90 min, goles sin minutos y pases 3/4 más 1/6. | 0 por 90, sin tasa y 40 % respectivamente. |
| FR-07 | Duplicar perfil, cambiar etiqueta, editar curva y recalcular un corte antiguo. | Dos resultados identificables. El original conserva su valor; renombrar conserva la huella. |
| FR-08 | Ordenar tabla, cambiar tema e idioma, abrir ficha, exportar y pedir explicación de la misma revisión. | Mismos valores internos y UID; sólo varía el formato u orden solicitado. |
| FR-09 | Traspaso en febrero y carga anterior de enero. | Conserva club actual y ofrece rama o carga histórica; el cierre mantiene pertenencia. |
| FR-10 | Cambiar idioma de dos roles de igual etiqueta en distintas fases. | Ambos ID, fases, puntuaciones y orden oficial se conservan. |
