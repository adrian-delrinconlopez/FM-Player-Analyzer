# Parámetros de visibilidad en FM27

Complemento del [contrato de visibilidad](11-importacion-y-visibilidad.md). Los números identifican atributos para la función determinista de intervalos. El orden de exposición es una prioridad inicial, perturbada por jugador antes de seleccionar los atributos que quedan en forma de intervalo. [Fuente de prioridades y etiquetas](../../old_version/desktop/src/core/Fm27SaveImport.cpp) · [Identificadores](../../old_version/desktop/src/core/Fm27Save.h).

| Prioridad desde 0 | Campo (ID) | Portería (ID) |
|---:|---|---|
| 0 | Long Throws (28) | Dribbling (1) |
| 1 | Kicking (15) | Finishing (2) |
| 2 | Punching (31) | Crossing (0) |
| 3 | Rushing Out (Tendency) (30) | Long Shots (4) |
| 4 | Command Of Area (13) | Heading (3) |
| 5 | Aerial Reach (12) | Off The Ball (6) |
| 6 | Communication (14) | Marking (5) |
| 7 | Throwing (16) | Tackling (9) |
| 8 | Handling (11) | Corners (25) |
| 9 | One On Ones (19) | Free Kick Taking (33) |
| 10 | Reflexes (21) | Penalty Taking (8) |
| 11 | Corners (25) | Long Throws (28) |
| 12 | Free Kick Taking (33) | Technique (23) |
| 13 | Eccentricity (29) | Flair (24) |
| 14 | Marking (5) | Passing (7) |
| 15 | Penalty Taking (8) | Vision (10) |
| 16 | Tackling (9) | First Touch (22) |
| 17 | Anticipation (17) | Work Rate (27) |
| 18 | Concentration (51) | Team Work (26) |
| 19 | Flair (24) | Leadership (38) |
| 20 | Vision (10) | Determination (49) |
| 21 | Finishing (2) | Aggression (43) |
| 22 | Crossing (0) | Anticipation (17) |
| 23 | Heading (3) | Bravery (41) |
| 24 | Off The Ball (6) | Stamina (35) |
| 25 | Positioning (20) | Natural Fitness (48) |
| 26 | Long Shots (4) | Balance (40) |
| 27 | Aggression (43) | Strength (34) |
| 28 | Leadership (38) | Pace (36) |
| 29 | Determination (49) | Acceleration (32) |
| 30 | Natural Fitness (48) | Agility (44) |
| 31 | Passing (7) | Jumping Reach (37) |
| 32 | Stamina (35) | Eccentricity (29) |
| 33 | Dribbling (1) | Punching (31) |
| 34 | Decisions (18) | Rushing Out (Tendency) (30) |
| 35 | Team Work (26) | Throwing (16) |
| 36 | Composure (50) | Kicking (15) |
| 37 | Technique (23) | Communication (14) |
| 38 | Bravery (41) | Composure (50) |
| 39 | Strength (34) | Concentration (51) |
| 40 | Work Rate (27) | Decisions (18) |
| 41 | First Touch (22) | Positioning (20) |
| 42 | Balance (40) | Command Of Area (13) |
| 43 | Agility (44) | One On Ones (19) |
| 44 | Jumping Reach (37) | Aerial Reach (12) |
| 45 | Acceleration (32) | Handling (11) |
| 46 | Pace (36) | Reflexes (21) |

## Personalidad derivada

Evaluar de arriba abajo, detenerse en la primera condición cierta. Se exige persona identificada, edad ≥0, ocho rasgos de personalidad válidos, Determination y Leadership entre 1 y 20. Dentro de esta tabla, los nombres abreviados remiten a Ambition, Loyalty, Pressure, Professionalism, Sportsmanship, Temperament, Determination y Leadership. Adaptability y Controversy se validan aunque no aparezcan en condiciones. La etiqueta vacía conserva el informe anterior.

