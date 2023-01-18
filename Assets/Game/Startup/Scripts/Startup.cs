using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Startup : MonoBehaviour
    {
        #region Fields
        [SerializeField] private int baseWidth;
        [SerializeField] private float scale;
        [SerializeField] private float speed;
        [SerializeField] private Material gridMaterial;
        [SerializeField] private Animator logoAnimator;
        [SerializeField] private AudioSource enterAudioSource;
        [SerializeField] private TextMeshProUGUI startText;
        [SerializeField] private Menu languageMenu;
        [SerializeField] private Toggle[] languageToggles;

        private bool isKeyPressed;
        #endregion

        #region Properties
        public bool IsLanguageSetup
        {
            get => PlayerPrefs.GetInt("IS_LANGUAGE_SETUP") == 1;
            set => PlayerPrefs.SetInt("IS_LANGUAGE_SETUP", value ? 1 : 0);
        }
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            float n = (float)baseWidth / Screen.width;
            float s = 1f / scale;
            gridMaterial.mainTextureScale = (n * s) * new Vector2(Screen.width, Screen.height);

            MusicManager.Instance.FadeTo("Fun", 0f, 1f);

            if (!IsLanguageSetup)
            {
                yield return new WaitForSeconds(1f);

                for (int i = 0; i < languageToggles.Length; i++)
                {
                    int index = i;
                    languageToggles[i].onValueChanged.AddListener(delegate (bool isOn)
                    {
                        if (isOn)
                        {
                            SettingsManager.Instance.SetLanguage((Settings.LanguageType)index);
                        }
                    });
                }
                SettingsManager.Instance.SetLanguage(Settings.LanguageType.English);

                languageMenu.Open();
            }
        }
        private void Update()
        {
            gridMaterial.mainTextureOffset -= speed * Time.deltaTime * Vector2.one;

            if (SteamManager.Initialized)
            {
                if (IsLanguageSetup && !languageMenu.IsOpen && Input.anyKeyDown && !isKeyPressed)
                {
                    LoadingManager.Instance.Load("MainMenu");
                    isKeyPressed = true;

                    logoAnimator.SetTrigger("Hide");
                    enterAudioSource.Play();
                }
                startText.text = "Press any button to start.";
            }
            else
            {
                startText.text = "Steam failed to initialize.";
            }
        }
        private void OnDestroy()
        {
            gridMaterial.mainTextureScale = Vector2.one;
            gridMaterial.mainTextureOffset = Vector2.zero;
        }
        #endregion
    }
}