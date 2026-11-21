# Protocolo de rendimiento y memoria

Objetivos **PROPUESTOS**, pendientes de validar. Registrar CPU, RAM, GPU/driver, SSD, resolución/escala, versión y carga de FM27, Windows, runtime, paquete y commit. Referencia candidata: Windows 11 x64, 16 GiB, SSD, CPU de seis núcleos y pantalla de 60 Hz. Publicar otra serie cuando cambie el equipo.

**Generador futuro.** Semilla 2701; tamaños de 10.000, 50.000, 100.000 y 150.000 personas. 47 atributos con 60 % exactos, 25 % rangos y 15 % desconocidos; 100 clubes, 10 competiciones, 1.000 seguidos, 36 cierres y 10 % de seguidos con cambios por mes. Dos carreras con UID solapados, índice gráfico opcional de 50.000 entradas. Guardar huella de fixture/perfil y repetir idéntica secuencia en Release x64. Separar caché fría/caliente y con/sin juego. Medir con reloj monotónico, ETW y contadores del proceso y GPU.

| Medida | Operación y repeticiones | Objetivo inicial a 150.000 |
|---|---|---|
| Arranque | 10 aperturas por condición hasta primera página interactiva | p95 ≤3 s; comprobar cómo escala la memoria inicial frente a 10.000. |
| Filtro y orden | 50 consultas selectivas y 50 amplias por edad, posición, salario y rol. Cambiar consulta en curso. | p95 ≤250 ms desde fin del intervalo de tecleo hasta página válida. Cero generaciones obsoletas publicadas. |
| Sincronización | 5 cargas iniciales y 20 lotes del 1 %, separando captura, transferencia, validación y commit | Inicial ≤120 s y lote incremental p95 ≤3 s con fuente sintética. Medir por separado la captura real y su coste en FM. |
| Cálculo | 50 XI de dos fases con 60 elegibles; 20 lotes de 1.000 jugadores × 79 roles | XI p95 ≤2 s, lote de notas p95 ≤5 s, respuesta a cancelación ≤300 ms. Registrar refinamientos agotados. |
| Desplazamiento | 60 s por tabla, con y sin fotos, tres repeticiones | p95 de fotograma ≤20 ms, <1 % de fotogramas >50 ms y ≤500 filas en ventana de datos. |
| Memoria, CPU y GPU | 100 ciclos listado → ficha → táctica → gráfico → listado, luego una hora de sesión mixta; muestrear cada segundo y tras 60 s en reposo | Privada estabilizada ≤400 MiB; aumento final en reposo ≤30 MiB. CPU inactiva ≤2 % del equipo y aumento GPU ≤16 MiB tras estabilizar. |
| Base de datos | 100 lotes idénticos y después 36 meses de cambios. Medir datos, índices y WAL tras checkpoint comparable | Cero hechos duplicados, crecimiento proporcional a cambios y cierres fijados recuperables. Registrar bytes/cambio para acordar retención. |

Informar p50/p95, peor caso, N, frío/caliente y referencia de FM en reposo equivalente, incluyendo variación de sus fotogramas durante captura. Percentil por rango más cercano (ceil(p·N)). Ante incumplimiento guardar traza, plan SQL, mayor coste, ajuste propuesto y repetición comparable. Estos valores no son medidas del producto.
