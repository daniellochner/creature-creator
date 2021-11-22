using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class NetworkHelpers : MonoBehaviour
    {
        [SerializeField] private NetworkObject[] helpers;
        [SerializeField] private UnityEvent onSpawned;

        private void Start()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                foreach (NetworkObject helper in helpers)
                {
                    Instantiate(helper).Spawn();
                }
            }
            onSpawned.Invoke();
        }
    }
}