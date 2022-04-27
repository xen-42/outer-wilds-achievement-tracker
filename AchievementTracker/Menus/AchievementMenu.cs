using AchievementTracker.External;
using AchievementTracker.Utit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Logger = AchievementTracker.Util.Logger;

namespace AchievementTracker.Menus
{
    public static class AchievementMenu
    {
        private static Font _font;
        private static Sprite _background;
        private static GameObject _menuRoot;
        private static GameObject _modList;
        private static GameObject _currentAchievementList;
        private static GameObject _buttonPrefab;

        private static string _currentModName;
        private static int _currentPage;
        private static int _shownAchievements;

        private const int PAGE_LIMIT = 8;

        public static bool IsOpen { get; private set; }

        public static void Open()
        {
            if (!_menuRoot) Create().transform.parent = GameObject.Find(SceneManager.GetActiveScene().name == "TitleScreen" ? "TitleMenu" : "PauseMenu").transform;

            if (_modList == null) MakeModList();

            IsOpen = true;
            _menuRoot.SetActive(true);
            _modList.SetActive(true);
        }

        public static void Close()
        {
            if (!_menuRoot) return;

            GameObject.Destroy(_menuRoot);

            IsOpen = false;
            _menuRoot.SetActive(false);

            // If we currently have an achievement list open, disappear it
            if (_currentAchievementList) _currentAchievementList.SetActive(false);

            HideAchievementsList();
        }

        private static GameObject Create()
        {
            if (!_background) _background = Resources.FindObjectsOfTypeAll<Sprite>().Where(x => x.name == "Background").FirstOrDefault();
            if (!_font) _font = Resources.FindObjectsOfTypeAll<Font>().Where(x => x.name == "Adobe - SerifGothicStd-ExtraBold").FirstOrDefault();
            if (!_buttonPrefab)
            {
                _buttonPrefab = GameObject.Instantiate(GameObject.Find("TitleMenu/OptionsCanvas/OptionsMenu-Panel/OptionsButtons/UIElement-SaveAndExit"));
                _buttonPrefab.GetComponent<ButtonWithHotkeyImageElement>().enabled = false;
                _buttonPrefab.GetComponent<SubmitActionCloseMenu>().enabled = false;
                _buttonPrefab.GetComponent<UIStyleApplier>().enabled = false;
                _buttonPrefab.transform.Find("ForegroundLayoutGroup").gameObject.SetActive(false);
                _buttonPrefab.transform.Find("SelectionArrows").gameObject.SetActive(false);
                _buttonPrefab.transform.Find("BackingImage").gameObject.SetActive(false);
                _buttonPrefab.SetActive(false);
                GameObject.DontDestroyOnLoad(_buttonPrefab);
            }

            _menuRoot = new GameObject();
            _menuRoot.SetActive(false);

            _menuRoot.name = "AchievementsCanvas";
            var canvas = _menuRoot.AddComponent<Canvas>();
            canvas.sortingOrder = 11;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _menuRoot.AddComponent<CanvasScaler>();
            _menuRoot.AddComponent<GraphicRaycaster>();

            var bgImage = _menuRoot.AddComponent<Image>();
            bgImage.sprite = _background;
            bgImage.type = Image.Type.Sliced;
            bgImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);

            var textObject = new GameObject();
            textObject.transform.parent = _menuRoot.transform;
            textObject.transform.localPosition = Vector3.zero;
            textObject.name = "AchievementText";

            var text = textObject.AddComponent<Text>();

            text.font = _font;
            text.alignment = TextAnchor.UpperCenter;

            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.fontSize = 40;
            text.color = new Color(1f, 0.5f, 0f, 1f);
            textObject.transform.localPosition = new Vector3(0, 320, 0);
            text.text = "ACHIEVEMENTS";

