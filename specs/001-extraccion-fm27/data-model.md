# Modelo de datos · SK-01

El grano de la evidencia es una observación por campo de un futbolista en un
corte y contexto confirmados. El validador consume el mismo modelo para las dos
vías y para fixtures sintéticas. [Especificación](spec.md) ·
[Contrato de captura](contracts/captura.md)

## Entidades y relaciones

| Entidad | Campos y relaciones |
| --- | --- |
| Contexto de captura | Edición FM27, compilación exacta, distribución e idioma cuando se verifican, instancia efímera, sesión, referencia local a la copia, identidad de carrera verificada o confirmación expresa de copia y corte, rama, perspectiva, club dirigido y fecha del juego. Incluye el ámbito y la revisión de política efectivos. Una recarga exige confirmar de nuevo el vínculo. |
| Política efectiva | Ámbito base de plantilla y conocidos u ojeados. Autorizaciones independientes para mundo, atributos ocultos y CA/PA con revisión por sesión. El contraste de CA y PA se acredita por separado, sea cual sea el diseño posterior de sus controles. |
| Muestra | Identificador de lote, esquema, contexto, vía y revisión del adaptador, instante UTC, límites y manifiesto de cierre. Contiene identidades autorizadas y observaciones. A-01 fija por anticipado la muestra esperada, por separado del contenido recuperado. |
| Futbolista | UID de origen textual con prefijos, tipo de fuente y carrera. El vínculo de club usa ID o asociación confirmada cuando hay evidencia. «Sin club» requiere confirmación explícita. Nombre, etiqueta de club o coincidencia de UID entre carreras son insuficientes para conciliar. |
| Observación | Jugador, clave de campo y calificador si aplica (por ejemplo, posición y lado), tipo y unidad o escala, valor sólo cuando corresponda, estados y procedencia del dato y de la evidencia de visibilidad. Hereda el contexto común de la muestra. Incluye referencia a la revisión del catálogo si se usa para interpretar el campo. |
| Comparación | Referencia independiente, entidad y campo comparables, esperados, recuperados, coincidencias, discrepancias, alcance, duración, memoria pico y clase de evidencia. La acreditación de dato y de cobertura se registra por vía, compilación, perspectiva y familia o campo. |

## Estados y reglas

| Dimensión | Regla |
| --- | --- |
| Conocimiento del mánager | Exacto, intervalo inclusivo o desconocimiento explícito. Una etiqueta posicional se mantiene como categoría. Un valor numérico requiere escala cotejada. |
| Observación por la vía | Observado, ausente de la vista o no disponible en la vía o compilación. Ausente significa que la vista no observó el campo. Existencia del dato en el juego se acredita con evidencia independiente de la mera capacidad técnica de lectura. |
| Autorización | Aceptar el valor sólo si ámbito, campo, perspectiva, capacidad y permiso lo admiten. Para un campo restringido sólo se informa un estado genérico si hacerlo está autorizado. Omitir identidades o campos de visibilidad dudosa. |
| Acreditación del campo | Acreditada para el contexto contrastado, parcial si sólo un subconjunto o nivel de precisión cumple, sin acreditar si falta evidencia o persisten discrepancias. CA y PA se evalúan individualmente. |
| Cobertura del universo | Completa con referencia independiente comparable, total reconciliado y ninguna omisión o duplicado conocido. Si existe inventario independiente, exige igualdad de conjuntos de UID. Parcial para un subconjunto con alcance contrastado. Indeterminada cuando no hay base para acotarla. |

Una observación con intervalo conserva ambos extremos ordenados en la misma
unidad. Ausencia, desconocimiento, incompatibilidad y restricción conservan
su causa sin introducir ceros ni valores por defecto. Con permiso base, el exacto
bajo máscara carece de autorización aunque otra vía lo ofrezca. Para las
comparaciones experimentales de CA/PA y exactos ocultos, los valores se conservan
sólo en memoria durante el contraste autorizado. El informe guarda el resultado
y el motivo, y los valores quedan fuera de la muestra y de los diagnósticos.

## Identidad, publicación y repetición

- **Antes de leer.** Comprobar instancia, sesión, corte, ámbito y política
  efectiva. Distinguir versión o familia incompatible de una vista válida
  pero parcial. Mantener en revisión identidades o campos sin contraste suficiente.
- **Antes de publicar.** Revalidar contexto, manifiesto y revisión de permisos.
  Clave de unicidad por contexto, tipo de fuente y UID para jugadores, y además
  clave de campo y calificador para observaciones dentro de un lote. Una
  discrepancia de identidad impide certificar la fila. Una discrepancia de
  campo impide certificar el campo afectado.
- **Si se repite.** La misma clave de lote y mismo contenido normalizado tienen
  un solo efecto. La misma clave con contenido distinto se rechaza. Una repetición
  de A-01 con otra clave compara el corte, los valores, estados y procedencia,
  dejando aparte sólo los metadatos que cambian legítimamente entre ejecuciones.
- **Si cambia el contexto o hay un fallo.** Descartar resultados pendientes
  afectados por recarga, cambio de instancia, carrera, rama, perspectiva, corte
  o permiso. Si se interrumpe o queda incompatible una vía, la última muestra
  válida se consulta únicamente en su contexto y bajo los permisos vigentes, con
  fecha y causa. Una vía independiente se valida por separado.

Si se ensaya una combinación, se exige coincidencia verificada de carrera, rama,
perspectiva, UID, corte y permiso. La evidencia de conocimiento puede proceder de
una fuente distinta de la que observó el valor y ambas se conservan. Una asociación
dudosa o una discrepancia sin resolver deja el dato sin certificar.
[Modelo funcional TO-BE](../../docs/to-be/03-modelo-funcional-de-datos.md) ·
[Política de visibilidad](../../docs/to-be/13-visibilidad-y-juego-limpio.md)
