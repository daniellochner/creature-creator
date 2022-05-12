// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(AudioSource))]
    public class CreatureEffector : MonoBehaviour
    {
        #region Fields
        [SerializeField] private SerializableDictionaryBase<string, AudioClip> soundFX;
        [SerializeField] private SerializableDictionaryBase<string, AudioClip> particleFX;

        private AudioSource audioSource;
        #endregion

        #region Properties
        public Action<string> OnPlaySound { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(string sound, float pitch = 1f, float volume = 1f)
        {
            float tmpVolume = audioSource.volume;
            float tmpPitch  = audioSource.pitch;

            audioSource.volume = volume;
            audioSource.pitch  = pitch;
            audioSource.PlayOneShot(soundFX[sound]);
            OnPlaySound?.Invoke(sound);

            audioSource.volume = tmpVolume;
            audioSource.pitch  = tmpPitch;
        }
        #endregion
    }
}