            MakeButton("BACK", new Vector3(0, -320, 0), new Vector2(240, 64), _menuRoot.transform, Back);
            MakeButton("<", new Vector3(-230, -320, 0), new Vector2(120, 64), _menuRoot.transform, () => ChangePage(-1));
            MakeButton(">", new Vector3(230, -320, 0), new Vector2(120, 64), _menuRoot.transform, () => ChangePage(1));

            return _menuRoot;
        }

        private static void ChangePage(int shift)
        {
            var pageCount = (int) (Mathf.Floor(_shownAchievements / PAGE_LIMIT) + 1);
            var newPage = 0;
            if(pageCount > 0)
            {
                newPage = (_currentPage + shift + pageCount) % pageCount;
            }

            Logger.Log($"Changing from page [{_currentPage}] to [{newPage}] out of [{pageCount}]");

            ShowAchievementsList(_currentModName, newPage);
        }

        private static void MakeButton(string text, Vector3 position, Vector2 size, Transform parent, UnityAction call)
        {
            var button = GameObject.Instantiate(_buttonPrefab);
            button.name = $"{text}_Button";
            button.SetActive(true);
            var rect = button.GetComponent<RectTransform>();
            rect.SetParent(parent);
            button.transform.localPosition = position;
            button.GetComponent<Button>().onClick.AddListener(call);

            var layout = button.GetComponent<LayoutElement>();
            layout.minHeight = size.y;
            layout.minWidth = size.x;

            rect.sizeDelta = size;

            var textObject = new GameObject();
            textObject.transform.parent = button.transform;
            textObject.transform.localPosition = new Vector3(0, -24, 0);
            textObject.name = $"{text}_Text";
            var textComponent = textObject.AddComponent<Text>();
            textComponent.font = _font;
            textComponent.alignment = TextAnchor.UpperCenter;
            textComponent.horizontalOverflow = HorizontalWrapMode.Overflow;
            textComponent.verticalOverflow = VerticalWrapMode.Overflow;
            textComponent.fontSize = 40;
            textComponent.color = new Color(1f, 0.5f, 0f, 1f);
            textComponent.text = $"{text}";
        }

        private static void MakeModList()
        {
            // Make list of mods
            _modList = new GameObject("AchievementsModList");
            var modListRect = _modList.AddComponent<RectTransform>();
            modListRect.SetParent(_menuRoot.transform);
            modListRect.transform.localPosition = new Vector3(-250, 240, 0);
            _modList.AddComponent<VerticalLayoutGroup>();

            foreach (var modName in AchievementManager.GetSupportedMods())
            {
                var ui = CreateModUI(modName);
                ui.GetComponent<RectTransform>().SetParent(_modList.transform);
            }
        }
        private static void Back()
        {
            if (_currentAchievementList) HideAchievementsList();
            else Close();
        }

        private static void ShowAchievementsList(string modName, int page)
        {
            if (!_menuRoot) return;

            _currentModName = modName;
            _currentPage = page;

            HideAchievementsList();

            _currentAchievementList = new GameObject("AchievementsList");
            _currentAchievementList.AddComponent<VerticalLayoutGroup>();
            _currentAchievementList.GetComponent<RectTransform>().SetParent(_menuRoot.transform);
            _currentAchievementList.transform.localPosition = new Vector3(-400, 240, 0);

            var modAchievements = AchievementManager.GetAchievements().Where(x => x.Value.ModName == modName);
            var nonHiddenAchievements = modAchievements.Where(x => !x.Value.Secret);
            var lockedAchievements = nonHiddenAchievements.Where(x => !AchievementData.HasAchievement(x.Value.UniqueID));
            var unlockedAchievements = modAchievements.Where(x => AchievementData.HasAchievement(x.Value.UniqueID));

            var hiddenCount = modAchievements.Count() - lockedAchievements.Count() - unlockedAchievements.Count();

            _shownAchievements = lockedAchievements.Count() + unlockedAchievements.Count() + (hiddenCount > 0 ? 1 : 0);

            var toSkip = page * PAGE_LIMIT;
            var count = 0;

            foreach (var achievement in unlockedAchievements.Concat(lockedAchievements))
            {
                if (toSkip-- > 0) continue;

                if (count++ >= PAGE_LIMIT) continue;

                var uniqueID = achievement.Value.UniqueID;
                var name = achievement.Value.GetName();
                var description = achievement.Value.GetDescription();
                var locked = !AchievementData.HasAchievement(achievement.Value.UniqueID);

                var ui = CreateAchievementUI(uniqueID, name, description, locked);
                ui.GetComponent<RectTransform>().SetParent(_currentAchievementList.transform);
            }

            if (hiddenCount > 0 && count < 8)
            {
                var ui = CreateAchievementUI("ACHIEVEMENTS_HIDDEN", $"{hiddenCount} achievement(s) are hidden.", "", false);
                ui.GetComponent<RectTransform>().SetParent(_currentAchievementList.transform);
            }

            // Stop showing the mod list
            _modList.SetActive(false);
        }

