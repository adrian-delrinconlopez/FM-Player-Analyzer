# Posiciones, compatibilidad y roles

<a id="fpa-func-004"></a>

## FPA-FUNC-004 · Convertir posiciones y asignar roles

FM24 reconoce 65 roles y FM27 reconoce 79. Ambos recursos suministran 14 posiciones para asignar roles. Las filas de [la tabla completa](16-tablas-de-roles-y-tacticas.md) especifican fase, pertenencia e importancia de cada atributo. FM24 incorpora funciones y cometidos en el identificador. FM27 distingue `ip` (con posesión) y `oop` (sin posesión). Dos roles con el mismo nombre visible pueden tener identificadores y atributos distintos. Los nombres ingleses conservan su significado dentro del catálogo.

Leer cada grupo separado por comas, con bases en mayúscula separadas por barras y opcionalmente lados R, L o C entre paréntesis. Emitir el producto bases × lados y deduplicar. Sin lado, ST equivale a ST (C). Descartar grupos que incumplan la sintaxis. La asignación automática toma la unión de los roles de esas posiciones y la añade a lo ya asignado en orden léxico, conservando las asignaciones manuales. Un fallo al guardar restaura lo anterior. Una primaria que ya no figura entre las asignadas pierde su efecto. Por ejemplo, D/WB (R), ST produce D (R), WB (R), ST (C). El conjunto resultante incluye los roles de las tres filas inversas del catálogo.

| Puesto táctico | Posición necesaria del jugador |
|---|---|
| GK | GK |
| DL / DR | D (L) / D (R), respectivamente |
| WBL, DML / WBR, DMR | WB (L) / WB (R) |
| DCL, DC, DCR | D (C) |
| DMCL, DMC, DMCR | DM |
| ML / MR | M (L) / M (R) |
| MCL, MC, MCR | M (C) |
| AML / AMR | AM (L) / AM (R) |
| AMCL, AMC, AMCR | AM (C) |
| STL, STC, ST, STR | ST (C) |

La geometría de elegibilidad se comparte en las dos versiones. Un buen DWRS en un rol asignado a mano no habilita un puesto incompatible. Por ejemplo WB (R) solo no habilita DR. La posición natural modifica el peso de selección, con multiplicador inicial 1. En lectura directa se usan competencias ≥15 para posiciones y exactamente 20 para naturales cuando el conocimiento permite importarlas. Se respeta la elección manual de naturales, incluido vaciarla expresamente. [Expansión](../../old_version/desktop/src/core/Utils.cpp) · [Correspondencias](../../old_version/desktop/src/core/PitchGeometry.cpp) · [Asignación aditiva](../../old_version/desktop/src/core/RoleAssignment.cpp).

<a id="fpa-func-005"></a>

## FPA-FUNC-005 · Diagnóstico individual del rol

Para cada atributo positivo del jugador utilizar la media del intervalo. Un key es fortaleza desde 15, debilidad hasta 9. Para preferable los límites inclusivos son 16 y 7. Desde 18 se marca élite. Opcionalmente incluir niveles globales altos que todavía no figuren entre las marcas, con los umbrales de key. La prueba del informe para portero usa literalmente GK-D, SK-D, SK-S, SK-A, por lo que los roles de portero FM27 pasan por los niveles globales de campo en esta parte opcional. Personalidad buena/mala añade una entrada favorable/desfavorable. Orden por personalidad, key, global, preferable y después valor descendente en pros y ascendente en contras. Ejemplo, key 14–16 → fortaleza 15, preferable 8 → sin observación.

Los mejores roles individuales toman únicamente valoraciones disponibles de roles asignados, truncan la normalizada a entero, ordenan estable de mayor a menor y conservan los primeros cinco por defecto. Un empate tras truncar mantiene el orden de asignación. [Diagnóstico y ranking](../../old_version/desktop/src/core/RoleAnalysis.cpp) · [Umbrales](../../old_version/desktop/src/core/RoleAnalysis.h).

### Comparar personas dentro de un mismo rol

El análisis de un rol selecciona sólo personas que lo tienen asignado y cuentan con nota de ese rol. Aplica el filtro de personalidad y reparte por primer club, segundo club y resto. Orden inicial por DWRS descendente. Muestra absoluta y normalizada separadas. El grupo denominado de ojeados aquí comprende el resto de fichas que pasan los criterios y puede incluir externos sin informe de ojeo. [Comparación por rol](../../old_version/desktop/src/app/pages/RoleAnalysisPage.cpp).
