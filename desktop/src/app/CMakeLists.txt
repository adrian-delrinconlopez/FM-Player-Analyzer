add_executable(fmplayeranalyzer
    main.cpp
    ScreenshotTour.cpp
    AboutDialog.h
    AboutDialog.cpp
    AppContext.h
    AppContext.cpp
    AppPaths.h
    AppPaths.cpp
    FirstRunDialog.h
    GraphicsLibrary.h
    GraphicsLibrary.cpp
    FirstRunDialog.cpp
    MainWindow.h
    MainWindow.cpp
    NewSaveWizard.h
    NewSaveWizard.cpp
    WelcomeWizard.h
    WelcomeWizard.cpp
    SaveLibrary.h
    SaveLibrary.cpp
    StartScreen.h
    StartScreen.cpp
    Navigation.h
    Navigation.cpp
    ImportRunner.h
    ImportRunner.cpp
    LiveSaveWatcher.h
    LiveSaveWatcher.cpp
    PlayerActions.h
    PlayerActions.cpp
    RecalcHelper.h
    RecalcHelper.cpp
    pages/AssignRolesPage.h
    pages/AssignRolesPage.cpp
    pages/BestXiPage.h
    pages/BestXiPage.cpp
    pages/BestXiDepth.cpp
    pages/DashboardPage.h
    pages/DashboardPage.cpp
    pages/DwrsProgressPage.h
    pages/DwrsProgressPage.cpp
    pages/EditPlayerPage.h
    pages/EditPlayerPage.cpp
    pages/NationalBestXiPage.h
    pages/NationalBestXiPage.cpp
    pages/NationalCallupPage.h
    pages/NationalCallupPage.cpp
    pages/RegistrationPage.h
    pages/RegistrationPage.cpp
    pages/TrainingPlanPage.h
    pages/TrainingPlanPage.cpp
    pages/NationalDashboardPage.h
    pages/NationalDashboardPage.cpp
    pages/NationalSquadMatrixPage.h
    pages/NationalSquadMatrixPage.cpp
    pages/NationalSquadSelectionPage.h
    pages/NationalSquadSelectionPage.cpp
    pages/NewTacticPage.h
    pages/NewTacticPage.cpp
    pages/PageBase.h
    pages/PageHelpers.h
    pages/PlaceholderPage.h
    pages/PlayerComparisonPage.h
    pages/PlayerComparisonPage.cpp
    pages/PlayerProfilePage.h
    pages/PlayerProfilePage.cpp
    pages/RoleAnalysisPage.h
    pages/RoleAnalysisPage.cpp
    pages/SigningAdvisorTab.h
    pages/SigningAdvisorTab.cpp
    pages/SettingsPage.h
    pages/SettingsPage.cpp
    pages/SquadMatrixPage.h
    pages/SquadMatrixPage.cpp
    pages/TacticExplorerPage.h
    pages/TacticExplorerPage.cpp
    pages/TransfersPage.h
    pages/TransfersPage.cpp
    theming/ThemeManager.h
    theming/ThemeManager.cpp
    theming/ThemePresets.h
    theming/ThemePresets.cpp
    widgets/CommandPalette.h
    widgets/CommandPalette.cpp
    widgets/DropZone.h
    widgets/ColumnChooser.h
    widgets/DwrsColorSchemeEditor.h
    widgets/DwrsColorSchemeEditor.cpp
    widgets/ColumnChooser.cpp
    widgets/FlowLayout.h
    widgets/FlowLayout.cpp
    widgets/FootStrengthWidget.h
    widgets/FootStrengthWidget.cpp
    widgets/RolePhaseSwitch.h
    widgets/TrainingProfileEditor.h
    widgets/TrainingProfileEditor.cpp
    widgets/RolePhaseSwitch.cpp
    widgets/ClubHeaderBar.h
    widgets/LineIcons.h
    widgets/LineIcons.cpp
    widgets/DropZone.cpp
    widgets/ClubHeaderBar.cpp
    widgets/BusyProgressDialog.h
    widgets/BusyProgressDialog.cpp
    widgets/CellStyleDelegate.h
    widgets/CellStyleDelegate.cpp
    widgets/NumericTableItem.h
    widgets/PlayerSearchModel.h
    widgets/Charts.h
    widgets/Charts.cpp
    widgets/PersonalityFilterWidget.h
    widgets/PersonalityFilterWidget.cpp
    widgets/PlayerTableModel.h
    widgets/PlayerTableModel.cpp
    widgets/ScoutRequirementsBar.h
    widgets/ScoutRequirementsBar.cpp
    widgets/StrengthGridWidget.h
    widgets/StrengthGridWidget.cpp
    widgets/TacticPitchWidget.h
    widgets/TacticPitchWidget.cpp
    resources/app.qrc
)

