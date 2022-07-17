using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class NetworkSpawner : MonoBehaviour
    {
        [SerializeField] private NetworkObject playerLPrefab;
        [SerializeField] private NetworkObject playerRPrefab;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private NetworkObject[] helpers;
        [SerializeField] private UnityEvent onSpawned;

        public Transform RandomSpawnPoint
        {
            get => spawnPoints[Random.Range(0, spawnPoints.Length)];
        }

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                foreach (NetworkObject helper in helpers)
                {
                    Instantiate(helper).Spawn();
                }
                //NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

                if (NetworkManager.Singleton.IsHost)
                {
                    Instantiate(, RandomSpawnPoint.position, RandomSpawnPoint.rotation).SpawnAsPlayerObject(clientId);
                }



                onSpawned.Invoke();
            }
        }
        private void OnDestroy()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                //NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            }
        }

        //private void Connect(byte[] data, ulong clientId, NetworkManager.ConnectionApprovedDelegate connectionApproved)
        //{

        //}


        //private void OnClientConnected(ulong clientId)
        //{
        //    Instantiate((NetworkManager.Singleton.LocalClientId == clientId) ? playerLPrefab : playerRPrefab, RandomSpawnPoint.position, RandomSpawnPoint.rotation).SpawnAsPlayerObject(clientId);
        //}
    }
}