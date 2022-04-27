using OWML.ModHelper;
using OWML.Common;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace TestAchievementMod
{
    public class TestAchievementMod : ModBehaviour
    {
        public static TestAchievementMod Instance;

        public IAchievements AchievementsAPI;

        private void Start()
        {
            Instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;

            AchievementsAPI = ModHelper.Interaction.GetModApi<IAchievements>("xen.AchievementTracker");

            // This one isn't a secret
            AchievementsAPI.RegisterAchievement("TEST.SOLAR_SYSTEM", false, this);

            // This one is (won't show up until you earn it)
            AchievementsAPI.RegisterAchievement("TEST.EYE", true, this);

            // This one will use the GlobalMessenger
            AchievementsAPI.RegisterAchievement("TEST.MARSHMALLOW", false, this);
            AchievementsAPI.RegisterAchievement("TEST.DIE", true, this);

            // Earn an achievement when you finish roasting a marshmallow
            GlobalMessenger.AddListener("ExitRoastingMode", OnExitRoastingMode);
            GlobalMessenger<DeathType>.AddListener("PlayerDeath", OnPlayerDeath);

            // These ones will use patches
            AchievementsAPI.RegisterAchievement("TEST.SIGNAL", false, this);
            AchievementsAPI.RegisterAchievement("TEST.DIALOGUE", false, this);

            Patches.Apply();

            // Now add the translation stuff
            AchievementsAPI.RegisterTranslation("TEST.SOLAR_SYSTEM", TextTranslation.Language.ENGLISH, "Start", "You started the game.");
            AchievementsAPI.RegisterTranslation("TEST.EYE", TextTranslation.Language.ENGLISH, "End", "You ended the game.");
            
            AchievementsAPI.RegisterTranslation("TEST.MARSHMALLOW", TextTranslation.Language.ENGLISH, "Marshmallow", "Roast a marshmallow. Better than NH.");
            AchievementsAPI.RegisterTranslation("TEST.DIE", TextTranslation.Language.ENGLISH, "Ow", "Die.");

            AchievementsAPI.RegisterTranslation("TEST.SIGNAL", TextTranslation.Language.ENGLISH, "Signals++", "Identify a signal.");
            AchievementsAPI.RegisterTranslation("TEST.DIALOGUE", TextTranslation.Language.ENGLISH, "Fait beau eh?", "Talk to somebody.");
        }

        private void OnDestroy()
        {
            GlobalMessenger.RemoveListener("ExitRoastingMode", OnExitRoastingMode);
            GlobalMessenger<DeathType>.RemoveListener("PlayerDeath", OnPlayerDeath);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Earn an achievement for playing the game
            if(scene.name == "SolarSystem")
            {
                AchievementsAPI.EarnAchievement("TEST.SOLAR_SYSTEM");
            }
            
            // Earn an achievement for reaching the eye
            if(scene.name == "EyeOfTheUniverse")
            {
                AchievementsAPI.EarnAchievement("TEST.EYE");
            }
        }

        private void OnExitRoastingMode()
        {
            AchievementsAPI.EarnAchievement("TEST.MARSHMALLOW");
        }

        private void OnPlayerDeath(DeathType _)
        {
            AchievementsAPI.EarnAchievement("TEST.DIE");
        }
    }
}
