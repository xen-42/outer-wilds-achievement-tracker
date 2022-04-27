using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAchievementMod
{
    public static class Patches
    {
        public static void Apply()
        {
            TestAchievementMod.Instance.ModHelper.HarmonyHelper.AddPrefix<AudioSignal>(nameof(AudioSignal.IdentifySignal), typeof(Patches), nameof(Patches.IdentifySignal));
            TestAchievementMod.Instance.ModHelper.HarmonyHelper.AddPrefix<CharacterDialogueTree>(nameof(CharacterDialogueTree.EndConversation), typeof(Patches), nameof(Patches.EndConversation));
        }

        public static void IdentifySignal()
        {
            TestAchievementMod.Instance.AchievementsAPI.EarnAchievement("TEST.SIGNAL");
        }

        public static void EndConversation()
        {
            TestAchievementMod.Instance.AchievementsAPI.EarnAchievement("TEST.DIALOGUE");
        }
    }
}
