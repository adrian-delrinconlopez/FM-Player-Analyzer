# Parámetros de roles, tácticas y personalidades

Datos iniciales de 2.3.1. Los nombres ingleses son identificadores del producto, con traducción oficial española pendiente. En las tablas, «—» significa conjunto vacío. Los factores, el tratamiento de valores ausentes y la selección de un único padre posicional figuran en [Valoraciones](03-valoraciones-y-formulas.md).

## FM24

Evidencia de los valores iniciales. [Definiciones FM24](../../old_version/desktop/src/app/resources/definitions.json).

| Rol | Nombre | Fase | Posiciones compatibles (primera = padre inicial) | Atributos key | Atributos preferable |
| --- | --- | --- | --- | --- | --- |
| GK-D | Goalkeeper (Defend) | both | GK | Aerial Reach, Command of Area, Communication, Handling, Kicking, Reflexes, Concentration, Positioning, Agility | One vs One, Throwing, Anticipation, Decisions |
| SK-D | Sweeper Keeper (Defend) | both | GK | Command of Area, Kicking, One vs One, Reflexes, Anticipation, Concentration, Positioning, Agility | Aerial Reach, Communication, First Touch, Handling, Passing, Rushing Out (Tendency), Throwing, Composure, Decisions, Vision, Acceleration |
| SK-S | Sweeper Keeper (Support) | both | GK | Command of Area, Kicking, One vs One, Reflexes, Rushing Out (Tendency), Anticipation, Composure, Concentration, Positioning, Agility | Aerial Reach, Communication, First Touch, Handling, Passing, Throwing, Decisions, Vision, Acceleration |
| SK-A | Sweeper Keeper (Attack) | both | GK | Command of Area, Kicking, One vs One, Reflexes, Anticipation, Composure, Concentration, Positioning, Agility | Aerial Reach, Communication, Eccentricity, First Touch, Handling, Passing, Throwing, Vision, Acceleration, Decisions, Rushing Out (Tendency) |
| BPD-Co | Ball-Playing Defender (Cover) | both | D (C) | Marking, Passing, Tackling, Anticipation, Composure, Concentration, Decisions, Positioning, Pace | Heading, First Touch, Technique, Bravery, Vision, Jumping Reach, Strength |
| BPD-D | Ball-Playing Defender (Defend) | both | D (C) | Heading, Marking, Passing, Tackling, Composure, Positioning, Jumping Reach | First Touch, Technique, Aggression, Anticipation, Bravery, Concentration, Decisions, Vision, Pace, Strength |
| BPD-St | Ball-Playing Defender (Stopper) | both | D (C) | Heading, Passing, Tackling, Aggression, Bravery, Composure, Decisions, Positioning, Jumping Reach, Strength | First Touch, Marking, Technique, Anticipation, Concentration, Vision |
| CD-Co | Central Defender (Cover) | both | D (C) | Marking, Tackling, Anticipation, Concentration, Decisions, Positioning, Pace | Heading, Bravery, Composure, Jumping Reach, Strength |
| CD-D | Central Defender (Defend) | both | D (C) | Heading, Marking, Tackling, Positioning, Jumping Reach, Strength | Aggression, Anticipation, Bravery, Composure, Concentration, Decisions, Pace |
| CD-St | Central Defender (Stopper) | both | D (C) | Heading, Tackling, Aggression, Bravery, Decisions, Positioning, Jumping Reach, Strength | Marking, Anticipation, Composure, Concentration |
| FBL-A | Full-Back Left (Attack) | both | D (L) | Crossing, Dribbling, Tackling, Technique, Off the Ball, Teamwork, Work Rate, Acceleration, Pace, Stamina | First Touch, Marking, Passing, Anticipation, Concentration, Decisions, Flair, Positioning, Agility, Balance |
| FBL-Au | Full-Back Left (Automatic) | both | D (L) | Marking, Tackling, Anticipation, Concentration, Positioning, Teamwork | Crossing, Dribbling, Passing, Technique, Decisions, Work Rate, Agility, Pace, Stamina |
| FBL-D | Full-Back Left (Defend) | both | D (L) | Marking, Tackling, Anticipation, Concentration, Positioning | Crossing, Passing, Decisions, Teamwork, Work Rate, Pace, Stamina |
| FBL-S | Full-Back Left (Support) | both | D (L) | Marking, Tackling, Anticipation, Concentration, Positioning, Teamwork | Crossing, Dribbling, Passing, Technique, Decisions, Work Rate, Pace, Stamina |
| FBR-A | Full-Back Right (Attack) | both | D (R) | Crossing, Dribbling, Tackling, Technique, Off the Ball, Teamwork, Work Rate, Acceleration, Pace, Stamina | First Touch, Marking, Passing, Anticipation, Concentration, Decisions, Flair, Positioning, Agility, Balance |
| FBR-Au | Full-Back Right (Automatic) | both | D (R) | Marking, Tackling, Anticipation, Concentration, Positioning, Teamwork | Crossing, Dribbling, Passing, Technique, Decisions, Work Rate, Agility, Pace, Stamina |
| FBR-D | Full-Back Right (Defend) | both | D (R) | Marking, Tackling, Anticipation, Concentration, Positioning | Crossing, Passing, Decisions, Teamwork, Work Rate, Pace, Stamina |
| FBR-S | Full-Back Right (Support) | both | D (R) | Marking, Tackling, Anticipation, Concentration, Positioning, Teamwork | Crossing, Dribbling, Passing, Technique, Decisions, Work Rate, Pace, Stamina |
| IFBL-D | Inverted Full-Back Left (Defend) | both | D (L) | Heading, Marking, Tackling, Positioning, Strength | Dribbling, First Touch, Passing, Technique, Aggression, Anticipation, Bravery, Composure, Concentration, Decisions, Work Rate, Agility, Jumping Reach, Pace |
| IFBR-D | Inverted Full-Back Right (Defend) | both | D (R) | Heading, Marking, Tackling, Positioning, Strength | Dribbling, First Touch, Passing, Technique, Aggression, Anticipation, Bravery, Composure, Concentration, Decisions, Work Rate, Agility, Jumping Reach, Pace |
| IWBL-A | Inverted Wing-Back Left (Attack) | both | D (L), WB (L) | First Touch, Passing, Tackling, Technique, Composure, Decisions, Off the Ball, Teamwork, Vision, Acceleration | Crossing, Dribbling, Long Shots, Marking, Anticipation, Concentration, Flair, Positioning, Work Rate, Agility, Pace, Stamina |
| IWBL-Au | Inverted Wing-Back Left (Automatic) | both | D (L), WB (L) | First Touch, Passing, Tackling, Composure, Decisions, Teamwork | Marking, Technique, Anticipation, Concentration, Off the Ball, Positioning, Vision, Work Rate, Acceleration, Agility, Stamina |
| IWBL-D | Inverted Wing-Back Left (Defend) | both | D (L), WB (L) | Passing, Tackling, Anticipation, Decisions, Positioning, Teamwork | First Touch, Marking, Technique, Composure, Concentration, Off the Ball, Work Rate, Acceleration, Agility, Stamina |
| IWBL-S | Inverted Wing-Back Left (Support) | both | D (L), WB (L) | First Touch, Passing, Tackling, Composure, Decisions, Teamwork | Marking, Technique, Anticipation, Concentration, Off the Ball, Positioning, Vision, Work Rate, Acceleration, Agility, Stamina |
| IWBR-A | Inverted Wing-Back Right (Attack) | both | D (R), WB (R) | First Touch, Passing, Tackling, Technique, Composure, Decisions, Off the Ball, Teamwork, Vision, Acceleration | Crossing, Dribbling, Long Shots, Marking, Anticipation, Concentration, Flair, Positioning, Work Rate, Agility, Pace, Stamina |
| IWBR-Au | Inverted Wing-Back Right (Automatic) | both | D (R), WB (R) | First Touch, Passing, Tackling, Composure, Decisions, Teamwork | Marking, Technique, Anticipation, Concentration, Off the Ball, Positioning, Vision, Work Rate, Acceleration, Agility, Stamina |
| IWBR-D | Inverted Wing-Back Right (Defend) | both | D (R), WB (R) | Passing, Tackling, Anticipation, Decisions, Positioning, Teamwork | First Touch, Marking, Technique, Composure, Concentration, Off the Ball, Work Rate, Acceleration, Agility, Stamina |
| IWBR-S | Inverted Wing-Back Right (Support) | both | D (R), WB (R) | First Touch, Passing, Tackling, Composure, Decisions, Teamwork | Marking, Technique, Anticipation, Concentration, Off the Ball, Positioning, Vision, Work Rate, Acceleration, Agility, Stamina |
| Lib-S | Libero (Support) | both | D (C) | First Touch, Heading, Marking, Passing, Tackling, Technique, Composure, Decisions, Positioning, Teamwork, Jumping Reach, Strength | Dribbling, Anticipation, Bravery, Concentration, Vision, Pace, Stamina |
| NNFBL-D | No-Nonsense Full-Back Left (Defend) | both | D (L) | Marking, Tackling, Anticipation, Positioning, Strength | Heading, Aggression, Bravery, Concentration, Teamwork |
| NNFBR-D | No-Nonsense Full-Back Right (Defend) | both | D (R) | Marking, Tackling, Anticipation, Positioning, Strength | Heading, Aggression, Bravery, Concentration, Teamwork |
| NNCB-Co | No-Nonsense Centre-Back (Cover) | both | D (C) | Marking, Tackling, Anticipation, Concentration, Positioning, Pace | Heading, Bravery, Jumping Reach, Strength |
| NNCB-D | No-Nonsense Centre-Back (Defend) | both | D (C) | Heading, Marking, Tackling, Positioning, Jumping Reach, Strength | Aggression, Anticipation, Bravery, Composure, Concentration, Pace |
| NNCB-St | No-Nonsense Centre-Back (Stopper) | both | D (C) | Heading, Tackling, Aggression, Bravery, Positioning, Jumping Reach, Strength | Marking, Anticipation, Concentration |
| WCB-A | Wide Centre-Back (Attack) | both | D (C) | Crossing, Dribbling, Heading, Marking, Tackling, Off the Ball, Jumping Reach, Pace, Stamina, Strength | First Touch, Passing, Technique, Aggression, Anticipation, Bravery, Composure, Concentration, Decisions, Positioning, Work Rate, Agility |
| WCB-D | Wide Centre-Back (Defend) | both | D (C) | Handling, Marking, Tackling, Positioning, Jumping Reach, Strength | Dribbling, First Touch, Passing, Technique, Aggression, Anticipation, Bravery, Composure, Concentration, Decisions, Work Rate, Agility, Pace |
| WCB-S | Wide Centre-Back (Support) | both | D (C) | Dribbling, Heading, Marking, Tackling, Positioning, Jumping Reach, Pace, Strength | Crossing, First Touch, Passing, Technique, Aggression, Anticipation, Bravery, Composure, Concentration, Decisions, Off the Ball, Work Rate, Agility, Stamina |
| WBL-A | Wing Back Left (Attack) | both | D (L), WB (L) | Crossing, Dribbling, Tackling, Technique, Off the Ball, Teamwork, Work Rate, Acceleration, Pace, Stamina | First Touch, Marking, Passing, Anticipation, Concentration, Decisions, Flair, Positioning, Agility, Balance |
| WBR-A | Wing Back Right (Attack) | both | D (R), WB (R) | Crossing, Dribbling, Tackling, Technique, Off the Ball, Teamwork, Work Rate, Acceleration, Pace, Stamina | First Touch, Marking, Passing, Anticipation, Concentration, Decisions, Flair, Positioning, Agility, Balance |
| CM-S | Central Midfielder (Support) | both | M (C) | First Touch, Passing, Tackling, Decisions, Teamwork | Technique, Anticipation, Composure, Concentration, Off the Ball, Vision, Work Rate, Stamina |
| DM-S | Defensive Midfielder (Support) | both | DM | Tackling, Anticipation, Concentration, Positioning, Teamwork | First Touch, Marking, Passing, Aggression, Composure, Decisions, Work Rate, Stamina, Strength |
| IWR-S | Inverted Winger Right (Support) | both | AM (R), M (R) | Crossing, Dribbling, Passing, Technique, Acceleration, Agility | First Touch, Long Shots, Composure, Decisions, Off the Ball, Vision, Work Rate, Balance, Pace, Stamina |
| RPM-S | Roaming Playmaker (Support) | both | AM (C), DM, M (C) | First Touch, Passing, Technique, Anticipation, Composure, Decisions, Off the Ball, Teamwork, Vision, Work Rate, Acceleration, Stamina | Dribbling, Long Shots, Concentration, Positioning, Agility, Balance, Pace |
| VOL-A | Segundo Volante (Attack) | both | DM | Finishing, Long Shots, Passing, Tackling, Anticipation, Off the Ball, Positioning, Work Rate, Pace, Stamina | First Touch, Marking, Composure, Concentration, Decisions, Acceleration, Balance, Strength |
| WL-S | Winger Left (Support) | both | AM (L), M (L) | Crossing, Dribbling, Technique, Acceleration, Agility | First Touch, Passing, Off the Ball, Work Rate, Balance, Pace, Stamina |
| WR-S | Winger Right (Support) | both | AM (R), M (R) | Crossing, Dribbling, Technique, Acceleration, Agility | First Touch, Passing, Off the Ball, Work Rate, Balance, Pace, Stamina |
| B2B-S | Box-to-Box Midfielder (Support) | both | M (C) | Passing, Tackling, Off the Ball, Teamwork, Work Rate, Stamina | Dribbling, Finishing, First Touch, Long Shots, Technique, Aggression, Anticipation, Composure, Decisions, Positioning, Acceleration, Balance, Pace, Strength |
| DLP-S | Deep Lying Playmaker (Support) | both | DM, M (C) | First Touch, Passing, Technique, Composure, Decisions, Teamwork, Vision | Anticipation, Off the Ball, Positioning, Balance |
| BWM-D | Ball Winning Midfielder (Defend) | both | DM, M (C) | Tackling, Aggression, Anticipation, Teamwork, Work Rate, Stamina | Marking, Bravery, Concentration, Positioning, Agility, Pace, Strength |
| AM-S | Attacking Midfielder (Support) | both | AM (C) | First Touch, Long Shots, Passing, Technique, Anticipation, Decisions, Flair, Off the Ball | Dribbling, Composure, Vision, Agility |
| AP-A | Advanced Playmaker (Attack) | both | AM (C), AM (L), AM (R), M (C) | First Touch, Passing, Technique, Composure, Decisions, Off the Ball, Teamwork, Vision | Dribbling, Anticipation, Flair, Acceleration, Agility |
| AF-A | Advanced Forward (Attack) | both | ST (C) | Dribbling, Finishing, First Touch, Technique, Composure, Off the Ball, Acceleration | Passing, Anticipation, Decisions, Work Rate, Agility, Balance, Pace, Stamina |
| AM-A | Attacking Midfielder (Attack) | both | AM (C) | Dribbling, First Touch, Long Shots, Passing, Technique, Anticipation, Decisions, Flair, Off the Ball | Finishing, Composure, Vision, Agility |
| CF-S | Complete Forward (Support) | both | ST (C) | Dribbling, First Touch, Heading, Long Shots, Passing, Technique, Anticipation, Composure, Decisions, Off the Ball, Vision, Acceleration, Agility, Strength | Finishing, Teamwork, Work Rate, Balance, Jumping Reach, Pace, Stamina |
| IFL-A | Inside Forward Left (Attack) | both | AM (L) | Dribbling, Finishing, First Touch, Technique, Anticipation, Off the Ball, Acceleration, Agility | Long Shots, Passing, Composure, Flair, Work Rate, Pace, Stamina, Balance |
| IFL-S | Inside Forward Left (Support) | both | AM (L) | Dribbling, Finishing, First Touch, Technique, Off the Ball, Acceleration, Agility | Long Shots, Passing, Anticipation, Composure, Flair, Vision, Work Rate, Pace, Stamina, Balance |
| IFR-A | Inside Forward Right (Attack) | both | AM (R) | Dribbling, Finishing, First Touch, Technique, Anticipation, Off the Ball, Acceleration, Agility | Long Shots, Passing, Composure, Flair, Work Rate, Pace, Stamina, Balance |
| IFR-S | Inside Forward Right (Support) | both | AM (R) | Dribbling, Finishing, First Touch, Technique, Anticipation, Acceleration, Agility | Long Shots, Passing, Composure, Flair, Work Rate, Balance, Pace, Stamina |
| PF-S | Pressing Forward (Support) | both | ST (C) | Aggression, Anticipation, Bravery, Decisions, Teamwork, Work Rate, Acceleration, Pace, Stamina | First Touch, Passing |
| P-A | Poacher (Attack) | both | ST (C) | Finishing, Anticipation, Composure, Off the Ball | First Touch, Heading, Technique, Decisions, Acceleration |
| FN-S | False Nine (Support) | both | ST (C) | Dribbling, First Touch, Passing, Technique, Composure, Decisions, Off the Ball, Vision, Acceleration, Agility | Finishing, Anticipation, Flair, Teamwork, Balance |
| RL-A | Raumdeuter Left (Attack) | both | AM (L) | Finishing, Anticipation, Composure, Concentration, Decisions, Off the Ball, Balance | First Touch, Technique, Work Rate, Acceleration, Stamina |
| RR-A | Raumdeuter Right (Attack) | both | AM (R) | Finishing, Anticipation, Composure, Concentration, Decisions, Off the Ball, Balance | First Touch, Technique, Work Rate, Acceleration, Stamina |
| AP-S | Advanced Playmaker (Support) | both | AM (C), AM (L), AM (R), M (C) | First Touch, Passing, Technique, Composure, Decisions, Off the Ball, Teamwork, Vision | Dribbling, Anticipation, Flair, Agility |
| DLF-S | Deep Lying Forward (Support) | both | ST (C) | First Touch, Passing, Technique, Composure, Decisions, Off the Ball, Teamwork | Finishing, Anticipation, Flair, Vision, Balance, Strength |