        private static void HideAchievementsList()
        {
            GameObject.Destroy(_currentAchievementList);
            _currentAchievementList = null;

            _modList.SetActive(true);
        }

        private static GameObject CreateModUI(string modName)
        {
            Texture2D texture = null;

            try
            {
                texture = ImageUtilities.GetTexture(Main.Instance, $"Icons/{modName}.jpg");
            }
            catch { }

            var panelObject = GameObject.Instantiate(_buttonPrefab);
            panelObject.name = $"Panel_{modName}";
            panelObject.SetActive(true);
            panelObject.GetComponent<Button>().onClick.AddListener(() => ShowAchievementsList(modName, 0));
            var layoutElement = panelObject.GetComponent<LayoutElement>();
            layoutElement.minHeight = 64;
            layoutElement.minWidth = 600;

            var imageObject = new GameObject($"Image_{modName}");
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
                imageText.color = new Color(1f, 0.5f, 0f);
                imageText.text = "?";
                imageText.horizontalOverflow = HorizontalWrapMode.Overflow;
                imageText.verticalOverflow = VerticalWrapMode.Overflow;
                imageText.alignment = TextAnchor.MiddleCenter;
            }
            var imageRT = imageObject.GetComponent<RectTransform>();
            imageRT.SetParent(panelObject.transform);
            imageRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 64);
            imageRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 64);
            imageObject.transform.localPosition = new Vector3(-256, 0, 0);

            var textObject = new GameObject($"Text_{modName}");
            var text = textObject.AddComponent<Text>();
            text.font = _font;
            text.text = $"{modName}\n{AchievementManager.GetCompletion(modName)}";
            text.alignment = TextAnchor.MiddleLeft;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            textObject.GetComponent<RectTransform>().SetParent(panelObject.transform);
            textObject.transform.localPosition = Vector3.zero;

            return panelObject;
        }

        private static GameObject CreateAchievementUI(string uniqueID, string name, string description, bool locked)
        {
            Texture2D texture = null;

            try
            {
                texture = ImageUtilities.GetTexture(Main.Instance, $"Icons/{uniqueID}.jpg");
                if (locked) texture = ImageUtilities.GreyscaleImage(texture);
            }
            catch { }

            var panelObject = new GameObject($"Panel_{uniqueID}");
            panelObject.AddComponent<CanvasRenderer>();
            var layoutElement = panelObject.AddComponent<LayoutElement>();
            layoutElement.minHeight = 64;
            layoutElement.minWidth = 600;

            var imageObject = new GameObject($"Image_{uniqueID}");
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

            var textObject = new GameObject($"Text_{uniqueID}");
            var text = textObject.AddComponent<Text>();
            text.font = _font;
            text.text = $"{name}\n{description}";
            text.alignment = TextAnchor.MiddleLeft;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            textObject.GetComponent<RectTransform>().SetParent(panelObject.transform);

            return panelObject;
        }
    }
}
