# PoC B · Calidad actual y potencial por rol

**24 de noviembre de 2026 · SK-02.** Banco en C# para .NET 10, separado del capturador y de la aplicación. Usa exclusivamente las tablas FM27 de AS-IS 2.3.1. El piloto presente reutiliza observaciones visibles de la copia del Valencia registrada en T021, Steam 26620374, 1 de agosto de 2026 a las 21:45. La validación cubre dos roles por ficha y una sola población, la masculina. [Protocolo B-01 a B-04](../roadmap/07-poc-y-modelo-v1.md#poc-b--calidad-actual-y-potencial-por-rol) · [Evidencia T021](../../specs/001-extraccion-fm27/evidencia.md#t021--atributos-visibles-y-familiaridad-por-posición)

| Ensayo | Resultado | Trazabilidad |
| --- | --- | --- |
| B-01 · Equivalencia | **Verificado.** 2.528/2.528 pares Classic y 237/237 notas de Curvas. | REQ-RATING-001/002/003, EQ-006/007/008 |
| B-02 · Familiaridad | **Contrato verificado.** 128 casos sintéticos y escala contrastada en la muestra T021. Elección de función y equivalencia entre el editor y las etiquetas de la ficha del mánager pendientes en Q-17. | REQ-RATING-006, V1-03 |
| B-03 · Proyección | **Verificado en el banco sintético.** 80 comparaciones con enumeración exhaustiva, 80 inversiones del orden de entrada y pruebas de frontera y revocación. | REQ-RATING-005, NV-28, V1-04 |
| B-04 · Partida | **Parcial.** Piloto presente IP/OOP y respuesta reproducible de la referencia de coste en campo y portería. Quedan la calibración, el contraste independiente y el recorrido de potencial en partida con costes contrastados. | REQ-RATING-005/006, V1-03/04, Q-17 |

## B-01 · Equivalencia y cobertura

- Classic reprodujo las normalizadas enteras de los 79 roles en 16 muestras con cada configuración. Error máximo de absoluta de `1,13687 × 10⁻¹³`, dentro de `10⁻¹² × máx(1, |referencia|)`. El control deportivo coincidió en los 1.896 casos con los atributos ponderados completos y devolvió ausencia en los 632 restantes. El ejemplo de Ritmo 12–16 y Aceleración 9 conserva A=92 y N=55. Si Aceleración falta, el oráculo devuelve A=56 y N=32 y el control deportivo se abstiene. Esta diferencia evita valoraciones e históricos sustentados en el centinela de ausencia. [Oráculo Classic](../as-is/20-oraculo-dwrs-classic.md)
- Curvas reprodujo las tres notas de los 79 roles con idéntica cifra publicada al redondear a una decimal. Error máximo antes de redondear de 0,049895 pp frente a una tolerancia de 0,05 pp. Incluye los seis roles de portero y sus marcas por fase. Se verificaron el corte isotónico, la meseta, el rango 9–11 con media 1/3, la abstención a 69,99 % y la publicación a 70 % y 70,01 %. A 70 % se comprobó A=14 y N=100 en el ejemplo de dos atributos. [Perfil y fórmulas](../as-is/03-valoraciones-y-formulas.md) · [Oráculo de Curvas](../as-is/22-oraculo-de-curvas-fm27.md)

## B-02 · Funciones de familiaridad

Los candidatos v0 son el multiplicador lineal `f/20` y una tabla sintética de afinidad por tramos. Sólo se evalúa familiaridad exacta autorizada de 1 a 20. T021 cotejó 38/38 referencias en dos lecturas, con observaciones dentro de 1–20 y escala declarada por el editor de 0–20. El significado deportivo del 0 queda por resolver. El banco devuelve estado desconocido para ese valor y mantiene la observación restringida separada, sin consultar su valor.

| Familiaridad | Afinidad sintética v0 |
| --- | ---: |
| 1–5 | 0,20 |
| 6–10 | 0,50 |
| 11–15 | 0,70 |
| 16–19 | 0,90 |
| 20 | 1,00 |

