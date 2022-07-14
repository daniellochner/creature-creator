using UnityEngine;

namespace Unity.Netcode
{
    public class NetworkVariant : MonoBehaviour
    {
        [SerializeField] private GameObject local;
        [SerializeField] private GameObject remote;

        public void Swap()
        {
            Transform localP = local.transform.parent;
            int localSI = local.transform.GetSiblingIndex();

            Transform remoteP = remote.transform.parent;
            int remoteSI = remote.transform.GetSiblingIndex();


            local.transform.parent = remoteP;
            local.transform.SetSiblingIndex(remoteSI);

            remote.transform.parent = localP;
            remote.transform.SetSiblingIndex(localSI);
        }
    }
}