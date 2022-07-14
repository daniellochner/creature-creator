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
        [SerializeField] private SerializableDictionaryBase<string, GameObject> particleFX;

        private AudioSource audioSource;
        #endregion

        #region Properties
        public SerializableDictionaryBase<string, AudioClip> SoundFX => soundFX;
        public SerializableDictionaryBase<string, GameObject> ParticleFX => particleFX;

        public Action<string> OnPlaySound { get; set; }
        public Action<string, Vector3> OnSpawnParticle { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(Sound[] sounds)
        {
            if (sounds.Length > 0)
            {
                Sound sound = sounds[UnityEngine.Random.Range(0, sounds.Length)];
                PlaySound(sound.name, sound.volume);
            }
        }
        public void PlaySound(string sound, float volume = 1f)
        {
            if (!soundFX.ContainsKey(sound)) return;

            audioSource.PlayOneShot(soundFX[sound], volume);
            OnPlaySound?.Invoke(sound);
        }

        public void SpawnParticle(string particle, Vector3 position)
        {
            if (!particleFX.ContainsKey(particle)) return;

            Instantiate(particleFX[particle], position, Quaternion.identity, Dynamic.Transform);
            OnSpawnParticle?.Invoke(particle, position);
        }
        #endregion

        #region Inner Classes
        [Serializable]
        public class Sound
        {
            public string name;
            [Range(0f, 1f)] public float volume = 1f;
        }
        #endregion
    }
}