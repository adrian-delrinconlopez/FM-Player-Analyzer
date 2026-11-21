# Valoración de un futbolista en un rol

Una puntuación pertenece a la combinación jugador, rol, versión y perfil activo. La absoluta conserva las unidades del modelo. La normalizada sitúa al jugador entre dos referencias teóricas del **mismo rol**. El porcentaje no es una probabilidad de ganar ni un percentil de los jugadores importados. El optimizador consume el DWRS normalizado disponible. El informe individual de pros y contras tiene sus propias reglas.

## FPA-FUNC-006 · DWRS Classic

Disponible en ambas versiones. Se parte de los niveles de la tabla siguiente y de las marcas del rol en [el catálogo](16-tablas-de-roles-y-tacticas.md). Cada atributo se utiliza una sola vez. Un atributo de un nivel conserva su nivel incluso si el rol lo marca. Fuera de los niveles sólo participa si el rol lo marca y existe una categoría con peso para esa marca. Esto agrega atributos a porteros. En FM24 se excluyen Communication y Eccentricity del cálculo Classic. Los pesos de la tabla y factores de marca son configurables.

| Tipo | Categoría | Peso inicial | Miembros |
|---|---|---:|---|
| Campo | Extremely Important | 8 | Pace, Acceleration |
| Campo | Important | 4 | Jumping Reach, Anticipation, Balance, Agility, Concentration, Finishing |
| Campo | Good | 2 | Work Rate, Dribbling, Stamina, Strength, Passing, Determination, Vision |
| Campo | Decent | 1 | Long Shots, Marking, Decisions, First Touch |
| Campo | Almost Irrelevant | 0,2 | Off the Ball, Tackling, Teamwork, Composure, Technique, Positioning |
| GK | Top Importance | 3 | Positioning, Aerial Reach, Handling |
| GK | High Importance | 2 | Agility, Anticipation, Kicking |
| GK | Medium Importance | 1 | Reflexes, Command of Area, Concentration, Throwing |
| GK | Other | 0,2 | Bravery, One vs One, Rushing Out (Tendency) |
| GK | Key / Preferable | 0,5 / 0,25 | Marcas del rol que quedan fuera de los cuatro grupos anteriores. Key tiene prioridad. |

1. Para cada atributo válido del plan, tomar `x=(lo+hi)/2`, incluso si ambos son cero. Factor `f=1,5` si es key, `1,2` si es preferable y 1 en los demás casos.
2. Para cada categoría no vacía, sea `n` el número de atributos incluidos, **también los ausentes en el jugador**. Definir `c_i=peso_categoria × f_i / n`. Absoluta `A=Σ c_i x_i`. Las referencias son `L=Σ c_i` (todos a 1) y `U=20 L` (todos a 20).
3. Normalizada `N=redondeo_al_par(100 (A−L)/(U−L))`. Denominador nulo da cero en el motor histórico y ausencia de valoración en el perfil compilado. Con datos completos la escala va de 0 a 100. Con todo ausente puede dar −5. La escala absoluta cambia al editar pesos.

**Ejemplo calculado manualmente.** Configurar sólo Extremely Important con peso 8, restantes cero. Rol sin marcas sobre Pace o Acceleration. Pace 12–16, Acceleration 9. Los dos coeficientes valen 4, A=92, L=8, U=160. N=55. Con Acceleration ausente, A=56 y N=32. El resultado conserva los dos miembros en el divisor.

El perfil Classic suministrado representa cada coeficiente de categoría antes del factor con base `peso/n`, amplitud `19 peso/n` y forma lineal. En políticas Classic editadas, interpola la contribución entre los enteros contiguos al punto medio. Para (0,0) usa el negativo de la contribución relativa a nivel 2. Suma las bases al final. [Categorías](../../old_version/desktop/src/core/AttributeTiers.cpp) · [Cálculo](../../old_version/desktop/src/core/DwrsEngine.cpp) · [Referencia congelada](../../old_version/desktop/src/tests/test_classicreference.cpp).

## FPA-FUNC-007 · DWRS por curvas y cobertura

El perfil determina por atributo cuatro propiedades independientes, base b, amplitud w≥0, forma s(1…20) no decreciente de 0 a 1, factor f≥0. Resolver primero el valor global, después los campos presentes de la única posición padre y finalmente los campos presentes del rol. El padre inicial es la primera posición compatible en orden léxico. Un factor explícito sustituye al de la marca. Rechazar perfiles incompatibles, datos no finitos, rol o posición desconocidos de la versión activa y vínculos rol–posición inválidos.

