using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class MovingPlatform : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private string playerTag;
        #endregion

        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                SetParentServerRpc(new NetworkObjectReference(other.gameObject), true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                SetParentServerRpc(new NetworkObjectReference(other.gameObject), false);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetParentServerRpc(NetworkObjectReference networkObjectRef, bool isParented)
        {
            if (networkObjectRef.TryGet(out NetworkObject networkObject))
            {
                networkObject.transform.parent = isParented ? transform : null;
            }
        }
        #endregion
    }
}