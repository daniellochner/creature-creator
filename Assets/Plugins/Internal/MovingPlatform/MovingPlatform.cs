using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class MovingPlatform : NetworkBehaviour
    {
        [SerializeField] private string playerTag;
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
    }
}