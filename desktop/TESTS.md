# Testnetz

Zwei Ebenen sichern die Rechenlogik ab. Die erste läuft automatisch, die zweite
braucht echte Spielstände und läuft deshalb von Hand vor jedem Merge auf `main`.

## 1 · Unit-Tests (automatisch in CI)

`ctest` führt alle Qt-Test-Suiten unter `src/tests/` aus. Die CI macht das bei
jedem Push auf Windows und Linux (`.github/workflows/ci.yml`, `linux.yml`), der
Release-Workflow noch einmal vor dem Release.

```
cmake --preset linux-release      # bzw. msvc-debug unter Windows
cmake --build --preset linux-release
ctest --preset linux-release
```

Abgedeckte Kernmodule (Auszug):

| Modul | Test |
|---|---|
| DWRS-Berechnung | `test_dwrs` |
| Best XI / B-Elf | `test_squadbuilder`, `test_roleratings` |
| Standardrollen-Zuweisung (additiv) | `test_roleassignment` |
| DWRS-Neuberechnung, 1-%-Schwelle, Cache-Patch | `test_ratingsupdater`, `test_roleratings` |
| Lückenanalyse (Verdrängung, falsche Seite, Abfall, leere Slots) | `test_gapanalysis` |
| Taktik-Explorer (Abdeckung, Tiefe, Median, Rangfolge) | `test_tacticexplorer` |
| Talentwert | `test_talentengine` |
| Import CSV/HTML | `test_csvimport`, `test_htmlimporter` |
| Spielfeld-Geometrie je Version | `test_pitchgeometry` |
| Spielstandprofile (Trennung, Migration von 2.0.4, atomares Speichern) | `test_profile` |
| Zwei Aufstellungen (Fluss, Brücken, Ausfall mit genau einem Ersatz, wer übernimmt, Startelf-Grenze) | `test_twophase` |
| Taktikansichten (Kombiniert-Karte zwischen beiden Plätzen, Übersichts-Marker, Klicks) | `test_pitchviews` |
| Scouting mit mehreren Anforderungen (UND, Bridge mit/ohne Ball) | `test_scoutsearch` |
| Profile über AppContext: Wechsel A → B → A, Neustart, Kopie, Logo | `test_profileswitch` |

Die FM27-Rollen und -Formationen werden in eigenen Tests geprüft.
Historische Definitionsdaten dienen einzelnen Algorithmentests als feste Testdaten.

## 2 · Optionale Tests mit einer FM27-Spielstanddatei

Für die Tests mit einem echten FM27-Spielstand die Umgebungsvariable
FM27_TEST_SAVE auf eine Kopie der Datei setzen. Dann test_fm27save und
test_ratingactivation ausführen. Ohne die Kopie werden diese Fälle übersprungen.
