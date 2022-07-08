using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Startup : MonoBehaviour
    {
        #region Fields
        [SerializeField] private string sceneToLoad;
        [SerializeField] private bool waitForKeyPress;
        [Space]
        [SerializeField] private Animator logoAnimator;
        [SerializeField] private Material gridMat;
        [SerializeField] private float scale;
        [SerializeField] private float speed;
        #endregion

        #region Methods
        private void Start()
        {
            gridMat.mainTextureScale = (1f / scale) * new Vector2(Screen.width, Screen.height);

            MusicManager.Instance.FadeTo("Fun", 0f, 0.5f);
        }
        private void Update()
        {
            gridMat.mainTextureOffset -= speed * Time.deltaTime * Vector2.one;

            if (waitForKeyPress && Input.anyKeyDown)
            {
                Fader.Fade(true, 1f, delegate
                {
                    SceneManager.LoadScene(sceneToLoad);
                    Fader.Fade(false, 1f);
                });
                waitForKeyPress = false;

                MusicManager.Instance.FadeTo(null);
                logoAnimator.SetTrigger("Hide");
            }
        }
        #endregion
    }
}