using OWML.ModHelper;
using OWML.Common;
using UnityEngine.SceneManagement;

namespace AchievementTracker
{
    public class AchievementTracker : ModBehaviour
    {
        public static AchievementTracker Instance;

        private void Awake()
        {

        }

        private void Start()
        {
            Instance = this;
            Patches.Apply();

            // Example of accessing game code.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AchievementData.Load();
        }
    }
}
