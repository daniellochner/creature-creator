using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace DanielLochner.Assets
{
    public class MusicManager : MonoBehaviourSingleton<MusicManager>
    {
        #region Fields
        [SerializeField] private SerializableDictionaryBase<string, AudioClip> music;
        [SerializeField] private AudioMixerGroup audioMixer;
        [SerializeField] private bool useTimeScaledPitch;

        private AudioSource[] sources = new AudioSource[2];
        private int current;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();

            sources[0] = gameObject.AddComponent<AudioSource>();
            sources[1] = gameObject.AddComponent<AudioSource>();

            sources[0].playOnAwake = sources[1].playOnAwake = false;
            sources[0].loop = sources[1].loop = true;
            sources[0].outputAudioMixerGroup = sources[1].outputAudioMixerGroup = audioMixer;
        }
        private void Update()
        {
            if (useTimeScaledPitch)
            {
                sources[0].pitch = sources[1].pitch = Time.timeScale;
            }
        }

        public void FadeTo(string m, float time = 1f, float volume = 1f)
        {
            int next = (current + 1) % 2;
            sources[next].clip = (m != null) ? music[m] : null;
            sources[next].volume = volume;
            sources[next].Play();

            StartCoroutine(FadeRoutine(sources[current], time, 0f));
            StartCoroutine(FadeRoutine(sources[next], time, 1f));

            current = next;
        }
        public static IEnumerator FadeRoutine(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }
        #endregion
    }
}