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

        #region Properties
        public string Name { get; private set; }
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

        public void Setup()
        {
            SetNameServerRpc(OwnerClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetNameServerRpc(ulong clientId)
        {
            SetNameClientRpc(NetworkHostManager.Instance.Players[clientId].username);
        }

        [ClientRpc]
        private void SetNameClientRpc(string name)
        {
            Name = name;

            if (!IsOwner)
            {
                nameGO = Instantiate(namePrefab, transform.position + transform.up * height, transform.rotation, transform);

                nameGO.GetComponentInChildren<TextMeshProUGUI>().text = name;
                nameGO.GetComponent<LookAtConstraint>().AddSource(new ConstraintSource() { sourceTransform = Camera.main.transform, weight = 1f });
            }
        }
        #endregion
    }
}