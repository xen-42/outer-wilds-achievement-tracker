using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        private static Queue<PopupQueueEntry> _queue;

        private static void Create()
        {
            Logger.Log("Adding achievement popup");

            _queue = new Queue<PopupQueueEntry>();

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
                _timer -= Time.unscaledDeltaTime;
                if (_timer < 0)
                {
                    _timer = 0;
                    _isShown = false;

                    GameObject.Destroy(_popup);

                    if (_queue.Count != 0)
                    {
                        var entry = _queue.Dequeue();
                        if (entry.IsProgressPopup)
						{
                            ShowProgress(entry.Achievement, entry.Current, entry.Final);
						}
						else
						{
                            Show(entry.Achievement);
						}
                    }
                }
            }
        }

        public static void Show(AchievementManager.AchievementInfo achievement)
        {
            if (!_popupRoot) Create();

            _isShown = true;
            _timer = SHOW_TIME;

            if (_popup != null)
            {
                GameObject.DestroyImmediate(_popup);
            }
            _popup = AchievementMenu.CreateAchievementUI(achievement.UniqueID, achievement.GetName(), achievement.GetDescription(), false, false, achievement.Mod);
            _popup.GetComponent<RectTransform>().SetParent(_popupRoot.transform);
            _popup.transform.position = new Vector3(160, 40, 0);
        }

        public static void ShowProgress(AchievementManager.AchievementInfo achievement, int current, int final)
		{
            if (!_popupRoot) Create();

            _isShown = true;
            _timer = SHOW_TIME;

            if (_popup != null)
            {
                GameObject.DestroyImmediate(_popup);
            }
            _popup = AchievementMenu.CreateAchievementUI(achievement.UniqueID, achievement.GetName(), $"{current} / {final}", true, achievement.ShowDescriptionNotAchieved, achievement.Mod);
            _popup.GetComponent<RectTransform>().SetParent(_popupRoot.transform);
            _popup.transform.position = new Vector3(160, 40, 0);
        }
    }

    public struct PopupQueueEntry
	{
        public AchievementManager.AchievementInfo Achievement;
        public bool IsProgressPopup;
        public int Current;
        public int Final;
	}
}
