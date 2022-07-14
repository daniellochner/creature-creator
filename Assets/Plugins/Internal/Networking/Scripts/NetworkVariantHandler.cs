using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class NetworkVariantHandler : MonoBehaviour
    {
        private void Start()
        {
            NetworkManager.Singleton.OnPreSpawn += HandleVariants;
        }
        public void HandleVariants(NetworkObject networkObject)
        {
            if (!networkObject.IsOwner) return;

            NetworkVariant[] varients = networkObject.GetComponentsInChildren<NetworkVariant>();
            foreach (NetworkVariant varient in varients)
            {
                Transform localParent = varient.Local.transform.parent;
                int localSiblingIndex = varient.Local.transform.GetSiblingIndex();

                Transform remoteParent = varient.Remote.transform.parent;
                int remoteSiblingIndex = varient.Remote.transform.GetSiblingIndex();


                varient.Local.transform.parent = remoteParent;
                varient.Local.transform.SetSiblingIndex(remoteSiblingIndex);

                varient.Remote.transform.parent = localParent;
                varient.Remote.transform.SetSiblingIndex(localSiblingIndex);
            }
        }
    }
}