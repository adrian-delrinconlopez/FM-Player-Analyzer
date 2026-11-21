# Import aus Football Manager 2027

FM27 hat keinen eingebauten Spielerexport mehr. Die Daten kommen vom
[FM View Exporter](https://github.com/ElectroHugin/FM-View-Exporter), einem
BepInEx-Plugin, das die gerade angezeigte Spielerliste per **F9** als CSV und HTML
speichert. Beide Programme sind aufeinander abgestimmt; diese Datei beschreibt den
Vertrag zwischen ihnen.

## Ablauf

1. FM View Exporter installieren (siehe dessen README).
2. Im Spiel die Ansicht **Analyzer** anlegen
   ([Anleitung und Spaltenliste](https://github.com/ElectroHugin/FM-View-Exporter/blob/main/docs/ANSICHT-ANALYZER-FM27.md)).
   Andere Ansichten gehen auch, solange `Unique ID` und `Player` dabei sind.
3. Kader, Spielersuche oder Scouting öffnen, **F9** drücken. Die Datei landet in
   `Dokumente\Sports Interactive\FM View Exporter`.
4. Im Analyzer einen neuen Spielstand anlegen.
5. Dashboard → *Neue Spielerdaten importieren* → CSV- oder HTML-Datei wählen. Der
   Dateidialog öffnet direkt den Ordner des Exporters.

## Spielstand direkt einlesen (ab 2.1.0)

Statt eines Exports lässt sich auch der Spielstand selbst wählen
(`Dokumente\Sports Interactive\Football Manager 27\games\*.fm`). Der Analyzer liest
die Datei nur, er schreibt nie hinein. Am besten eine gerade gespeicherte Datei nehmen,
nicht während das Spiel speichert.

Was übernommen wird, regelt die Einstellung **Attribut-Maskierung wie im Spiel**
(Einstellungen → Import, je Spielstand, Standard an):

| Spieler | Maskierung an | Maskierung aus |
| --- | --- | --- |
| eigener Kader (auch zweite Mannschaft) | exakte Werte | exakte Werte |
| Weltreputation ab 8.000 | exakte Werte | exakte Werte |
| eigenes Scouting, Kenntnisgrad ab 78 % | exakte Werte | exakte Werte |
| eigenes Scouting, 56 bis 77 % | teils Spannen (vom Analyzer gebildet) | exakte Werte |
| Vorkenntnis, unter 56 % | alle Attribute als Spannen (2 bis 4 Punkte) | exakte Werte |
| nicht gescoutet, mit Verein | alle Attribute als Spannen (4 bis 7 Punkte) | exakte Werte |
| nicht gescoutet, ohne Verein | nicht übernommen | nicht übernommen |

- Der Kenntnisgrad steht im Spielstand (Abschnitt `fog_of_war`). Die Spannen des Spiels
  stehen nicht darin; der Analyzer würfelt eigene, einmal fest je Spieler und Attribut.
  Der wahre Wert liegt irgendwo darin, nicht nach Regel in der Mitte. Mit mehr
  Kenntnis werden es weniger und schmalere Spannen. Lieber mehr Spieler als zu wenig:
  auch mit Maskierung fehlt niemand mit Verein, nur die Genauigkeit sinkt.
- Verdeckte Werte (Potenzial, Fähigkeit, Konstanz, Verletzungsanfälligkeit, wichtige
  Spiele, Härte, Vielseitigkeit) liest der Analyzer nie in die Datenbank.
- Persönlichkeit: eindeutige Namen werden aus den gelesenen Persönlichkeitswerten abgeleitet.
  Mit Maskierung gilt das für den eigenen Verein und vollständig gescoutete Spieler
  (Kenntnis 100 %); ohne Maskierung für alle übernommenen Spieler. Unbekannte oder
  mehrdeutige Namen bleiben leer und überschreiben keine frühere Exportangabe.
  Die Rohwerte werden nicht in der Datenbank gespeichert.
- Medienverhalten wird noch nicht aus dem Spielstand übernommen. Frühere Exportangaben
  bleiben erhalten. Nationen sind für die 152 Länder hinterlegt, die in Exporten vorkamen.
- Kooperationsvereine und Gegner, die das Spiel ohne Scoutbericht zeigt, fehlen noch.
- Nach einem FM-Patch kann sich das Dateiformat ändern. Dann bricht der Import mit einer
  Meldung ab, statt falsche Werte zu übernehmen.
- Das Format folgt [fmsave](https://github.com/rhiever/fmsave) (MIT), siehe
  `THIRD_PARTY_NOTICES.md`. Werkzeug zum Abgleich: `fm27savedump`.

## Spaltenzuordnung

Der Exporter schreibt die
Spaltenköpfe, die FM27 zeigt (englische Spieloberfläche). Die Reihenfolge der Spalten
ist egal, der Analyzer sucht sie über den Namen. Quelle im Code:
`desktop/src/core/ImportSchema.cpp` (`attributeMapping()`).

### Pflicht

| Spalte in FM27 | Feld im Analyzer |
| --- | --- |
| `Unique ID` | Unique ID |
| `Player` (Spielerdatenbank: `Name`) | Name |

### Stammdaten

| Spalte in FM27 | Feld im Analyzer | Hinweis |
| --- | --- | --- |
| `Position` | Position | |
| `Age` | Age | |
| `Club` | Club | |
| `Nation` | Nationality | Kürzel (`GER`) oder voller Name (`Germany`), gespeichert wird das Kürzel; bei zwei `Nation`-Spalten zählt die erste |
| `2nd Nat` | Second Nationality | voller Name, gespeichert wird das Kürzel |
| `Height` | Height | `193 cm` |
| `Preferred Foot` | Preferred Foot | |
| `Left Foot` | Left Foot | |
| `Right Foot` | Right Foot | |
| `Personality` | Personality | |
| `Media Handling` | Media Handling | |
| `Playing Time` | Agreed Playing Time | |
| `Wage` | Wage | |
| `Transfer Value` | Transfer Value | Spanne → untere Grenze |
| `Rating` | Average Rating | `-` = keine Spiele |
| `Inf` | Information | wird gelesen, aber nicht ausgewertet |

### Attribute (40)

| Spalte in FM27 | Feld im Analyzer |
| --- | --- |
| `Aerial Reach` | Aerial Reach |
| `Command Of Area` | Command of Area |
| `Handling` | Handling |
| `Kicking` | Kicking |
| `One On Ones` | One vs One |
| `Reflexes` | Reflexes |
| `Rushing Out (Tendency)` | Rushing Out (Tendency) |
| `Throwing` | Throwing |
| `Aggression` | Aggression |
| `Anticipation` | Anticipation |
| `Bravery` | Bravery |
| `Composure` | Composure |
| `Concentration` | Concentration |
| `Decisions` | Decisions |
| `Determination` | Determination |
| `Flair` | Flair |
| `Leadership` | Leadership |
| `Off The Ball` | Off the Ball |
| `Positioning` | Positioning |
| `Team Work` | Teamwork |
| `Vision` | Vision |
| `Work Rate` | Work Rate |
| `Acceleration` | Acceleration |
| `Agility` | Agility |
| `Balance` | Balance |
| `Jumping Reach` | Jumping Reach |
| `Pace` | Pace |
| `Stamina` | Stamina |
| `Strength` | Strength |
| `Corners` | Corners |
| `Crossing` | Crossing |
| `Dribbling` | Dribbling |
| `Finishing` | Finishing |
| `First Touch` | First Touch |
| `Heading` | Heading |
| `Long Shots` | Long Shots |
| `Marking` | Marking |
| `Passing` | Passing |
| `Tackling` | Tackling |
| `Technique` | Technique |

Gescoutete Attribute kommen als Spanne (`12-15`) und werden als
Unter- und Obergrenze gespeichert.

Beide CSV-Stile des Exporters gehen: *English* (Komma, `7.05`) und *German*
(Semikolon, `7,05`). Der Analyzer erkennt das Trennzeichen selbst und liest die
Note mit Punkt oder Komma.

### Nicht gelesen

Der Analyzer überspringt bekannte Spalten, die er nicht auswertet, etwa die Scout-Empfehlung. Als *unbekannte Spalte* meldet der Import nur Köpfe, die
in keiner der beiden Listen stehen, also z. B. Spalten, die ein Patch umbenannt hat.

## Hinweise zum Import

- **Fehlende Spalten überschreiben nichts:** Fehlt einer Datei eine Spalte, behält
  der Analyzer den bisher gespeicherten Wert. Beispiel: Die Ansicht für
  *Recruitment > Player Database* bietet keine `Personality`; die Persönlichkeit
  aus einem Kader-Import bleibt erhalten, neue Spieler aus der Datenbank haben
  zunächst keine.

- **Keine Registrierung:** FM27 hat keine Spalte `Reg`. Der Registrierungs-Assistent
  verwendet die Angaben im Analyzer und die Regeln der ausgewählten Liga.
- **Rollen und Taktiken:** die 79 FM27-Rollen (40 mit Ball, 39 ohne Ball) mit Key- und
  Preferable-Attributen aus dem Spiel, erzeugt aus `desktop/data/fm27/` durch
  `desktop/scripts/make_definitions_fm27.py` (`definitions_fm27.json`). Eine FM27-Taktik hat zwei
  unabhängige Aufstellungen (mit Ball, ohne Ball) ohne feste Paare; die Best XI wählt die Spieler
  und für jeden einen Platz in beiden Aufstellungen per Min-Cost-Flow
  (`SquadBuilder::calculateTwoPhaseSquad`). Der Analyzer liest auch die
  Torwart-Attribute `Communication` und `Eccentricity`.
- **Gewichte:** DWRS Classic nutzt die bisherigen Attributkategorien und Gewichte
  (eigener Abschnitt `[Weights_fm27]` in `config.ini`).

## Beide Programme synchron halten

- Maßgeblich für die Spaltennamen ist das Spaltenverzeichnis des Exporters:
  [`docs/columns/fm27.csv`](https://github.com/ElectroHugin/FM-View-Exporter/blob/main/docs/columns/fm27.csv),
  Spalte `analyzer_field`. Jede Zeile mit Eintrag dort steht in
  `attributeMapping()`, alle übrigen in `fm27IgnoredExportColumns()`.
- Benennt ein FM27-Patch eine Spalte um, zeigt der Import sie als unbekannt an.
  Dann beide Stellen anpassen: `fm27.csv` im Exporter und `ImportSchema.cpp` hier.
- Der Test `test_csvimport` (`fm27ExporterCsv`, `fm27MappingComplete`) prüft eine
  Datei im Format des Exporters und dass alle 40 bewerteten Attribute erreichbar sind.

## Persönlichkeit: Grenzen und Einstufung (ab 2.2.3)

Die Rekonstruktion nutzt die veröffentlichten Schwellen und Prioritäten der
[FM-Referenzanalyse](https://github.com/Napas/fm-mcp/blob/main/internal/fm/personality.go)
für FM26.3.2. Eine vollständige unabhängige Bestätigung sämtlicher Namen in FM27
steht noch aus. Der eigene Parser kennt weder das Zusatzflag für negative
Persönlichkeitsnamen noch die Vereinsbindung für „Devoted“. Sobald eines davon
für die Auswahl nötig wäre, wird kein Name geraten. Auch ungültige Werte liefern
keinen Namen. CSV/HTML können weiterhin den direkt vom Spiel angezeigten Namen liefern.

Unter Einstellungen → Persönlichkeiten lässt sich die Einstufung im aktuellen FM27-Spielstand ändern.
Talent-Score, Filter und Analyse lesen dieselbe Einteilung.
