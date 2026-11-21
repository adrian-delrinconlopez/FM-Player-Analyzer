# Casos límite y errores observables

La misma ausencia puede producir resultados diferentes según la operación. Esta tabla indica qué dato queda almacenado, qué valor se usa y en qué momento se ve el efecto. Los ID corresponden al [catálogo](01-catalogo-funcional.md).

| ID | Desencadenante | Comportamiento observable |
|---|---|---|
| 001, 024 | Importación masiva con nueve cabeceras aunque incluya UID y nombre | Rechazo por columnas insuficientes. Con diez aún hacen falta las dos obligatorias de la versión |
| 002, 024 | Filas duplicadas por UID y cabeceras duplicadas | En filas gana la última. En columnas gana la primera. Una fila sin UID no crea ficha. Si no queda ninguna, la carga falla |
| 002 | Igual nombre, UID diferentes sin la relación numérica con r- | Mantener dos personas. Cambiar sólo el nombre para el mismo UID no implica crear una persona |
| 024 | Fichero individual con dos filas, o con una fila pero sin el UID objetivo en la partida activa | Rechazo. Si existe el objetivo y hay una sola fila, puede aplicarse aunque el UID de esa fila sea distinto del objetivo |
| 024 | Falta toda la columna Pac en una carga posterior, o está presente con guion | Con diez o más cabeceras y el resto válido, la primera conserva Pace. La segunda lo deja ausente |
| 024, 027 | Se guardaron fichas pero falla el posterior recálculo | La importación de fichas y el resultado del recálculo tienen estados separados. Consultar la última valoración disponible puede seguir devolviendo un dato anterior |
| 025 | Agente libre sin informe frente a externo en un club y sin informe | Con máscara el primero puede quedar fuera mientras el segundo entra con intervalos. Sin máscara se exige club en esa vía. La lista resultante puede cambiar en ambos sentidos |
| 025 | Reactivar máscara después de importar exactos | La siguiente lectura aplica la política activa. La acción por sí misma no purga los históricos ni todas las fichas anteriores |
| 026 | Mismo autosave todavía creciendo o nueva escritura tras tres fallos | Esperar a estabilidad. Alcanzado el límite para la modificación previa, un nuevo sello de modificación permite reintentar |
| 006, 007 | Todos los atributos ausentes | Classic con escala positiva incluye ceros y puede devolver −5. Curvas devuelve cobertura insuficiente. Un cero mostrado, una falta de valoración y un atributo ausente son estados distintos |
| 006, 007 | Todos los pesos cero | Classic histórico devuelve cero al no tener escala. Un perfil compilado puede devolver NoVariation, sin nota normalizada |
| 007, 021 | Intervalo 9–11 en una curva no lineal | En curvas se promedian los tres niveles. En Classic se evalúa el punto medio. Sólo un incremento con efecto en la curva produce ganancia marginal |
| 005, 009 | Dos notas 74,1 y 74,9 | Pueden verse iguales en un ranking que trunca. Una elección que use precisión completa puede distinguirlas |
| 009, 010 | Un candidato con la mejor nota asignada pero posición incompatible | No ocupa el puesto. Si ninguna persona cumple, queda un hueco. Una nota alta en una fase no reemplaza la inelegibilidad en la otra |
| 011 | Sustituir a un lesionado obliga a mover a otros | El plan identifica movimientos y entrante. Un bloqueo manual y el tope de grupo reducen las alternativas. La proyección se calcula sobre el conjunto disponible |
| 014 | No hay ningún jugador utilizable | Hay cero puestos cubiertos y la profundidad es cero. La mediana sin datos tiene señal de ausencia. No equivale a una plantilla compuesta por notas cero |
| 016, 018 | Un candidato mejora la comparación directa pero no puede entrar en el XI | El comparador individual y el asesor de fichajes pueden discrepar. El asesor exige efecto de recalcular el equipo y limita el mercado que ensaya |
| 003, 030 | Salario desconocido en inscripción con mínimo | No impide el alta ni aumenta el gasto, por lo que una lista teóricamente ajustada puede costar más en el juego |
| 020 | Edad y atributos de carácter ausentes | Para la puntuación de talento se sustituyen por tope de edad y 10. El filtro de promesas exige edad conocida y no hace esa misma sustitución |
| 021, 022 | Un atributo está al máximo, otro está en una banda sólo de mentoría | El primero no ofrece mejora por un punto. El segundo puede originar consejo de mentoría sin añadir prioridad a un método de entrenamiento |
| 023, 027 | Fecha de juego ausente, o dos archivos observados en orden inverso al de la carrera | Una importación sin fecha no produce observaciones mensuales fechadas. La persistencia de la ficha actual y la del último estado válido del mes siguen reglas diferentes |
| 028 | Jugador externo mayor y obsoleto en shortlist | La shortlist no impide la retirada inferida. Un sello posterior puede devolver vigencia si el club no está marcado Retired |
| 029, 031 | 21 años en automático / cero hombres inscritos / portero UEFA exento | Edad incierta consume plaza en la propuesta. Lista totalmente vacía no filtra. Portero exento no cubre el mínimo de la lista A |
| 032 | Dos copias en el mismo segundo, o dos partidas cuyos nombres comparten prefijo | Una copia recibe sufijo y la rotación se limita a coincidencia del nombre entero. Eliminar exige copia satisfactoria |
| 033 | Perfil de otro juego, rol de ámbito inválido, edición con revisión anterior | La activación, compilación o actualización correspondiente falla. Renombrar sin cambiar la fórmula sí conserva la equivalencia numérica |
| 034 | Falta un puesto en una fase o falta definición de rol nuevo | Abortar importación de la táctica. Un layout incoherente con un once válido se puede reconstruir |
| 036, 037 | Grupo sin edades positivas o categoría de comparación sin datos | En selección la edad media se presenta ausente. En comparación un grupo sin atributos devuelve cero |
| 040 | Faltan miembros de ambos clubes en la carga marcada como plantilla | Se proponen salidas individuales. Cancelar mantiene sus clubes. Esto es independiente del sello de vigencia de los presentes |
| 041 | Falta un recurso distribuido o no se puede escribir uno de los archivos | Se informa del grupo realmente copiado, o de error si ninguno pudo copiarse |
| 042 | Un punto editado cae por debajo del anterior | Se rechaza la serie y se conserva la última curva válida. Un cambio aceptado seguido de otro invalida el historial de rehacer de la rama anterior |

