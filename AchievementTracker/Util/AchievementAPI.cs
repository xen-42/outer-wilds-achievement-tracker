using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementTracker.Util
{
    public class AchievementAPI
    {
        public void RegisterAchievement(string uniqueID, bool secret, ModBehaviour mod)
        {
            AchievementManager.RegisterAchievement(uniqueID, secret, mod.ModHelper.Manifest.Name);
        }

        public void RegisterTranslation(string uniqueID, TextTranslation.Language language, string name, string description)
        {
            AchievementManager.RegisterTranslation(uniqueID, language, name, description);
        }

        public void EarnAchievement(string uniqueID)
        {
            AchievementManager.Earn(uniqueID);
        }
    }
}
