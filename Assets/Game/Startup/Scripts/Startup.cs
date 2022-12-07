using TMPro;
using UnityEngine;

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

            MusicManager.Instance.FadeTo("Fun", 0f, 0.5f);
        }
        private void Update()
        {
            gridMaterial.mainTextureOffset -= speed * Time.deltaTime * Vector2.one;

            if (SteamManager.Initialized)
            {
                if (Input.anyKeyDown && !isKeyPressed)
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