| Condición | Etiqueta |
|---|---|
| determination ≥ 14 y ambition ≥ 12 y professional ≥ 15 y temperament ≥ 15 y pressure ≥ 14 y loyalty ≥ 15 y sporting ≥ 15 | Model Citizen |
| determination ≥ 14 y ambition ≥ 14 y professional ≥ 14 y temperament ≤ 9 | Perfectionist |
| temperament ≤ 4 y professional ≤ 10 | Vacía |
| edad ≥ 23 y leadership = 20 y determination = 20 | Born Leader |
| edad ≥ 23 y leadership ≥ 19 y temperament ≥ 18 y sporting ≥ 18 | Charismatic Leader |
| edad ≥ 23 y leadership ≥ 19 | Leader |
| professional = 20 y temperament ≥ 10 y edad ≥ 23 | Model Professional |
| professional ≥ 18 y temperament ≥ 10 | Professional |
| determination ≥ 18 y ambition ≥ 12 | Driven |
| determination ≥ 18 | Determined |
| professional ≤ 4 y determination ≤ 9 | Vacía |
| loyalty ≤ 6 y ambition ≥ 16 | Vacía |
| ambition = 20 y loyalty ≤ 9 | Very Ambitious |
| ambition ≥ 16 y loyalty ≤ 9 | Ambitious |
| ambition ≤ 5 y loyalty ≥ 11 | Vacía |
| sporting = 20 y determination ≤ 9 | Honest |
| sporting ≥ 18 y determination ≤ 9 | Sporting |
| sporting ≤ 4 y determination ≥ 11 | Vacía |
| determination ≤ 5 y ambition ≤ 9 | Vacía |
| loyalty = 20 y ambition ≤ 7 | Vacía |
| loyalty ≥ 18 y ambition ≤ 7 | Loyal |
| pressure = 20 y determination ≥ 15 | Iron Willed |
| pressure ≥ 17 y determination ≥ 15 | Resilient |
| pressure ≤ 3 y determination ≤ 9 | Vacía |
| pressure ≥ 15 y temperament ≥ 10 y sporting ≥ 15 | Light-Hearted |
| pressure ≥ 15 y temperament ≥ 10 y professional ≥ 11 | Spirited |
| pressure ≥ 15 y temperament ≥ 10 | Jovial |
| determination ≥ 15 y professional ≥ 15 | Resolute |
| determination ≥ 15 | Fairly Determined |
| professional ≥ 15 | Fairly Professional |
| ambition ≥ 15 | Fairly Ambitious |
| loyalty ≥ 15 | Fairly Loyal |
| sporting ≥ 15 | Fairly Sporting |
| Ninguna condición anterior | Balanced |

## País del jugador en lectura directa

Correspondencia del número de nación con el texto devuelto. Cualquier otro número produce cadena vacía.

