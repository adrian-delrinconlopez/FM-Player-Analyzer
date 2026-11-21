# Contrato de las tablas de entrada

Estas cabeceras se comparan por texto. La lista permite reproducir la diferencia entre una columna importada, una que el programa conoce y descarta y una desconocida que genera aviso. Los 47 atributos tienen su correspondencia específica en el [diccionario](17-parametros-y-atributos.md). Se exige UID en FM24 o Unique ID en FM27 además del nombre y el mínimo de diez columnas en la carga masiva.

| Destino conceptual | FM24 | FM27 |
|---|---|---|
| Age | Age | Age |
| Agreed Playing Time | Agreed Playing Time | Playing Time |
| Average Rating | Av Rat | Rating |
| Club | Club | Club |
| Height | Height | Height |
| Information | Inf | Inf |
| Left Foot | Left Foot | Left Foot |
| Media Handling | Media Handling | Media Handling |
| Name | Name | Player |
| Nationality | Nat | Nation |
| Personality | Personality | Personality |
| Position | Position | Position |
| Preferred Foot | Preferred Foot | Preferred Foot |
| Registration | Reg | — |
| Right Foot | Right Foot | Right Foot |
| Second Nationality | 2nd Nat | 2nd Nat |
| Transfer Value | Transfer Value | Transfer Value |
| Unique ID | UID | Unique ID |
| Wage | Wage | Wage |

Las marcas Registration e Information reconocidas no actualizan la inscripción manual. En FM27, Name y Rec. se convierten en Player y Recommendation respectivamente sólo si falta el destino canónico. La primera repetición de cabecera ya normalizada prevalece. [Lectura](11-importacion-y-visibilidad.md) · [Correspondencias versionadas](../../old_version/desktop/src/core/ImportSchema.cpp).

## FM24 · 3 cabeceras que se descartan sin aviso

- «Ability», «CON», «Position/Role/Duty».

## FM27 · 307 cabeceras que se descartan sin aviso