### Tácticas incluidas

Cada pareja asigna un puesto a un rol. Se conserva el orden del recurso.

| Táctica | Fase | Puesto → rol |
| --- | --- | --- |
| 4-2-4 EF 424 IF HP | both | GK → SK-D, DR → FBR-A, DCR → BPD-D, DCL → BPD-D, DL → FBL-A, DMCR → DM-S, DMCL → DM-S, AMR → IFR-A, AML → IFL-S, STR → PF-S, STL → AF-A |
| 4-2-3-1 Highway Star | both | GK → SK-D, DR → FBR-A, DCR → BPD-D, DCL → BPD-D, DL → FBL-A, DMCR → DM-S, DMCL → DM-S, AMR → IFR-S, AMC → AM-A, AML → IFL-S, ST → AF-A |
| 4-5-1 Speed King | both | GK → SK-D, DR → FBR-A, DCR → BPD-D, DCL → BPD-D, DL → FBL-A, DMC → DM-S, MCR → CM-S, MCL → CM-S, AMR → IFR-S, AML → IFL-S, ST → AF-A |
| 4-2-2-2 Silvermoon | both | GK → SK-D, DR → FBR-A, DCR → BPD-D, DCL → BPD-D, DL → FBL-A, DMCR → VOL-A, DMCL → RPM-S, MR → IWR-S, AMCL → AM-A, STC → PF-S, STL → AF-A |
| 4-2-1-1-2 Perfect Strangers | both | GK → SK-D, DR → FBR-A, DCR → BPD-D, DCL → BPD-D, DL → IWBL-A, DMCR → DM-S, DMCL → VOL-A, ML → WL-S, AMR → IFR-S, STR → PF-S, STL → AF-A |
| 4-2-2-2 The In Law | both | GK → SK-D, DR → FBR-A, DCR → BPD-D, DCL → BPD-D, DL → FBL-A, DMCR → RPM-S, DMCL → VOL-A, AMCR → AM-A, AML → IFL-A, STR → AF-A, STC → CF-S |
| 4-3-3 Striker Force | both | GK → SK-D, DL → IWBL-A, DCL → BPD-D, DCR → BPD-D, DR → FBR-A, DMCL → VOL-A, DMCR → DM-S, ML → WL-S, STL → AF-A, STC → CF-S, STR → PF-S |
| 4-2-1-3 Emerald Cucumber | both | GK → SK-D, DL → FBL-A, DCL → BPD-D, DCR → BPD-D, DR → FBR-A, DMCL → VOL-A, DMCR → DM-S, AMR → IFR-S, STL → PF-S, STC → AF-A, STR → PF-S |
| 4-1-1-4 Moo Grob | both | GK → SK-D, DL → FBL-A, DCL → BPD-D, DCR → BPD-D, DR → FBR-A, DMCR → DM-S, MCL → CM-S, AML → IFL-S, AMR → IFR-S, STL → AF-A, STR → PF-S |
| 4-4-2 GIN 25 | both | GK → SK-D, DL → FBL-S, DCL → BPD-D, DCR → BPD-D, DR → FBR-S, ML → WL-S, MCL → VOL-A, MCR → VOL-A, MR → WR-S, STL → AF-A, STR → PF-S |
| 4-1-2-1-2 Snowblind | both | GK → SK-D, DCL → BPD-D, STR → PF-S, STL → AF-A, DL → IWBL-A, DCR → BPD-D, MCL → B2B-S, AMR → IFR-S, DMCR → DM-S, DR → FBR-A, ML → WL-S |
| 3-4-2-1 Amorim Speciale | both | GK → SK-S, DCR → BPD-D, MCL → BWM-D, DMR → WBR-A, AMCL → AP-S, DML → WBL-A, STC → AF-A, DCL → BPD-D, AMR → IFR-A, MCR → DLP-S, DC → BPD-D |
| 3-3-3-1 Cruyff Perfect | both | GK → SK-D, DC → BPD-D, AMR → WR-S, MCR → CM-S, AML → WL-S, MCL → RPM-S, DCL → WCB-D, DMC → DM-S, AMC → AM-S, DCR → WCB-D, STC → DLF-S |
| 3-3-2-2 Broken | both | GK → SK-D, STL → AF-A, MR → WR-S, DC → BPD-D, ML → WL-S, AMCR → AM-S, AMCL → AM-S, DCL → WCB-D, DMC → DM-S, DCR → WCB-D, STR → AF-A |
| 3-5-2 Most Hated | both | GK → SK-S, MCR → AP-A, DML → WBL-A, STL → AF-A, DC → BPD-Co, DCL → BPD-D, DMC → DM-S, DCR → BPD-D, MCL → B2B-S, STR → AF-A, DMR → WBR-A |

