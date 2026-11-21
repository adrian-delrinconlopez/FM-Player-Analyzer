# Configuración que cambia resultados

<a id="fpa-func-032"></a>

## FPA-FUNC-032 · Partidas, copias y datos anteriores

Una partida identifica un conjunto independiente de jugadores, valoraciones, convocatoria, historial, competiciones y ajustes. Cambiar de partida sustituye todo el conjunto activo. Una partida nueva sin personalización parte de los valores iniciales. Las partidas antiguas que preceden al aislamiento de ajustes toman como semilla la configuración compartida disponible. Crear una partida **copiando ajustes** copia la configuración reutilizable y las definiciones de roles/tácticas de las versiones que posea el origen. Quedan ligados al origen sus jugadores, club propio y segundo club, identificación y distintivos del club/selección y fuente de seguimiento de FM27 (ruta, activación y última lectura). Por ejemplo, copiar ajustes de una carrera con seguimiento habilitado no programa la lectura de su siguiente autosave en la nueva.

**Duplicar una partida completa** sí obtiene una copia consistente de todos sus datos confirmados, incluidos los cambios pendientes de consolidación del diario de la base. Importar una base externa también toma una instantánea y conserva el origen. Renombrar y eliminar exigen que la partida esté cerrada. Alta de nombre: recortar extremos, longitud 1…80, sin punto al inicio o al final, caracteres de control inferiores a 32 ni barra inversa, barra, dos puntos, asterisco, interrogación, comillas dobles, menor que, mayor que o barra vertical. Rechazar nombres ya existentes sin distinguir caja. El borrado exige una copia previa satisfactoria. Las copias ordinarias conservan las tres más recientes de **ese nombre exacto**, usando contador para copias en el mismo segundo. Un fallo en la copia opcional antes de importar produce aviso. Para bases con versión interna 1…17 se hace una copia especial previa a 2.3.0 fuera de esa rotación, sin sobrescribirla al reintentar.

La conversión del formato histórico lee una base anterior, crea un destino temporal y sólo lo publica al completar. Recupera fichas, históricos de DWRS, ajustes, convocatoria y shortlist. Convierte campos antes textuales, cuenta referencias sin jugador y registra incidencias de conversión. Un histórico huérfano no crea un jugador ficticio. [Biblioteca de partidas](../../old_version/desktop/src/app/SaveLibrary.cpp) · [Aislamiento y copia](../../old_version/desktop/src/app/AppContext.cpp) · [Copias y actualizaciones](../../old_version/desktop/src/core/Database.cpp) · [Conversión histórica](../../old_version/desktop/src/core/LegacyMigrator.cpp).

### Primera carga y conversión de una base anterior

El asistente propone el juego más reciente y ofrece importar inmediatamente, con asignación automática inicialmente marcada. Al escoger un .fm también se propone seguirlo, inicialmente marcado en el asistente. La preferencia persistente de una partida sin fuente está desactivada. Tras una carga correcta se vincula la ruta si se aceptó el seguimiento. Una vez que la partida contiene jugadores puede elegirse club por los datos disponibles. [Alta de partida](../../old_version/desktop/src/app/NewSaveWizard.cpp) · [Primera configuración](../../old_version/desktop/src/app/WelcomeWizard.cpp).

El conversor anterior exige tabla de jugadores y campo Unique ID. Salta UID vacíos. Lee los atributos por nombre canónico, acepta entero 1…20 o par ascendente con guion. Desconocido o inválido queda (0,0). Las filas pueden omitir columnas añadidas en versiones posteriores. Para el lado manual sólo conserva Left o Right. Las listas históricas entre corchetes admiten cadenas con comillas simples o dobles, incluyendo comas internas. Recupera las marcas de venta y cesión a partir de enteros no cero. Al recuperar DWRS histórico elimina un signo % final y convierte el número restante sin dividir por cien. Un valor normalizado ilegible se convierte en 0. Vincula las observaciones y las dos listas por UID, omitiendo y contando huérfanos. Se omite el antiguo marcador de fusión newgen_merge_v1_complete. El informe limita las notas de corrección a 200 aunque haya más incidencias. Ante cancelación o error anterior a la publicación, no se presenta el temporal como partida terminada. Ejemplo: «72.5%» → 72,5, y «21-22» → atributo desconocido con incidencia. [Conversión histórica](../../old_version/desktop/src/core/LegacyMigrator.cpp) · [Casos sintéticos](../../old_version/desktop/src/tests/test_migrator.cpp).

### Parámetros por defecto

Los intervalos de edición siguientes corresponden a los controles de 2.3.1. Los parámetros transitorios de búsqueda y los de una competición se detallan en sus propios capítulos.

