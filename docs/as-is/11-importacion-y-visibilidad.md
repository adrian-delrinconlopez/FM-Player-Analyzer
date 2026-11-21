# Importación, actualización y conocimiento

## FPA-FUNC-024 · Importación tabular y actualización individual

FM24 admite HTML y CSV. FM27 acepta además .fm, y los CSV/HTML del FM View Exporter. En el lector por archivo, extensiones csv y fm identifican sus respectivos formatos, las demás se intentan como HTML. El HTML usa la primera tabla y requiere cabeceras en la primera fila, quita etiquetas anidadas y decodifica entidades. El CSV prueba UTF-8 (BOM opcional) y después Windows-1252. En la primera línea, fuera de comillas, escoge tabulador si supera ambos competidores, punto y coma si hay al menos uno y no menos que comas, y coma en el resto. Admite campos entre comillas, comillas dobles escapadas y saltos internos. Omite filas en blanco y contabiliza las de longitud distinta.

Para una carga masiva exigir al menos diez columnas. En FM27, Name → Player y Rec. → Recommendation cuando falta la cabecera canónica. Aplicar alias de cabecera, exigir UID y Name en FM24 o Unique ID y Player en FM27. Conservar la primera cabecera repetida. Las columnas desconocidas y opcionales ausentes generan avisos separados. Registration e Information no se aplican a la ficha. Deduplicar por UID y conciliar como en [Identidad](02-modelo-funcional-de-datos.md). Partir de la ficha anterior, aplicar únicamente las columnas presentes. Una celda presente puede borrar la información anterior, salvo reglas específicas como la personalidad vacía. La ficha actual sigue el orden de importación aunque se importe después un archivo con fecha de juego anterior. Las instantáneas mensuales sí comparan fechas de juego.

Guardar fichas, cambios de identidad, sello de observación y registro de carga en una transacción. Después, si se pidió, completar roles y recalcular para los UID importados o que adquirieron roles. Un error en esa fase posterior se informa por separado del resultado de importar fichas. La actualización individual exige exactamente una fila y que el UID objetivo confirmado exista. Aplica las columnas de la fila incluso si su UID difiere, conserva el UID objetivo y toma los naturales de la entrada asociada al UID de origen. Refresca sin avanzar el contador global y permite actualizar naturales y caducar exenciones por edad. Ejemplo, segunda carga FM24 con diez cabeceras (UID, Name, Age y otras siete, sin Pace) cambia 20→21 sin tocar el intervalo de Pace. Si además llega Pace='-', el intervalo se pierde. [Lector tabular](../../old_version/desktop/src/core/ImportTable.cpp) · [CSV](../../old_version/desktop/src/core/CsvReader.cpp) · [Importación y actualización](../../old_version/desktop/src/core/HtmlImporter.cpp) · [Secuencia completa](../../old_version/desktop/src/core/ImportPipeline.cpp).

## FPA-FUNC-025 · Qué información entra desde una partida FM27

La fuente se lee sin escribir en ella. Se exige versión que empiece por 27., fecha de juego, jugadores identificables y tabla de clubes reconocida. Rechazar si menos del 95 % de los registros tienen persona, menos del 95 % UID público o menos del 50 % club. La revisión 2.3.1 incorpora el cambio de tabla de clubes de 27.1. Los atributos visibles se escalan mediante el máximo entre 1 y la división entera de (bruto+2)/5. Se elige carrera femenina si hay más jugadoras que jugadores con club y entrada en el conocimiento, masculina en empate o sin datos. Se identifica el club propio entre los de nombre corto coincidente por mayor número de jugadores de ese grupo. Una ficha requiere persona, UID público, nombre y pertenencia al grupo de la carrera.

| Precedencia con máscara activa (predeterminada) | Atributos | Naturales | En conjunto de ojeados para histórico |
|---|---|---|---|
| Club propio o club que alinea al jugador igual al propio | Exactos | Sí | No por esta condición |
| Reputación mundial ≥8000 | Exactos | Sí | No por esta condición |
| Conocimiento k≥78 | Exactos | Sí | Sí |
| 56≤k<78 | Parte en intervalos | No se actualizan | Sí |
| 0≤k<56 | Los 47 en intervalos | No se actualizan | No |
| Sin entrada k=−1 | Los 47 en intervalos si tiene club. Sin club se omite | No se actualizan | No |

Sin máscara, incluir a todos los del grupo con club, con valores exactos, sueldo y valoración exactos a la precisión del formato. Un agente libre ojeado puede por tanto entrar **con** máscara y ser omitido al desactivarla. En ambas políticas, nombre, club, edad, país, posiciones ≥15, estatura, fuerza de los dos pies, tiempo pactado y media de temporada pueden entrar aunque se desconozcan sus atributos. Con máscara sólo el club propio o conocimiento real ≥100 permite derivar la personalidad. Fama por sí sola no activa esa etiqueta. La personalidad se deduce de rasgos ocultos y dos visibles con la tabla ordenada del [anexo](18-parametros-de-visibilidad.md). Los rasgos ocultos individuales no salen en la fila. El código consulta **CA y PA para validar registros**, sin guardarlos como CA/PA del jugador ni usarlos como puntuación de talento, DWRS o fichajes. La reputación sí decide visibilidad.

