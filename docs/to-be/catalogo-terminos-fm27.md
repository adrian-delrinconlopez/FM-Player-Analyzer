# Catálogo de términos FM27

Referencia de localización castellana para la v1. Las etiquetas y los ordinales de atributos y roles proceden de la interfaz de Football Manager 27 para PC, Steam build `26620374`, observada el 25 de noviembre de 2026. Los ID y las etiquetas inglesas de referencia proceden del AS-IS 2.3.1. La correspondencia entre ID y etiqueta se documenta por significado, fase y familia del rol. El inglés oficial y las abreviaturas individuales de roles y atributos quedan pendientes de cotejo. [A16: roles y posiciones](../as-is/16-tablas-de-roles-y-tacticas.md) · [A17: atributos](../as-is/17-parametros-y-atributos.md)

## Atributos

**ES-27-01 · Primer equipo → ficha del jugador → Vista general.** Orden de arriba abajo dentro de cada grupo. `—` indica que la ficha de ese tipo no muestra el atributo. El catálogo reúne 47 claves distintas. La ficha de campo muestra 36 y la de portero muestra 38.

| Ficha | Columna izquierda | Columna central | Columna derecha |
|---|---|---|---|
| Campo | Técnico (10) y Balón parado (4) | Mental (14) | Físico (8) |
| Portero | Portería (13) | Mental (14) | Físico (8) y Técnico (3) |

| ID | EN de referencia | ES observado | Grupo y orden en campo | Grupo y orden en portero |
|---|---|---|---|---|
| attr.heading | Heading | Cabeceo | Técnico 1 | — |
| attr.crossing | Crossing | Centros | Técnico 2 | — |
| attr.first_touch | First Touch | Control | Técnico 3 | Portería 4 |
| attr.tackling | Tackling | Entradas | Técnico 4 | — |
| attr.marking | Marking | Marcaje | Técnico 5 | — |
| attr.passing | Passing | Pase | Técnico 6 | Portería 8 |
| attr.dribbling | Dribbling | Regate | Técnico 7 | — |
| attr.finishing | Finishing | Remate | Técnico 8 | — |
| attr.technique | Technique | Técnica | Técnico 9 | Técnico 2 |
| attr.long_shots | Long Shots | Tiros lejanos | Técnico 10 | — |
| attr.penalty_taking | Penalty Taking | Penaltis | Balón parado 1 | Técnico 1 |
| attr.corners | Corners | Saques de esquina | Balón parado 2 | — |
| attr.long_throws | Long Throws | Saques largos | Balón parado 3 | — |
| attr.free_kick_taking | Free Kick Taking | Tiros libres | Balón parado 4 | Técnico 3 |
| attr.aggression | Aggression | Agresividad | Mental 1 | Mental 1 |
| attr.anticipation | Anticipation | Anticipación | Mental 2 | Mental 2 |
| attr.positioning | Positioning | Colocación | Mental 3 | Mental 3 |
| attr.concentration | Concentration | Concentración | Mental 4 | Mental 4 |
| attr.decisions | Decisions | Decisiones | Mental 5 | Mental 5 |
| attr.off_the_ball | Off The Ball | Desmarques | Mental 6 | Mental 6 |
| attr.determination | Determination | Determinación | Mental 7 | Mental 7 |
| attr.teamwork | Team Work | Juego en equipo | Mental 8 | Mental 8 |
| attr.leadership | Leadership | Liderazgo | Mental 9 | Mental 9 |
| attr.work_rate | Work Rate | Sacrificio | Mental 10 | Mental 10 |
| attr.composure | Composure | Serenidad | Mental 11 | Mental 11 |
| attr.flair | Flair | Talento | Mental 12 | Mental 12 |
| attr.bravery | Bravery | Valentía | Mental 13 | Mental 13 |
| attr.vision | Vision | Visión | Mental 14 | Mental 14 |
| attr.acceleration | Acceleration | Aceleración | Físico 1 | Físico 1 |
| attr.agility | Agility | Agilidad | Físico 2 | Físico 2 |
| attr.jumping_reach | Jumping Reach | Alcance de salto | Físico 3 | Físico 3 |
| attr.balance | Balance | Equilibrio | Físico 4 | Físico 4 |
| attr.strength | Strength | Fuerza | Físico 5 | Físico 5 |
| attr.natural_fitness | Natural Fitness | Recuperación física | Físico 6 | Físico 6 |
| attr.stamina | Stamina | Resistencia | Físico 7 | Físico 7 |
| attr.pace | Pace | Velocidad | Físico 8 | Físico 8 |
| attr.aerial_reach | Aerial Reach | Alcance aéreo | — | Portería 1 |
| attr.handling | Handling | Blocaje | — | Portería 2 |
| attr.communication | Communication | Comunicación | — | Portería 3 |
| attr.eccentricity | Eccentricity | Excentricidad | — | Portería 5 |
| attr.punching | Punching | Golpeo de puños (tendencia) | — | Portería 6 |
| attr.command_of_area | Command Of Area | Mando en el área | — | Portería 7 |
| attr.reflexes | Reflexes | Reflejos | — | Portería 9 |
| attr.rushing_out_tendency | Rushing Out (Tendency) | Salidas (tendencia) | — | Portería 10 |
| attr.throwing | Throwing | Saques de mano | — | Portería 11 |
| attr.kicking | Kicking | Saques de puerta | — | Portería 12 |
| attr.one_vs_one | One On Ones | Uno contra uno | — | Portería 13 |

