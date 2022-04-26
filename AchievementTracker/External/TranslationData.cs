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

        public TranslationData(string fileName)
        {
            Dictionary<string, object> dict = JObject.Parse(File.ReadAllText(fileName)).ToObject<Dictionary<string, object>>();

            if (dict.ContainsKey(nameof(AchievementTranslations)))
            {
                AchievementTranslations = (Dictionary<string, AchievementTranslation>)(dict[nameof(AchievementTranslations)] as Newtonsoft.Json.Linq.JObject).ToObject(typeof(Dictionary<string, AchievementTranslation>));
            }
            if(dict.ContainsKey(nameof(TitleTranslation)))
            {
                TitleTranslation = (string)dict[nameof(TitleTranslation)];
            }
        }

        public class AchievementTranslation
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
