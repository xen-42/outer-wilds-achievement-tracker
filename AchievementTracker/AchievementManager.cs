using AchievementTracker.External;
using AchievementTracker.Menus;
using AchievementTracker.Util;
using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementTracker
{
    public static class AchievementManager
    {
        private static Dictionary<string, AchievementInfo> _achievements = new Dictionary<string, AchievementInfo>();

        public static void Init()
        {
            _achievements.Clear();

            // Register all the stock achievements
            foreach (Achievements.Type type in Enum.GetValues(typeof(Achievements.Type)))
            {
                // Idk what this achievement is but we don't want it
                if (type == Achievements.Type.TOTAL) continue;

                var info = GetStockAchievementInfo(type);

                RegisterStockAchievement(type.ToString(), info.Secret, info.ModName);
            }

            // Register their translations
            RegisterTranslationsFromFiles(Main.Instance, "Translations");
        }

        public static void RegisterAchievement(string uniqueID, bool secret, ModBehaviour mod)
        {
            if (_achievements.ContainsKey(uniqueID)) return;

            _achievements.Add(uniqueID, new AchievementInfo(uniqueID, mod, secret));
        }

        public static void RegisterStockAchievement(string uniqueID, bool secret, string modName)
        {
            if (_achievements.ContainsKey(uniqueID)) return;

            _achievements.Add(uniqueID, new AchievementInfo(uniqueID, modName, secret));
        }

        public static void RegisterTranslation(string uniqueID, TextTranslation.Language language, string name, string description)
        {
            if (_achievements.TryGetValue(uniqueID, out var info))
            {
                info.AddTranslation(language, name, description);
            }
        }

        public static void RegisterTranslationsFromFiles(ModBehaviour mod, string folderPath)
        {
            foreach (TextTranslation.Language lang in Enum.GetValues(typeof(TextTranslation.Language)))
            {
                var folder = $"{mod.ModHelper.Manifest.ModFolderPath}";
                var filename = $"{folderPath}/{lang.ToString().ToLower()}.json";
                if (File.Exists($"{folder}{filename}"))
                {
                    try
                    {
                        var data = new TranslationData($"{folder}{filename}", lang);

                        if (data.AchievementTranslations == null)
                        {
                            Logger.LogWarning($"Missing AchievementTranslations for {lang}");
                            continue;
                        }

                        var translationTable = data.AchievementTranslations;
                        foreach (var uniqueID in translationTable.Keys)
                        {
                            var name = translationTable[uniqueID].Name;
                            var description = translationTable[uniqueID].Description;
                            RegisterTranslation(uniqueID, lang, name, description);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Failed to load translation data for {lang}\n{ex.Message}, {ex.StackTrace}");
                    }
                }
            }
        }

        public static void Earn(string uniqueID)
        {
            if (!_achievements.TryGetValue(uniqueID, out AchievementInfo achievement)) return;

            if (!Main.OneShotPopups || !AchievementData.HasAchievement(uniqueID))
            {
                Logger.Log($"Earned achievement! [{achievement.ModName}] [{achievement.GetName()}] [{achievement.GetDescription()}]");
                AchievementData.EarnAchievement(uniqueID);
                AchievementPopup.Show(achievement);
            }
        }

        public static void Progress(string uniqueID, int current, int final, bool showPopup)
		{
            if (!_achievements.TryGetValue(uniqueID, out AchievementInfo achievement))
                return;

            AchievementData.UpdateProgress(uniqueID, current);

            if (showPopup)
			{
                AchievementPopup.ShowProgress(achievement, current, final);
            }
        }

        public static int GetProgress(string uniqueID)
		{
            return AchievementData.GetProgress(uniqueID);
		}

        public static Dictionary<string, AchievementInfo> GetAchievements()
        {
            return _achievements;
        }

        public static string GetCompletion(string modName)
        {
            var achievements = _achievements.Where(x => x.Value.ModName == modName);
            var totalCount = achievements.Count();
            var earnedCount = achievements.Where(x => AchievementData.HasAchievement(x.Value.UniqueID)).Count();

            return $"{earnedCount} / {totalCount}";
        }

        public static Tuple<ModBehaviour, string>[] GetSupportedMods()
        {
            // Wow this sucks
            var mods = _achievements.Select(x => x.Value.Mod).Distinct().ToArray();
            var modsAndNames = new List<Tuple<ModBehaviour, string>>();
            foreach(var mod in mods)
            {
                if(mod == Main.Instance)
                {
                    modsAndNames.Add(new Tuple<ModBehaviour, string>(mod, "Outer Wilds"));
                    modsAndNames.Add(new Tuple<ModBehaviour, string>(mod, "Echoes of the Eye"));
                }
                else
                {
                    modsAndNames.Add(new Tuple<ModBehaviour, string>(mod, mod.ModHelper.Manifest.Name));
                }
            }
            return modsAndNames.ToArray();
        }

        private static AchievementInfo GetStockAchievementInfo(Achievements.Type type)
        {
            string modName;
            bool secret;
            switch (type)
            {
                case Achievements.Type.TERRIBLE_FATE:
                case Achievements.Type.WHATS_THIS_BUTTON:
                case Achievements.Type.ALPHA_PILOT:
                case Achievements.Type.YOU_TRIED:
                case Achievements.Type.BEGINNERS_LUCK:
                case Achievements.Type.SATELLITE:
                case Achievements.Type.DEEP_IMPACT:
                case Achievements.Type.MUSEUM:
                case Achievements.Type.CARCINOGENS:
                case Achievements.Type.MICAS_WRATH:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.HEARTH_TO_MOON:
                case Achievements.Type.HARMONIC_CONVERGENCE:
                case Achievements.Type.DIEHARD:
                case Achievements.Type.PCHOOOOOOO:
                case Achievements.Type.GONE_IN_60_SECONDS:
                case Achievements.Type.CUTTING_IT_CLOSE:
                case Achievements.Type.STUDIOUS:
                    modName = "Outer Wilds";
                    secret = false;
                    break;
                case Achievements.Type.AROUND_THE_WORLD:
                case Achievements.Type.SILENCED_CARTOGRAPHER:
                case Achievements.Type.TUBULAR:
                case Achievements.Type.EARLY_ADOPTER:
                case Achievements.Type.GRATE_FILTER:
                case Achievements.Type.FLAT_HEARTHER:
                case Achievements.Type.CELCIUS:
                case Achievements.Type.GHOSTS:
                case Achievements.Type.SLEEP_WAKE_REPEAT:
                case Achievements.Type.SIMULATION:
                case Achievements.Type.FIRE_ARROWS:
                case Achievements.Type.ONE_NINE:
                case Achievements.Type.TAKEMEALIVE:
                case Achievements.Type.OOFMYBONES:
                case Achievements.Type.FOUND_SIGNAL:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                default:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
            }

            return new AchievementInfo(type.ToString(), modName, secret);
        }

        public class AchievementInfo
        {
            private readonly Dictionary<TextTranslation.Language, string> _nameDict;
            private readonly Dictionary<TextTranslation.Language, string> _descriptionDict;
            public ModBehaviour Mod { get; private set; }
            public string UniqueID { get; private set; }
            public string ModName { get; private set; }
            public bool Secret { get; private set; }

            public AchievementInfo(string uniqueID, ModBehaviour mod, bool secret)
            {
                _nameDict = new Dictionary<TextTranslation.Language, string>();
                _descriptionDict = new Dictionary<TextTranslation.Language, string>();
                Mod = mod;
                ModName = Mod.ModHelper.Manifest.Name;
                UniqueID = uniqueID;
                Secret = secret;
            }

            public AchievementInfo(string uniqueID, string modName, bool secret)
            {
                _nameDict = new Dictionary<TextTranslation.Language, string>();
                _descriptionDict = new Dictionary<TextTranslation.Language, string>();
                Mod = Main.Instance;
                ModName = modName;
                UniqueID = uniqueID;
                Secret = secret;
            }

            public void AddTranslation(TextTranslation.Language language, string name, string description)
            {
                _nameDict[language] = name;
                _descriptionDict[language] = description;
            }

            public string GetName()
            {
                var language = TextTranslation.Get().m_language;

                if (_nameDict.TryGetValue(language, out string name))
                {
                    return name;
                }
                else if (_nameDict.TryGetValue(TextTranslation.Language.ENGLISH, out name))
                {
                    return name;
                }
                else
                {
                    return UniqueID;
                }
            }

            public string GetDescription()
            {
                var language = TextTranslation.Get().m_language;

                if (_descriptionDict.TryGetValue(language, out string description))
                {
                    return description;
                }
                else if (_descriptionDict.TryGetValue(TextTranslation.Language.ENGLISH, out description))
                {
                    return description;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
