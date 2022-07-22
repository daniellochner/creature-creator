using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAge))]
    public class NetworkCreatureAge : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private NetworkVariable<int> age;
        #endregion

        #region Properties
        private CreatureAge Age { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Age = GetComponent<CreatureAge>();
        }

        private void Start()
        {
            if (IsOwner)
            {
                Age.OnAgeChanged += SetAgeServerRpc;
            }
            else
            {
                age.OnValueChanged += UpdateAge;
            }
        }

        [ServerRpc]
        private void SetAgeServerRpc(int a)
        {
            age.Value = a;
        }
        private void UpdateAge(int oldA, int newA)
        {
            Age.Age = newA;
        }
        #endregion
    }
}