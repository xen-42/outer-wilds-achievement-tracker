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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.ResetGame))]
        public static void PlayerData_ResetGame()
        {
            AchievementData.Reset();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.SaveCurrentGame))]
        public static void PlayerData_SaveCurrentGame()
        {
            AchievementData.Save();
        }
    }
}
