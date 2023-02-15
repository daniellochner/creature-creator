using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace DanielLochner.Assets
{
    public abstract class SoundsManager<T> : MonoBehaviourSingleton<T> where T : SoundsManager<T>
    {
        #region Fields
        [SerializeField] private SerializableDictionaryBase<string, AudioClip> sounds;
        [SerializeField] private AudioMixerGroup audioMixer;
        [SerializeField] private bool useTimeScaledPitch;
        [SerializeField] private string startSound;

        private Coroutine fadeFromCoroutine, fadeToCoroutine;
        private AudioSource[] sources = new AudioSource[2];
        private int current;
        #endregion

        #region Properties
        public string CurrentSound
        {
            get; set;
        }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();

            sources[0] = gameObject.AddComponent<AudioSource>();
            sources[1] = gameObject.AddComponent<AudioSource>();

            sources[0].playOnAwake = sources[1].playOnAwake = false;
            sources[0].loop = sources[1].loop = true;
            sources[0].dopplerLevel = sources[1].dopplerLevel = 0f;
            sources[0].outputAudioMixerGroup = sources[1].outputAudioMixerGroup = audioMixer;
        }
        private void Start()
        {
            if (startSound != "")
            {
                FadeTo(startSound);
            }
        }
        private void Update()
        {
            if (useTimeScaledPitch)
            {
                sources[0].pitch = sources[1].pitch = Time.timeScale;
            }
        }

        public void FadeTo(string sound)
        {
            FadeTo(sound, 1f, 1f);
        }
        public void FadeTo(string sound, float time, float volume)
        {
            if (sources[0] == null || sources[current] == null) return;
            CurrentSound = sound;

            int next = (current + 1) % 2;
            sources[next].clip = (sound != null) ? sounds[sound] : null;
            sources[next].volume = volume;
            sources[next].Play();

            if (fadeFromCoroutine != null)
            {
                StopCoroutine(fadeFromCoroutine);
            }
            fadeFromCoroutine = StartCoroutine(sources[current].FadeRoutine(time, 0f));

            if (fadeToCoroutine != null)
            {
                StopCoroutine(fadeToCoroutine);
            }
            fadeToCoroutine = StartCoroutine(sources[next].FadeRoutine(time, 1f));

            current = next;
        }
        #endregion
    }
}