### Clasificación inicial de la personalidad

| Clase | Personalidades |
| --- | --- |
| good | Model Citizen, Perfectionist, Resolute, Model Professional, Professional, Fairly Professional, Spirited, Driven, Determined, Fairly Determined, Iron Willed, Resilient, Charismatic Leader, Born Leader, Leader, Very Ambitious, Ambitious, Fairly Ambitious, Fickle, Mercenary |
| neutral | Jovial, Light-Hearted, Devoted, Very Loyal, Loyal, Fairly Loyal, Honest, Sporting, Fairly Sporting, Unsporting, Realist, Balanced |
| bad | Slack, Casual, Temperamental, Easily Discouraged, Low Determination, Spineless, Low Self-Belief, Unambitious |


## FM27

Evidencia de los valores iniciales. [Definiciones FM27](../../old_version/desktop/src/app/resources/definitions_fm27.json).

| Rol | Nombre | Fase | Posiciones compatibles (primera = padre inicial) | Atributos key | Atributos preferable |
| --- | --- | --- | --- | --- | --- |
| BGK | Ball-Playing Goalkeeper | ip | GK | Aerial Reach, Command of Area, Communication, Handling, Kicking, Reflexes, Concentration, Positioning, Agility | Eccentricity, One vs One, Passing, Throwing, Anticipation, Composure, Decisions |
| GK-IP | Goalkeeper | ip | GK | Aerial Reach, Command of Area, Communication, Handling, Reflexes, Concentration, Positioning, Agility | Kicking, One vs One, Throwing, Anticipation, Decisions |
| NGK | No-Nonsense Goalkeeper | ip | GK | Aerial Reach, Command of Area, Communication, Handling, Reflexes, Concentration, Positioning, Agility | One vs One, Anticipation, Decisions |
| GK-OOP | Goalkeeper | oop | GK | Aerial Reach, Command of Area, Communication, Handling, Reflexes, Concentration, Positioning, Agility | One vs One, Rushing Out (Tendency), Anticipation, Decisions |
| LHK | Line-Holding Goalkeeper | oop | GK | Aerial Reach, Command of Area, Communication, Handling, Reflexes, Concentration, Positioning, Agility | One vs One, Anticipation, Decisions |
| SK | Sweeper Keeper | oop | GK | Aerial Reach, Command of Area, Communication, Handling, One vs One, Reflexes, Rushing Out (Tendency), Anticipation, Decisions, Agility | Eccentricity, Concentration, Positioning, Acceleration |
| ACB | Advanced Centre-Back | ip | D (C) | Heading, Marking, Passing, Tackling, Technique, Anticipation, Composure, Decisions, Positioning, Teamwork, Jumping Reach, Strength | Dribbling, First Touch, Aggression, Bravery, Concentration, Vision, Pace, Stamina |
| BCB | Ball-Playing Centre-Back | ip | D (C) | Heading, Marking, Passing, Tackling, Anticipation, Composure, Positioning, Jumping Reach, Strength | First Touch, Technique, Aggression, Bravery, Concentration, Decisions, Vision, Pace |
| CB-IP | Centre-Back | ip | D (C) | Heading, Marking, Tackling, Anticipation, Positioning, Jumping Reach, Strength | Aggression, Bravery, Composure, Concentration, Decisions, Pace |
| NCB | No-Nonsense Centre-Back | ip | D (C) | Heading, Marking, Tackling, Anticipation, Positioning, Jumping Reach, Strength | Aggression, Bravery, Concentration, Pace |
| OCB | Overlapping Centre-Back | ip | D (C) | Crossing, Heading, Marking, Tackling, Anticipation, Work Rate, Jumping Reach, Pace, Stamina, Strength | Dribbling, Technique, Aggression, Bravery, Composure, Concentration, Decisions, Off the Ball, Positioning, Acceleration, Agility |
| WCB-IP | Wide Centre-Back | ip | D (C) | Heading, Marking, Tackling, Anticipation, Positioning, Jumping Reach, Strength | Dribbling, Aggression, Bravery, Composure, Concentration, Decisions, Work Rate, Acceleration, Agility, Pace, Stamina |
| FB-IP | Full-Back | ip | D (L), D (R) | Marking, Tackling, Anticipation, Concentration, Positioning, Teamwork, Acceleration | Crossing, Dribbling, Passing, Technique, Decisions, Work Rate, Agility, Pace, Stamina |
| IFB | Inside Full-Back | ip | D (L), D (R) | Heading, Marking, Tackling, Anticipation, Positioning, Strength | Dribbling, Aggression, Bravery, Composure, Concentration, Decisions, Work Rate, Acceleration, Agility, Jumping Reach, Pace, Stamina |
| IWB | Inside Wing-Back | ip | D (L), D (R), WB (L), WB (R) | Passing, Tackling, Anticipation, Composure, Decisions, Positioning, Teamwork, Acceleration | First Touch, Marking, Technique, Concentration, Work Rate, Agility, Pace, Stamina |
| PWB-IP | Playmaking Wing-Back | ip | D (L), D (R), WB (L), WB (R) | First Touch, Passing, Tackling, Technique, Composure, Decisions, Teamwork, Vision, Acceleration | Crossing, Dribbling, Marking, Anticipation, Concentration, Off the Ball, Positioning, Work Rate, Agility, Pace, Stamina |
| WB-IP | Wing-Back | ip | D (L), D (R), WB (L), WB (R) | Crossing, Marking, Tackling, Teamwork, Work Rate, Acceleration, Pace, Stamina | Dribbling, First Touch, Passing, Technique, Anticipation, Concentration, Decisions, Off the Ball, Positioning, Agility, Balance |
| AWB | Advanced Wing-Back | ip | WB (L), WB (R) | Crossing, Dribbling, Technique, Off the Ball, Teamwork, Work Rate, Acceleration, Agility, Pace, Stamina | First Touch, Marking, Passing, Tackling, Anticipation, Decisions, Flair, Positioning, Balance |
| CB-OOP | Centre-Back | oop | D (C) | Heading, Marking, Tackling, Anticipation, Positioning, Jumping Reach, Strength | Aggression, Bravery, Composure, Concentration, Decisions, Pace |
| CCB | Covering Centre-Back | oop | D (C) | Heading, Marking, Tackling, Anticipation, Concentration, Decisions, Positioning, Jumping Reach, Pace, Strength | Bravery, Acceleration |
| CWD | Covering Wide Centre-Back | oop | D (C) | Heading, Marking, Tackling, Anticipation, Concentration, Decisions, Positioning, Jumping Reach, Pace, Strength | Bravery, Acceleration, Agility |
| SCB | Stopping Centre-Back | oop | D (C) | Heading, Marking, Tackling, Aggression, Anticipation, Bravery, Positioning, Jumping Reach, Strength | Concentration, Decisions, Pace |
| SWD | Stopping Wide Centre-Back | oop | D (C) | Heading, Marking, Tackling, Aggression, Anticipation, Bravery, Positioning, Jumping Reach, Strength | Concentration, Decisions, Acceleration, Agility, Pace |
| WCB-OOP | Wide Centre-Back | oop | D (C) | Heading, Marking, Tackling, Anticipation, Positioning, Jumping Reach, Strength | Aggression, Bravery, Concentration, Decisions, Acceleration, Agility, Pace |
| FB-OOP | Full-Back | oop | D (L), D (R) | Marking, Tackling, Anticipation, Positioning, Teamwork, Acceleration | Aggression, Concentration, Decisions, Work Rate, Agility, Pace, Stamina |
| HFB | Holding Full-Back | oop | D (L), D (R) | Marking, Tackling, Anticipation, Concentration, Decisions, Positioning, Acceleration | Teamwork, Agility, Pace |
| PFB | Pressing Full-Back | oop | D (L), D (R) | Marking, Tackling, Aggression, Anticipation, Positioning, Teamwork, Work Rate, Acceleration | Bravery, Concentration, Decisions, Agility, Pace, Stamina |
| HWB | Holding Wing-Back | oop | WB (L), WB (R) | Marking, Tackling, Anticipation, Concentration, Decisions, Positioning, Acceleration | Teamwork, Agility, Pace, Stamina |
| PWB-OOP | Pressing Wing-Back | oop | WB (L), WB (R) | Marking, Tackling, Aggression, Anticipation, Positioning, Teamwork, Work Rate, Acceleration, Stamina | Bravery, Concentration, Decisions, Agility, Pace |
| WB-OOP | Wing-Back | oop | WB (L), WB (R) | Marking, Tackling, Anticipation, Positioning, Teamwork, Acceleration, Stamina | Aggression, Concentration, Decisions, Work Rate, Agility, Pace |
| BBM | Box-To-Box Midfielder | ip | DM | Passing, Tackling, Off the Ball, Teamwork, Work Rate, Stamina | Dribbling, Finishing, First Touch, Long Shots, Technique, Aggression, Anticipation, Composure, Decisions, Positioning, Acceleration, Balance, Pace, Strength |
| BBP | Box-To-Box Playmaker | ip | DM | First Touch, Passing, Technique, Composure, Decisions, Off the Ball, Teamwork, Vision, Work Rate, Stamina | Dribbling, Long Shots, Marking, Tackling, Anticipation, Positioning, Acceleration, Agility, Balance, Pace |
| DLP | Deep-Lying Playmaker | ip | DM | First Touch, Passing, Technique, Composure, Decisions, Teamwork, Vision | Marking, Tackling, Anticipation, Concentration, Off the Ball, Positioning, Work Rate, Agility, Balance, Stamina |
| DM-IP | Defensive Midfielder | ip | DM | Tackling, Anticipation, Concentration, Positioning, Teamwork | First Touch, Marking, Passing, Aggression, Composure, Decisions, Work Rate, Stamina, Strength |
| HB | Half Back | ip | DM | Heading, Marking, Tackling, Anticipation, Concentration, Positioning, Teamwork, Jumping Reach, Strength | First Touch, Passing, Aggression, Bravery, Composure, Decisions, Work Rate, Stamina |
| AP | Advanced Playmaker | ip | AM (C), M (C) | First Touch, Passing, Technique, Composure, Decisions, Off the Ball, Teamwork, Vision | Crossing, Dribbling, Anticipation, Flair, Acceleration, Agility |
| AM-IP | Attacking Midfielder | ip | AM (C), M (C) | First Touch, Long Shots, Passing, Technique, Composure, Flair, Off the Ball | Crossing, Dribbling, Finishing, Anticipation, Decisions, Vision, Acceleration, Agility |
| CM-IP | Central Midfielder | ip | M (C) | First Touch, Passing, Tackling, Decisions, Teamwork | Technique, Anticipation, Composure, Concentration, Off the Ball, Positioning, Vision, Work Rate, Stamina |
| CHM | Channel Midfielder | ip | AM (C), M (C) | Crossing, First Touch, Passing, Technique, Composure, Off the Ball, Work Rate, Acceleration | Dribbling, Long Shots, Anticipation, Decisions, Flair, Vision, Agility, Pace, Stamina |
| MPM | Midfield Playmaker | ip | M (C) | First Touch, Passing, Technique, Composure, Decisions, Off the Ball, Teamwork, Vision | Dribbling, Tackling, Anticipation, Flair, Positioning, Work Rate, Agility, Stamina |
| WCM-IP | Wide Central Midfielder | ip | M (C) | First Touch, Passing, Tackling, Decisions, Teamwork | Crossing, Dribbling, Technique, Anticipation, Composure, Concentration, Off the Ball, Positioning, Vision, Work Rate, Agility, Stamina |
| IW | Inside Winger | ip | AM (L), AM (R), M (L), M (R) | Dribbling, First Touch, Technique, Composure, Teamwork, Acceleration, Agility | Crossing, Long Shots, Passing, Anticipation, Flair, Off the Ball, Vision, Work Rate, Balance, Pace, Stamina |
| PW | Playmaking Winger | ip | AM (L), AM (R), M (L), M (R) | Crossing, Dribbling, First Touch, Passing, Technique, Composure, Decisions, Off the Ball, Teamwork, Vision, Acceleration | Anticipation, Flair, Work Rate, Agility, Pace, Stamina |
| WM | Wide Midfielder | ip | M (L), M (R) | Crossing, Passing, Technique, Decisions, Teamwork, Work Rate, Pace, Stamina | Dribbling, First Touch, Anticipation, Composure, Off the Ball, Vision, Acceleration, Agility |
| W-IP | Winger | ip | AM (L), AM (R), M (L), M (R) | Crossing, Dribbling, Technique, Teamwork, Acceleration, Agility, Pace | First Touch, Passing, Anticipation, Flair, Off the Ball, Work Rate, Balance, Stamina |
| DM-OOP | Defensive Midfielder | oop | DM | Tackling, Anticipation, Decisions, Positioning, Teamwork, Work Rate | Marking, Aggression, Concentration, Stamina, Strength |
| DDM | Dropping Defensive Midfielder | oop | DM | Marking, Tackling, Anticipation, Concentration, Decisions, Positioning, Teamwork, Work Rate, Jumping Reach, Strength | Aggression, Stamina |
| PDM | Pressing Defensive Midfielder | oop | DM | Tackling, Aggression, Anticipation, Decisions, Positioning, Teamwork, Work Rate, Stamina | Marking, Concentration, Pace, Strength |
| SDM | Screening Defensive Midfielder | oop | DM | Marking, Tackling, Anticipation, Concentration, Decisions, Positioning | Teamwork, Work Rate, Stamina, Strength |
| WDM | Wide Covering Defensive Midfielder | oop | DM | Marking, Tackling, Anticipation, Decisions, Positioning, Teamwork, Work Rate, Agility, Stamina | Aggression, Concentration, Pace, Strength |
| CM-OOP | Central Midfielder | oop | M (C) | Tackling, Decisions, Teamwork, Work Rate | Marking, Aggression, Anticipation, Concentration, Positioning, Stamina |
| PCM | Pressing Central Midfielder | oop | M (C) | Tackling, Aggression, Anticipation, Decisions, Teamwork, Work Rate, Stamina | Marking, Concentration, Positioning, Pace, Strength |
| SCM | Screening Central Midfielder | oop | M (C) | Marking, Tackling, Concentration, Decisions, Positioning | Anticipation, Teamwork, Work Rate, Stamina |
| WCM-OOP | Wide Covering Central Midfielder | oop | M (C) | Marking, Tackling, Decisions, Teamwork, Work Rate, Agility, Stamina | Aggression, Anticipation, Concentration, Positioning, Pace |
| TWM | Tracking Wide Midfielder | oop | M (L), M (R) | Aggression, Anticipation, Decisions, Teamwork, Work Rate, Acceleration, Stamina | Marking, Tackling, Off the Ball, Positioning, Agility, Pace |
| WMF | Wide Midfielder | oop | M (L), M (R) | Decisions, Teamwork, Work Rate, Acceleration | Marking, Aggression, Anticipation, Off the Ball, Agility, Pace, Stamina |
| OWM | Wide Outlet Wide Midfielder | oop | M (L), M (R) | Anticipation, Concentration, Decisions, Off the Ball, Teamwork, Pace | Dribbling, First Touch, Marking, Composure, Acceleration, Agility |
| FR | Free Role | ip | AM (C) | Dribbling, First Touch, Long Shots, Passing, Technique, Composure, Flair, Off the Ball, Vision | Crossing, Finishing, Anticipation, Decisions, Acceleration, Agility |
| SS | Second Striker | ip | AM (C) | Finishing, First Touch, Anticipation, Composure, Off the Ball, Acceleration | Dribbling, Long Shots, Passing, Technique, Concentration, Decisions, Work Rate, Agility, Pace, Stamina |
| IF | Inside Forward | ip | AM (L), AM (R) | Dribbling, First Touch, Technique, Anticipation, Composure, Off the Ball, Acceleration, Agility | Crossing, Finishing, Long Shots, Passing, Flair, Vision, Work Rate, Balance, Pace, Stamina |
| WFD | Wide Forward | ip | AM (L), AM (R) | Dribbling, First Touch, Technique, Anticipation, Off the Ball, Acceleration, Agility, Pace | Crossing, Finishing, Passing, Composure, Flair, Work Rate, Balance, Stamina |
| CFD-IP | Centre Forward | ip | ST (C) | Finishing, First Touch, Heading, Technique, Composure, Off the Ball, Acceleration, Strength | Dribbling, Passing, Anticipation, Decisions, Agility, Balance, Jumping Reach, Pace |
| CHF | Channel Forward | ip | ST (C) | Dribbling, Finishing, First Touch, Technique, Composure, Off the Ball, Work Rate, Acceleration | Crossing, Heading, Passing, Anticipation, Decisions, Agility, Balance, Pace, Stamina |
| DLF | Deep-Lying Forward | ip | ST (C) | Finishing, First Touch, Technique, Composure, Off the Ball, Strength | Dribbling, Passing, Anticipation, Decisions, Teamwork, Vision, Balance |
| F9 | False Nine | ip | ST (C) | Dribbling, First Touch, Passing, Technique, Composure, Decisions, Off the Ball, Teamwork, Vision, Acceleration | Finishing, Anticipation, Flair, Agility, Balance |
| P | Poacher | ip | ST (C) | Finishing, Heading, Anticipation, Composure, Concentration, Off the Ball, Acceleration | First Touch, Technique, Decisions, Balance |
| TF | Target Forward | ip | ST (C) | Finishing, Heading, Aggression, Bravery, Composure, Off the Ball, Balance, Jumping Reach, Strength | First Touch, Anticipation, Decisions, Teamwork |
| AM-OOP | Attacking Midfielder | oop | AM (C) | Anticipation, Decisions, Work Rate | Marking, Aggression, Off the Ball, Teamwork, Stamina |
| OAM | Central Outlet Attacking Midfielder | oop | AM (C) | Anticipation, Concentration, Decisions, Off the Ball, Teamwork, Balance | First Touch, Marking, Composure, Strength |
| SAM | Splitting Outlet Attacking Midfielder | oop | AM (C) | Anticipation, Concentration, Decisions, Off the Ball, Teamwork, Pace | First Touch, Marking, Composure, Acceleration |
| TAM | Tracking Attacking Midfielder | oop | AM (C) | Aggression, Anticipation, Decisions, Teamwork, Work Rate, Stamina | Marking, Off the Ball, Positioning |
| IOW | Inside Outlet Winger | oop | AM (L), AM (R) | Anticipation, Concentration, Decisions, Off the Ball, Teamwork, Balance | Dribbling, First Touch, Marking, Composure, Acceleration, Strength |
| TW | Tracking Winger | oop | AM (L), AM (R) | Aggression, Anticipation, Decisions, Teamwork, Work Rate, Acceleration, Stamina | Marking, Tackling, Off the Ball, Positioning, Agility, Pace |
| WOW | Wide Outlet Winger | oop | AM (L), AM (R) | Anticipation, Concentration, Decisions, Off the Ball, Teamwork, Pace | Dribbling, First Touch, Marking, Composure, Acceleration, Agility |
| W-OOP | Winger | oop | AM (L), AM (R) | Anticipation, Decisions, Teamwork, Work Rate, Acceleration | Marking, Aggression, Off the Ball, Agility, Pace, Stamina |
| OCF | Central Outlet Centre Forward | oop | ST (C) | Anticipation, Concentration, Decisions, Off the Ball, Teamwork, Balance | First Touch, Marking, Composure, Strength |
| CFD-OOP | Centre Forward | oop | ST (C) | Anticipation, Decisions, Work Rate | Marking, Aggression, Off the Ball, Teamwork, Stamina |
| SCF | Splitting Outlet Centre Forward | oop | ST (C) | Anticipation, Concentration, Decisions, Off the Ball, Teamwork, Pace | First Touch, Marking, Composure, Acceleration |
| TCF | Tracking Centre Forward | oop | ST (C) | Aggression, Anticipation, Decisions, Teamwork, Work Rate, Stamina | Marking, Off the Ball, Positioning |