| Parámetro | Inicio | Intervalo o efecto |
|---|---:|---|
| Pesos de categoría y factores de rol Classic | [Tabla de pesos](03-valoraciones-y-formulas.md), key 1,5 y preferable 1,2 | 0…20 en el editor Classic |
| Peso por tiempo de juego pactado (APT) | 1 por condición | 0…5 en el editor, sólo en selecciones que lo aplican |
| Posición natural | 1 | 1…2 como multiplicador de selección |
| Cuota IP de la táctica FM27 | 50 % | 0…100 %, distinta de las cuotas de atributos del perfil |
| Tolerancia de puente | 0,1 | 0…1 puntos para todo el XI |
| Roles por jugador de profundidad | 2 | 1…10 |
| Juventud campo / portería | 20 / 25 años | 15…30 / 15…35 |
| Talento mínimo para cesión | 45 | 0…200 |
| Edad mínima de cesión / umbral juvenil | 17 / 18 | Ambos 14…25 |
| Desplazamiento / caída / penalización de lado | 8 / 8 / 5 | Cada uno 0…50 |
| Edad de retiro inferido / cargas completas de antigüedad | 35 / 2 | 25…60 / 1…50 |
| Selección nacional | País sin configurar, edad abierta a 99 | Límite editable 15…99, cuenta también la segunda nacionalidad |
| Liga / UEFA / porteros / presupuesto / mínimo salarial | Ninguna / apagada / 2 / 0 / 0 | [Reglas y activación](10-inscripcion-y-elegibilidad.md) |
| Historia mensual | Temporada desde 1 de julio, sólo sujetos seguidos | Fecha de inicio por partida, opción de registrar a todos |
| Agrupación de cargas | 10 minutos en el flujo normal | [Relojes y grupos](09-historicos.md) |
| Máscara y seguimiento de partida | Activada / apagado | [Prioridades de conocimiento y lectura](11-importacion-y-visibilidad.md) |
| Perfil por defecto en partida nueva | Classic FM24 / harvestgreen22 FM27 | Una partida FM27 anterior sigue su proceso de activación |

La siembra de una configuración nueva fija el bono de posición natural en 1,00. Si la clave falta al consultar una configuración incompleta, el valor de reserva del lector para ese bono es 1,05. Cambiar pesos de selección modifica quién entra en el equipo. Los pesos de valoración cambian el perfil que produce las puntuaciones. Los métodos de entrenamiento tienen una tercera familia de pesos. [Valores y persistencia](../../old_version/desktop/src/core/AppConfig.cpp) · [Límites de edición](../../old_version/desktop/src/app/pages/SettingsPage.cpp).

<a id="fpa-func-033"></a>

## FPA-FUNC-033 · Intercambiar, editar y activar perfiles DWRS

Una biblioteca permite importar, duplicar, editar, restaurar y borrar perfiles. La selección activa, su generación y la fórmula efectiva pertenecen a la partida. En una FM27 previa se conserva Classic y se ofrece mantener el perfil anterior o cambiar. Se preservan las ponderaciones personales previas en el proceso de activación. Rechazar perfil de otro juego, definición inválida o revisión obsoleta antes de confirmar la selección. Primero se guarda la selección y se recompila. Después se solicita un recálculo si cambió la fórmula efectiva y hay jugadores. Si ese trabajo posterior falla, la selección ya guardada y las notas existentes pueden quedar temporalmente desalineadas. Las valoraciones ya registradas conservan su procedencia. Renombrar un perfil no cambia su fórmula. Restaurar un perfil devuelve el contenido original conservando el nombre actual. Duplicar asigna un nuevo ID. Al insertar se rechazan ID reservados y nombres coincidentes tras recortar y comparar sin distinguir caja. El borrado de la entrada disponible conserva sus instantáneas históricas.

| Campo del intercambio JSON | Tipo y semántica |
|---|---|
| format, version | Literales fm-dwrs-profile y número 1 |
| id, name, description, origin, sourceVersion | Texto. Identificador y nombre no vacíos |
| sources, compatibleGames | Listas de textos. Compatibilidad no vacía, sólo fm24 y fm27 |
| calculationVersion | classic-midpoint-v1 o curves-uniform-range-v1 |
| keyFactor, preferableFactor | Números finitos no negativos |
| attributes | Objeto por nombre canónico. Cada valor tiene weight finito ≥0 y shape de 20 números finitos no decrecientes, primero 0 y último 1. baseline finita, cero si ausente. La suma base+peso debe ser finita |
| positions, roles | Objetos por ámbito. Cada atributo puede sobrescribir sólo baseline, weight, shape o factor. posición = juego/posición y rol = juego/fase/ID. Fase both, ip u oop |
| rolePositions | Objeto rol → ámbito de posición del mismo juego. Si existe un vínculo, se resuelve por campo global → posición → rol |
| rolePolicies, attributeEvidence | Opcionales. Política por rol con los mismos dos identificadores de cálculo. Evidencia por atributo: measured, noise-filtered, not-investigated o legacy |
| roleShare, goalkeeperRoleShare | Opcionales, 0 si ausentes, entre 0 y 0,60 inclusive |
| tierThresholds | Campo antiguo admitido e ignorado. Se rechazan las demás claves raíz ajenas al contrato |

