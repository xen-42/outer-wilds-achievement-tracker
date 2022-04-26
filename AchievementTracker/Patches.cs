using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementTracker
{
    public static class Patches
    {
        public static void Apply()
        {
            Main.Instance.ModHelper.HarmonyHelper.AddPrefix<Achievements>(nameof(Achievements.Earn), typeof(Patches), nameof(Patches.OnAchievementEarn));
        }

        public static void OnAchievementEarn(Achievements.Type type)
        {
            AchievementManager.Earn(type.ToString());
        }
    }
}