Sea `d_i=w_i f_i`, `C_i(x)=d_i s_i(x)`. Si d es cero, omitir también su base en esta etapa. Acumular `B=Σ b_i f_i` para las entradas de base activas. Después añadir los componentes lineales de rol a d y C como se indica abajo. Sea `D=Σ d_i`. Para cada par válido calcular `E_i=Σ C_i(x)/(hi−lo+1)` para todos los enteros inclusivos. (0,0) omite ese atributo tanto del numerador como del peso conocido K. Un intervalo parcial con cero, invertido o superior a 20 invalida el resultado si participa en el plan. Un intervalo válido aporta **todo** su peso a K, incluso si es ancho.

Con D=0 el rol no tiene escala. Con K≤0 o K<0,7D−10⁻⁹ devolver ausencia por cobertura, con cobertura K/D cuando D>0. Para los demás, `S=Σ limitar(E_i,0,d_i)`, `absoluta=B+S`, `normalizada=limitar(100 S/K,0,100)`. B incluye las bases activas del plan completo aunque falten observaciones. Conservar la precisión para cálculos. Curvas suelen mostrarse con una decimal, Classic sin decimales. El formateador redondea mitades alejándose de cero. Algunas celdas de XI y el ranking individual truncan antes de formatear.

**Ejemplo manual.** Dos atributos lineales con amplitudes 7 y 3, bases 2 y 5, factores 1, sin componente de rol. El primero a 20 y el segundo ausente dan K/D=70 %, A=14 y N=100. Intercambiando cuál es conocido queda 30 % y ninguna valoración. Para una curva que vale 0 hasta el nivel 10 inclusive y 1 desde 11, intervalo 9–11, la forma esperada es 1/3 aunque en el punto medio 10 sea cero.

**FM27 harvestgreen22.** Cada registro visible medido tiene cuatro muestras en niveles 1, 6, 12 y 18. Ajuste isotónico con bloques inicialmente de un punto y peso unitario. Mientras la media del bloque anterior supere a la siguiente, unirlos y usar la media ponderada por sus tamaños. Expandir las medias a los cuatro nudos. Si último−primero ≤4 puntos, amplitud cero. En otro caso ésa es la amplitud, base cero, forma en cada nudo (nivel ajustado−primero)/amplitud. Interpolar linealmente entre nudos, mantener 1 en 18–20. Sin medir queda peso base cero. Los originales completos se encuentran en [las tablas de parámetros](17-parametros-y-atributos.md). Ejemplo manual con medidas 0, 6, 4, 12 → 0, 5, 5, 12. Nivel 6 contribuye 5 antes del factor, nivel 9 también, nivel 19 contribuye 12. El corte es una decisión del perfil.

**Parte del rol de campo.** Tras aplicar factores 1,5/1,2 a los pesos base, sea D₀ su suma. Para cuota r (por defecto 0,33 en harvestgreen22), añadir un total `T=D₀ r/(1−r)` si D₀>0 y hay marcas visibles de campo. Resolver duplicados dando prioridad a key. Repartir T en proporción 1,5 por key y 1 por preferable, excluyendo los once atributos de portería del diccionario. Añadir la parte de cada atributo a su peso y su contribución lineal `parte × (x−1)/19`, aun cuando su curva global estuviera desactivada. Con D₀=67, una marca de cada tipo y r=0,33 se añaden 19,8 y 13,2. La mezcla se efectúa antes del cálculo de cobertura. [Perfil inicial](../../old_version/desktop/src/core/BuiltinRatingProfiles.cpp) · [Evaluación](../../old_version/desktop/src/core/RatingSnapshot.cpp) · [Ajuste de los nudos](../../old_version/desktop/src/core/RatingCurve.cpp).

## FPA-FUNC-008 · Componente específico de portería FM27

La posición padre de portería sustituye la base por curvas lineales de base cero. Pesos 12 para Anticipation, Handling, Reflexes, Positioning. Pesos 8 para Concentration, Communication, One vs One, Agility. Pesos 4 para Aerial Reach, Command of Area, Composure. Pesos 2 para Kicking, Throwing, Bravery, Decisions. Los demás tienen cero. Aplicar los factores del rol antes de sumar D₀. Estos pesos provienen del análisis de porteros del proyecto y no de los cuatro nudos de campo.

Para cada rol de portero y atributo asignar marcas 1,5/1/0 para key/preferable/ausente, eliminando Eccentricity. Restar a cada marca el mínimo de ese atributo entre porteros de **la misma fase**. Sea Δ la diferencia y M el máximo, entre **todos** los roles de portero, de la suma de sus diferencias. Si M>0, sumar a cada atributo el peso lineal `D₀ × r/(1−r) × Δ/M`, r=0,20 por defecto. Si todas las diferencias del rol son cero, sólo usa su base. La cuota máxima se alcanza en el rol de mayor diferenciación. Ejemplo manual D₀=100, M=5 y única diferencia del rol Δ=1 → extra 5 y cuota efectiva 5/105. [Pesos de portero](../../old_version/desktop/src/core/BuiltinRatingProfiles.cpp) · [Comparación por fase](../../old_version/desktop/src/core/RatingSnapshot.cpp).
