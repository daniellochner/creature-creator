using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class MovingPlatform : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private string playerTag;
        #endregion

        #region Methods
        public void OnTriggerEnter(Collider other)
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
        private void SetParentServerRpc(NetworkObjectReference networkObjectRef, bool isParented)
        {
            if (networkObjectRef.TryGet(out NetworkObject networkObject))
            {
                networkObject.transform.parent = isParented ? transform : null;
            }
        }
        #endregion
    }
}