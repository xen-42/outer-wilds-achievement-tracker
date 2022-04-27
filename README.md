# Outer Wilds Achievements+

Tracks Steam achievements in an in-game menu for people playing on other platforms that don't track achievements.

Also tracks custom mod achievements.

## For mod developers

Add this code to your mod for the Achievements+ API:

```cs
public interface IAchievements
{
    void RegisterAchievement(string uniqueID, bool secret, ModBehaviour mod);
    void RegisterTranslation(string uniqueID, TextTranslation.Language language, string name, string description);
    void RegisterTranslationsFromFiles(ModBehaviour mod, string folderPath);
    void EarnAchievement(string uniqueID);
}
```

Then to access this in your mod, do:

```cs
var AchievementsAPI = ModHelper.Interaction.GetModApi<IAchievements>("xen.AchievementTracker");
```

The `RegisterTranslationsFromFiles` method will look in the folder path you give it for files named `english.json`, `spanish_la.json`, `german.json`, `french.json`, `italian.json`, `polish.json`, `portuguese_br.json`, `japanese.json`, `russian.json`, `chinese_simple.json`, `korean.json`, and `turkish.json`.

The translations files should look like this:

```json
{
    "AchievementTranslations": {
        "TERRIBLE_FATE": {
            "Name": "You've met a terrible fate.",
            "Description": "No going back."
        },
        "WHATS_THIS_BUTTON": {
            "Name": "Hey, what's this button do?",
            "Description": "Press the ejection button in the ship."
        },
        ...
}
```

Where `AchievementTranslations` is a dictionary of key-value pairs where the keys are the `uniqueID` strings for the achievements, and each value has a `Name` and `Description` field.
