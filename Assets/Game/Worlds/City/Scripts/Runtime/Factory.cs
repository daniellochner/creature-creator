using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Factory : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private GameObject[] effects;
        #endregion

        #region Properties
        public NetworkVariable<bool> IsSmoking { get; set; } = new NetworkVariable<bool>(false);
        #endregion

        #region Methods
        private void Start()
        {
            IsSmoking.OnValueChanged += OnSmokingChanged;
            OnSmokingChanged(default, IsSmoking.Value);
        }

        public void Smoke()
        {
            SmokeServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void SmokeServerRpc()
        {
            IsSmoking.Value = true;
        }

        private void OnSmokingChanged(bool oldValue, bool newValue)
        {
            foreach (GameObject effect in effects)
            {
                effect.SetActive(newValue);
            }
        }
        #endregion
    }
}