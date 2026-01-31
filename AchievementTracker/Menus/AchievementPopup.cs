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
        private const int QUEUE_CUTOFF_SIZE = 20; // Arbitrary, just to ensure someone does not add to the queue every frame

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
                    // Note: cannot immediately dequeue and show next entry, because the popup may take some time to be destroyed (and using DestroyImmediate bugs out on triggers)
                }
            }
            else if (_popup == null && _queue.Count != 0)
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

        public static void Show(AchievementManager.AchievementInfo achievement)
        {
            if (_popup != null)
            {
                if (_queue.Count > QUEUE_CUTOFF_SIZE)
                {
                    Logger.LogWarning($"Achievement popup queue getting long ({_queue.Count}). Ignoring new elements until queue is shorter. Check if your achievements are triggering too often, for example on every frame.");
                    return;
                }

                var newEntry = new PopupQueueEntry { Achievement = achievement, IsProgressPopup = false };
                _queue.Enqueue(newEntry);
                Logger.Log($"Queueing achievement [{achievement.ModName}] [{achievement.GetName()}]. Queue length {_queue.Count}");
                return;
            }

            if (!_popupRoot) Create();

            _isShown = true;
            _timer = SHOW_TIME;

            _popup = AchievementMenu.CreateAchievementUI(achievement.UniqueID, achievement.GetName(), achievement.GetDescription(), false, false, achievement.Mod);
            _popup.GetComponent<RectTransform>().SetParent(_popupRoot.transform);
            _popup.transform.position = new Vector3(160, 40, 0);
        }

        public static void ShowProgress(AchievementManager.AchievementInfo achievement, int current, int final)
        {
            if (_popup != null)
            {
                if (_queue.Count > QUEUE_CUTOFF_SIZE)
                {
                    Logger.LogWarning($"Achievement popup queue getting long ({_queue.Count}). Ignoring new elements until queue is shorter. Check if your achievements are triggering too often, for example on every frame.");
                    return;
                }

                var newEntry = new PopupQueueEntry { Achievement = achievement, IsProgressPopup = true, Current = current, Final = final };
                _queue.Enqueue(newEntry);
                Logger.Log($"Queueing achievement [{achievement.ModName}] [{achievement.GetName()}]. Queue length {_queue.Count}");
                return;
            }

            if (!_popupRoot) Create();

            _isShown = true;
            _timer = SHOW_TIME;

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
