using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

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
        #endregion

        #region Properties
        private bool ShowIntro
        {
            get => PlayerPrefs.GetInt("SHOW_INTRO", 1) == 1;
            set => PlayerPrefs.SetInt("SHOW_INTRO", value ? 1 : 0);
        }
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            float n = (float)baseWidth / Screen.width;
            float s = 1f / scale;
            gridMaterial.mainTextureScale = (n * s) * new Vector2(Screen.width, Screen.height);

            MusicManager.Instance.FadeTo("Fun", 0f, 1f);

            // Localize
            yield return new WaitUntil(() => LocalizationSettings.InitializationOperation.IsDone);

            // Authenticate
            while (AuthenticationManager.Instance.Status != AuthenticationManager.AuthStatus.Success)
            {
                startText.text = LocalizationUtility.Localize("startup_authenticating");

                yield return new WaitUntil(() => AuthenticationManager.Instance.Status != AuthenticationManager.AuthStatus.Busy);

                if (AuthenticationManager.Instance.Status == AuthenticationManager.AuthStatus.Fail)
                {
                    startText.text = LocalizationUtility.Localize("startup_failed-to-authenticate");
                    yield return new WaitUntil(() => Input.anyKeyDown && !CanvasUtility.IsPointerOverUI);
                    AuthenticationManager.Instance.Authenticate();
                }
            }

            // Start
            startText.text = LocalizationUtility.Localize(SystemUtility.IsDevice(DeviceType.Handheld) ? "startup_tap-to-start" : "startup_press-any-button");
            yield return new WaitUntil(() => Input.anyKeyDown && !CanvasUtility.IsPointerOverUI);

            if (ShowIntro)
            {
                Fader.FadeInOut(1f, delegate
                {
                    LoadingManager.Instance.Load("Intro");
                });
                ShowIntro = false;
            }
            else
            {
                LoadingManager.Instance.Load("MainMenu");
            }
            logoAnimator.SetTrigger("Hide");
            enterAudioSource.Play();
        }

        private void Update()
        {
            gridMaterial.mainTextureOffset -= speed * Time.deltaTime * Vector2.one;
        }
        private void OnDestroy()
        {
            gridMaterial.mainTextureScale = Vector2.one;
            gridMaterial.mainTextureOffset = Vector2.zero;
        }
        #endregion
    }
}