La revisión de fórmula es SHA-256 del JSON compacto de la definición, sin id, name, description, origin, sourceVersion, sources, attributeEvidence ni tierThresholds. Se omite rolePolicies si está vacío. La revisión del documento completo sí detecta cambios de metadatos para impedir que dos ediciones se sobrescriban. La selección efectiva identifica además la versión del juego y el plan de roles compilado. Ejemplo: modificar sólo la descripción conserva la revisión de fórmula. Modificar la forma de Pace genera una revisión diferente aunque la ficha observada no cambie. [Formato](../../old_version/desktop/src/core/RatingProfile.cpp) · [Biblioteca](../../old_version/desktop/src/core/RatingProfileLibrary.cpp) · [Activación e histórico](../../old_version/desktop/src/core/RatingSelectionStore.cpp).

### Estado de la activación

La decisión de partida antigua puede ser pending (FM27 anterior con base Classic disponible), kept (el usuario mantuvo el cálculo), switched (se aplicó un perfil desde pending) o not-needed. Conservar el anterior exige pending y la misma generación. Una activación guarda el perfil completo y las definiciones, emite una generación nueva y, sólo si cambia la clave efectiva, añade una transición con claves previa y nueva, instante UTC, fecha de juego si hay, ID y revisión del perfil. Al reabrir, se comprueba que la definición archivada se pueda interpretar y que su clave coincida. Una fecha de juego suministrada pero inválida impide la transición. [Selección y archivo](../../old_version/desktop/src/core/RatingSelectionStore.cpp) · [Solicitud de recálculo](../../old_version/desktop/src/app/AppContext.cpp).

<a id="fpa-func-034"></a>

## FPA-FUNC-034 · Crear, conservar e intercambiar tácticas

FM24 almacena puesto→rol de una alineación. FM27 admite once con balón y once sin balón, con roles distintos y los puentes personales por partida. Se pueden crear, editar, renombrar, eliminar y elegir dos favoritas. Renombrar traslada referencias de favoritas y puentes. Después de editar se descartan puentes incompatibles. Las actualizaciones del catálogo respetan ediciones, nombres trasladados, borrados y roles añadidos por el usuario según su procedencia.

El fichero .fmpatactic exporta un objeto JSON con format=fm-player-analyzer-tactic, format_version=1, app_version, fm_version, exported_at (fecha UTC), tactic y roles. tactic contiene name y, según versión, lineup o los dos objetos ip y oop, más layout (línea→lista de puestos). roles contiene los roles usados, identificados por ID, con name, category, positions, key, preferable y, en FM27, phase. Por fase deben figurar exactamente 11 puestos, incluido GK, todos conocidos, cada uno con un rol no vacío. Rechazar formato inválido, versión de intercambio superior, versión de juego incorrecta, fase incompatible o nombre vacío. Si el layout no contiene exactamente los diez puestos de campo de la base, derivarlo del once base (OOP en FM27). Un rol local distinto con igual ID conserva la definición local y se informa del conflicto. Un rol desconocido se añade si tiene nombre, categoría, posiciones, al menos un key y, si corresponde, fase. En otro caso no puede importarse la táctica. Ejemplo: exportar una táctica FM27 que incluye un nuevo rol MYDM con key Tackling e importarla en FM27 conserva ambas fases y da de alta MYDM. La misma entrada en FM24 devuelve incompatibilidad. [Intercambio](../../old_version/desktop/src/core/TacticLibrary.cpp) · [Pruebas de ida y vuelta](../../old_version/desktop/src/tests/test_tacticlibrary.cpp).

Una **actualización del catálogo suministrado** y una **importación de táctica de usuario** tienen políticas diferentes para IDs duplicados. En una actualización sólo se mezcla si aumenta la revisión de datos. Se parte del catálogo nuevo y se recuperan roles marcados como personalizados que aún no existen en el nuevo catálogo, con sus posiciones, fase y marcas. Si el catálogo suministrado adopta ese mismo ID, prevalece su definición. Las tácticas editadas propias se conservan y las eliminadas no reaparecen. Cuando el catálogo renombra una táctica sin editar, traslada sus referencias. Si la antigua estaba eliminada, propaga la eliminación al nombre nuevo. En una importación independiente de .fmpatactic se conserva el rol local ante conflicto. [Mezcla de revisiones](../../old_version/desktop/src/core/Definitions.cpp) · [Regresiones ejecutadas](../../old_version/desktop/src/tests/test_tacticlibrary.cpp).

