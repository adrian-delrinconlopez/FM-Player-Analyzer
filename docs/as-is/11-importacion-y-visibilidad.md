# Importación, actualización y conocimiento

<a id="fpa-func-024"></a>

## FPA-FUNC-024 · Importación tabular y actualización individual

FM24 admite HTML y CSV. FM27 acepta además .fm, y los CSV/HTML del FM View Exporter. En el lector por archivo, extensiones csv y fm identifican sus respectivos formatos, las demás se intentan como HTML. El HTML usa la primera tabla y requiere cabeceras en la primera fila, quita etiquetas anidadas y decodifica entidades. El CSV prueba UTF-8 (BOM opcional) y después Windows-1252. En la primera línea, fuera de comillas, escoge tabulador si supera ambos competidores, punto y coma si hay al menos uno y no menos que comas, y coma en el resto. Admite campos entre comillas, comillas dobles escapadas y saltos internos. Omite filas en blanco y contabiliza las de longitud distinta.

Para una carga masiva exigir al menos diez columnas. En FM27, Name → Player y Rec. → Recommendation cuando falta la cabecera canónica. Aplicar alias de cabecera, exigir UID y Name en FM24 o Unique ID y Player en FM27. Conservar la primera cabecera repetida. Las columnas desconocidas y opcionales ausentes generan avisos separados. Registration e Information no se aplican a la ficha. Deduplicar por UID y conciliar como en [Identidad](02-modelo-funcional-de-datos.md). Partir de la ficha anterior, aplicar únicamente las columnas presentes. Una celda presente puede borrar la información anterior, salvo reglas específicas como la personalidad vacía. La ficha actual sigue el orden de importación aunque se importe después un archivo con fecha de juego anterior. Las instantáneas mensuales sí comparan fechas de juego.

Guardar fichas, cambios de identidad, sello de observación y registro de carga en una transacción. Después, si se pidió, completar roles y recalcular para los UID importados o que adquirieron roles. Un error en esa fase posterior se informa por separado del resultado de importar fichas. La actualización individual exige exactamente una fila y que el UID objetivo confirmado exista. Aplica las columnas de la fila incluso si su UID difiere, conserva el UID objetivo y toma los naturales de la entrada asociada al UID de origen. Refresca sin avanzar el contador global y permite actualizar naturales y caducar exenciones por edad. Ejemplo, segunda carga FM24 con diez cabeceras (UID, Name, Age y otras siete, sin Pace) cambia 20→21 sin tocar el intervalo de Pace. Si además llega Pace='-', el intervalo se pierde. [Lector tabular](../../old_version/desktop/src/core/ImportTable.cpp) · [CSV](../../old_version/desktop/src/core/CsvReader.cpp) · [Importación y actualización](../../old_version/desktop/src/core/HtmlImporter.cpp) · [Secuencia completa](../../old_version/desktop/src/core/ImportPipeline.cpp).

<a id="fpa-func-025"></a>

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

Un bloque de ojeo truncado, con recuento incoherente, referencia nula o conocimiento >100 se descarta **completo**. Se puede seguir leyendo la partida sin entradas de conocimiento; el analizador no rescata sólo las filas válidas de ese bloque. Ello afecta tanto a los intervalos como a la inferencia de carrera masculina/femenina. Una referencia repetida en un bloque válido toma el último nivel leído. [Validación de conocimiento](../../old_version/desktop/src/core/Fm27Save.cpp).

Primero se memoriza el ID de club cuando el nombre propio configurado coincide con el de una carga que suministra identidades de club. En cargas posteriores, si el mismo ID tiene otro nombre y el nombre anterior ha dejado de aparecer, se actualiza el club configurado. El segundo equipo sigue su ID previamente memorizado. Si el nombre anterior aún existe entre los clubes de la carga, no se infiere el cambio por semejanza de nombres. Las tablas sin mapa de IDs no provocan este seguimiento. Ejemplo: club 500 llamado Norte cambia a Norte FC con igual ID y sin club aún llamado Norte; la partida del analizador pasa a usar Norte FC como primer club. [Seguimiento de nombres](../../old_version/desktop/src/core/HtmlImporter.cpp).

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

<a id="fpa-func-026"></a>

## FPA-FUNC-026 · Seguimiento de la carrera

Con seguimiento habilitado por partida, buscar en la misma carpeta el .fm de fecha de modificación más reciente cuyo nombre base coincida, sin distinguir caja y quitando el sufijo « (vN)». Por ejemplo, Career (v02).fm entra en la familia de Career.fm. Un nombre arbitrario de autosave no basta por sí solo. Comprobación inicial tras 1,5 s y cada 5 s. Exigir una marca posterior a la última leída, y ruta, partida, tamaño y modificación estables entre comprobaciones, con antigüedad mínima de 4 s. Esperar si hay diálogo modal o trabajo activo. Tras éxito guardar la marca y autoasignar roles. Tres fallos para la misma modificación suspenden nuevos intentos hasta cambio o comprobación explícita. Si se cambia de partida durante la lectura, se descarta su notificación y actualización de marca. [Familia de archivos](../../old_version/desktop/src/core/Fm27SaveFamily.cpp) · [Seguimiento](../../old_version/desktop/src/app/LiveSaveWatcher.cpp).

