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

        private bool isKeyPressed;
        #endregion

        #region Methods
        private void Start()
        {
            float n = (float)baseWidth / Screen.width;
            float s = 1f / scale;
            gridMaterial.mainTextureScale = (n * s) * new Vector2(Screen.width, Screen.height);

            MusicManager.Instance.FadeTo("Fun", 0f, 1f);
        }
        private void Update()
        {
            gridMaterial.mainTextureOffset -= speed * Time.deltaTime * Vector2.one;

            if (LocalizationSettings.InitializationOperation.IsDone)
            {
                string entry = "";
                if (SteamManager.Initialized)
                {
                    if (Input.anyKeyDown && !CanvasUtility.IsPointerOverUI && !isKeyPressed)
                    {
                        LoadingManager.Instance.Load("MainMenu");
                        isKeyPressed = true;

                        logoAnimator.SetTrigger("Hide");
                        enterAudioSource.Play();
                    }
                    entry = "startup_press-any-button";
                }
                else
                {
                    entry = "startup_failed-to-initialize";
                }
                startText.text = LocalizationSettings.StringDatabase.GetLocalizedString("ui-static", entry);
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