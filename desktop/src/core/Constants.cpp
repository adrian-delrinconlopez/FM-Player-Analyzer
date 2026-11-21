#include "Constants.h"

#include "Attributes.h"

#include <QCoreApplication>

namespace fm {

const std::array<QString, kAttrCount> &attrNames()
{
    static const std::array<QString, kAttrCount> names = {
        QStringLiteral("One vs One"),
        QStringLiteral("Acceleration"),
        QStringLiteral("Aerial Reach"),
        QStringLiteral("Aggression"),
        QStringLiteral("Agility"),
        QStringLiteral("Anticipation"),
        QStringLiteral("Balance"),
        QStringLiteral("Bravery"),
        QStringLiteral("Command of Area"),
        QStringLiteral("Concentration"),
        QStringLiteral("Composure"),
        QStringLiteral("Corners"),
        QStringLiteral("Crossing"),
        QStringLiteral("Decisions"),
        QStringLiteral("Determination"),
        QStringLiteral("Dribbling"),
        QStringLiteral("Finishing"),
        QStringLiteral("First Touch"),
        QStringLiteral("Flair"),
        QStringLiteral("Handling"),
        QStringLiteral("Heading"),
        QStringLiteral("Jumping Reach"),
        QStringLiteral("Kicking"),
        QStringLiteral("Leadership"),
        QStringLiteral("Long Shots"),
        QStringLiteral("Marking"),
        QStringLiteral("Off the Ball"),
        QStringLiteral("Pace"),
        QStringLiteral("Passing"),
        QStringLiteral("Positioning"),
        QStringLiteral("Reflexes"),
        QStringLiteral("Rushing Out (Tendency)"),
        QStringLiteral("Stamina"),
        QStringLiteral("Strength"),
        QStringLiteral("Tackling"),
        QStringLiteral("Teamwork"),
        QStringLiteral("Technique"),
        QStringLiteral("Throwing"),
        QStringLiteral("Vision"),
        QStringLiteral("Work Rate"),
        QStringLiteral("Communication"),
        QStringLiteral("Eccentricity"),
        QStringLiteral("Natural Fitness"),
        QStringLiteral("Free Kick Taking"),
        QStringLiteral("Long Throws"),
        QStringLiteral("Penalty Taking"),
        QStringLiteral("Punching"),
    };
    return names;
}

QString attributeLabel(int index)
{
    static const std::array<const char *, kAttrCount> labels = {
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Eins gegen Eins"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Antritt"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Reichweite"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Aggressivität"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Beweglichkeit"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Antizipation"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Balance"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Mut"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Strafraumbeherrschung"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Konzentration"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Nervenstärke"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Ecken"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Flanken"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Entscheidungen"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Zielstrebigkeit"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Dribbling"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Abschluss"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Ballannahme"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Flair"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Fangsicherheit"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Kopfballtechnik"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Sprunghöhe"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Abschläge"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Führungsqualitäten"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Weitschüsse"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Deckung"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Ohne Ball"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Schnelligkeit"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Passen"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Stellungsspiel"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Reflexe"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Herauslaufen (Tendenz)"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Ausdauer"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Kraft"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Tackling"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Teamwork"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Technik"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Abwürfe"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Übersicht"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Einsatzfreude"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Kommunikation"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Exzentrik"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Grundfitness"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Freistöße"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Weite Einwürfe"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Elfmeter"),
        QT_TRANSLATE_NOOP("fm::RatingProfileEditor", "Fausten")};
    return index >= 0 && index < kAttrCount
               ? QCoreApplication::translate("fm::RatingProfileEditor", labels[index])
               : QString();
}
QString attributeLabel(const QString &key)
{
    const int index = attrIndexByName(key);
    return index >= 0 ? attributeLabel(index) : key;
}

int attrIndexByName(const QString &fullName)
{
    static const QHash<QString, int> index = [] {
        QHash<QString, int> h;
        const auto &names = attrNames();
        for (int i = 0; i < kAttrCount; ++i)
            h.insert(names[i], i);
        return h;
    }();
    return index.value(fullName, -1);
}

const QList<FmDataVersion> &fmDataVersions()
{
    static const QList<FmDataVersion> versions = {
        {QStringLiteral("fm27"), QStringLiteral("Football Manager 2027"), true,
         QStringLiteral("definitions_fm27.json"), QStringLiteral("FM View Exporter"),
         {QStringLiteral("csv"), QStringLiteral("html"), QStringLiteral("fm")},
         /*provisionalWeights*/ true, /*registrationRules*/ true,
         /*unratedAttributes*/ {}},
    };
    return versions;
}

QString defaultFmVersionId()
{
    return QStringLiteral("fm27");
}

QString newestFmVersionId()
{
    for (const FmDataVersion &version : fmDataVersions()) {
        if (version.supported)
            return version.id;
    }
    return defaultFmVersionId();
}

const FmDataVersion &fmDataVersion(const QString &versionId)
{
    const QList<FmDataVersion> &versions = fmDataVersions();
    for (const FmDataVersion &version : versions) {
        if (version.id == versionId)
            return version;
    }
    for (const FmDataVersion &version : versions) {
        if (version.id == defaultFmVersionId())
            return version;
    }
    return versions.first();
}

bool attributeRated(const QString &versionId, int attrIndex)
{
    if (attrIndex < 0 || attrIndex >= kAttrCount)
        return false;
    return !fmDataVersion(versionId).unratedAttributes.contains(attrNames()[attrIndex]);
}

