// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public CreatureSource Source => source;
        public CreatureTargetBase Target => target;
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        private void Start()
        {
            Setup();
        }

        private void Initialize()
        {
            networkObject = GetComponent<NetworkObject>();
            networkTransform = GetComponent<NetworkTransform>();
        }
        public virtual void Setup()
        {
            if (NetworkConnectionManager.IsConnected)
            {
                if (IsOwner)
                {
                    source.Health.OnHealthChanged += SetHealthServerRpc;
                    source.Energy.OnEnergyChanged += SetEnergyServerRpc;
                    source.Age.OnAgeChanged += SetAgeServerRpc;

                    source.Animator.OnSetTrigger += SetTriggerServerRpc;
                    source.Animator.OnSetBool += SetBoolServerRpc;
                }
                else
                {
                    Health.OnValueChanged += UpdateHealth;
                    Energy.OnValueChanged += UpdateEnergy;
                    Age.OnValueChanged += UpdateAge;
                }
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
            if (!IsOwner)
            {
                target.Animator.Animator.SetTrigger(param);
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
            if (!IsOwner)
            {
                target.Animator.Animator.SetBool(param, value);
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
            if (!IsOwner)
            {
                target.gameObject.SetActive(false);
            }
        }
        public void Hide()
        {
            HideServerRpc();
        }
        #endregion

        #region Reconstruct And Show
        [ServerRpc]
        public void ReconstructAndShowServerRpc(string creatureData)
        {
            ReconstructAndShowClientRpc(creatureData);
            IsHidden.Value = false;
        }
        [ClientRpc]
        public void ReconstructAndShowClientRpc(string creatureData, ClientRpcParams clientRpcParams = default)
        {
            CreatureData data = JsonUtility.FromJson<CreatureData>(creatureData);
            if (!IsOwner)
            {
                target.Constructor.Demolish();
                target.gameObject.SetActive(true);
                target.Constructor.Construct(data);
            }
            NetworkCreaturesMenu.Instance.SetName(OwnerClientId, data.Name);
        }
        public void ReconstructAndShow()
        {
            ReconstructAndShowServerRpc(JsonUtility.ToJson(source.Constructor.Data));
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
            if (!IsOwner)
            {
                Destroy(target.Killer.Corpse);
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
            target.Health.Health = newHealth;
        }
        private void UpdateEnergy(float oldEnergy, float newEnergy)
        {
            target.Energy.Energy = newEnergy;
        }
        private void UpdateAge(int oldAge, int newAge)
        {
            target.Age.Age = newAge;
        }
        #endregion
        #endregion
    }
}