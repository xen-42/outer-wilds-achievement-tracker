using AchievementTracker.External;
using AchievementTracker.Menus;
using AchievementTracker.Util;
using OWML.Common;
using OWML.ModHelper;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Logger = AchievementTracker.Util.Logger;

namespace AchievementTracker
{
    public class Main : ModBehaviour
    {
        public static Main Instance;
        public static bool OneShotPopups { get; private set; }

        public override object GetApi()
        {
            return new AchievementAPI();
        }

        public override void Configure(IModConfig config)
        {
            base.Configure(config);
            OneShotPopups = config.GetSettingsValue<bool>("One-Shot Achievement Pop-Ups");

            Logger.Log($"One-Shot Achievement Pop-Ups enabled? [{OneShotPopups}]");
        }

        private void Start()
        {
            Instance = this;

            AchievementManager.Init();

            Patches.Apply();

            SceneManager.sceneLoaded += OnSceneLoaded;

            ModHelper.Menus.MainMenu.OnInit += InitTitleMenu;
            ModHelper.Menus.PauseMenu.OnInit += InitPauseMenu;
            ModHelper.Menus.PauseMenu.OnClosed += () => AchievementMenu.Close(false);
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            ModHelper.Menus.MainMenu.OnInit -= InitTitleMenu;
            ModHelper.Menus.PauseMenu.OnInit -= InitPauseMenu;
            ModHelper.Menus.PauseMenu.OnClosed -= () => AchievementMenu.Close(false);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AchievementMenu.Close();
            AchievementData.Load();
        }

        private void InitTitleMenu()
        {
            Logger.Log("Adding title screen button");

            var achievementsButton = ModHelper.Menus.MainMenu.SwitchProfileButton.Duplicate(TranslationData.GetTitle().ToUpper());
            achievementsButton.Button.gameObject.GetComponentInChildren<Text>().gameObject.AddComponent<AchievementLocalizedText>();
            achievementsButton.OnClick += OnClickAchievementsButton;
        }

        private void InitPauseMenu()
        {
            Logger.Log("Adding pause screen button");

            var achievementsButton = ModHelper.Menus.PauseMenu.OptionsButton.Duplicate(TranslationData.GetTitle().ToUpper());
            achievementsButton.Button.gameObject.GetComponentInChildren<Text>().gameObject.AddComponent<AchievementLocalizedText>();
            achievementsButton.OnClick += OnClickAchievementsButton;
        }

        private void OnClickAchievementsButton()
        {
            if (AchievementMenu.IsOpen) AchievementMenu.Close();
            else AchievementMenu.Open();
        }

        private void Update()
        {
            if (AchievementMenu.IsOpen && OWInput.IsNewlyPressed(InputLibrary.pause) || OWInput.IsNewlyPressed(InputLibrary.cancel))
            {
                AchievementMenu.Close();
            }

            AchievementPopup.Update();
        }
    }
}
