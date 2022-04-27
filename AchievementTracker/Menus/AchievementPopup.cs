using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Logger = AchievementTracker.Util.Logger;

namespace AchievementTracker.Menus
{
    public static class AchievementPopup
    {
        private const float SHOW_TIME = 4f;

        private static bool _isShown = false;
        private static float _timer;
        private static GameObject _popupRoot;
        private static GameObject _popup;
        private static Font _font;

        private static Queue<AchievementManager.AchievementInfo> _queue;

        private static void Create()
        {
            Logger.Log("Adding achievement popup");

            _queue = new Queue<AchievementManager.AchievementInfo>();

            if (!_font) _font = Resources.FindObjectsOfTypeAll<Font>().Where(x => x.name == "Adobe - SerifGothicStd-ExtraBold").FirstOrDefault();

            _popupRoot = new GameObject();
            _popupRoot.SetActive(false);

            _popupRoot.name = "AchievementsPopup";
            var canvas = _popupRoot.AddComponent<Canvas>();
            canvas.sortingOrder = 11;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            _popupRoot.SetActive(true);
        }

        public static void Update()
        {
            if (!_popupRoot) return;

            if (_isShown)
            {
                _timer -= Time.deltaTime;
                if (_timer < 0)
                {
                    _timer = 0;
                    _isShown = false;

                    GameObject.Destroy(_popup);

                    if (_queue.Count != 0) Show(_queue.Dequeue());
                }
            }
        }

        public static void Show(AchievementManager.AchievementInfo achievement)
        {
            if (!_popupRoot) Create();

            _isShown = true;
            _timer = SHOW_TIME;

            if (_popup)
            {
                // Add it to the queue but also try to tone down the number of duplicates
                if (!_queue.Contains(achievement)) _queue.Enqueue(achievement);
            }
            else
            {
                _popup = AchievementMenu.CreateAchievementUI(achievement.UniqueID, achievement.GetName(), achievement.GetDescription(), false, achievement.Mod);
                _popup.GetComponent<RectTransform>().SetParent(_popupRoot.transform);
                _popup.transform.position = new Vector3(160, 40, 0);
            }
        }
    }
}
