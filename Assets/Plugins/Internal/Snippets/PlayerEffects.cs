// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using RotaryHeart.Lib.SerializableDictionary;
using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerEffects : NetworkBehaviour
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
        public void PlaySound(string soundId, float volume)
        {
            PlaySoundSelf(soundId, volume);
            PlaySoundServerRpc(soundId, volume, NetworkManager.Singleton.LocalClientId);
        }
        [ServerRpc(RequireOwnership = false)]
        public void PlaySoundServerRpc(string soundId, float volume, ulong clientId)
        {
            PlaySoundClientRpc(soundId, volume, NetworkUtils.DontSendTo(clientId));
        }
        [ClientRpc]
        public void PlaySoundClientRpc(string soundId, float volume, ClientRpcParams clientRpcParams)
        {
            PlaySoundSelf(soundId, volume);
        }
        public void PlaySoundSelf(string soundId, float volume)
        {
            if (soundFX.TryGetValue(soundId, out var clip))
            {
                audioSource.PlayOneShot(clip, volume);
                OnPlaySound?.Invoke(soundId);
            }
        }

        public void StopSounds()
        {
            StopSoundsSelf();
            StopSoundsServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        [ServerRpc(RequireOwnership = false)]
        public void StopSoundsServerRpc(ulong clientId)
        {
            StopSoundsClientRpc(NetworkUtils.DontSendTo(clientId));
        }
        [ClientRpc]
        public void StopSoundsClientRpc(ClientRpcParams clientRpcParams)
        {
            StopSoundsSelf();
        }
        public void StopSoundsSelf()
        {
            audioSource.Stop();
        }

        public void SpawnParticle(string particleId, Vector3 position)
        {
            SpawnParticleSelf(particleId, position);
            SpawnParticleServerRpc(particleId, position, NetworkManager.Singleton.LocalClientId);
        }
        [ServerRpc(RequireOwnership = false)]
        public void SpawnParticleServerRpc(string particleId, Vector3 position, ulong clientId)
        {
            SpawnParticleClientRpc(particleId, position, NetworkUtils.DontSendTo(clientId));
        }
        [ClientRpc]
        public void SpawnParticleClientRpc(string particleId, Vector3 position, ClientRpcParams clientRpcParams)
        {
            SpawnParticleSelf(particleId, position);
        }
        public void SpawnParticleSelf(string particleId, Vector3 position)
        {
            if (particleFX.TryGetValue(particleId, out var particle))
            {
                Instantiate(particle, position, Quaternion.identity, Dynamic.Transform);
                OnSpawnParticle?.Invoke(particleId, position);
            }
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