### Tácticas incluidas

Cada pareja asigna un puesto a un rol. Se conserva el orden del recurso.

| Táctica | Fase | Puesto → rol |
| --- | --- | --- |
| Control Press · 4-2-3-1 | ip | GK → BGK, DL → WB-IP, DCL → BCB, DCR → BCB, DR → WB-IP, DMCL → DLP, DMCR → DM-IP, AML → IF, AMC → AM-IP, AMR → IW, STC → DLF |
| Control Press · 4-2-3-1 | oop | GK → SK, DL → FB-OOP, DCL → CB-OOP, DCR → CB-OOP, DR → FB-OOP, DMC → SDM, MCL → PCM, MCR → CM-OOP, AML → TW, AMR → TW, STC → CFD-OOP |
| Counter Surge · 3-4-2-1 | ip | GK → GK-IP, DCL → WCB-IP, DC → BCB, DCR → WCB-IP, DML → WB-IP, DMCL → BBM, DMCR → DLP, DMR → WB-IP, AMCL → AP, AMCR → CHM, STC → TF |
| Counter Surge · 3-4-2-1 | oop | GK → GK-OOP, DL → HFB, DCL → CWD, DC → CCB, DCR → CWD, DR → HFB, DMCL → PDM, DMCR → SDM, ML → TWM, MR → OWM, STC → OCF |