**Consecuencia para el usuario.** La partida proporciona el valor real y el analizador construye un rango determinista que lo contiene. También incluye jugadores con club sin informe alguno. Se puede ordenar ese conjunto por DWRS, que usa esos intervalos. La promesa de ver sólo lo que se ve en el juego no establece equivalencia de candidatos, rangos ni etiquetas con FM27. Con máscara desactivada, los visibles reales entran aunque no exista informe. Volver a activarla cambia la próxima lectura, y no elimina por sí solo observaciones históricas anteriores. La igualdad con una sesión de FM para un observador concreto queda **NO VERIFICADA**. [Validación de registros](../../old_version/desktop/src/core/Fm27Save.cpp) · [Selección y filas](../../old_version/desktop/src/core/Fm27SaveImport.cpp) · [Casos sintéticos](../../old_version/desktop/src/tests/test_fm27save.cpp).

### Construcción reproducible de intervalos

Con 56≤k<78 enmascarar `limitar(redondeo_mitad_arriba(20−0,55 max(0,k−60)),0,20)` atributos. Para escogerlos, ordenar por índice de prioridad inicial + 10u(UID,atributo,3), después por identificador de atributo. Usar prioridad de portero si competencia GK≥15. En el [anexo de visibilidad](18-parametros-de-visibilidad.md) se especifican los dos órdenes y los identificadores necesarios para reproducirlos.

Función u: formar el entero de 64 bits `x=(UID << 20) XOR (atributo << 8) XOR sal`. Sumar 0x9E3779B97F4A7C15, sustituir x por `(x XOR (x >> 30)) × 0xBF58476D1CE4E5B9`, luego por `(x XOR (x >> 27)) × 0x94D049BB133111EB`, y luego por `x XOR (x >> 31)`. La aritmética es módulo 2⁶⁴, desplazamiento lógico. Devolver (x >> 11)/2⁵³.

| Conocimiento | Ancho hi−lo según u(UID,atributo,1), de menor a mayor |
|---|---|
| Ausente | 4 si u<0,30, 5 si u<0,65, 6 si u<0,90, si no 7 |
| Por debajo de 56 | 2 si u<0,35, 3 si u<0,80, si no 4 |
| Parcial | 1, 2, 3, 4 o 5 según cortes 0,13, 0,56, 0,89, 0,99. Si k≥70 restar uno sin bajar de 1 |

Para ancho w y valor real v, calcular `lo=v−min(w,⌊u(UID,atributo,2)(w+1)⌋)`, hi=lo+w. Desplazar el intervalo completo hasta quedar entre 1 y 20. A k=62 se ocultan 19 de los 47 atributos, a 77 once (20−9,35=10,65), a 78 ninguno.

### Contrato y cantidades

Escoger contrato del club actual o del que lo alinea, excluyendo comienzos futuros. Preferir contrato con estatus identificable y después el de inicio más reciente. La tasa de presentación es 1,1641 euros por libra. A partir de un millón mostrar M, de mil K, una decimal hasta diez millones o diez mil respectivamente, después cero, suprimiendo ceros finales. Por debajo de mil redondear a entero. Valores ≤0 quedan vacíos, sueldo cero se admite.

Para valor monetario z y sal s, tomar `r=minRatio+(maxRatio−minRatio)u(UID,900+s,11)`, `p=0,15+0,7u(UID,950+s,12)`. Mostrar los extremos formateados de `z/r^p` y `z r^(1−p)`, uno solo si coinciden al formatear. Propio: sueldo exacto, valor s=0 y ratio 1,1…1,35. Externo: sueldo s=1, ratio 1,1…1,4 o hasta 1,7 si k efectivo<78. Valor s=2, ratio 1,2…1,7 o 1,5…3 con conocimiento parcial/menor. Famosos reciben el tratamiento de pleno conocimiento monetario. Los resultados de mercado vuelven a usar el extremo inferior presentado.

## FPA-FUNC-026 · Seguimiento de la carrera

Con seguimiento habilitado por partida, buscar en la misma carpeta el .fm de fecha de modificación más reciente cuyo nombre base coincida, sin distinguir caja y quitando el sufijo « (vN)». Por ejemplo, Career (v02).fm entra en la familia de Career.fm. Un nombre arbitrario de autosave no basta por sí solo. Comprobación inicial tras 1,5 s y cada 5 s. Exigir una marca posterior a la última leída, y ruta, partida, tamaño y modificación estables entre comprobaciones, con antigüedad mínima de 4 s. Esperar si hay diálogo modal o trabajo activo. Tras éxito guardar la marca y autoasignar roles. Tres fallos para la misma modificación suspenden nuevos intentos hasta cambio o comprobación explícita. Si se cambia de partida durante la lectura, se descarta su notificación y actualización de marca. [Familia de archivos](../../old_version/desktop/src/core/Fm27SaveFamily.cpp) · [Seguimiento](../../old_version/desktop/src/app/LiveSaveWatcher.cpp).

## Exportación técnica de diagnóstico de la partida

El modo tabular de la herramienta de diagnóstico produce la misma tabla sujeta a conocimiento. Su opción explícita de quitar la máscara produce la tabla exacta. Existe también una salida **bruta** de esa herramienta, distinta de la importación de la aplicación. Por defecto vuelca todos los registros decodificados y escribe edad y fecha de nacimiento, país, club, reputación mundial, conocimiento, valor, sueldo, estatus, nota, ambos pies, 15 competencias, 52 atributos exactos y ocho rasgos de personalidad. Entre los cinco atributos adicionales figuran Dirtiness, Consistency, Important Matches, Injury Proneness y Versatility. Los ocho rasgos son Adaptability, Ambition, Loyalty, Pressure, Professionalism, Sportsmanship, Temperament y Controversy. CA y PA tampoco son columnas de ese CSV. **Confirmado por código.** El fichero bruto permite conocer información oculta aunque el usuario tenga la máscara activa en su partida del analizador. [Contrato de las tres salidas](../../old_version/desktop/src/tools/fm27savedump_cli.cpp) · [Campos](../../old_version/desktop/src/core/Fm27Save.h).
