using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayRandomSound : MonoBehaviour
    {
        [SerializeField] private bool playOnAwake;
        [SerializeField] private AudioClip[] sounds;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private MinMax minMaxPitch = new MinMax(1f, 1f);

        private void Awake()
        {
            Play();
        }
        public void Play()
        {
            audioSource.pitch = minMaxPitch.Random;
            AudioClip sound = sounds[UnityEngine.Random.Range(0, sounds.Length)];
            audioSource.PlayOneShot(sound);
        }
    }
}