Los dos candidatos conservan la nota DWRS de origen y a 20 producen el mismo encaje que esa nota. APT y bono natural son neutros en la prueba. Con base 80 y familiaridad 15 el lineal da 60, la tabla 56 y el escenario a 20 da 80. Frente a una base 58 con familiaridad 20 cambia el orden de ambos jugadores. La separación máxima entre funciones alcanza el 20 % del DWRS de origen. El escenario a 20 se conserva cuando la familiaridad actual está desconocida o restringida. Usar esa hipótesis no concede elegibilidad posicional ni crea una arista utilizable en el XI presente. Estos son resultados del contrato matemático. La comparación táctica con el servicio real de XI queda para su integración.

## B-03 · Proyección sintética

Se optimiza un presupuesto entero `PA − CA` por posición de desarrollo. El banco usa costes positivos aditivos y beneficios marginales decrecientes por atributo. En caso de empate prefiere menor coste y después el vector con mayor incremento en el primer atributo discordante según el diccionario canónico. El modelo puede dejar margen sin gastar y nunca sale de 1–20. Cada prueba declara los atributos que puede cambiar. Los demás permanecen fijos en la evaluación de ambos roles.

- El ejemplo aprobado parte de Ritmo 18 y Anticipación 19. Con presupuesto 5, el reparto produce Ritmo 19 y Anticipación 20, coste 5 y ganancia 4. El escalado proporcional acotado coincide en ese ejemplo. Con presupuesto 2, el reparto mejora Ritmo en un punto y el escalado proporcional devuelve cambio cero. Diferencia observada sólo bajo los costes sintéticos de la prueba.
- La programación dinámica igualó coste, ganancia y vector de la enumeración para cinco configuraciones y 16 presupuestos por configuración. Verificó invariancia al invertir el orden, monotonía del objetivo al aumentar el presupuesto, tope de 20, margen sobrante, CA=PA y dos vectores distintos con el mismo PA. El resultado queda ausente con PA<CA, valores fuera de escala, sin datos suficientes o con rangos.
- Las dos fuentes gemelas eran iguales en atributos visibles y distintas en CA/PA. Con permiso insuficiente o fuente sin acreditar el banco realizó cero lecturas y devolvió ausencia en ambas fuentes. También descartó el resultado al revocar CA o PA durante el cálculo y antes de entregarlo. La reutilización IP/OOP se verificó en Curvas. Ordenación, XI, exportación y cachés reales requieren pruebas en sus propios casos de uso.

## B-04 · Piloto presente y referencia de coste

El piloto recibió los atributos exactos visibles de T021 de una ficha de campo y una de portería. Ambas notas son DWRS de atributos en la revisión experimental `harvestgreen22` con cuotas iniciales. El escenario a 20 conserva los mismos atributos. La nota ajustada a familiaridad real queda ausente porque el piloto sólo recibe atributos visibles de la copia.

| Puesto | Fase y rol | Atributos exactos de entrada | Cobertura ponderada | DWRS base y escenario 20 |
| --- | --- | ---: | ---: | ---: |
| Carrilero izquierdo | IP · AWB | 36 | 100 % | 74,6 |
| Carrilero izquierdo | OOP · WB-OOP | 36 | 100 % | 73,3 |
| Portero | IP · GK-IP | 38 | 100 % | 66,0 |
| Portero | OOP · GK-OOP | 38 | 100 % | 68,5 |

