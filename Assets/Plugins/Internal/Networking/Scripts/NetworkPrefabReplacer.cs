// Adapted from: https://gist.github.com/JesseOlmer/8b0c3d541f76634a75bc853eca9798b9

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class NetworkPrefabReplacer : MonoBehaviour, INetworkPrefabInstanceHandler
    {
        #region Fields
        [SerializeField] private NetworkObject localPrefab;
        [SerializeField] private NetworkObject remotePrefab;
        #endregion

        #region Methods
        public void Start()
        {
            NetworkManager.Singleton.PrefabHandler.AddHandler(remotePrefab, this);
        }
        public void OnDestroy()
        {
            NetworkManager.Singleton.PrefabHandler.RemoveHandler(localPrefab);
        }

        public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
        {
            NetworkObject obj = null;
            if (ownerClientId == NetworkManager.Singleton.LocalClientId)
            {
                obj = Instantiate(localPrefab);
            }
            else
            {
                obj = Instantiate(remotePrefab);
            }
            obj.transform.SetPositionAndRotation(position, rotation);

            return obj;
        }

        public void Destroy(NetworkObject networkObject)
        {
            Destroy(networkObject.gameObject);
        }
        #endregion
    }
}