# Plantilla, profundidad y sustituciones

REQ-SQUAD-001 a 005, REQ-NAT-001/002. Partir del club y fecha de juego seleccionados. La matriz admite primer equipo, filial, cantera, cedidos y lista de seguimiento. Una persona conserva un solo ID aunque pertenezca a varias listas. Mostrar edad, posición, rol y nota, minutos disponibles, estado, contrato y vigencia de la observación.

**Recorrido de decisión.** Elegir táctica y competición; calcular A/B; abrir un puesto para ver titular, alternativa y profundidad. Detallar primer motivo de exclusión y restricciones restantes. Aplicar la [asignación de sobrantes y reserva por rol](../as-is/06-analisis-de-plantilla.md). Una alternativa utilizable en tres puestos no cubre tres bajas simultáneas. Simular una ausencia vuelve a optimizar y lista los movimientos. Mostrar media y mediana de puestos ocupados junto a vacantes; la media de profundidad cuenta opciones por puesto. Conservar los distintos denominadores originales.

**Comparador.** Entre dos y cinco jugadores, fijar mismo perfil y fecha, separar campo/portería y alternar ficha oficial, áreas y diferencias por rol. Para dos rangos [a,b] y [c,d], el delta posible segundo menos primero es [c−b,d−a]. Mostrar empate indeterminado si incluye cero. En áreas usar media de puntos medios conocidos y cobertura k/n. Una anotación manual cuenta como tal en los resultados. Al cerrar, restaurar filas, selección y desplazamiento.

## Exportación de tablas

REQ-EXPORT-001 aplica a las tablas que admitan exportación. Los primeros casos son una matriz con los atributos de la plantilla propia o de una lista concreta de jugadores. El mismo contrato se aplica a las tablas derivadas que se habiliten para exportación, por ejemplo una comparación por rol.

El usuario elige el ámbito, las columnas autorizadas y el destino. La exportación conserva los filtros y el orden del conjunto elegido e incluye todas las filas que los cumplen, aunque la interfaz muestre sólo una página. Las cabeceras son legibles y cada fila contiene los mismos datos que la consulta autorizada. En Excel los valores numéricos exactos son números. Ambos formatos conservan intervalos y distinguen un valor desconocido de cero.

El formato preferido es Excel (.xlsx). CSV es la alternativa si la salida a Excel no está disponible. Si falla la generación de Excel, informar del motivo y ofrecer CSV con la misma selección. Para ambos formatos se comprueban los permisos vigentes, incluidos los de CA/PA. Las columnas ocultas en la vista quedan fuera de la selección inicial y sólo se incluyen si el usuario las elige expresamente y tiene permiso. [Política de juego limpio](13-visibilidad-y-juego-limpio.md)

## Cambios de plantilla y selecciones nacionales

**Ciclo de plantilla.** Una actualización parcial nunca inicia el diálogo de salidas. Sólo fuente completa verificable y mismo club/fecha permiten proponer ausentes. El usuario confirma destino, venta o cesión local y revisa la limpieza de marcas. Mantener observaciones y contrato de origen como historia. Lesión y sanción tienen fecha, fuente y ámbito; si son desconocidas figuran como tales.

**Selecciones nacionales.** Se posponen el XI nacional, la convocatoria por cupo de porteros y la mejora por rol hasta disponer de nacionalidades y elegibilidad observables. Se preservan el caso G10 y la gestión manual para su futura entrega. [Operaciones de ficha y selección](../as-is/23-operaciones-con-fichas-y-selecciones.md)
