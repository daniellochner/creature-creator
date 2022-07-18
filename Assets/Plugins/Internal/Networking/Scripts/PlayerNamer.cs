using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets
{
    public class PlayerNamer : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private GameObject namePrefab;
        [SerializeField] protected float height;

        protected GameObject nameGO;
        #endregion

        #region Methods
        private void OnEnable()
        {
            if (nameGO != null)
            {
                nameGO.SetActive(true);
            }
        }
        private void OnDisable()
        {
            if (nameGO != null)
            {
                nameGO.SetActive(false);
            }
        }

        public virtual void Setup()
        {
            nameGO = Instantiate(namePrefab, transform.position + transform.up * height, transform.rotation, transform);
            SetNameServerRpc(OwnerClientId, NetworkManager.Singleton.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetNameServerRpc(ulong clientId, ulong sendToClientId)
        {
            SetNameClientRpc(NetworkHostManager.Instance.Players[clientId].username, NetworkUtils.SendTo(sendToClientId));
        }
        [ClientRpc]
        private void SetNameClientRpc(string name, ClientRpcParams clientRpc = default)
        {
            nameGO.GetComponentInChildren<TextMeshProUGUI>().text = name;
            nameGO.GetComponent<LookAtConstraint>().AddSource(new ConstraintSource() { sourceTransform = Camera.main.transform, weight = 1f });
        }
        #endregion
    }
}