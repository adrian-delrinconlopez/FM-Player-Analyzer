# Parámetros de valoración y entrenamiento

## Medidas de campo de FM27

Cada columna es una observación en puntos porcentuales. Aplicar el ajuste isotónico y el corte descritos en el [capítulo 03](03-valoraciones-y-formulas.md) a la columna de medidas originales. Las marcas de rol pueden añadir peso lineal a atributos visibles con peso base cero. Los tres atributos ocultos de la fuente no se incluyen en el perfil. [Datos del baseline](../../old_version/docs/dwrs/harvestgreen-comparison.json).

| Atributo | Medidas a 1, 6, 12 y 18 | Uso en la base |
| --- | --- | --- |
| Passing | 2.0, 1.0, 0, -0.8 | Ajuste y corte |
| Crossing | -4.6, -4.0, 0, 0.6 | Ajuste y corte |
| Marking | -4.2, -0.6, 0, -2.2 | Ajuste y corte |
| Penalty Taking | Sin medir | Peso cero |
| Technique | -0.4, -1.2, 0, -5.4 | Ajuste y corte |
| Corners | Sin medir | Peso cero |
| Long Throws | Sin medir | Peso cero |
| Dribbling | -6.8, -3.8, 0, 3.6 | Ajuste y corte |
| Tackling | -1.0, 1.6, 0, -5.0 | Ajuste y corte |
| Free Kick Taking | Sin medir | Peso cero |
| Finishing | -7.8, -4.8, 0, 2.2 | Ajuste y corte |
| First Touch | 0.8, 1.6, 0, -3.6 | Ajuste y corte |
| Heading | -6.8, -1.6, 0, 0 | Ajuste y corte |
| Long Shots | -9.8, -4.6, 0, 0 | Ajuste y corte |
| Flair | -0.6, -1.6, 0, 1.8 | Ajuste y corte |
| Positioning | 3.4, 0.4, 0, 2.6 | Ajuste y corte |
| Work Rate | -22.0, -6.2, 0, 0 | Ajuste y corte |
| Concentration | -13.4, -5.6, 0, 4.4 | Ajuste y corte |
| Decisions | -1.6, -2.4, 0, 1.0 | Ajuste y corte |
| Leadership | Sin medir | Peso cero |
| Aggression | 0.2, 1.4, 0, 1.2 | Ajuste y corte |
| Vision | -8.2, -8.2, 0, -5.2 | Ajuste y corte |
| Teamwork | Sin medir | Peso cero |
| Off the Ball | 1.6, -2.4, 0, -6.0 | Ajuste y corte |
| Determination | -6.0, 0, 0, 2.8 | Ajuste y corte |
| Bravery | -2.4, -2.4, 0, 1.4 | Ajuste y corte |
| Anticipation | -19.8, -8.8, 0, 4.8 | Ajuste y corte |
| Composure | -5.0, -2.0, 0, -2.0 | Ajuste y corte |
| Acceleration | -34.0, -21.4, 0, 14.1 | Ajuste y corte |
| Jumping Reach | -18.0, -12.2, 0, 15.4 | Ajuste y corte |
| Agility | -8.4, -3.2, 0, 0.2 | Ajuste y corte |
| Stamina | -29.4, -17.6, 0, 14.0 | Ajuste y corte |
| Balance | -14.2, -8.2, 0, 0 | Ajuste y corte |
| Strength | -18.8, -8.8, 0, 4.6 | Ajuste y corte |
| Pace | -40.0, -26.8, 0, 19.4 | Ajuste y corte |
| Natural Fitness | -12.6, -4.0, 0, 2.8 | Ajuste y corte |
| Consistency | -6.4, -2.0, 0, 1.2 | Excluido por ser oculto |
| Important Matches | -8.8, -5.0, 0, 0 | Excluido por ser oculto |
| Pressure | -15.6, -2.0, 0, 5.2 | Excluido por ser oculto |

## Diccionario de atributos visibles

Cada fila es un par de enteros, exacto o intervalo inclusivo de 1 a 20. El par (0,0) representa ausencia. La columna de entrada identifica la cabecera declarada por versión. Los niveles de DWRS Classic, marcas de rol y medidas FM27 determinan por separado su participación efectiva. La familia descriptiva del atributo y su banda de entrenamiento cumplen funciones distintas. [Cabeceras](../../old_version/desktop/src/core/ImportSchema.cpp) · [Bandas](../../old_version/desktop/src/core/TrainingAdvice.cpp).

