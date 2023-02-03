using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LanguageUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Toggle toggle;
        #endregion

        #region Methods
        public void Setup(Locale locale)
        {
            //nameText.text = locale.LocaleName;
            //nameText.font = LocalizationSettings.AssetDatabase. GetLocalizedAsset<TMP_FontAsset>("fonts", "regular", locale);
            nameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("ui-misc", "language", locale);

            toggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                if (isOn)
                {
                    SettingsManager.Instance.SetLocale(locale.Identifier.Code);
                }
            });
            toggle.group = GetComponentInParent<ToggleGroup>();
            toggle.SetIsOnWithoutNotify(locale == LocalizationSettings.SelectedLocale);
        }
        #endregion
    }
}