QString personalityLabel(const QString &personality)
{
    return QCoreApplication::translate("FootballPersonality", personality.toUtf8().constData());
}

QString playingTimeLabel(const QString &playingTime)
{
    return QCoreApplication::translate("FootballPlayingTime", playingTime.toUtf8().constData());
}

const QHash<QString, QString> &personalityDefaults()
{
    static const QHash<QString, QString> defaults = {
        // Good (positive)
        {QStringLiteral("Model Citizen"), QStringLiteral("good")},
        {QStringLiteral("Perfectionist"), QStringLiteral("good")},
        {QStringLiteral("Resolute"), QStringLiteral("good")},
        {QStringLiteral("Model Professional"), QStringLiteral("good")},
        {QStringLiteral("Professional"), QStringLiteral("good")},
        {QStringLiteral("Fairly Professional"), QStringLiteral("good")},
        {QStringLiteral("Spirited"), QStringLiteral("good")},
        {QStringLiteral("Driven"), QStringLiteral("good")},
        {QStringLiteral("Determined"), QStringLiteral("good")},
        {QStringLiteral("Fairly Determined"), QStringLiteral("good")},
        {QStringLiteral("Iron Willed"), QStringLiteral("good")},
        {QStringLiteral("Resilient"), QStringLiteral("good")},
        {QStringLiteral("Charismatic Leader"), QStringLiteral("good")},
        {QStringLiteral("Born Leader"), QStringLiteral("good")},
        {QStringLiteral("Leader"), QStringLiteral("good")},
        {QStringLiteral("Very Ambitious"), QStringLiteral("good")},
        {QStringLiteral("Ambitious"), QStringLiteral("good")},
        {QStringLiteral("Fairly Ambitious"), QStringLiteral("good")},
        {QStringLiteral("Fickle"), QStringLiteral("good")},
        {QStringLiteral("Mercenary"), QStringLiteral("good")},
        // Bad (negative)
        {QStringLiteral("Slack"), QStringLiteral("bad")},
        {QStringLiteral("Casual"), QStringLiteral("bad")},
        {QStringLiteral("Temperamental"), QStringLiteral("bad")},
        {QStringLiteral("Easily Discouraged"), QStringLiteral("bad")},
        {QStringLiteral("Low Determination"), QStringLiteral("bad")},
        {QStringLiteral("Spineless"), QStringLiteral("bad")},
        {QStringLiteral("Low Self-Belief"), QStringLiteral("bad")},
        {QStringLiteral("Unambitious"), QStringLiteral("bad")},
        // Neutral
        {QStringLiteral("Jovial"), QStringLiteral("neutral")},
        {QStringLiteral("Light-Hearted"), QStringLiteral("neutral")},
        {QStringLiteral("Devoted"), QStringLiteral("neutral")},
        {QStringLiteral("Very Loyal"), QStringLiteral("neutral")},
        {QStringLiteral("Loyal"), QStringLiteral("neutral")},
        {QStringLiteral("Fairly Loyal"), QStringLiteral("neutral")},
        {QStringLiteral("Honest"), QStringLiteral("neutral")},
        {QStringLiteral("Sporting"), QStringLiteral("neutral")},
        {QStringLiteral("Fairly Sporting"), QStringLiteral("neutral")},
        {QStringLiteral("Unsporting"), QStringLiteral("neutral")},
        {QStringLiteral("Realist"), QStringLiteral("neutral")},
        {QStringLiteral("Balanced"), QStringLiteral("neutral")},
    };
    return defaults;
}

const QStringList &fieldPlayerAptOptions()
{
    static const QStringList options = {
        QStringLiteral("None"), QStringLiteral("Star Player"), QStringLiteral("Important Player"),
        QStringLiteral("Regular Starter"), QStringLiteral("Squad Player"), QStringLiteral("Impact Sub"),
        QStringLiteral("Fringe Player"), QStringLiteral("Breakthrough Prospect"),
        QStringLiteral("Future Prospect"), QStringLiteral("Youngster"), QStringLiteral("Emergency Backup"),
        QStringLiteral("2nd Team Regular"), QStringLiteral("Surplus to Requirements"),
    };
    return options;
}

const QStringList &gkAptOptions()
{
    static const QStringList options = {
        QStringLiteral("None"), QStringLiteral("Star Player"), QStringLiteral("Important Player"),
        QStringLiteral("First-Choice Goalkeeper"), QStringLiteral("Cup Goalkeeper"),
        QStringLiteral("Backup"), QStringLiteral("Breakthrough Prospect"), QStringLiteral("Future Prospect"),
        QStringLiteral("Youngster"), QStringLiteral("Emergency Backup"),
        QStringLiteral("Surplus to Requirements"),
    };
    return options;
}

const QHash<QString, QString> &aptAbbreviations()
{
    static const QHash<QString, QString> abbreviations = {
        {QStringLiteral("Important Player"), QStringLiteral("Important")},
        {QStringLiteral("Breakthrough Prospect"), QStringLiteral("Prospect")},
        {QStringLiteral("Future Prospect"), QStringLiteral("Future")},
        {QStringLiteral("Emergency Backup"), QStringLiteral("Backup")},
        {QStringLiteral("2nd Team Regular"), QStringLiteral("2nd Team")},
        {QStringLiteral("Surplus to Requirements"), QStringLiteral("Surplus")},
    };
    return abbreviations;
}

} // namespace fm