## Roles de jugador

**ES-27-02 · Contratación → Base de datos de jugadores → Editar búsqueda → Criterios de búsqueda.** Con «Posición» en «Cualquiera», los dos selectores dan 79 entradas de rol, 40 en «Rol (con posesión)» y 39 en «Rol (sin posesión)». El ordinal empieza en el primer rol, después de la opción «Cualquiera». Se conserva el orden de cada lista general. En táctica, la disponibilidad depende del puesto, la fase y el dibujo. Los roles homónimos de fases distintas conservan su propio ID. [Compatibilidad del AS-IS](../as-is/16-tablas-de-roles-y-tacticas.md#fm27)

### Con posesión

| Orden | ID | EN de referencia | ES observado |
|---:|---|---|---|
| 1 | role.GK-IP | Goalkeeper | Portero |
| 2 | role.BGK | Ball-Playing Goalkeeper | Portero con juego de pies |
| 3 | role.NGK | No-Nonsense Goalkeeper | Portero sin complicaciones |
| 4 | role.FB-IP | Full-Back | Lateral |
| 5 | role.WB-IP | Wing-Back | Lateral ofensivo |
| 6 | role.IWB | Inside Wing-Back | Carrilero por dentro |
| 7 | role.IFB | Inside Full-Back | Lateral por dentro |
| 8 | role.PWB-IP | Playmaking Wing-Back | Lateral ofensivo creador |
| 9 | role.CB-IP | Centre-Back | Central |
| 10 | role.ACB | Advanced Centre-Back | Central avanzado |
| 11 | role.BCB | Ball-Playing Centre-Back | Central con toque |
| 12 | role.NCB | No-Nonsense Centre-Back | Central defensivo |
| 13 | role.WCB-IP | Wide Centre-Back | Central lateral |
| 14 | role.OCB | Overlapping Centre-Back | Central con proyección ofensiva |
| 15 | role.AWB | Advanced Wing-Back | Lateral ofensivo avanzado |
| 16 | role.DM-IP | Defensive Midfielder | Mediocentro |
| 17 | role.DLP | Deep-Lying Playmaker | Pivote organizador |
| 18 | role.BBM | Box-To-Box Midfielder | Centrocampista todoterreno |
| 19 | role.HB | Half Back | Medio cierre |
| 20 | role.BBP | Box-To-Box Playmaker | Organizador todoterreno |
| 21 | role.WM | Wide Midfielder | Centrocampista de banda |
| 22 | role.W-IP | Winger | Extremo |
| 23 | role.PW | Playmaking Winger | Extremo organizador |
| 24 | role.IW | Inside Winger | Extremo por dentro |
| 25 | role.CM-IP | Central Midfielder | Centrocampista central |
| 26 | role.AM-IP | Attacking Midfielder | Mediapunta |
| 27 | role.AP | Advanced Playmaker | Organizador adelantado |
| 28 | role.WCM-IP | Wide Central Midfielder | Mediocampista de banda |
| 29 | role.CHM | Channel Midfielder | Centrocampista interior |
| 30 | role.MPM | Midfield Playmaker | Centrocampista organizador |
| 31 | role.IF | Inside Forward | Delantero por dentro |
| 32 | role.WFD | Wide Forward | Delantero escorado |
| 33 | role.FR | Free Role | Rol libre |
| 34 | role.SS | Second Striker | Segundo delantero |
| 35 | role.DLF | Deep-Lying Forward | Delantero de apoyo |
| 36 | role.CFD-IP | Centre Forward | Delantero centro |
| 37 | role.TF | Target Forward | Delantero objetivo |
| 38 | role.P | Poacher | Ariete |
| 39 | role.CHF | Channel Forward | Delantero buscaespacios |
| 40 | role.F9 | False Nine | Falso nueve |

### Sin posesión

| Orden | ID | EN de referencia | ES observado |
|---:|---|---|---|
| 1 | role.GK-OOP | Goalkeeper | Portero |
| 2 | role.SK | Sweeper Keeper | Portero cierre |
| 3 | role.LHK | Line-Holding Goalkeeper | Portero de línea de gol |
| 4 | role.FB-OOP | Full-Back | Lateral |
| 5 | role.PFB | Pressing Full-Back | Lateral presionante |
| 6 | role.HFB | Holding Full-Back | Lateral de contención |
| 7 | role.CB-OOP | Centre-Back | Central |
| 8 | role.SCB | Stopping Centre-Back | Central contundente |
| 9 | role.CCB | Covering Centre-Back | Central de cobertura |
| 10 | role.WCB-OOP | Wide Centre-Back | Central lateral |
| 11 | role.SWD | Stopping Wide Centre-Back | Central lateral contundente |
| 12 | role.CWD | Covering Wide Centre-Back | Central lateral de cobertura |
| 13 | role.WB-OOP | Wing-Back | Lateral ofensivo |
| 14 | role.PWB-OOP | Pressing Wing-Back | Carrilero presionante |
| 15 | role.HWB | Holding Wing-Back | Carrilero de contención |
| 16 | role.DM-OOP | Defensive Midfielder | Mediocentro |
| 17 | role.DDM | Dropping Defensive Midfielder | Mediocentro retrasado |
| 18 | role.PDM | Pressing Defensive Midfielder | Mediocentro defensivo presionante |
| 19 | role.SDM | Screening Defensive Midfielder | Mediocentro de contención |
| 20 | role.WDM | Wide Covering Defensive Midfielder | Mediocentro de amplia cobertura |
| 21 | role.WMF | Wide Midfielder | Centrocampista de banda |
| 22 | role.TWM | Tracking Wide Midfielder | Centrocampista de banda marcador |
| 23 | role.OWM | Wide Outlet Wide Midfielder | Centrocampista de banda interior |
| 24 | role.CM-OOP | Central Midfielder | Centrocampista central |
| 25 | role.PCM | Pressing Central Midfielder | Mediocampista presionante |
| 26 | role.SCM | Screening Central Midfielder | Mediocampista de contención |
| 27 | role.WCM-OOP | Wide Covering Central Midfielder | Mediocampista de despliegue |
| 28 | role.W-OOP | Winger | Extremo |
| 29 | role.TW | Tracking Winger | Extremo marcador |
| 30 | role.IOW | Inside Outlet Winger | Extremo por dentro con salida |
| 31 | role.WOW | Wide Outlet Winger | Extremo de salida |
| 32 | role.AM-OOP | Attacking Midfielder | Mediapunta |
| 33 | role.TAM | Tracking Attacking Midfielder | Mediapunta itinerante |
| 34 | role.OAM | Central Outlet Attacking Midfielder | Mediapunta central iniciador |
| 35 | role.SAM | Splitting Outlet Attacking Midfielder | Mediapunta con salida |
| 36 | role.CFD-OOP | Centre Forward | Delantero centro |
| 37 | role.TCF | Tracking Centre Forward | Delantero centro marcador |
| 38 | role.OCF | Central Outlet Centre Forward | Delantero centro de enlace |
| 39 | role.SCF | Splitting Outlet Centre Forward | Delantero centro buscaespacios |

## Otras familias

Registrar cada posición y lado con la clave del catálogo de compatibilidad FM27 y cada instrucción con su fase y ámbito (equipo/persona). Mantener filas diferenciadas para el concepto de posesión y la abreviatura visible. Hasta reunir una vista verificada de instrucciones no se habilita su selector oficial. Los métodos de entrenamiento conservan su identificador de familia para distinguir homónimos.

### Posiciones, fases y métodos de referencia

| ID | EN de referencia | Categoría inicial | ES oficial | EN oficial | Abrev. | Orden oficial | Evidencia |
|---|---|---|---|---|---|---|---|
| pos.gk | GK | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.d_l | D (L) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.d_c | D (C) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.d_r | D (R) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.wb_l | WB (L) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.wb_r | WB (R) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.dm | DM | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.m_l | M (L) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.m_c | M (C) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.m_r | M (R) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.am_l | AM (L) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.am_c | AM (C) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.am_r | AM (R) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| pos.st_c | ST (C) | Posición | Pend. | Pend. | Pend. | Pend. | A16 |
| training.injury_rehab.quickness | Quickness | Método · Injury Rehab | Pend. | Pend. | Pend. | Pend. | A17 |
| training.injury_rehab.agility_and_balance | Agility and Balance | Método · Injury Rehab | Pend. | Pend. | Pend. | Pend. | A17 |
| training.injury_rehab.strength | Strength | Método · Injury Rehab | Pend. | Pend. | Pend. | Pend. | A17 |
| training.injury_rehab.endurance | Endurance | Método · Injury Rehab | Pend. | Pend. | Pend. | Pend. | A17 |
| training.injury_rehab.general_rehab | General Rehab | Método · Injury Rehab | Pend. | Pend. | Pend. | Pend. | A17 |
| training.set_pieces.free_kick_taking | Free Kick Taking | Método · Set Pieces | Pend. | Pend. | Pend. | Pend. | A17 |
| training.set_pieces.corner_taking | Corner Taking | Método · Set Pieces | Pend. | Pend. | Pend. | Pend. | A17 |
| training.set_pieces.penalty_taking | Penalty Taking | Método · Set Pieces | Pend. | Pend. | Pend. | Pend. | A17 |
| training.set_pieces.long_throws | Long Throws | Método · Set Pieces | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.quickness | Quickness | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.agility_and_balance | Agility and Balance | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.strength | Strength | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.endurance | Endurance | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.defensive_positioning | Defensive Positioning | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.attacking_movement | Attacking Movement | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.final_third | Final Third | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.shooting | Shooting | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.passing | Passing | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.crossing | Crossing | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.ball_control | Ball Control | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.aerial | Aerial | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.gk_reactions | GK Reactions | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.gk_tactical | GK Tactical | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.gk_technique | GK Technique | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.gk_sweeping | GK Sweeping | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.gk_distribution_long | GK Distribution (Long) | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| training.attributes.gk_distribution_short | GK Distribution (Short) | Método · Attributes | Pend. | Pend. | Pend. | Pend. | A17 |
| phase.ip | In possession | Fase | Con posesión | Pend. | CP (Rol CP) | Pend. | ES-27-03 |
| phase.oop | Out of possession | Fase | Sin posesión | Pend. | SP (Rol SP) | Pend. | ES-27-03 |

## Reproducción en FM27

1. Confirmar castellano, build Steam `26620374` y copia local de prueba. La observación se hizo en la copia del Valencia CF del 1 de agosto de 2026 a las 21:45 de la partida, utilizada en SK-01. [Copia y comprobación de la build](../../specs/001-extraccion-fm27/quickstart.md#atributos-visibles-y-familiaridad)
2. Para **ES-27-01**, abrir desde «Primer equipo» la «Vista general» de un jugador de campo y de un portero. Se cotejaron las fichas de José Gayà y Stole Dimitrievski. Leer cada grupo de arriba abajo y comprobar el cambio de sección de «Control», «Pase», «Penaltis» y «Tiros libres» entre fichas. El registro conserva únicamente nombres y posiciones en pantalla.
3. Para **ES-27-02**, abrir «Editar búsqueda» y desmarcar las posiciones seleccionadas en el mapa hasta que indique «Cualquiera». Recorrer ambos desplegables de arriba abajo e ignorar la opción «Cualquiera». Como control, filtrar temporalmente por «Defensa (Centro)» y comprobar que aparecen los seis roles de central de cada fase, incluidos los tres roles con «Central lateral» sin posesión. Cancelar el borrador. La búsqueda original regresó con sus tres criterios activos y 461 resultados.
4. Para **ES-27-03**, abrir «Tácticas → Vista general → Dibujo táctico». Las pestañas muestran «Con posesión» y «Sin posesión» y la tabla combinada muestra «Rol CP» y «Rol SP». Se cotejó también «Portero con juego de pies» en el desplegable de portería con posesión, sin seleccionar otro rol.

En terminal, `rg -c '^\| attr\.' docs/to-be/catalogo-terminos-fm27.md` devolvió **47** y `rg -c '^\| [0-9]+ \| role\.' docs/to-be/catalogo-terminos-fm27.md` devolvió **79**. La comprobación documental cotejó todas las claves con el catálogo inicial, los ordinales consecutivos de cada grupo y fase y la separación por ID de los roles homónimos. Este bloque documental se verificó con el juego y el catálogo. La prueba de presentación en WinUI queda para SK-06 y SK-07.

Esta observación cubre el castellano de la build indicada, las fichas de campo y portero y los selectores generales de roles. Quedan pendientes el cotejo independiente, las etiquetas inglesas, las abreviaturas individuales y las familias de puesto, rasgos e instrucciones. [Q-05](../OPEN-QUESTIONS.md) · [NV-24](../testing/02-casos-de-prueba.md)