- «AT Apps», «AT Gls», «AT League Apps», «AT League Goals», «Ability», «Active Non Promotion Release Clause», «Active Relegation Release Clause», «Actual Playing Time».
- «Additional Focus», «Aer A/90», «Agent», «Appearance Fee», «Apps», «Asking Price», «Assist Bonus», «Assists».
- «Asts/90», «Attacking - Tactical», «Average Rating Club», «Average Rating International», «Based In», «Best Pos», «Best Role», «Birth Region».
- «Blk», «Blk/90», «Build Up», «CCC», «CON», «Ch C/90», «Chance Creation», «Clean Sheet Bonus».
- «Clean Sheets», «Clearances», «Cln/90», «Clr/90», «Club Form», «Con/90», «Cons», «Contract Start Date».
- «Contract Status», «Contract Type», «Conv %», «Cr A», «Cr C», «Cr C/90», «Cr C/A», «Crs A/90».
- «Current Year +1 Future Playing Time», «Current Year +2 Future Playing Time», «Current Year +3 Future Playing Time», «Current Year +4 Future Playing Time», «Defending - Technical», «Dist/90», «Distance», «Division».
- «DoB», «Drb», «Drb/90», «Due Date», «EU National», «Effectiveness», «Eligible», «Expires».
- «Expiry Date Of Offered Contract», «Extension After Promotion», «FFP Contribution», «Fatigue», «Favoured Clubs», «Favoured Personnel», «Fee», «Fitness».
- «Form», «Form International», «Form with Value», «Fouls», «Fouls Against», «Free Kick Shots», «Future Playing Time», «Game Win Ratio».
- «Games Drawn», «Games Lost», «Games Missed In A Row», «Games Won», «Goal Bonus», «Goalkeeping», «Goals», «Goals Conceded».
- «Goals From Outside The Box», «Goals per 90 minutes», «Happiness», «Hdr %», «Hdrs», «Hdrs A», «Hdrs L/90», «Hdrs W/90».
- «Hierarchy», «Home-Grown Status», «In Possession Shape», «Injured On», «Injury», «Injury Release Clause», «Injury Risk», «Injury Susceptibility».
- «Injury preventing training», «Int Apps», «Int Cap Bonus», «Int Gls», «Int/90», «International Appearances (Season)», «International Assists», «International Goals Conceded».
- «International Happiness», «Itc», «K Hdrs/90», «K Tck», «K Tck/90», «KP/90», «Key», «Last 5 Games Club».
- «Last 5 Games International», «Last Club», «Last Match Rating», «Last Transfer Fee», «Line of Engagement», «Loan Duration», «Loan Status», «Long-Term Plans».
- «MLG», «Manager Tendencies», «Managerial Support», «Match Highest Earner Clause», «Match Load», «Media Description», «Minimum Fee Release Clause», «Minimum Fee Release Clause (Clubs in a Continental Competition)».
- «Minimum Fee Release Clause (Clubs in a Continental Competition) - Expiry Date», «Minimum Fee Release Clause (Clubs in a Major Continental Competition)», «Minimum Fee Release Clause (Clubs in a Major Continental Competition) - Expiry Date», «Minimum Fee Release Clause (Domestic Clubs in Higher Division)», «Minimum Fee Release Clause (Domestic Clubs in Higher Division) - Expiry Date», «Minimum Fee Release Clause (Domestic Clubs)», «Minimum Fee Release Clause (Domestic Clubs) - Expiry Date», «Minimum Fee Release Clause (Foreign Clubs)».
- «Minimum Fee Release Clause (Foreign Clubs) - Expiry Date», «Minimum Fee Release Clause - Expiry Date», «Mins/Gl», «Mins/Gm», «Minutes», «Minutes Since Last Conceded», «Minutes Since Last Goal», «Morale».
- «NP-xG», «NP-xG/90», «Nation of Birth», «National Team», «Next Match Info», «No.», «Non Promotion Release Clause», «Non-Playing Job Offer Release Clause».
- «OP-Cr %», «OP-Crs A», «OP-Crs A/90», «OP-Crs C», «OP-Crs C/90», «OP-KP/90», «Off», «On Loan From».
- «One-Year Extension After League Games (Avoid Relegation Final Season)», «One-Year Extension After League Games (Final Season)», «One-Year Extension After League Games (Promoted Final Season)», «Opposition Player Knowledge», «Optional Contract Extension By Club», «Out Of Possession Shape», «Overall Fee», «Overall Happiness».
- «Overall Risk», «Overall Training Improvement», «Pas %», «Pas A», «Passing Style», «Pen/R», «Pens», «Pens Faced».
- «Pens S», «Pens Saved», «Pens Saved Ratio», «Percentage of Wages paid by Sponsor», «Player Ability», «Player Buy-In», «Player Capability», «Player Role Suitability».
- «Player Tactical Ability», «Player Trait», «Player's Interest», «Playing Time Happiness», «PoM», «Position/Role», «Poss Lost/90», «Poss Won/90».
- «Possession - Tactical», «Possession - Technical», «Postion Suitability», «Potential», «Pr passes/90», «Pref.», «Pres A», «Pres A/90».
- «Pres C», «Pres C/90», «Pressing Intensity», «Pressing Trap», «Progress», «Prom.», «Promises», «Promotion Wage Rise».
- «Pros», «Ps A/90», «Ps C», «Ps C/90», «PsP», «Pts/Gm», «Quality of Next Opposition Report», «Recent Changes (Last 2 Weeks)».
- «Recommendation», «Recurring Injury», «Red», «Relegation Release Clause», «Relegation Wage Drop», «Role Ability», «Role Ability Out Of Possession», «SHP».
- «Saves/90», «Seasonal Landmark Assist Bonus», «Seasonal Landmark Assist Bonus - Number Of Assists», «Seasonal Landmark Combined Goal and Assist Bonus», «Seasonal Landmark Combined Goal and Assist Bonus - Number Of Combined Goals and Assists», «Seasonal Landmark Goal Bonus», «Seasonal Landmark Goal Bonus - Number Of Goals», «Sec. Position».
- «Sell On Fee Percentage», «Set For Release», «Set Pieces», «ShT», «ShT/90», «Short-term Plans», «Shot %», «Shot/90».
- «Shots», «Shots From Outside The Box Per 90 minutes», «Shts Blckd», «Shts Blckd/90», «Social Groups», «Sprints/90», «Starts», «Status Of Offered Contract».
- «Style», «Sv %», «Svh», «Svp», «Svt», «Tactical Complexity», «Tactical Comprehension», «Tactical Understanding».
- «Tck A», «Tck C», «Tck R», «Tck/90», «Tcon», «Tcon/90», «Team Goals», «Teammates and Staff Relationships».
- «Tgls/90», «Time Missed», «Top Division Promotion Wage Rise», «Top Division Relegation Wage Drop», «Training Happiness», «Training Happiness Details», «Training Load», «Training Rating».
- «Transfer Fees Received», «Transfer Options», «Transfer Status», «Type», «Unused Substitute Fee», «WP Needed», «Wage After Reaching Club League Games», «Wage After Tax».
- «Wage Contribution», «Wage Of Offered Contract», «Waive Club Compensation For Managerial Role», «Work Permit Required», «Workload», «World Reputation», «Yearly Wage Rise», «Yel».
- «Yth Apps», «Yth Gls», «xA», «xA/90», «xG», «xG-OP», «xG/90», «xG/shot».
- «xGP», «xGP/90», «xSv %».

