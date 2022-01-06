using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementTracker
{
    public static class AchievementData
    {
        private static AchievementSaveFile _saveFile;
        private static AchievementProfile _activeProfile;
        private static string _activeProfileName;
        private static string _fileName = "save.json";

        public static bool Load()
        {
            _activeProfileName = StandaloneProfileManager.SharedInstance.currentProfile.profileName;
            try
            {
                _saveFile = AchievementTracker.Instance.ModHelper.Storage.Load<AchievementSaveFile>(_fileName);
                if (!_saveFile.Profiles.ContainsKey(_activeProfileName)) _saveFile.Profiles.Add(_activeProfileName, new AchievementProfile());
                _activeProfile = _saveFile.Profiles[_activeProfileName];
                Logger.Log($"Loaded save data for {_activeProfileName}");
            }
            catch (Exception)
            {
                try
                {
                    Logger.Log($"Couldn't load save data from {_fileName}, creating a new file");
                    _saveFile = new AchievementSaveFile();
                    _saveFile.Profiles.Add(_activeProfileName, new AchievementProfile());
                    _activeProfile = _saveFile.Profiles[_activeProfileName];
                    AchievementTracker.Instance.ModHelper.Storage.Save(_saveFile, _fileName);
                    Logger.Log($"Loaded save data for {_activeProfileName}");
                }
                catch (Exception e)
                {
                    Logger.LogError($"Couldn't create save data {e.Message}, {e.StackTrace}");
                    return false;
                }
            }
            return true;
        }

        public static void Save()
        {
            if (_saveFile == null) return;
            AchievementTracker.Instance.ModHelper.Storage.Save(_saveFile, _fileName);
        }

        public static void Reset()
        {
            if (_saveFile == null || _activeProfile == null)
            {
                Load();
            }
            Logger.Log($"Reseting save data for {_activeProfileName}");
            _activeProfile = new AchievementProfile();
            _saveFile.Profiles[_activeProfileName] = _activeProfile;

            Save();
        }

        public static bool HasAchievement(string uniqueID)
        {
            if (_activeProfile == null && Load()) return true;
            return _activeProfile.EarnedAchievements.Contains(uniqueID);
        }

        public static void EarnAchievement(string uniqueID)
        {
            if (_activeProfile == null && Load()) return;
            if (!HasAchievement(uniqueID))
            {
                _activeProfile.EarnedAchievements.Add(uniqueID);
                Save();
            }
        }

        private class AchievementSaveFile
        {
            public AchievementSaveFile()
            {
                Profiles = new Dictionary<string, AchievementProfile>();
            }

            public Dictionary<string, AchievementProfile> Profiles { get; set; }
        }

        private class AchievementProfile
        {
            public AchievementProfile()
            {
                EarnedAchievements = new List<string>();
            }

            public List<string> EarnedAchievements { get; set; }
        }
    }
}
