using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LocalizationMenu : Dialog<LocalizationMenu>
    {
        #region Fields
        [SerializeField] private LanguageUI languagePrefab;
        [SerializeField] private RectTransform languagesRT;
        [SerializeField] private ToggleGroup languagesTG;
        [SerializeField] private RectTransform unofficialLanguagesRT;
        [SerializeField] private GameObject disclaimer;
        #endregion

        #region Methods
        protected override void Start()
        {
            base.Start();
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

            StartCoroutine(SetupRoutine());
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        private IEnumerator SetupRoutine()
        {
            yield return LocalizationSettings.InitializationOperation;

            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                LanguageUI languageUI = Instantiate(languagePrefab, languagesRT);
                languageUI.Setup(locale);

                if (LocalizationManager.Instance.OfficialLanguages.Contains(locale.Identifier.Code))
                {
                    languageUI.transform.SetSiblingIndex(unofficialLanguagesRT.GetSiblingIndex());
                }
                else
                {
                    unofficialLanguagesRT.gameObject.SetActive(true);
                    languageUI.transform.SetAsLastSibling();
                }
            }
        }

        private void OnLocaleChanged(Locale locale)
        {
            disclaimer.SetActive(!LocalizationManager.Instance.OfficialLanguages.Contains(locale.Identifier.Code));
        }
        #endregion
    }
}