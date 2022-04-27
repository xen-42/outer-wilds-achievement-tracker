using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementTracker.External
{
    public class TranslationData
    {
        public string TitleTranslation { get; set; }
        public Dictionary<string, AchievementTranslation> AchievementTranslations { get; set; }

        public static Dictionary<TextTranslation.Language, string> achievementTitleDict = new Dictionary<TextTranslation.Language, string>();

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
        }

        public class AchievementTranslation
        {
            public string Name { get; set; }
            public string Description { get; set; }
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
    }
}
