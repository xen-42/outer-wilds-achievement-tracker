using AchievementTracker.External;
using UnityEngine;
using UnityEngine.UI;

namespace AchievementTracker.Menus
{
    public class AchievementLocalizedText : MonoBehaviour
	{
		private Text _text;

		protected virtual void Start()
		{
			_text = GetComponent<Text>();

			UpdateLanguage();

			TextTranslation.Get().OnLanguageChanged += UpdateLanguage;
		}

		protected virtual void OnDestroy()
		{
			TextTranslation.Get().OnLanguageChanged -= UpdateLanguage;
		}

		protected virtual void UpdateLanguage()
		{
			_text.text = TranslationData.GetTitle().ToUpper();
			_text.font = UIHandler.GetFont();
			_text.SetAllDirty();
		}
	}
}
