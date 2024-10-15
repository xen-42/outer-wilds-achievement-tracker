using AchievementTracker.External;
using AchievementTracker.Menus;
using AchievementTracker.Util;
using OWML.Common;
using OWML.ModHelper;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Logger = AchievementTracker.Util.Logger;

namespace AchievementTracker
{
    public class Main : ModBehaviour
    {
        public static Main Instance;

        public static OWAudioSource AchievementAudio;
        public static bool OneShotPopups { get; private set; }
        public static bool ShowMoreLogs { get; private set; }

        public override object GetApi()
        {
            return new AchievementAPI();
        }

        public override void Configure(IModConfig config)
        {
            base.Configure(config);
            OneShotPopups = config.GetSettingsValue<bool>("One-Shot Achievement Pop-Ups");
            ShowMoreLogs = config.GetSettingsValue<bool>("Show More Logs");

            Logger.Log($"One-Shot Achievement Pop-Ups enabled? [{OneShotPopups}]");
        }

        public static OWAudioSource MakeOneShot(GameObject parent, OWAudioMixer.TrackName track)
        {

            var go = new GameObject($"OneShot");
            go.transform.parent = parent.transform;
            go.transform.localPosition = Vector3.zero;
            go.SetActive(false);

            var audioSource = go.AddComponent<AudioSource>();
            var oneShotAudioSource = go.AddComponent<OWAudioSource>();
            oneShotAudioSource._audioSource = audioSource;
            oneShotAudioSource.spatialBlend = 1f;
            oneShotAudioSource.SetTrack(track);

            go.SetActive(true);

            go.GetComponent<AudioSource>().clip = Instance.ModHelper.Assets.GetAudio("Icons/steam-achievement.mp3");

            return oneShotAudioSource;
        }

        public static void PlayAchievementSound()
        {
            // prevent achievement sounds from playing on start-up
            if (Time.time < 3) return;
            if (AchievementAudio == null)
            {
                AchievementAudio = MakeOneShot(Instance.gameObject, OWAudioMixer.TrackName.Environment_Unfiltered);

                if (AchievementAudio == null)
                {
                    Logger.LogError("Unable to play requested audio!");
                    return;
                }
            }

            AchievementAudio.Play();
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

            AchievementAudio = MakeOneShot(Instance.gameObject, OWAudioMixer.TrackName.Menu);
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
