using OWML.ModHelper;
using OWML.Common;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using AchievementTracker.Util;
using AchievementTracker.External;
using Logger = AchievementTracker.Util.Logger;
using AchievementTracker.Utit;
using System.Linq;
using AchievementTracker.Menus;

namespace AchievementTracker
{
    public class Main : ModBehaviour
    {
        public static Main Instance;

        public override object GetApi()
        {
            return new AchievementAPI();
        }

        private void Start()
        {
            Instance = this;

            AchievementManager.Init();

            Patches.Apply();

            SceneManager.sceneLoaded += OnSceneLoaded;

            ModHelper.Menus.MainMenu.OnInit += InitTitleMenu;
            ModHelper.Menus.PauseMenu.OnInit += InitPauseMenu;
            ModHelper.Menus.PauseMenu.OnClosed += AchievementMenu.Close;
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            ModHelper.Menus.MainMenu.OnInit -= InitTitleMenu;
            ModHelper.Menus.PauseMenu.OnInit -= InitPauseMenu;
            ModHelper.Menus.PauseMenu.OnClosed -= AchievementMenu.Close;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AchievementMenu.Close();
            AchievementData.Load();
        }

        private void InitTitleMenu()
        {
            Logger.Log("Adding title screen button");

            var achievementsButton = ModHelper.Menus.MainMenu.SwitchProfileButton.Duplicate("Achievements".ToUpper());
            achievementsButton.OnClick += OnClickAchievementsButton;
        }

        private void InitPauseMenu()
        {
            Logger.Log("Adding pause screen button");

            var achievementsButton = ModHelper.Menus.PauseMenu.OptionsButton.Duplicate("Achievements".ToUpper());
            achievementsButton.OnClick += OnClickAchievementsButton;
        }

        private void OnClickAchievementsButton()
        {
            if (AchievementMenu.IsOpen) AchievementMenu.Close();
            else AchievementMenu.Open();
        }

        private void Update()
        {
            if(AchievementMenu.IsOpen && OWInput.IsNewlyPressed(InputLibrary.pause) || OWInput.IsNewlyPressed(InputLibrary.cancel))
            {
                AchievementMenu.Close();
            }
        }
    }
}