<a id="fpa-func-035"></a>

## FPA-FUNC-035 · Definir roles y modificar asignaciones

El editor de rol nuevo corresponde a FM24. Para crear uno hacen falta nombre, cometido, categoría y una o más posiciones. Formar el ID con las iniciales en mayúscula de las palabras del nombre, guion y primera letra del cometido (las dos primeras para Cover y Stopper). Rechazar ID existente. Una marca key prevalece si también está marcada preferable. Las listas de atributos pueden quedar vacías en este editor. Añadir el rol al catálogo personalizado y a las posiciones elegidas sin asignarlo retroactivamente a toda persona. Ejemplo: Deep Runner con cometido Support → DR-S si está libre. FM27 dispone de roles con fase en definiciones e intercambio.

En ambas versiones una edición por jugador sustituye sus roles por la selección guardada. Completar predeterminados añade los que falten. Restablecer todos sustituye por los de las posiciones cuando esas posiciones producen una lista no vacía. Limpiar una primaria desaparecida y recalcular los afectados. Ejemplo: con rol manual X y predeterminado Y, completar produce X e Y, restablecer produce sólo Y. [Identificador y validación](../../old_version/desktop/src/app/pages/NewRolePage.cpp) · [Cambios masivos y confirmación](../../old_version/desktop/src/app/pages/AssignRolesPage.cpp) · [Catálogos iniciales](16-tablas-de-roles-y-tacticas.md).

<a id="fpa-func-039"></a>

## FPA-FUNC-039 · Personalizar bandas de desarrollo

El perfil de entrenamiento contiene format=fmpa-training-profile, version=1 y bands ordenadas. El lector comprueba el formato y exige una lista no vacía. No discrimina por version. Cada banda exige name no vacío. peak_age parte de 21 y se limita a 14…50, locked_age parte del pico y se limita a pico…50, weight parte de 1 y se limita a 0…5. mentoring_only parte de falso. En attributes sólo se aceptan nombres canónicos y la primera aparición gana entre bandas. Una banda sin atributos es posible. El perfil se aplica al consejo individual y al plan, junto con la edad y el rol objetivo. Ejemplo: pico 60, bloqueo 30, peso 8 → 50, 50 y 5. Un atributo que queda fuera de todas las bandas no recibe prioridad de entrenamiento. [Intercambio y valores](../../old_version/desktop/src/core/TrainingAdvice.cpp) · [Cálculo resultante](08-desarrollo-y-entrenamiento.md).

<a id="fpa-func-042"></a>

## FPA-FUNC-042 · Construir una curva de aportación desde veinte puntos

Disponible en ambas versiones dentro de un perfil editable. Un perfil personal nuevo empieza con el juego actual, identificador nuevo y curva lineal de peso cero para cada atributo. En el editor se trabaja sobre un borrador. Para un atributo con base b, peso w y forma s se muestran las aportaciones p(i)=b+w·s(i), i=1…20. Al editar un punto o la serie completa, exigir todos los números finitos y p(i) no decreciente. Dejar el último estado válido si se incumple. Adoptar b=p(1), w=p(20)−p(1). Con w>0, s(i)=(p(i)−p(1))/w. Con w=0 conservar la forma previa, pues las aportaciones son constantes. Un cambio real entra en el historial de deshacer y elimina la rama de rehacer; una operación idéntica no añade paso. Restablecer en este editor recupera la curva cargada al comenzar la edición.

El preajuste lineal da s(i)=(i−1)/19. El preajuste de meseta conserva base y peso y usa s(i)=mín(1,(i−1)/(t−1)), t editable entre 2 y 20, inicialmente 18. Se pueden cambiar los factores de marcas y las cuotas de rol permitidas por la política; Classic no usa la mezcla de rol. Guardar un perfil y activarlo para una partida son acciones separadas. Ejemplo ejecutado: curva lineal con b=50, w=19, nivel 10 y factor 1,5. Aportación absoluta (50+9)×1,5=88,5; normalizada 900/19. En el rol AF-A de la prueba, Pace es preferable, factor 1,2, por lo que la absoluta pasa a 70,8 con la misma normalizada. [Edición y transformación](../../old_version/desktop/src/app/widgets/RatingCurveEditor.cpp) · [Borrador y guardado](../../old_version/desktop/src/app/widgets/RatingProfileEditor.cpp) · [Caso baselineRoundTripAndNormalization](../../old_version/desktop/src/tests/test_ratingcurveeditor.cpp).
