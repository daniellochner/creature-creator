using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHunger))]
    public class NetworkCreatureHunger : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private NetworkVariable<float> hunger;
        #endregion

        #region Properties
        private CreatureHunger Hunger { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Hunger = GetComponent<CreatureHunger>();
        }

        private void Start()
        {
            if (IsOwner)
            {
                Hunger.OnHungerChanged += SetHungerServerRpc;
            }
            else
            {
                hunger.OnValueChanged += UpdateHunger;
            }
        }

        [ServerRpc]
        private void SetHungerServerRpc(float h)
        {
            hunger.Value = h;
        }
        private void UpdateHunger(float oldH, float newH)
        {
            Hunger.Hunger = newH;
        }
        #endregion
    }
}