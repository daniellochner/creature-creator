using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LocalizationMenu : MenuSingleton<LocalizationMenu>
    {
        #region Fields
        [SerializeField] private Toggle[] languages;
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            for (int i = 0; i < languages.Length; i++)
            {
                int index = i;
                languages[i].onValueChanged.AddListener(delegate (bool isOn)
                {
                    if (isOn)
                    {
                        SettingsManager.Instance.SetLanguage((Settings.LanguageType)index);
                    }
                });
            }

            yield return LocalizationSettings.InitializationOperation;

            SetupLanguage();
        }

        private void SetupLanguage()
        {
            if (PlayerPrefs.GetInt("AUTO_DETECT_LANG") == 0)
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Chinese:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.ChineseSimplified);
                        break;
                    case SystemLanguage.Russian:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Russian);
                        break;
                    case SystemLanguage.Spanish:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Spanish);
                        break;
                    case SystemLanguage.Portuguese:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Portuguese);
                        break;
                    case SystemLanguage.German:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.German);
                        break;
                    case SystemLanguage.French:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.French);
                        break;
                    case SystemLanguage.Japanese:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Japanese);
                        break;
                    case SystemLanguage.Polish:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Polish);
                        break;
                    case SystemLanguage.Turkish:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Turkish);
                        break;
                    case SystemLanguage.Korean:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Korean);
                        break;
                    case SystemLanguage.Thai:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Thai);
                        break;
                    case SystemLanguage.Italian:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Italian);
                        break;
                    case SystemLanguage.Czech:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Czech);
                        break;
                    case SystemLanguage.Vietnamese:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.Vietnamese);
                        break;
                    default:
                        SettingsManager.Instance.SetLanguage(Settings.LanguageType.English);
                        break;
                }
                PlayerPrefs.SetInt("AUTO_DETECT_LANG", 1);
            }
            else
            {
                SettingsManager.Instance.SetLanguage(SettingsManager.Data.Language);
            }
            languages[(int)SettingsManager.Data.Language].isOn = true;
        }
        #endregion
    }
}