### Plantillas de formación

| Fase | Formación | Puestos |
| --- | --- | --- |
| ip | 3-2-4-1 | GK, DCL, DC, DCR, DMCL, DMCR, AML, AMCL, AMCR, AMR, STC |
| ip | 3-4-3 DM Narrow | GK, DCL, DC, DCR, DML, DMCL, DMCR, DMR, AMCL, AMCR, STC |
| ip | 3-4-3 DM Wide | GK, DCL, DC, DCR, DML, DMCL, DMCR, DMR, AML, AMR, STC |
| ip | 4-2-2-2 AM Narrow | GK, DL, DCL, DCR, DR, DMCL, DMCR, AMCL, AMCR, STL, STR |
| ip | 4-2-2-2 DM Wide | GK, DL, DCL, DCR, DR, DMCL, DMCR, ML, MR, STL, STR |
| ip | 4-2-3-1 DM Wide | GK, DL, DCL, DCR, DR, DMCL, DMCR, AML, AMC, AMR, STC |
| ip | 4-2-4 DM Wide | GK, DL, DCL, DCR, DR, DMCL, DMCR, AML, AMR, STL, STR |
| ip | 4-3-2-1 DM Narrow | GK, DL, DCL, DCR, DR, DMC, MCL, MCR, AMCL, AMCR, STC |
| ip | 4-3-3 DM Wide | GK, DL, DCL, DCR, DR, DMC, MCL, MCR, AML, AMR, STC |
| ip | 4-4-2 Diamond Narrow | GK, DL, DCL, DCR, DR, DMC, MCL, MCR, AMC, STL, STR |
| ip | 5-1-2-2 DM WB | GK, DCL, DC, DCR, DML, DMC, DMR, MCL, MCR, STL, STR |
| ip | 5-2-1-2 DM WB | GK, DCL, DC, DCR, DML, DMCL, DMCR, DMR, AMC, STL, STR |
| oop | 3-4-3 DM Narrow | GK, DCL, DC, DCR, DML, DMCL, DMCR, DMR, AMCL, AMCR, STC |
| oop | 3-4-3 DM Wide | GK, DCL, DC, DCR, DML, DMCL, DMCR, DMR, AML, AMR, STC |
| oop | 4-1-4-1 DM | GK, DL, DCL, DCR, DR, DMC, ML, MCL, MCR, MR, STC |
| oop | 4-2-2-2 DM Wide | GK, DL, DCL, DCR, DR, DMCL, DMCR, ML, MR, STL, STR |
| oop | 4-3-2-1 DM Narrow | GK, DL, DCL, DCR, DR, DMC, MCL, MCR, AMCL, AMCR, STC |
| oop | 4-3-3 DM Wide | GK, DL, DCL, DCR, DR, DMC, MCL, MCR, AML, AMR, STC |
| oop | 4-4-1-1 2 DM | GK, DL, DCL, DCR, DR, DMCL, DMCR, ML, MR, AMC, STC |
| oop | 4-4-2 Diamond Narrow | GK, DL, DCL, DCR, DR, DMC, MCL, MCR, AMC, STL, STR |
| oop | 5-1-2-2 DM WB | GK, DCL, DC, DCR, DML, DMC, DMR, MCL, MCR, STL, STR |
| oop | 5-2-1-2 DM WB | GK, DCL, DC, DCR, DML, DMCL, DMCR, DMR, AMC, STL, STR |