| Números y códigos |
|---|
| 0=ALG · 1=ANG · 2=BEN · 4=BFA · 5=BDI · 6=CMR · 7=CPV · 8=CTA |
| 9=CHA · 11=EGY · 12=EQG · 14=GAB · 15=GAM · 16=GHA · 17=GUI · 18=GNB |
| 19=CIV · 20=KEN · 22=LBR · 23=LBY · 24=MAD · 26=MLI · 27=MTN · 28=MRI |
| 29=MAR · 30=MOZ · 31=NAM · 32=NIG · 33=NGA · 36=SEN · 38=SLE · 39=SOM |
| 40=RSA · 41=SDN · 44=CGO · 45=TOG · 46=TUN · 48=COD · 49=ZAM · 50=ZIM |
| 51=AFG · 52=BHR · 55=CHN · 56=HKG · 57=IND · 59=IRN · 60=IRQ · 61=JPN |
| 62=JOR · 64=KAZ · 66=KGZ · 68=LBN · 70=MAS · 75=OMA · 77=QAT · 78=KSA |
| 80=KOR · 82=SYR · 83=TPE · 84=TJK · 85=THA · 86=PHI · 88=UAE · 89=UZB |
| 90=FRA · 97=CAN · 99=CRC · 100=CUB · 102=SLV · 104=GUA · 106=HAI · 107=HON |
| 108=JAM · 109=MEX · 111=ESP · 112=PAN · 113=PUR · 117=SUR · 119=TRI · 120=USA |
| 126=ALB · 127=AND · 128=ARM · 129=AUT · 130=AZE · 131=BEL · 132=BLR · 133=BIH |
| 134=BUL · 135=CRO · 137=CZE · 138=DEN · 139=ENG · 140=EST · 142=FIN · 143=FRA |
| 144=GEO · 145=GER · 146=GRE · 147=HUN · 148=ISL · 149=ISR · 150=ITA · 151=LVA |
| 152=LIE · 153=LTU · 154=LUX · 155=MKD · 158=NED · 159=NIR · 160=NOR · 161=POL |
| 162=POR · 163=IRL · 164=ROU · 165=RUS · 166=SMR · 167=SCO · 168=SVK · 169=SVN |
| 170=ESP · 171=SWE · 172=SUI · 173=TUR · 174=UKR · 176=SRB · 177=AUS · 180=NZL |
| 183=TAH · 187=ARG · 188=BOL · 189=BRA · 190=CHI · 191=COL · 192=ECU · 193=PAR |
| 194=PER · 195=URU · 196=VEN · 197=PLE · 210=NCL · 212=DOM · 216=GIB · 219=KOS |
| 225=GUF · 226=GLP · 227=MTQ · 229=SMN · 230=FRA · 231=FRA · 234=COM · 247=MNE |

## Contrato de la fila visible

Las primeras 14 columnas son Unique ID, Player, Age, Club, Nation, Position, Height, Left Foot, Right Foot, Wage, Transfer Value, Playing Time, Rating y Personality. Las columnas 15 a 61 se emiten en el orden literal del [caso G12](14-casos-de-prueba-de-referencia.md#g12), que incluye también una fila exacta y otra con máscara. Las prioridades del comienzo de este anexo sólo deciden qué atributos quedan en intervalo. En lectura directa, edad negativa y estatura no positiva dejan celda vacía. Rating se emite con dos decimales si es positivo. Una fuerza de pie de 18…20 produce Very Strong, 15…17 Strong, 12…14 Fairly Strong, 9…11 Reasonable, 5…8 Weak y por debajo Very Weak. En interpretación tabular de enteros se acepta sólo 1…20. Se asignan niveles 1…6 de menor a mayor, y los seis valores representativos que usa el analizador son 3, 9, 12, 13, 16 y 19. Los textos alemanes Sehr schwach, Schwach, Ordentlich/Ausreichend, Ziemlich stark/Recht stark, Stark y Sehr stark se reconocen sin distinguir caja.

Para Position recorrer en orden GK, D, WB, DM, M, AM, ST. Exigir competencia ≥15. En cada línea emitir lados R, L, C en ese orden (WB sólo R/L). Unir grupos vecinos no vacíos con los mismos lados mediante barra, excepto ST, que se expresa ST (C) y permanece separado. GK y DM no llevan lados. La competencia SW por sí sola no emite posición. Naturales usa la misma conversión después de dejar únicamente competencias iguales a 20.

| Código de tiempo de juego en el contrato | Valor emitido |
|---|---|
| 1 / 2 / 3 / 4 / 5 | Star Player / Important Player / Regular Starter / Squad Player / Impact Sub |
| 7 / 9 / 10 / 11 / 13 / 14 | Fringe Player / Emergency Backup / Breakthrough Prospect / Future Prospect / Youngster / 2nd Team Regular |
| 15 / 16–18 / 20 / 21 / 22 | First-Choice Goalkeeper / Cup Goalkeeper / Backup / Emergency Backup / Surplus to Requirements |
| Resto | Vacío |

El parámetro de portería no cambia este mapeo de contrato. [Formación de la fila](../../old_version/desktop/src/core/Fm27SaveImport.cpp) · [Normalización de pies](../../old_version/desktop/src/core/Utils.cpp).
