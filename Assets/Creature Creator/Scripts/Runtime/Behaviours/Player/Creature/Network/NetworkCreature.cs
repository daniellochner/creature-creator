// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(NetworkObject), typeof(NetworkTransform))]
    public class NetworkCreature : NetworkBehaviour
    {
        #region Fields
        [SerializeField] protected CreatureSource source;
        [SerializeField] protected CreatureTargetBase target;

        protected NetworkObject networkObject;
        protected NetworkTransform networkTransform;
        #endregion

        #region Properties
        public NetworkVariable<float> Health { get; private set; } = new NetworkVariable<float>();
        public NetworkVariable<float> Energy { get; private set; } = new NetworkVariable<float>();
        public NetworkVariable<int> Age { get; private set; } = new NetworkVariable<int>();
        public NetworkVariable<bool> IsHidden { get; private set; } = new NetworkVariable<bool>();

        public CreatureSource SourceCreature => source;
        public virtual CreatureTargetBase TargetCreature => target;

        public bool IsOWNER { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            networkObject = GetComponent<NetworkObject>();
            networkTransform = GetComponent<NetworkTransform>();
        }
        public virtual void Setup(bool isOwner)
        {
            IsOWNER = isOwner;

            source.gameObject.SetActive(isOwner);
            target.gameObject.SetActive(!isOwner);

            if (isOwner)
            {
                SourceCreature.Health.OnHealthChanged += SetHealthServerRpc;
                SourceCreature.Energy.OnEnergyChanged += SetEnergyServerRpc;
                SourceCreature.Age.OnAgeChanged += SetAgeServerRpc;

                SourceCreature.Animator.OnSetTrigger += SetTriggerServerRpc;
                SourceCreature.Animator.OnSetBool += SetBoolServerRpc;
            }
            else
            {
                Health.OnValueChanged += UpdateHealth;
                Energy.OnValueChanged += UpdateEnergy;
                Age.OnValueChanged += UpdateAge;
            }
        }

        #region Animations
        [ServerRpc]
        private void SetTriggerServerRpc(string param)
        {
            SetTriggerClientRpc(param);
        }
        [ClientRpc]
        private void SetTriggerClientRpc(string param)
        {
            if (!IsOWNER)
            {
                TargetCreature.Animator.Animator.SetTrigger(param);
            }
        }

        [ServerRpc]
        private void SetBoolServerRpc(string param, bool value)
        {
            SetBoolClientRpc(param, value);
        }
        [ClientRpc]
        private void SetBoolClientRpc(string param, bool value)
        {
            if (!IsOWNER)
            {
                TargetCreature.Animator.Animator.SetBool(param, value);
            }
        }
        #endregion

        #region Hide
        [ServerRpc]
        private void HideServerRpc()
        {
            HideClientRpc();
            IsHidden.Value = true;
        }
        [ClientRpc]
        private void HideClientRpc()
        {
            if (!IsOWNER)
            {
                TargetCreature.gameObject.SetActive(false);
            }
        }
        public void Hide()
        {
            HideServerRpc();
        }
        #endregion

        #region Reconstruct And Show
        [ServerRpc]
        protected void ReconstructAndShowServerRpc(string creatureData)
        {
            ReconstructAndShowClientRpc(creatureData);
            IsHidden.Value = false;
        }
        [ClientRpc]
        protected void ReconstructAndShowClientRpc(string creatureData, ClientRpcParams clientRpcParams = default)
        {
            CreatureData data = JsonUtility.FromJson<CreatureData>(creatureData);
            if (!IsOWNER)
            {
                TargetCreature.Constructor.Demolish();
                TargetCreature.gameObject.SetActive(true);
                TargetCreature.Constructor.Construct(data);
            }
            NetworkCreaturesMenu.Instance.SetName(OwnerClientId, data.Name);
        }
        public void ReconstructAndShow()
        {
            ReconstructAndShowServerRpc(JsonUtility.ToJson(SourceCreature.Constructor.Data));
        }
        #endregion

        #region Respawn
        [ServerRpc]
        private void RespawnServerRpc()
        {
            RespawnClientRpc();
        }
        [ClientRpc]
        private void RespawnClientRpc()
        {
            if (!IsOWNER)
            {
                Destroy(TargetCreature.Killer.Corpse);
            }
        }
        #endregion
        

        #region Information
        [ServerRpc]
        private void SetHealthServerRpc(float health)
        {
            Health.Value = health;
        }
        [ServerRpc]
        private void SetEnergyServerRpc(float energy)
        {
            Energy.Value = energy;
        }
        [ServerRpc]
        private void SetAgeServerRpc(int age)
        {
            Age.Value = age;
        }

        private void UpdateHealth(float oldHealth, float newHealth)
        {
            TargetCreature.Health.Health = newHealth;
        }
        private void UpdateEnergy(float oldEnergy, float newEnergy)
        {
            TargetCreature.Energy.Energy = newEnergy;
        }
        private void UpdateAge(int oldAge, int newAge)
        {
            TargetCreature.Age.Age = newAge;
        }
        #endregion
        #endregion
    }
}