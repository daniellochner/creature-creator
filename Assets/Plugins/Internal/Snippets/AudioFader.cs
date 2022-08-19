using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioFader : MonoBehaviour
    {
        [SerializeField] private float time;
        private Coroutine fadeCoroutine;

        private AudioSource audioSource;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void FadeTo(float volume)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(audioSource.FadeRoutine(time, volume));
        }
    }
}