## Comportamientos conservados que afectan a la equivalencia

- La inscripción suiza comprueba tanto el cupo de lista como el de titulares. Los dos controles pueden dar respuestas distintas sobre el mismo jugador. [Inscripción y competiciones](10-inscripcion-y-elegibilidad.md).
- Un resultado combinado por fases puede perder decimales al aparecer como celda del XI. Al comparar primero hay que elegir si se comprueba la nota del jugador por fase, la combinación precisa, la celda o la media de esas celdas. [Alineación](05-tacticas-y-optimizacion.md).
- Al importar un nombre de personalidad vacío se puede conservar el anterior. Una ficha con personalidad visible después de la carga puede proceder de una lectura previa más informativa. [Diccionario](02-modelo-funcional-de-datos.md) · [Visibilidad](11-importacion-y-visibilidad.md).
- Una serie DWRS arrastra el último dato entre sesiones y distingue recálculo y mejora deportiva. Una observación mensual registra cambios en otra escala temporal. La ausencia de una fila intermedia en uno de los dos históricos es esperable. [Históricos](09-historicos.md).

El [registro de pruebas](21-ejecucion-de-las-pruebas.md) permite distinguir qué condiciones tienen ejecución propia. La [matriz](15-trazabilidad-y-lagunas.md) identifica los casos sin contraste externo y las afirmaciones sustentadas en lectura del comportamiento.
