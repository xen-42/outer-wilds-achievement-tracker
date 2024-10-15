using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AchievementTracker.Menus
{
    public static class UIHandler
    {
        public static Transform GetRootMenu()
        {
            return GameObject.Find(SceneManager.GetActiveScene().name == "TitleScreen" ? "TitleMenu" : "PauseMenu").transform;
        }

        public static Font GetFont()
        {
            var root = SceneManager.GetActiveScene().name == "TitleScreen" ? "TitleMenu" : "PauseMenu";
            var text = GameObject.Find($"{root}/OptionsCanvas/OptionsMenu-Panel/OptionsButtons/UIElement-SaveAndExit").GetComponentInChildren<Text>();
            return text.font;
        }
    }
}
