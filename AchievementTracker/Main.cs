using OWML.ModHelper;
using OWML.Common;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using AchievementTracker.Util;
using AchievementTracker.External;
using Logger = AchievementTracker.Util.Logger;
using AchievementTracker.Utit;

namespace AchievementTracker
{
    public class Main : ModBehaviour
    {
        public static Main Instance;

        private bool _menuOpen = false;

        private GameObject _menu;

        private Font _font;

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
            ModHelper.Menus.PauseMenu.OnClosed += CloseMenu;
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            ModHelper.Menus.MainMenu.OnInit -= InitTitleMenu;
            ModHelper.Menus.PauseMenu.OnInit -= InitPauseMenu;
            ModHelper.Menus.PauseMenu.OnClosed -= CloseMenu;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            CloseMenu();
            AchievementData.Load();
        }

        private void InitTitleMenu()
        {
            Logger.Log("Adding title screen button");

            var achievementsButton = ModHelper.Menus.MainMenu.SwitchProfileButton.Duplicate("Achievements".ToUpper());
            achievementsButton.OnClick += OnClickAchievementsButton;

            _menu = CreateMenu();
            _menu.transform.parent = GameObject.Find("TitleMenu").transform;
        }

        private void InitPauseMenu()
        {
            Logger.Log("Adding pause screen button");

            var achievementsButton = ModHelper.Menus.PauseMenu.OptionsButton.Duplicate("Achievements".ToUpper());
            achievementsButton.OnClick += OnClickAchievementsButton;
        }

        private void OnClickAchievementsButton()
        {
            if (_menuOpen) CloseMenu();
            else OpenMenu();
        }

        private void OpenMenu()
        {
            _menuOpen = true;
            if (_menu)
            {
                RefreshText();
                _menu.SetActive(true);
            }
        }

        private void CloseMenu()
        {
            _menuOpen = false;
            if (_menu) _menu.SetActive(false);
        }

        private GameObject CreateMenu()
        {
            // Stuff we're going to copy from
            var optionsCanvas = GameObject.Find("TitleMenu/OptionsCanvas");
            var optionsDisplayPanel = GameObject.Find("TitleMenu/OptionsCanvas/OptionsMenu-Panel/OptionsDisplayPanel");
            var optionsPanelBackground = GameObject.Find("TitleMenu/OptionsCanvas/OptionsMenu-Panel/OptionsDisplayPanel/Background");
            var optionsButtons = GameObject.Find("TitleMenu/OptionsCanvas/OptionsMenu-Panel/OptionsButtons");
            var saveButton = GameObject.Find("TitleMenu/OptionsCanvas/OptionsMenu-Panel/OptionsButtons/UIElement-SaveAndExit");

            var menuRoot = new GameObject();
            menuRoot.SetActive(false);

            menuRoot.name = "AchievementsCanvas";
            var canvas = menuRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            menuRoot.AddComponent<CanvasScaler>();
            menuRoot.AddComponent<GraphicRaycaster>();

            var textObject = new GameObject();
            textObject.transform.parent = menuRoot.transform;
            textObject.transform.localPosition = Vector3.zero;
            textObject.name = "AchievementText";

            var text = textObject.AddComponent<Text>();
            if (_font == null) _font = optionsButtons.GetComponentInChildren<Text>().font;
            text.font = _font;
            text.alignment = TextAnchor.UpperCenter;

            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.fontSize = 40;
            text.color = new Color(1f, 0.5f, 0f, 1f);
            textObject.transform.localPosition = new Vector3(0, 320, 0);
            text.text = "ACHIEVEMENTS";

            return menuRoot;
        }

        private void RefreshText()
        {
            var i = 0;
            foreach (var achievement in AchievementManager.GetAchievements())
            {
                if (!AchievementData.HasAchievement(achievement.Value.UniqueID) && achievement.Value.Secret)
                {
                    continue;
                }
                else
                {
                    var ui = CreateAchievementUI(achievement.Value);
                    ui.GetComponent<RectTransform>().SetParent(_menu.transform);
                    ui.transform.localPosition = new Vector3(0, 240 - (i++ * 72), 0);
                }
            }
        }

        private GameObject CreateAchievementUI(AchievementManager.AchievementInfo achievement)
        {
            Texture2D texture = null;

            var locked = !AchievementData.HasAchievement(achievement.UniqueID);

            try
            {
                texture = ImageUtilities.GetTexture(this, $"Icons/{achievement.UniqueID}.jpg");
                if (locked) texture = ImageUtilities.GreyscaleImage(texture);
            }
            catch { }

            var panelObject = new GameObject($"Panel_{achievement.UniqueID}");
            panelObject.AddComponent<CanvasRenderer>();
            var panelScaler = panelObject.AddComponent<CanvasScaler>();
            panelScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            var panelRect = panelObject.GetComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(600, 64);

            var imageObject = new GameObject($"Image_{achievement.UniqueID}");
            if (texture != null)
            {
                var image = imageObject.AddComponent<Image>();
                image.sprite = ImageUtilities.MakeSprite(texture);
            }
            else
            {
                var imageText = imageObject.AddComponent<Text>();
                imageText.fontSize = 64;
                imageText.font = _font;
                imageText.color = locked ? new Color(0.59f, 0.59f, 0.59f) : new Color(1f, 0.5f, 0f);
                imageText.text = "?";
                imageText.horizontalOverflow = HorizontalWrapMode.Overflow;
                imageText.verticalOverflow = VerticalWrapMode.Overflow;
                imageText.alignment = TextAnchor.MiddleCenter;
            }
            var imageRT = imageObject.GetComponent<RectTransform>();
            imageRT.SetParent(panelObject.transform);
            imageRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 64);
            imageRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 64);
            imageObject.transform.localPosition = new Vector3(-100, 0, 0);

            var textObject = new GameObject($"Text_{achievement.UniqueID}");
            var text = textObject.AddComponent<Text>();
            text.font = _font;
            text.text = $"{achievement.GetName()}\n{achievement.GetDescription()}";
            text.alignment = TextAnchor.MiddleLeft;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            textObject.GetComponent<RectTransform>().SetParent(panelObject.transform);

            return panelObject;
        }
    }
}
