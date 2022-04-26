﻿using AchievementTracker.External;
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

                RegisterAchievement(type.ToString(), info.Secret, info.ModName);
            }

            // Register their translations
            foreach (TextTranslation.Language lang in Enum.GetValues(typeof(TextTranslation.Language)))
            {
                var folder = $"{Main.Instance.ModHelper.Manifest.ModFolderPath}";
                var filename = $"Translations/{lang.ToString().ToLower()}.json";
                if (File.Exists($"{folder}{filename}"))
                {
                    try
                    {
                        var translationTable = new TranslationData($"{folder}{filename}").AchievementTranslations;
                        foreach (var uniqueID in translationTable.Keys)
                        {
                            var name = translationTable[uniqueID].Name;
                            var description = translationTable[uniqueID].Description;
                            RegisterTranslation(uniqueID, lang, name, description);
                        }
                    }
                    catch(Exception ex)
                    {
                        Logger.LogError($"Failed to load translation data for {lang}\n{ex.Message}, {ex.StackTrace}");
                    }
                }
            }
        }

        public static void RegisterAchievement(string uniqueID, bool secret, string modName)
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

        public static void Earn(string uniqueID)
        {
            if (!_achievements.TryGetValue(uniqueID, out AchievementInfo achievement)) return;

            Logger.Log($"Earned achievement {achievement.GetName()} : {achievement.GetDescription()} from {achievement.ModName}");
            AchievementData.EarnAchievement(uniqueID);
        }

        public static Dictionary<string, AchievementInfo> GetAchievements()
        {
            return _achievements;
        }

        private static AchievementInfo GetStockAchievementInfo(Achievements.Type type)
        {
            string modName;
            bool secret;
            switch (type)
            {
                case Achievements.Type.TERRIBLE_FATE:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.WHATS_THIS_BUTTON:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.ALPHA_PILOT:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.YOU_TRIED:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.BEGINNERS_LUCK:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.SATELLITE:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.HEARTH_TO_MOON:
                    modName = "Outer Wilds";
                    secret = false;
                    break;
                case Achievements.Type.DEEP_IMPACT:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.HARMONIC_CONVERGENCE:
                    modName = "Outer Wilds";
                    secret = false;
                    break;
                case Achievements.Type.MUSEUM:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.DIEHARD:
                    modName = "Outer Wilds";
                    secret = false;
                    break;
                case Achievements.Type.PCHOOOOOOO:
                    modName = "Outer Wilds";
                    secret = false;
                    break;
                case Achievements.Type.GONE_IN_60_SECONDS:
                    modName = "Outer Wilds";
                    secret = false;
                    break;
                case Achievements.Type.CARCINOGENS:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.CUTTING_IT_CLOSE:
                    modName = "Outer Wilds";
                    secret = false;
                    break;
                case Achievements.Type.MICAS_WRATH:
                    modName = "Outer Wilds";
                    secret = true;
                    break;
                case Achievements.Type.STUDIOUS:
                    modName = "Outer Wilds";
                    secret = false;
                    break;
                case Achievements.Type.AROUND_THE_WORLD:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.SILENCED_CARTOGRAPHER:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.TUBULAR:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.EARLY_ADOPTER:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.GRATE_FILTER:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.FLAT_HEARTHER:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.CELCIUS:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.GHOSTS:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.SLEEP_WAKE_REPEAT:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.SIMULATION:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.FIRE_ARROWS:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.ONE_NINE:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.TAKEMEALIVE:
                    modName = "Echoes of the Eye";
                    secret = true;
                    break;
                case Achievements.Type.OOFMYBONES:
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
            private Dictionary<TextTranslation.Language, string> _nameDict;
            private Dictionary<TextTranslation.Language, string> _descriptionDict;
            public string UniqueID { get; private set; }
            public string ModName { get; private set; }
            public bool Secret { get; private set; }
            public AchievementInfo(string uniqueID, string modName, bool secret)
            {
                _nameDict = new Dictionary<TextTranslation.Language, string>();
                _descriptionDict = new Dictionary<TextTranslation.Language, string>();
                UniqueID = uniqueID;
                ModName = modName;
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

                string name;
                if (_nameDict.TryGetValue(language, out name))
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

                string description;
                if (_descriptionDict.TryGetValue(language, out description))
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
