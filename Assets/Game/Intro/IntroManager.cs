using UnityEngine;
using UnityEngine.Video;

namespace DanielLochner.Assets.CreatureCreator
{
    public class IntroManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private VideoPlayer videoPlayer;

        private bool isSkipping;
        #endregion

        #region Methods
        private void Start()
        {
            videoPlayer.loopPointReached += OnVideoEnded;
        }
        private void Update()
        {
            if (Input.anyKeyDown && !isSkipping)
            {
                OnVideoEnded(videoPlayer);
                isSkipping = true;
            }
        }
        private void OnDestroy()
        {
            videoPlayer.targetTexture.Release();
        }

        private void OnVideoEnded(VideoPlayer source)
        {
            LoadingManager.Instance.Load("MainMenu");
        }
        #endregion
    }
}