# Windows only: GUI subsystem (no console window) and the exe icon resource.
if(WIN32)
    # Icon + version info; the version comes from project() in the top-level
    # CMakeLists.txt.
    set(FMPA_ICON_PATH "${CMAKE_CURRENT_SOURCE_DIR}/resources/favicon.ico")
    configure_file(app.rc.in "${CMAKE_CURRENT_BINARY_DIR}/app.rc" @ONLY)
    target_sources(fmplayeranalyzer PRIVATE "${CMAKE_CURRENT_BINARY_DIR}/app.rc")
    set_target_properties(fmplayeranalyzer PROPERTIES WIN32_EXECUTABLE TRUE)
endif()

target_include_directories(fmplayeranalyzer PRIVATE ${CMAKE_CURRENT_SOURCE_DIR})

target_link_libraries(fmplayeranalyzer PRIVATE
    fmcore
    Qt6::Widgets
    Qt6::Charts
)

# --- Translations -----------------------------------------------------------
# German uses source text. Compile the other catalogs into application resources.
set(FM_QM_FILES)
foreach(catalog IN ITEMS fmplayeranalyzer_en fmplayeranalyzer_es football_terms_es)
    set(ts_file "${PROJECT_SOURCE_DIR}/translations/${catalog}.ts")
    set(qm_name "${catalog}.qm")
    set(qm_file "${CMAKE_CURRENT_BINARY_DIR}/${qm_name}")
    add_custom_command(
        OUTPUT "${qm_file}"
        COMMAND Qt6::lrelease -nounfinished "${ts_file}" -qm "${qm_file}"
        DEPENDS "${ts_file}"
        VERBATIM
        COMMENT "Compiling ${catalog} translations (lrelease)"
    )
    set_source_files_properties("${qm_file}" PROPERTIES QT_RESOURCE_ALIAS "${qm_name}")
    list(APPEND FM_QM_FILES "${qm_file}")
endforeach()
add_custom_target(fm_translation_catalogs DEPENDS ${FM_QM_FILES})
add_dependencies(fmplayeranalyzer fm_translation_catalogs)
set(qtbase_es "${QT6_INSTALL_PREFIX}/${QT6_INSTALL_TRANSLATIONS}/qtbase_es.qm")
if(EXISTS "${qtbase_es}")
    set_source_files_properties("${qtbase_es}" PROPERTIES QT_RESOURCE_ALIAS qtbase_es.qm)
    list(APPEND FM_QM_FILES "${qtbase_es}")
endif()
qt_add_resources(fmplayeranalyzer "translations"
    PREFIX "/i18n"
    FILES ${FM_QM_FILES}
)
target_sources(fmplayeranalyzer PRIVATE widgets/RatingCurveEditor.h widgets/RatingCurveEditor.cpp)
target_sources(fmplayeranalyzer PRIVATE widgets/RatingProfileEditor.h widgets/RatingProfileEditor.cpp)
target_sources(fmplayeranalyzer PRIVATE RatingProfileDefaults.h RatingProfileDefaults.cpp)
