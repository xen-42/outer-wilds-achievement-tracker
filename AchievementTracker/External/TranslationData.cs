using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace AchievementTracker.External
{
    public class TranslationData
    {
        public string TitleTranslation { get; set; }
        public string AchievementHiddenTranslation { get; set; }
        public Dictionary<string, AchievementTranslation> AchievementTranslations { get; set; }

        public static Dictionary<TextTranslation.Language, string> achievementTitleDict = new Dictionary<TextTranslation.Language, string>();
        public static Dictionary<TextTranslation.Language, string> achievementHiddenDict = new Dictionary<TextTranslation.Language, string>();

        public TranslationData(string fileName, TextTranslation.Language lang)
        {
            Dictionary<string, object> dict = JObject.Parse(File.ReadAllText(fileName)).ToObject<Dictionary<string, object>>();

            if (dict.ContainsKey(nameof(AchievementTranslations)))
            {
                // Have to do really weird casting here for some reason, because it's an object I guess
                AchievementTranslations = (Dictionary<string, AchievementTranslation>)(dict[nameof(AchievementTranslations)] as Newtonsoft.Json.Linq.JObject).ToObject(typeof(Dictionary<string, AchievementTranslation>));
            }

            if(dict.ContainsKey(nameof(TitleTranslation)))
            {
                TitleTranslation = (string)dict[nameof(TitleTranslation)];
            }
            if (TitleTranslation != null)
            {
                achievementTitleDict[lang] = TitleTranslation;
            }

            if (dict.ContainsKey(nameof(AchievementHiddenTranslation)))
            {
                AchievementHiddenTranslation = (string)dict[nameof(AchievementHiddenTranslation)];
            }
            if (AchievementHiddenTranslation != null)
            {
                achievementHiddenDict[lang] = AchievementHiddenTranslation;
            }
        }

        public class AchievementTranslation
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string DescriptionNotAchieved { get; set; }
        }

        public static string GetTitle()
        {
            var language = TextTranslation.Get().m_language;

            if (achievementTitleDict.TryGetValue(language, out string title))
            {
                return title;
            }
            else if (achievementTitleDict.TryGetValue(TextTranslation.Language.ENGLISH, out title))
            {
                return title;
            }
            else
            {
                return "ACHIEVEMENTS";
            }
        }
        public static string GetAchievementHidden()
        {
            var language = TextTranslation.Get().m_language;

            if (achievementHiddenDict.TryGetValue(language, out string achievementHidden))
            {
                return achievementHidden;
            }
            else if (achievementHiddenDict.TryGetValue(TextTranslation.Language.ENGLISH, out achievementHidden))
            {
                return achievementHidden;
            }
            else
            {
                return "$COUNT achievement(s) hidden.";
            }
        }
    }
}
