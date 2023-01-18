using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LocalizationMenu : MenuSingleton<LocalizationMenu>
    {
        [SerializeField] private Toggle[] languages;

        public static bool IsLanguageSetup
        {
            get => PlayerPrefs.GetInt("IS_LANGUAGE_SETUP") == 1;
            set => PlayerPrefs.SetInt("IS_LANGUAGE_SETUP", value ? 1 : 0);
        }

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

            if (!IsLanguageSetup)
            {
                yield return new WaitForEndOfFrame();
                SettingsManager.Instance.SetLanguage(Settings.LanguageType.English);
                yield return new WaitForSeconds(1f);
                Open();
            }
            else
            {
                languages[(int)SettingsManager.Data.Language].isOn = true;
            }
        }
    }
}