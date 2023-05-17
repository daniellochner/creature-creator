using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LocalizationManager : MonoBehaviourSingleton<LocalizationManager>
    {
        #region Fields
        [SerializeField] private List<string> officialLanguages;
        #endregion

        #region Properties
        public List<string> OfficialLanguages => officialLanguages;

        private bool AutoDetectLanguage
        {
            get => PlayerPrefs.GetInt("AUTO_DETECT_LANGUAGE", 1) == 1;
            set => PlayerPrefs.SetInt("AUTO_DETECT_LANGUAGE", value ? 1 : 0);
        }
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;

            if (AutoDetectLanguage)
            {
                ILocalesProvider locales = LocalizationSettings.AvailableLocales;

                Locale locale = locales.GetLocale("en");
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Chinese:
                        locale = locales.GetLocale("zh-Hans");
                        break;
                    case SystemLanguage.Russian:
                        locale = locales.GetLocale("ru");
                        break;
                    case SystemLanguage.Spanish:
                        locale = locales.GetLocale("es");
                        break;
                    case SystemLanguage.Portuguese:
                        locale = locales.GetLocale("pt-BR");
                        break;
                    case SystemLanguage.German:
                        locale = locales.GetLocale("de");
                        break;
                    case SystemLanguage.French:
                        locale = locales.GetLocale("fr");
                        break;
                    case SystemLanguage.Japanese:
                        locale = locales.GetLocale("ja");
                        break;
                    case SystemLanguage.Polish:
                        locale = locales.GetLocale("pl");
                        break;
                    case SystemLanguage.Korean:
                        locale = locales.GetLocale("ko");
                        break;
                    case SystemLanguage.Thai:
                        locale = locales.GetLocale("th");
                        break;
                    case SystemLanguage.Italian:
                        locale = locales.GetLocale("it");
                        break;
                    case SystemLanguage.Turkish:
                        locale = locales.GetLocale("tr");
                        break;
                }
                SettingsManager.Instance.SetLocale(locale.Identifier.Code);

                AutoDetectLanguage = false;
            }
            else
            {
                SettingsManager.Instance.SetLocale(SettingsManager.Data.Locale);
            }
        }
        #endregion
    }
}