| Atributo canónico | Familia | Cabecera FM24 | Cabecera FM27 | Banda inicial |
| --- | --- | --- | --- | --- |
| One vs One | Portería | 1v1 | One On Ones | Technical |
| Acceleration | Física | Acc | Acceleration | Explosive |
| Aerial Reach | Portería | Aer | Aerial Reach | Technical |
| Aggression | Mental | Agg | Aggression | Character |
| Agility | Física | Agi | Agility | Explosive |
| Anticipation | Mental | Ant | Anticipation | Mental |
| Balance | Física | Bal | Balance | Strength |
| Bravery | Mental | Bra | Bravery | Character |
| Command of Area | Portería | Cmd | Command Of Area | Technical |
| Concentration | Mental | Cnt | Concentration | Mental |
| Composure | Mental | Cmp | Composure | Mental |
| Corners | Técnica | Cor | Corners | Technical |
| Crossing | Técnica | Cro | Crossing | Technical |
| Decisions | Mental | Dec | Decisions | Mental |
| Determination | Mental | Det | Determination | Character |
| Dribbling | Técnica | Dri | Dribbling | Technical |
| Finishing | Técnica | Fin | Finishing | Technical |
| First Touch | Técnica | Fir | First Touch | Technical |
| Flair | Mental | Fla | Flair | Mental |
| Handling | Portería | Han | Handling | Technical |
| Heading | Técnica | Hea | Heading | Technical |
| Jumping Reach | Física | Jum | Jumping Reach | Strength |
| Kicking | Portería | Kic | Kicking | Technical |
| Leadership | Mental | Ldr | Leadership | Character |
| Long Shots | Técnica | Lon | Long Shots | Technical |
| Marking | Técnica | Mar | Marking | Technical |
| Off the Ball | Mental | OtB | Off The Ball | Mental |
| Pace | Física | Pac | Pace | Explosive |
| Passing | Técnica | Pas | Passing | Technical |
| Positioning | Mental | Pos | Positioning | Mental |
| Reflexes | Portería | Ref | Reflexes | Technical |
| Rushing Out (Tendency) | Portería | TRO | Rushing Out (Tendency) | Technical |
| Stamina | Física | Sta | Stamina | Strength |
| Strength | Física | Str | Strength | Strength |
| Tackling | Técnica | Tck | Tackling | Technical |
| Teamwork | Mental | Tea | Team Work | Mental |
| Technique | Técnica | Tec | Technique | Technical |
| Throwing | Portería | Thr | Throwing | Technical |
| Vision | Mental | Vis | Vision | Mental |
| Work Rate | Mental | Wor | Work Rate | Character |
| Communication | Portería | — | Communication | Technical |
| Eccentricity | Portería | — | Eccentricity | Technical |
| Natural Fitness | Física | Nat F | Natural Fitness | Strength |
| Free Kick Taking | Técnica | Fre | Free Kick Taking | Technical |
| Long Throws | Técnica | L Th | Long Throws | Technical |
| Penalty Taking | Técnica | Pen | Penalty Taking | Technical |
| Punching | Portería | Pun | Punching | Technical |

## Métodos de entrenamiento

Se conservan las cadenas que se comparan literalmente, incluido «Corner Taking» frente al atributo «Corners».

| Método | Familia | Atributos buscados | Excluido por rehabilitación |
| --- | --- | --- | --- |
| Quickness | Injury Rehab | Pace, Acceleration | Sí |
| Agility and Balance | Injury Rehab | Agility, Balance | Sí |
| Strength | Injury Rehab | Strength, Jumping Reach | Sí |
| Endurance | Injury Rehab | Stamina | Sí |
| General Rehab | Injury Rehab | Acceleration, Agility, Balance, Jumping Reach, Pace, Strength, Stamina | Sí |
| Free Kick Taking | Set Pieces | Free Kick Taking, Technique | No |
| Corner Taking | Set Pieces | Corner Taking, Technique | No |
| Penalty Taking | Set Pieces | Penalty Taking, Technique | No |
| Long Throws | Set Pieces | Long Throws | No |
| Quickness | Attributes | Pace, Acceleration | No |
| Agility and Balance | Attributes | Agility, Balance | No |
| Strength | Attributes | Strength, Jumping Reach | No |
| Endurance | Attributes | Stamina, Work Rate | No |
| Defensive Positioning | Attributes | Marking, Positioning, Decisions | No |
| Attacking Movement | Attributes | Off the Ball, Anticipation, Decisions | No |
| Final Third | Attributes | Composure, Decisions | No |
| Shooting | Attributes | Finishing, Long Shots, Technique | No |
| Passing | Attributes | Vision, Passing, Technique | No |
| Crossing | Attributes | Crossing, Technique | No |
| Ball Control | Attributes | First Touch, Dribbling, Technique | No |
| Aerial | Attributes | Heading, Bravery | No |
| GK Reactions | Attributes | Reflexes, Anticipation, Concentration | No |
| GK Tactical | Attributes | Positioning, Communication, Decisions | No |
| GK Technique | Attributes | Handling, Composure, Technique | No |
| GK Sweeping | Attributes | One vs One, Command of Area, Rushing Out (Tendency) | No |
| GK Distribution (Long) | Attributes | Kicking, Throwing | No |
| GK Distribution (Short) | Attributes | First Touch, Passing, Vision | No |

