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

            // This one is semi-secret (will show with a hint description until you earn it, then will show the normal description and icon)
            AchievementsAPI.RegisterAchievement("TEST.SEMISECRET", false, showDescriptionNotAchieved:true, this);

            // These ones will use the GlobalMessenger
            AchievementsAPI.RegisterAchievement("TEST.MARSHMALLOW", false, this);
            AchievementsAPI.RegisterAchievement("TEST.DIE", true, this);

            // Earn an achievement when you finish roasting a marshmallow / when you die
            GlobalMessenger.AddListener("ExitRoastingMode", OnExitRoastingMode);
            GlobalMessenger<DeathType>.AddListener("PlayerDeath", OnPlayerDeath);

            // These ones will use patches
            AchievementsAPI.RegisterAchievement("TEST.SIGNAL", false, this);
            AchievementsAPI.RegisterAchievement("TEST.DIALOGUE", false, this);

            Patches.Apply();

            // Now add the translation stuff
            AchievementsAPI.RegisterTranslation("TEST.SOLAR_SYSTEM", TextTranslation.Language.ENGLISH, "Start", "You started the game.");
            AchievementsAPI.RegisterTranslation("TEST.EYE", TextTranslation.Language.ENGLISH, "End", "You ended the game.");
            AchievementsAPI.RegisterTranslation("TEST.SEMISECRET", TextTranslation.Language.ENGLISH, "Clues for everyone", "Get the semi-secret achievement by dying.", "I love hints! This achievement is the same as 'Ow'");

            AchievementsAPI.RegisterTranslation("TEST.MARSHMALLOW", TextTranslation.Language.ENGLISH, "Marshmallow", "Roast a marshmallow. Better than NH.");
            AchievementsAPI.RegisterTranslation("TEST.DIE", TextTranslation.Language.ENGLISH, "Ow", "Die.");

            AchievementsAPI.RegisterTranslation("TEST.SIGNAL", TextTranslation.Language.ENGLISH, "Signals++", "Identify a signal.");
            AchievementsAPI.RegisterTranslation("TEST.DIALOGUE", TextTranslation.Language.ENGLISH, "Fait beau eh?", "Talk to somebody.");

            // I'll do french too idk
            AchievementsAPI.RegisterTranslation("TEST.SOLAR_SYSTEM", TextTranslation.Language.FRENCH, "Le début", "Tu as commencé le jeu.");
            AchievementsAPI.RegisterTranslation("TEST.EYE", TextTranslation.Language.FRENCH, "La fin", "Tu as fini le jeu.");
            AchievementsAPI.RegisterTranslation("TEST.SEMISECRET", TextTranslation.Language.FRENCH, "Indices pour tout le monde", "Obtenir le succès semi-secret en mourant.", "J'adore les indices! Ce succès est le même que 'Ow'");

            AchievementsAPI.RegisterTranslation("TEST.MARSHMALLOW", TextTranslation.Language.FRENCH, "Guimauve", "Rôtir une guimauve. Mieux que NH. Cette blague ne marche pas en français.");
            AchievementsAPI.RegisterTranslation("TEST.DIE", TextTranslation.Language.FRENCH, "Ow", "Mourez.");

            AchievementsAPI.RegisterTranslation("TEST.SIGNAL", TextTranslation.Language.FRENCH, "Signals++", "Identifier un signal.");
            AchievementsAPI.RegisterTranslation("TEST.DIALOGUE", TextTranslation.Language.FRENCH, "Nice day eh?", "Parler à quelqu'un.");
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
            AchievementsAPI.EarnAchievement("TEST.SEMISECRET");
        }
    }
}