## Exportación técnica de diagnóstico de la partida

El modo tabular de la herramienta de diagnóstico produce la misma tabla sujeta a conocimiento. Su opción explícita de quitar la máscara produce la tabla exacta. Existe también una salida **bruta** de esa herramienta, distinta de la importación de la aplicación. Por defecto vuelca todos los registros decodificados y escribe edad y fecha de nacimiento, país, club, reputación mundial, conocimiento, valor, sueldo, estatus, nota, ambos pies, 15 competencias, 52 atributos exactos y ocho rasgos de personalidad. Entre los cinco atributos adicionales figuran Dirtiness, Consistency, Important Matches, Injury Proneness y Versatility. Los ocho rasgos son Adaptability, Ambition, Loyalty, Pressure, Professionalism, Sportsmanship, Temperament y Controversy. CA y PA tampoco son columnas de ese CSV. **Confirmado por código.** El fichero bruto permite conocer información oculta aunque el usuario tenga la máscara activa en su partida del analizador. [Contrato de las tres salidas](../../old_version/desktop/src/tools/fm27savedump_cli.cpp) · [Campos](../../old_version/desktop/src/core/Fm27Save.h).

<a id="fpa-func-041"></a>

## FPA-FUNC-041 · Instalar las vistas y tácticas de ejemplo para el juego FM27

La distribución permite instalar en el juego dos grupos independientes de archivos .fmf. Las vistas preparan columnas de Football Manager para la exportación tabular; las tácticas permiten probar en el juego las formaciones de ejemplo. La instalación toma los recursos del grupo elegido y los copia con su nombre a views o tactics, respectivamente, bajo Football Manager 27 en los documentos del usuario. Crea la carpeta si falta, retira el fichero homónimo antes de copiar y hace escribible por el usuario el resultado. Informa de los nombres copiados. Si no puede crear la carpeta o no copia ninguno, informa de error. Puede haber éxito parcial en un grupo. Instalar las vistas no crea jugadores y a la inversa los .fmpatactic del analizador son un contrato distinto.

| Grupo | Archivo distribuido | Tamaño en bytes | SHA-256 |
|---|---|---:|---|
| Tácticas | [FM27-PlayerAnalyzer-Tactic-Control-Press-4-2-3-1.fmf](../../old_version/desktop/src/app/resources/tactics/FM27-PlayerAnalyzer-Tactic-Control-Press-4-2-3-1.fmf) | 2266853 | e2d3918e44dec2f255be354392db6cc137d48923b83151fb5a2bf486a3320f5b |
| Tácticas | [FM27-PlayerAnalyzer-Tactic-Counter-Surge-3-4-2-1.fmf](../../old_version/desktop/src/app/resources/tactics/FM27-PlayerAnalyzer-Tactic-Counter-Surge-3-4-2-1.fmf) | 2048275 | ab5091053f96579ddee176d7a95c53316c4096b4db3cc06ba8318518df058121 |
| Vistas | [FM27-PlayerAnalyzer-PlayerDatabase-Standard.fmf](../../old_version/desktop/src/app/resources/views/FM27-PlayerAnalyzer-PlayerDatabase-Standard.fmf) | 141079 | e6af1863f26aaf5a0f880c47bc1b2c27c91e58f4ef4590b1ac700f6454e5fca6 |
| Vistas | [FM27-PlayerAnalyzer-Shortlist-Standard.fmf](../../old_version/desktop/src/app/resources/views/FM27-PlayerAnalyzer-Shortlist-Standard.fmf) | 141285 | c4b591b95cd3cfc138ed5f2758f7934f60dafedf59f83402c7461213fe3af505 |
| Vistas | [FM27-PlayerAnalyzer-Squad-Standard.fmf](../../old_version/desktop/src/app/resources/views/FM27-PlayerAnalyzer-Squad-Standard.fmf) | 142903 | 2274b0624cb569afb2ac7cc8294641bd0395650740487f5daf88ac8f482c8879 |

Ejemplo: en una instalación limpia, seleccionar el grupo de vistas copia los tres Standard y no escribe las dos tácticas. La tabla permite verificar los archivos exactos de la distribución. El contenido binario de esos archivos de FM no se incluye en esta especificación. [Operación y tratamiento de errores](../../old_version/desktop/src/app/pages/SettingsPage.cpp).
