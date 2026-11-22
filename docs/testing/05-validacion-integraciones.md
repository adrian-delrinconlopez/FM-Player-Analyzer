# Pruebas de integración y recuperación

| Frontera | Estímulo | Salida exigida |
|---|---|---|
| FM27 → captura autorizada | Misma fecha y compilación, jugador propio, parcialmente ojeado y desconocido. Comparar pantalla y lote por cada vía candidata. | Todo dato publicado respeta lo visible y el permiso vigente. Personas no visibles quedan fuera de agregados. |
| Captura híbrida | BepInEx aporta un intervalo 12–14 y memoria un exacto dentro del rango. Repetir con corte o identidad incompatibles. | Se conserva el intervalo y la procedencia de la observación autorizada. Se descarta la combinación cuando no se puede verificar el mismo contexto. |
| Identidad | Dos carreras del mismo club con UID compartido; cambiar entre páginas | Pausa o confirmación de nuevo vínculo. Cero observaciones cruzadas. |
| Compatibilidad | Cambiar versión mayor, build, idioma no validado o semántica de columna | Bloqueo o degradación explícita por capacidad; conservar última revisión válida. |
| Transporte | Reordenar páginas, truncar marco, duplicar lote, desconectar antes y después de commit | Estado íntegro anterior o nuevo y confirmación idempotente. |
| Tabular | Misma vista FM27 HTML y CSV con rangos y decimal local; una fila sin identidad confirmada | Igual proyección para datos válidos y revisión de la fila dudosa. |
| Persistencia | Copia con cambio confirmado en WAL, disco lleno, migración interrumpida y restauración temporal | Último cambio confirmado incluido, cero relaciones huérfanas y base anterior recuperable. |
| Recursos | Dos homónimos, retrato y escudo con mismo UID, conflicto, XML inválido y raíz movida | Resolver por tipo e ID, sustituto en fallo e índice previo utilizable al cancelar. |
| OpenAI simulado | Sin permiso, herramienta desconocida, 401, 429, revocación en curso y stream interrumpido | Sin transferencia desautorizada, error tipado y resto de la aplicación operativo. Ningún cargo automático por otra vía. |
| OpenAI real optativo | Con cuenta elegible y consentimiento ejecutar PC-09 con fixture ficticia y desconectar | Un modelo disponible, herramienta permitida, fin de transmisión y revocación sin secretos en logs. |

Para pruebas reales anotar tienda, compilación, idioma, alcance y fecha de juego. Trabajar con una copia de prueba y evidencia anonimizada. Resultados de otro editor o dobles simulados no acreditan otra build. Reportar omisiones por falta de partida o cuenta separadas de los aprobados.
