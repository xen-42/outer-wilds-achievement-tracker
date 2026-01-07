# Outer Wilds Achievements+

![Achievements_plus thumbnail](https://user-images.githubusercontent.com/22628069/165662430-4291d875-5162-4782-8450-2a4d8d80937c.png)

Tracks Steam achievements in an in-game menu for people playing on other platforms that don't track achievements.

Also tracks custom mod achievements.

**If you make a mod that has achievements, please open a PR to add it to this list!**

## Supported Mods

- [Core Collapse](https://outerwildsmods.com/mods/corecollapse/)
- [Cut Achievements](https://outerwildsmods.com/mods/cutachievements/)
- [Forgotten Castaways](https://outerwildsmods.com/mods/forgottencastaways/)
- [Fret's Quest](https://outerwildsmods.com/mods/fretsquest/)
- [Grapefruit](https://outerwildsmods.com/mods/grapefruit/)
- [Hearthian Golf The Front Nine](https://outerwildsmods.com/mods/hearthiangolfthefrontnine/)
- [New Horizons](https://outerwildsmods.com/mods/newhorizons/)
- [New Horizons Examples](https://outerwildsmods.com/mods/newhorizonsexamples/)
- [Outer Thomas Echoes of the Tank Engine](https://outerwildsmods.com/mods/outerthomasechoesofthetankengine/)
- [Real Solar System](https://outerwildsmods.com/mods/realsolarsystem/)
- [Ship Enhancements](https://outerwildsmods.com/mods/shipenhancements/)
- [Signals+](https://outerwildsmods.com/mods/signals/)
- [The Mystery of I Cannot Find My Cat](https://outerwildsmods.com/mods/themysteryoficannotfindmycat/)
- [The Stranger They Are](https://outerwildsmods.com/mods/thestrangertheyare/)

And more to come...

## Settings

- "One-Shot Achievement Pop-Ups": Set to true if you only want notifications when you get an achievement for the first time. Set to false if you want to know each time you meet the requirements.

## For mod developers

Add this code to your mod for the Achievements+ API:

```cs
public interface IAchievements
{
    void RegisterAchievement(string uniqueID, bool secret, ModBehaviour mod);
    void RegisterAchievement(string uniqueID, bool secret, bool showDescriptionNotAchieved, ModBehaviour mod);
    void RegisterTranslation(string uniqueID, TextTranslation.Language language, string name, string description);
    void RegisterTranslation(string uniqueID, TextTranslation.Language language, string name, string description, string descriptionNotAchieved);
    void RegisterTranslationsFromFiles(ModBehaviour mod, string folderPath);
    void EarnAchievement(string uniqueID);
    bool HasAchievement(string uniqueID);
}
```

Then to access this in your mod, do:

```cs
var AchievementsAPI = ModHelper.Interaction.GetModApi<IAchievements>("xen.AchievementTracker");
```

Be sure to register achievements and their translations in the `Start` method of your mod so that they are set up immediately. 

The `uniqueID` field for your achievements should be unique not only in your mod, but among all mods. For example, if I were to add an achievement to [New Horizons](https://github.com/xen-42/outer-wilds-new-horizons) for discovering 5 planets, the `uniqueID` could be something like `NEWHORIZONS.5PLANETS`. 

There is an examples mod in the repo called `TestAchievementMod` which shows how to register achievements, register translations in two languages, and earn achievements on scene load, via the `GlobalMessenger` system, and using harmony patches.

### Custom icons

If you want custom icons to show up beside your achievements, simple add a folder called `Icons` to your mod. To create an icon for an achievement, put an image in this folder with its name equal to the `uniqueID` of the achievement. Similarly to make an icon for your mod, put an image in the folder with its name equal to the mod's name (not to its unique ID, just the human readable name from the `manifest.json` file in your mod).

### Register Translations from Files

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
        "ACHIEVEMENT_WITH_HINT_DESCRIPTION": {
            "Name": "Hint Achievement",
            "Description": "This description will be shown AFTER the player gets the achievement.",
			"DescriptionNotAchieved": "This description will be shown BEFORE the player gets the achievement."
        },
        ...
}
```

Where `AchievementTranslations` is a dictionary of key-value pairs where the keys are the `uniqueID` strings for the achievements, and each value has a `Name` and `Description` field.
You can also specify a `DescriptionNotAchieved` field if you want the achievement to be semi-hidden, with the intended purpose of DescriptionNotAchieved being a hint of some form for the players.

## Credits

I wanna thank those who contributed translations:
- Russian ([Tllya](https://github.com/Tllya))
- Spanish ([LikeMauro](https://github.com/LikeMauro))