## Nacionalidad en exportaciones tabulares

Recortar los extremos y comparar el nombre completo con distinción de mayúsculas. Si hay coincidencia, devolver el código de la fila. En cualquier otro caso conservar literalmente el texto recortado, incluidos los códigos ya presentes. Por ejemplo, Spain → ESP y spain → spain. Esta conversión recibe texto, mientras que la [partida FM27](18-parametros-de-visibilidad.md) aporta un identificador numérico.

| Código | Entradas literales |
|---|---|
| AFG | Afghanistan |
| AIA | Anguilla |
| ALB | Albania |
| ALG | Algeria |
| AND | Andorra |
| ANG | Angola |
| ARG | Argentina |
| ARM | Armenia |
| ARU | Aruba |
| ASA | American Samoa |
| ATG | Antigua and Barbuda, Antigua & Barbuda |
| AUS | Australia |
| AUT | Austria |
| AZE | Azerbaijan |
| BAH | Bahamas, The Bahamas |
| BAN | Bangladesh |
| BDI | Burundi |
| BEL | Belgium |
| BEN | Benin |
| BER | Bermuda |
| BFA | Burkina Faso |
| BHR | Bahrain |
| BHU | Bhutan |
| BIH | Bosnia and Herzegovina, Bosnia & Herzegovina, Bosnia-Herzegovina |
| BLR | Belarus |
| BLZ | Belize |
| BOL | Bolivia |
| BOT | Botswana |
| BRA | Brazil |
| BRB | Barbados |
| BRU | Brunei, Brunei Darussalam |
| BUL | Bulgaria |
| CAM | Cambodia |
| CAN | Canada |
| CAY | Cayman Islands |
| CGO | Congo |
| CHA | Chad |
| CHI | Chile |
| CHN | China, China PR |
| CIV | Ivory Coast, Côte d'Ivoire |
| CMR | Cameroon |
| COD | DR Congo, Congo DR |
| COK | Cook Islands |
| COL | Colombia |
| COM | Comoros |
| CPV | Cape Verde, Cape Verde Islands, Cabo Verde |
| CRC | Costa Rica |
| CRO | Croatia |
| CTA | Central African Republic |
| CUB | Cuba |
| CUW | Curaçao, Curacao |
| CYP | Cyprus |
| CZE | Czech Republic, Czechia |
| DEN | Denmark |
| DJI | Djibouti |
| DMA | Dominica |
| DOM | Dominican Republic |
| ECU | Ecuador |
| EGY | Egypt |
| ENG | England |
| EQG | Equatorial Guinea |
| ERI | Eritrea |
| ESP | Spain |
| EST | Estonia |
| ETH | Ethiopia |
| FIJ | Fiji |
| FIN | Finland |
| FRA | France |
| FRO | Faroe Islands |
| GAB | Gabon |
| GAM | Gambia, The Gambia |
| GEO | Georgia |
| GER | Germany |
| GHA | Ghana |
| GIB | Gibraltar |
| GLP | Guadeloupe |
| GNB | Guinea-Bissau |
| GRE | Greece |
| GRN | Grenada |
| GUA | Guatemala |
| GUI | Guinea |
| GUM | Guam |
| GUY | Guyana |
| HAI | Haiti |
| HKG | Hong Kong |
| HON | Honduras |
| HUN | Hungary |
| IDN | Indonesia |
| IND | India |
| IRL | Ireland, Republic of Ireland |
| IRN | Iran |
| IRQ | Iraq |
| ISL | Iceland |
| ISR | Israel |
| ITA | Italy |
| JAM | Jamaica |
| JOR | Jordan |
| JPN | Japan |
| KAZ | Kazakhstan |
| KEN | Kenya |
| KGZ | Kyrgyzstan, Kyrgyz Republic |
| KOR | South Korea, Korea Republic |
| KOS | Kosovo |
| KSA | Saudi Arabia |
| KUW | Kuwait |
| LAO | Laos |
| LBN | Lebanon |
| LBR | Liberia |
| LBY | Libya |
| LCA | St Lucia, St. Lucia |
| LES | Lesotho |
| LIE | Liechtenstein |
| LTU | Lithuania |
| LUX | Luxembourg |
| LVA | Latvia |
| MAC | Macau, Macao |
| MAD | Madagascar |
| MAR | Morocco |
| MAS | Malaysia |
| MDA | Moldova |
| MDV | Maldives |
| MEX | Mexico |
| MKD | North Macedonia, Macedonia, FYR Macedonia |
| MLI | Mali |
| MLT | Malta |
| MNE | Montenegro |
| MNG | Mongolia |
| MOZ | Mozambique |
| MRI | Mauritius |
| MSR | Montserrat |
| MTN | Mauritania |
| MTQ | Martinique |
| MWI | Malawi |
| MYA | Myanmar |
| NAM | Namibia |
| NCA | Nicaragua |
| NCL | New Caledonia |
| NED | Netherlands, Holland |
| NEP | Nepal |
| NGA | Nigeria |
| NIG | Niger |
| NIR | Northern Ireland |
| NOR | Norway |
| NZL | New Zealand |
| OMA | Oman |
| PAK | Pakistan |
| PAN | Panama |
| PAR | Paraguay |
| PER | Peru |
| PHI | Philippines |
| PLE | Palestine |
| PNG | Papua New Guinea |
| POL | Poland |
| POR | Portugal |
| PRK | North Korea, Korea DPR |
| PUR | Puerto Rico |
| QAT | Qatar |
| ROU | Romania |
| RSA | South Africa |
| RUS | Russia |
| RWA | Rwanda |
| SAM | Samoa |
| SCO | Scotland |
| SDN | Sudan |
| SEN | Senegal |
| SEY | Seychelles |
| SIN | Singapore |
| SKN | St Kitts and Nevis, St. Kitts and Nevis, St Kitts & Nevis |
| SLE | Sierra Leone |
| SLV | El Salvador |
| SMR | San Marino |
| SOL | Solomon Islands |
| SOM | Somalia |
| SRB | Serbia |
| SRI | Sri Lanka |
| SSD | South Sudan |
| STP | São Tomé and Príncipe, São Tomé e Príncipe, Sao Tome and Principe |
| SUI | Switzerland |
| SUR | Suriname |
| SVK | Slovakia |
| SVN | Slovenia |
| SWE | Sweden |
| SWZ | Eswatini, Swaziland |
| SYR | Syria |
| TAH | Tahiti |
| TAN | Tanzania |
| TCA | Turks and Caicos Islands, Turks & Caicos Islands |
| TGA | Tonga |
| THA | Thailand |
| TJK | Tajikistan |
| TKM | Turkmenistan |
| TLS | Timor-Leste, East Timor |
| TOG | Togo |
| TPE | Chinese Taipei, Taiwan |
| TRI | Trinidad and Tobago, Trinidad & Tobago |
| TUN | Tunisia |
| TUR | Türkiye, Turkey |
| UAE | United Arab Emirates |
| UGA | Uganda |
| UKR | Ukraine |
| URU | Uruguay |
| USA | U.S.A., USA, United States |
| UZB | Uzbekistan |
| VAN | Vanuatu |
| VEN | Venezuela |
| VGB | British Virgin Islands |
| VIE | Vietnam |
| VIN | St Vincent and the Grenadines, St. Vincent and the Grenadines, St Vincent & The Grenadines, St Vincent |
| VIR | US Virgin Islands, U.S. Virgin Islands |
| WAL | Wales |
| YEM | Yemen |
| ZAM | Zambia |
| ZIM | Zimbabwe |