### Desplazamiento asociado al rol

Valores del recurso por posición. Su uso afecta a la representación táctica. La distancia del optimizador se calcula entre anclas de los puestos.

| Rol | Posición | Valor |
| --- | --- | --- |
| BGK | GK | 2.3 |
| LHK | GK | -2.3 |
| SK | GK | 2.3 |
| ACB | D (C) | 1.1 |
| OCB | D (C) | 2.2 |
| WCB-IP | D (C) | 1.1 |
| IWB | D (L) | 1.1 |
| IWB | D (R) | 1.1 |
| IWB | WB (L) | 1.1 |
| IWB | WB (R) | 1.1 |
| PWB-IP | D (L) | 2.2 |
| PWB-IP | D (R) | 2.2 |
| PWB-IP | WB (L) | 2.3 |
| PWB-IP | WB (R) | 2.3 |
| WB-IP | D (L) | 2.2 |
| WB-IP | D (R) | 2.2 |
| WB-IP | WB (L) | 2.3 |
| WB-IP | WB (R) | 2.3 |
| AWB | WB (L) | 4.5 |
| AWB | WB (R) | 4.5 |
| CCB | D (C) | -2.1 |
| CWD | D (C) | -1.1 |
| SCB | D (C) | 2.3 |
| SWD | D (C) | 3.4 |
| WCB-OOP | D (C) | 1.1 |
| HFB | D (L) | -1.1 |
| HFB | D (R) | -1.1 |
| PFB | D (L) | 2.3 |
| PFB | D (R) | 2.3 |
| HWB | WB (L) | -1.2 |
| HWB | WB (R) | -1.2 |
| PWB-OOP | WB (L) | 2.1 |
| PWB-OOP | WB (R) | 2.1 |
| BBM | DM | 2.3 |
| BBP | DM | 2.3 |
| HB | DM | -2.3 |
| AP | M (C) | 2.3 |
| AM-IP | M (C) | 2.3 |
| CHM | AM (C) | 2.2 |
| CHM | M (C) | 2.3 |
| W-IP | AM (L) | 3.2 |
| W-IP | AM (R) | 3.2 |
| W-IP | M (L) | 3.4 |
| W-IP | M (R) | 3.4 |
| DDM | DM | -2.3 |
| PDM | DM | 2.1 |
| SDM | DM | -1.2 |
| PCM | M (C) | 2.3 |
| SCM | M (C) | -1.1 |
| TWM | M (L) | -2.3 |
| TWM | M (R) | -2.3 |
| OWM | M (L) | 2.3 |
| OWM | M (R) | 2.3 |
| SS | AM (C) | 2.2 |
| IF | AM (L) | 3.2 |
| IF | AM (R) | 3.2 |
| WFD | AM (L) | 3.4 |
| WFD | AM (R) | 3.4 |
| CHF | ST (C) | 1.1 |
| DLF | ST (C) | -1.1 |
| F9 | ST (C) | -3.4 |
| P | ST (C) | 2.3 |
| OAM | AM (C) | 2.3 |
| SAM | AM (C) | 2.3 |
| TAM | AM (C) | -2.2 |
| IOW | AM (L) | 2.3 |
| IOW | AM (R) | 2.3 |
| TW | AM (L) | -2.2 |
| TW | AM (R) | -2.2 |
| WOW | AM (L) | 2.2 |
| WOW | AM (R) | 2.2 |
| OCF | ST (C) | 2.3 |
| SCF | ST (C) | 2.1 |
| TCF | ST (C) | -2.3 |

### Clasificación inicial de la personalidad

| Clase | Personalidades |
| --- | --- |
| good | Model Citizen, Perfectionist, Resolute, Model Professional, Professional, Fairly Professional, Spirited, Driven, Determined, Fairly Determined, Iron Willed, Resilient, Charismatic Leader, Born Leader, Leader, Very Ambitious, Ambitious, Fairly Ambitious, Fickle, Mercenary |
| neutral | Jovial, Light-Hearted, Devoted, Very Loyal, Loyal, Fairly Loyal, Honest, Sporting, Fairly Sporting, Unsporting, Realist, Balanced |
| bad | Slack, Casual, Temperamental, Easily Discouraged, Low Determination, Spineless, Low Self-Belief, Unambitious |

