using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class NetworkHelpers : MonoBehaviour
    {
        #region Fields
        [SerializeField] private NetworkObject[] helpers;
        [SerializeField] private UnityEvent onSpawned;
        #endregion

        #region Methods
        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                foreach (NetworkObject helper in helpers)
                {
                    Instantiate(helper).Spawn();
                }
            }
            onSpawned.Invoke();
        }
        #endregion
    }
}