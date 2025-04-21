using AchievementTracker.External;
using HarmonyLib;

namespace AchievementTracker
{
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Achievements), nameof(Achievements.Earn))]
        public static void OnAchievementEarn(Achievements.Type type)
        {
            AchievementManager.Earn(type.ToString());
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StandaloneProfileManager), nameof(StandaloneProfileManager.SwitchProfile))]
        public static void OnSwitchProfile()
        {
            AchievementData.Load();
        }

        private static bool _saveFileExists;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.ResetGame))]
        public static void PlayerData_ResetGame_Prefix()
        {
            _saveFileExists = PlayerData._currentGameSave != null && PlayerData._currentGameSave.loopCount > 1;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.ResetGame))]
        public static void PlayerData_ResetGame()
        {
            // After getting beginners luck, no save file is actually made
            // If you go to continue playing on this profile, the achievement would get reset meaning no profile can 100% all achievements
            // To get around this we skip resetting if theres no save file made yet
            if (_saveFileExists)
            {
                AchievementData.Reset();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.SaveCurrentGame))]
        public static void PlayerData_SaveCurrentGame()
        {
            AchievementData.Save();
        }
    }
}
