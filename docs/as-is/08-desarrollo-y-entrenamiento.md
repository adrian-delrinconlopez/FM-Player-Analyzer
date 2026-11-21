# Talento, entrenamiento y progresión

<a id="fpa-func-020"></a>

## FPA-FUNC-020 · Talento relativo al rol y la edad

En ambas versiones, con mejor DWRS b dentro del conjunto de roles considerado, edad e y límite c, usar `T=b+2(c−e)+(Determination+Work Rate−20)/4+P`. La personalidad P vale +3 para good, −5 para bad, 0 para neutral, vacía o no clasificada. Las clasificaciones de cada versión son editables y se enumeran en el capítulo 16. Para un intervalo válido usar el punto medio, para un atributo ausente/inválido usar 10 y para edad desconocida usar c. No recortar T a 0–100: superar el tope de edad resta dos puntos por año. Es una heurística de desarrollo, sin potencial oculto, simulación de minutos de partido ni pronóstico de una edad final.

El b usado por la matriz de talentos es el mejor de los roles del conjunto mostrado (mínimo inicial 0), y c su máximo de edad elegido, inicialmente 21. La planificación de excedentes usa el mejor de los asignados y c=20 de campo, 25 GK por defecto. La ficha individual se apoya en el ranking de roles disponibles de la ficha. La cifra puede por ello diferir para el mismo jugador. Ejemplo ejecutado: b=70, e=18, c=24, ambos atributos a 15, Model Citizen → 70+12+2,5+3=87,5. [Talento](../../old_version/desktop/src/core/TalentEngine.cpp) · [Caso de referencia](../../old_version/desktop/src/tests/test_talentengine.cpp).

El filtro de promesas (desactivado al iniciar) exige edad conocida y ≤límite (15–24 seleccionable), y suma de los **puntos medios importados** de Determination y Work Rate ≥ umbral (0–40, inicialmente 20). Aquí ausente aporta 0, aunque el mismo dato para la puntuación T valga 10. Si se pide, exigir personalidad good. Ámbito doméstico si alguna nacionalidad coincide; extranjero requiere principal conocida y ninguna coincidente con el país propio configurado. Ejemplo: desconocido en ambos y personalidad buena da término mental cero en T, pero queda excluido del filtro inicial 20. [Selección y puntuación en la matriz](../../old_version/desktop/src/app/pages/SquadMatrixPage.cpp).

<a id="fpa-func-021"></a>

## FPA-FUNC-021 · Ganancia marginal por atributo

El rol entrenable sale de la intersección de los roles de la táctica (ambas fases) con los asignados al jugador, o con los deducidos de sus posiciones si aún no hay asignación. Preferir el objetivo manual si pertenece al conjunto; si no, mayor DWRS entre las opciones en su orden natural, conservando el primero en empate (valor ausente tratado como 0 al elegir). Sin opciones falta el rol objetivo.

Sobre el perfil activo calcular primero el DWRS y la cobertura. Sin valoración no hay ganancia. Para curva, `gᵢ=100/K × media[Cᵢ(min(20,x+1))−Cᵢ(x)]` para todos los x enteros del intervalo. En Classic compilado, tomar la diferencia de la contribución interpolada en el punto medio de ambos extremos incrementados y limitados a 20 frente al punto medio original, con K=D. En la vía Classic histórica sin perfil compilado, g es constante `100(peso_categoria fᵢ/n)/(U−L)`. Una meseta o nivel 20 puede dar cero aunque el atributo sea importante. El cálculo usa la ganancia anterior al redondeo visible del DWRS.

Con banda de desarrollo de peso w calcular factor d por edad. Si sólo mentoría, 0. Si edad de máximo ≥50, 1 vitalicio. Para los demás, d=1 hasta el máximo inclusive, 0 desde el bloqueo, y (bloqueo−edad)/(bloqueo−máximo) entre ambos. Con límites iguales es un escalón. Inicialmente explosividad 21/24, fuerza 23/25, técnica 21/27, mental vitalicia, carácter sólo mentoría. La asignación atributo→banda está en el [diccionario](17-parametros-y-atributos.md). La edad almacenada 0 entra directamente en esta rampa.

Prioridad del perfil activo = gᵢ d w. La vía histórica añade factor max(0,20−media_atributo). Nunca aplicarlo dos veces. Omitir desconocidos, atributos sin banda y prioridades no positivas. Para mentoría conservar atributos con margen y ganancia positivos aunque d=0. Ordenar entrenamiento por prioridad descendente, g descendente y valor actual redondeado ascendente, mentoría sólo por g descendente. Señalar forma limitante si 0<calificación_media<7. Ejemplo manual, un único atributo lineal de 1 a 20 a nivel 10 aporta 100/19 por incremento. Banda explosiva y edad 22 → d=2/3, prioridad con w=1 de 3,50877. A 24 sale de los entrenables. [Ganancias](../../old_version/desktop/src/core/RatingSnapshot.cpp) · [Perfil y prioridad](../../old_version/desktop/src/core/TrainingAdvice.cpp) · [Selección de objetivo](../../old_version/desktop/src/app/pages/PageHelpers.h).

<a id="fpa-func-022"></a>

## FPA-FUNC-022 · Recomendar un método y un plan

Para cada método de la tabla del capítulo 17, sumar prioridades positivas de los atributos entrenables coincidentes **por nombre exacto**. Excluir rehabilitación. Sólo los métodos cuyo nombre empieza por GK compiten para un rol de portería; para los de campo se usan los demás. No se divide por número de atributos. Omitir suma cero, ordenar por suma descendente, y después alfabéticamente por nombre. Listar los atributos contribuyentes por prioridad. «Todos mentales» significa que todos los contribuyentes tienen banda vitalicia; «limitado» si alguna banda no vitalicia tiene d<0,999. Ejemplo manual, Pace=2 y Acceleration=1 en prioridad → Quickness de Attributes obtiene 3; el método homónimo de Injury Rehab nunca participa. El plan de equipo reúne resultados individuales para primer y segundo club por la táctica elegida. [Métodos](../../old_version/desktop/src/core/TrainingAdvice.cpp) · [Plan](../../old_version/desktop/src/app/pages/TrainingPlanPage.cpp).

### Elección del rol en el plan de equipo

Intersectar roles asignados con los de la táctica, incluyendo las dos fases en FM27. Sólo cuando no hay ninguno asignado se toma la unión de roles de las posiciones del futbolista. Si sigue siendo válida, gana la elección de entrenamiento guardada. En otro caso tomar la mayor DWRS de las opciones en orden natural, usando cero por ausencia de nota y conservando la primera en empate. Si no hay opciones, la persona sigue en el plan de su club pero sin rol. Con rol, mostrar hasta tres métodos en orden de prioridad. Ejemplo: si el rol guardado R2 desaparece de la táctica y entre R1=68 y R3=65 ambos son válidos, el plan usa R1. [Elección](../../old_version/desktop/src/app/pages/PageHelpers.h) · [Resultado por club](../../old_version/desktop/src/app/pages/TrainingPlanPage.cpp).
