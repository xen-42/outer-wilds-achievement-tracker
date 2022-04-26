using OWML.ModHelper;
using OWML.Common;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace TestAchievementMod
{
    public class TestAchievementMod : ModBehaviour
    {
        public IAchievements AchievementsAPI;

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            AchievementsAPI = ModHelper.Interaction.GetModApi<IAchievements>("xen.AchievementTracker");

            // This one isn't a secret
            AchievementsAPI.RegisterAchievement("TEST.SOLAR_SYSTEM", false, this);

            // This one is (won't show up until you earn it)
            AchievementsAPI.RegisterAchievement("TEST.EYE", true, this);

            // This one will use the GlobalMessenger
            AchievementsAPI.RegisterAchievement("TEST.MARSHMALLOW", false, this);

            // Earn an achievement when you finish roasting a marshmallow
            GlobalMessenger.AddListener("ExitRoastingMode", new Callback(OnExitRoastingMode));

            // Now add the translation stuff
            AchievementsAPI.RegisterTranslation("TEST.SOLAR_SYSTEM", TextTranslation.Language.ENGLISH, "Start", "You started the game.");
            AchievementsAPI.RegisterTranslation("TEST.EYE", TextTranslation.Language.ENGLISH, "End", "You ended the game.");
            AchievementsAPI.RegisterTranslation("TEST.MARSHMALLOW", TextTranslation.Language.ENGLISH, "Marshmallow", "Roast a marshmallow. Better than NH.");
        }

        private void OnDestroy()
        {
            GlobalMessenger.RemoveListener("ExitRoastingMode", new Callback(OnExitRoastingMode));
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
    }
}
