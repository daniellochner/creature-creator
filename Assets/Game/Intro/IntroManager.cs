using UnityEngine;
using UnityEngine.Video;

namespace DanielLochner.Assets.CreatureCreator
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;

        private void Start()
        {
            videoPlayer.loopPointReached += OnVideoEnded;
        }

        private void OnVideoEnded(VideoPlayer source)
        {
            LoadingManager.Instance.Load("MainMenu");
        }
    }
}