En FM27, **Editor del juego → Atributos del jugador → Atributos clave** mostró «Calidad actual recomendada» junto a dos campos separados de calidad actual y potencial. La primera inspección de la ficha autorizada de Gayà fue de sólo lectura en el mismo corte visible de 1/8/2026 a las 21:45. Sports Interactive documenta la disponibilidad del editor y de su información contextual. [Editor oficial de FM27](https://www.footballmanager.com/fm27/features/fm27-2d-match-engine-game-editor-and-deeper-world-upgrades)

### Respuesta de la calidad recomendada

El 24 de noviembre de 2026 se creó con **Guardar partida como → Local** la copia `SK-02 B04 Valencia 2026-08-01` desde `SK-01 Valencia 2026-08-01`. En Steam 26620374, con la interfaz en castellano y el reloj a 1/8/2026 21:45, se variaron dos atributos de José Gayà y dos de Stole Dimitrievski. Se editaron únicamente esas cuatro celdas en la copia de laboratorio. No se avanzó el reloj ni se editaron CA, PA, posiciones, familiaridades o pie. Los campos CA y PA permanecieron constantes a la vista. La calidad recomendada se actualizó al pulsar Intro o salir de la celda.

Los deltas se miden frente a la lectura de base de cada ficha. La tabla contiene valores visibles de atributos y deltas de calidad recomendada.

| Grupo y atributo | Nivel base | Cambio +1 | Δ recomendada | Cambio +3 | Δ recomendada |
| --- | ---: | ---: | ---: | ---: | ---: |
| Campo · Velocidad | 15 | 16 | +1 | 18 | +5 |
| Campo · Aceleración | 14 | 15 | +2 | 17 | +7 |
| Portería · Reflejos | 16 | 17 | +2 | 19 | +6 |
| Portería · Blocaje | 14 | 15 | +1 | 17 | +5 |

| Combinación +3/+3 | Suma de los deltas individuales | Δ recomendada conjunta | Δ conjunta menos suma |
| --- | ---: | ---: | ---: |
| Velocidad y Aceleración | +12 | +13 en ambos órdenes | +1 |
| Reflejos y Blocaje | +11 | +11 en ambos órdenes | 0 |

Antes del primer +3 se restituyó el atributo principal tras su +1. Para medir el segundo atributo por separado se restituyó el primero mientras el segundo seguía elevado. Después se restauró la base completa y se repitió el +3/+3 empezando por el segundo atributo. Las dos combinaciones y sus lecturas de base fueron reproducibles. Se repusieron los cuatro atributos, se recargó `SK-01 Valencia 2026-08-01` y se volvió a la búsqueda inicial de 461 resultados. La copia SK-01 mantuvo 43.957.632 bytes y fecha UTC `2026-11-23T13:51:14.0444918Z`. La copia SK-02 mantuvo desde su creación 43.960.719 bytes y fecha UTC `2026-11-24T18:10:45.9208025Z`. Las variaciones quedaron en la sesión de laboratorio. El control de archivos consultó sus metadatos.

**Implicación para el presupuesto.** Multiplicar por tres el primer delta falla en tres de los cuatro atributos. Además, sumar los deltas enteros individuales infravalora en un punto la combinación de campo. Ese punto es compatible con redondear una suma aditiva de valores fraccionarios. Por ejemplo, `redondear(−0,35 + 1,80 × ΔVelocidad + 2,50 × ΔAceleración)` reproduce las seis lecturas de campo, incluida la línea base y la combinación. El ejemplo correspondiente para portería es `redondear(−0,20 + 2,00 × ΔReflejos + 1,65 × ΔBlocaje)`. Estos ejemplos sólo prueban compatibilidad matemática con las lecturas enteras. La fórmula interna, la pauta de redondeo y los coeficientes transferibles siguen sin acreditarse.

La prueba convierte el indicador en una referencia candidata repetible para perturbaciones controladas. El indicador de base llegó a diferir de CA en una ficha. El contraste debe medir **delta de calidad recomendada** frente a su propia línea base y conservar la CA observada como ancla del presupuesto. Medir el coste de referencia tampoco acredita que el jugador vaya a desarrollar esos atributos ni cuándo.

### Protocolo propuesto para Q-17

1. **Ensayo de respuesta. Completado para la pareja piloto.** Para repetirlo, abrir la copia local en el mismo corte y seguir las celdas y niveles de la tabla. Leer la base, aplicar +1 al primer atributo, restaurar, aplicar +3 y luego +3 al segundo. Restaurar el primero para leer el segundo aislado, medir también +1, volver a la base y repetir la combinación en orden inverso. Las variaciones usan el editor oficial. La captura de Analyzer se mantiene de sólo lectura.
2. **Separación de muestra.** Propuesta inicial de 48 fichas independientes, 24 de calibración y 24 reservadas para contraste. En cada conjunto, 8 de portería y 16 de campo. Excluir a las dos fichas exploratorias del contraste reservado. Cubrir polivalencia, edades 16–21, 22–28 y 29+, familiaridad natural y secundaria y márgenes bajos (0–10) y altos (>10). Registrar huecos por estrato y repetir el diseño con muestras propias si se declara cobertura en fútbol femenino. Congelar selección, exclusiones, perturbaciones combinadas y perfil antes de abrir el contraste. Comparar costes enteros por incremento con un candidato que sume costes fraccionarios y redondee el total. Medir diferencias de calidad recomendada sobre la misma ficha y anclar el escenario a su CA observada. Si falla como referencia, usar pares comparables del mismo jugador y carrera en dos o más fechas.
3. **Puertas propuestas.** Por grupo posicional, al menos el 90 % de las comparaciones previstas con referencia utilizable, error medio absoluto de coste ≤1 unidad de CA y percentil 90 del error absoluto ≤2 unidades de CA frente a la referencia elegida. Verificar que el coste de cada vector publicado respeta el margen. Informar saturación, margen sin gastar y cambios de orden al variar supuestos, frente a cambio cero y escalado proporcional acotado. Conservar los números especiales sólo durante el contraste autorizado y publicar recuentos y métricas agregadas. Estos umbrales quedan sujetos a validación y decisión del propietario antes del contraste reservado.
4. **Recorrido final.** Ejecutar en partida, para una ficha de campo y una de portería, la valoración actual y el potencial con ambos permisos y roles IP/OOP. Comprobar la ausencia de derivados al revocar cualquiera de los permisos. Publicar los grupos que cumplan los criterios aprobados. La falta de trayectorias comparables limita el horizonte a un techo condicionado, sin fecha ni probabilidad de alcanzarlo.

El piloto usa un único corte por ficha. Los cortes de Solihull y Valencia pertenecen a carreras distintas y no forman una trayectoria. Los costes de B-03 comprueban el repartidor mediante valores sintéticos. Q-17 sigue abierta y B-04 determina si esos costes pueden trasladarse a partidas.

## Reproducción

Desde la raíz del repositorio con SDK .NET 10. La configuración Laboratorio también compila el adaptador BepInEx con sus referencias locales.

```powershell
dotnet build FM27Analyzer.slnx -c Release
dotnet test FM27Analyzer.slnx -c Release --no-build
dotnet run -c Release --no-build --project experiments/FM27Analyzer.RatingProbe -- docs/as-is
dotnet build FM27Analyzer.slnx -c Laboratorio '-p:FM27GameRoot=C:/Program Files (x86)/Steam/steamapps/common/Football Manager 27'
dotnet test FM27Analyzer.slnx -c Laboratorio --no-build
```

SDK empleado 10.0.404. Las compilaciones Release y Laboratorio quedaron con 0 errores y 0 advertencias. La batería de extracción pasó 74/74 en ambas configuraciones. El banco de valoración devuelve código 0 en la reproducción completa y código distinto de cero si una comprobación falla. El piloto devolvió código 1 al recibir una clave CA, atributos por debajo de la cobertura y una combinación incompatible de puesto y rol.

El piloto usa la referencia local `referencia-prioritarios.json` de T021, SHA-256 `BF29C809584D0F737CE6E80E8B1EC702B4B552F614AA6C004331EA6232268A06`. El filtro envía al banco sólo atributos visibles exactos de cada UID de la referencia. Los valores especiales quedan fuera de la entrada.

```powershell
$ruta = Join-Path $env:LOCALAPPDATA 'FM27Analyzer/SK-01/viabilidad/referencia-prioritarios.json'
$referencia = Get-Content -LiteralPath $ruta -Raw | ConvertFrom-Json
$filas = $referencia.observations | Where-Object {
    $_.state.availability -eq 'observed' -and $_.state.disclosure -eq 'visible' -and
    $_.state.knowledge -eq 'exact' -and $_.type -eq 'numeric' -and $null -eq $_.field.qualifier
}
function Invoke-Piloto($uid, $puesto, $ip, $oop) {
    $filas | Where-Object { $_.player.uid -eq $uid } |
        ForEach-Object { '{0}={1}' -f $_.field.name, $_.value.low } |
        dotnet run -c Release --no-build --project experiments/FM27Analyzer.RatingProbe -- docs/as-is --presente $puesto $ip $oop
    if ($LASTEXITCODE -ne 0) { throw "Falló el piloto del puesto $puesto" }
}
$portero = ($filas | Where-Object { $_.field.name -eq 'handling' } | Select-Object -First 1).player.uid
Invoke-Piloto '67178177' 'WB (L)' AWB WB-OOP
Invoke-Piloto $portero GK GK-IP GK-OOP
```
