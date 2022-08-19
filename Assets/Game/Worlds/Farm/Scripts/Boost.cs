using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Boost : NetworkBehaviour
    {
        [SerializeField] private float boostSpeed;
        [SerializeField] private float boostTime;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player/Local"))
            {
                BoostServerRpc(NetworkManager.Singleton.LocalClientId);
                audioSource.Play();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void BoostServerRpc(ulong clientId)
        {
            CreatureSpeedup s = NetworkManager.SpawnManager.GetPlayerNetworkObject(clientId).GetComponent<CreatureSpeedup>();
            s.SpeedUp(boostSpeed, boostTime);
        }
    }
}