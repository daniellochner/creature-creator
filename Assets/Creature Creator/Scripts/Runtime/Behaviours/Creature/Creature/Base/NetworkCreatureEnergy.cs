using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureEnergy))]
    public class NetworkCreatureEnergy : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private NetworkVariable<float> energy;
        #endregion

        #region Properties
        private CreatureEnergy Energy { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Energy = GetComponent<CreatureEnergy>();
        }

        private void Start()
        {
            if (IsOwner)
            {
                Energy.OnEnergyChanged += SetEnergyServerRpc;
            }
            else
            {
                energy.OnValueChanged += UpdateEnergy;
            }
        }

        [ServerRpc]
        private void SetEnergyServerRpc(float e)
        {
            energy.Value = e;
        }
        private void UpdateEnergy(float oldE, float newE)
        {
            Energy.Energy = newE;
        }
        #endregion
    }
}