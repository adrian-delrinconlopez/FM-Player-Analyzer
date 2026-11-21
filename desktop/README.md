# FM Player Analyzer — Desktop (C++ / Qt 6)

Desktop companion for **Football Manager 2027**, optimized for large scouting
databases. It reads FM27 saves directly and imports CSV and HTML files from
FM View Exporter. It includes DWRS ratings, squad analysis, possession-phase
line-ups, player development, scouting, registrations, transfers and tactics.

Earlier FM24 databases can still be used with an earlier Analyzer release.
The data folder retains its original name so existing FM27 profiles are found.

The interface defaults to English. German and Spanish are available from the
**Language** menu or the language button on the start screen. Changes take effect
after restarting.

The Spanish interface catalog is `translations/fmplayeranalyzer_es.ts`. To edit
the Spanish role names or other game terminology, open
`translations/football_terms_es.ts` in Qt Linguist or a text editor and update
the relevant `<translation>` values. Rebuild the app to embed the updated catalogs.

## Build requirements

- Visual Studio 2022 Build Tools (C++ workload, x64)
- CMake ≥ 3.28, Ninja
- Qt 6.8 LTS (`msvc2022_64`) with the **Qt Charts** add-on, expected at
  `C:/Qt/6.8.3/msvc2022_64` (adjust `CMakePresets.json` if elsewhere)
- Inno Setup 6 (only for building the installer)

## Building

```powershell
.\scripts\build.ps1 -Preset msvc-release -Test
```

or manually from a *x64 Native Tools* command prompt:

```
cmake --preset msvc-release
cmake --build --preset msvc-release
ctest --preset msvc-release
```

The app binary lands at `build/msvc-release/src/app/fmplayeranalyzer.exe`
(needs `C:\Qt\6.8.3\msvc2022_64\bin` on `PATH` to run outside the installer,
or run `windeployqt` on it).

## Packaging / Installer

```powershell
.\scripts\package.ps1
```

builds the release, stages it with `windeployqt` into `installer/staging/`
and — if Inno Setup 6 is installed — compiles
`installer/output/FMPlayerAnalyzer-<version>-Windows-x64-Setup.exe`. Without Inno Setup
the staging folder works as a portable install. The app version comes from
`src/core/Version.cpp`.

## Layout

- `src/core/` — `fmcore` static library: all domain logic (DWRS engine,
  squad builder, importer, database). Links Qt Core/Sql/Concurrent only —
  no Widgets — so it stays headless-testable.
- `src/app/` — `fmplayeranalyzer` executable: Qt Widgets UI.
- `src/tests/` — Qt Test unit tests and golden-master tests (`ctest`).
- `installer/` — Inno Setup script (built by `scripts/package.ps1`).
