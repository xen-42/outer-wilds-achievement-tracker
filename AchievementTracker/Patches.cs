using AchievementTracker.External;

namespace AchievementTracker
{
    public static class Patches
    {
        public static void Apply()
        {
            Main.Instance.ModHelper.HarmonyHelper.AddPrefix<Achievements>(
                nameof(Achievements.Earn), 
                typeof(Patches), 
                nameof(Patches.OnAchievementEarn)
            );

            Main.Instance.ModHelper.HarmonyHelper.AddPostfix<StandaloneProfileManager>(
                nameof(StandaloneProfileManager.SwitchProfile),
                typeof(Patches), 
                nameof(Patches.OnSwitchProfile)
            );
        }

        public static void OnAchievementEarn(Achievements.Type type)
        {
            AchievementManager.Earn(type.ToString());
        }

        public static void OnSwitchProfile()
        {
            AchievementData.Load();
        }
    }
}
