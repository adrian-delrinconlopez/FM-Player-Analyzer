#pragma once

#include <QHash>
#include <QList>
#include <QPair>
#include <QString>
#include <QStringList>

// Split-out parts of the version profile; included here so every user of
// Constants.h keeps seeing them.
#include "AttributeTiers.h"
#include "ImportSchema.h"
#include "PitchGeometry.h"

namespace fm {

// --- Static domain constants ported 1:1 from legacy/src/constants.py ---
// Dynamic definition accessors (roles, tactics, ...) live in Definitions.

// A supported Football Manager release, identified by a stable id stored in
// the per-database settings ("fm_version"). Each release has its own profile:
// export layout, roles/tactics (definitions file) and attribute importance.
struct FmDataVersion {
    QString id;                  // stable settings key, e.g. "fm27"
    QString displayName;         // e.g. "Football Manager 2027"
    bool supported;              // false = shown in the picker but not yet importable
    QString definitionsFileName; // roles/tactics in the data dir, e.g. "definitions_fm27.json"
    QString exportFolderName;    // game folder under "Documents/Sports Interactive"
    QStringList importFormats;   // export formats the importer reads, e.g. {"html", "csv"}
    bool provisionalWeights = false; // true = attribute tiers/weights are assumed, not yet measured
    bool registrationRules = false;  // true = league/UEFA registration rules are modelled
    QStringList unratedAttributes;   // attributes this release neither imports nor rates
};

// All FM releases the app knows (newest first). To add a release: an entry
// here plus its branches in ImportSchema.cpp (attributeMapping(),
// ignoredExportColumns(), requiredImportColumns()) and AttributeTiers.cpp
// (globalStatCategories(), gkStatCategories(), weightDefaults(),
// gkWeightDefaults()), and a bundled ":/defaults/<definitionsFileName>".
const QList<FmDataVersion> &fmDataVersions();
// Default for new profiles. Opening an existing database requires an explicit version.
QString defaultFmVersionId();
// Newest supported version: preselected for newly created databases.
QString newestFmVersionId();
// Registry entry for an id; unknown/empty ids fall back to the default version.
const FmDataVersion &fmDataVersion(const QString &versionId);
// Whether the attribute is available for rating.
bool attributeRated(const QString &versionId, int attrIndex);

// Version of the DWRS formula. Bump it whenever a change makes the ratings
// stored in a database outdated; each database records the version its
// ratings were computed with ("dwrs_scheme"), and the app offers a full
// recalculation when it is older. 2 = goalkeepers rate all 13 goalkeeping
// attributes, role lists count as Key/Preferable (v1.4.13).
inline constexpr int kDwrsSchemeVersion = 2;

// Default role multipliers.
inline constexpr double kKeyMultiplierDefault = 1.5;
inline constexpr double kPreferableMultiplierDefault = 1.2;

// Personality name -> "good" | "neutral" | "bad" (defaults; definitions.json
// may override).
const QHash<QString, QString> &personalityDefaults();
QString personalityLabel(const QString &personality);
QString playingTimeLabel(const QString &playingTime);

// Agreed Playing Time options.
const QStringList &fieldPlayerAptOptions();
const QStringList &gkAptOptions();
const QHash<QString, QString> &aptAbbreviations();

} // namespace fm
