﻿using AchievementTracker.External;
using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementTracker.Util
{
    public class AchievementAPI : IAchievements
    {
        public void RegisterAchievement(string uniqueID, bool secret, ModBehaviour mod)
        {
            AchievementManager.RegisterAchievement(uniqueID, secret, mod);
        }

        public void RegisterTranslation(string uniqueID, TextTranslation.Language language, string name, string description)
        {
            AchievementManager.RegisterTranslation(uniqueID, language, name, description);
        }

        public void RegisterTranslationsFromFiles(ModBehaviour mod, string folderPath)
        {
            AchievementManager.RegisterTranslationsFromFiles(mod, folderPath);
        }

        public void EarnAchievement(string uniqueID)
        {
            AchievementManager.Earn(uniqueID);
        }

        public void UpdateProgress(string uniqueID, int current, int final, bool showPopup)
		{
            AchievementManager.Progress(uniqueID, current, final, showPopup);
		}

        public int GetProgress(string uniqueId)
		{
            return AchievementManager.GetProgress(uniqueId);
		}

        public bool HasAchievement(string uniqueID)
        {
            return AchievementData.HasAchievement(uniqueID);
        }
    }
}
