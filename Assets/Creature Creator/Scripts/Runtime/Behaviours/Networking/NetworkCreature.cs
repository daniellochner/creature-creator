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
        public override void OnNetworkSpawn()
        {
            Setup(IsOwner);
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
                SourceCreature.Health.OnHealthChanged += (float health) => SetInfo(health, SetHealthServerRpc, SetHealth);
                SourceCreature.Energy.OnEnergyChanged += (float energy) => SetInfo(energy, SetEnergyServerRpc, SetEnergy);
                SourceCreature.Age.OnAgeChanged += (int age) => SetInfo(age, SetAgeServerRpc, SetAge);

                SourceCreature.Informer.OnRespawn += delegate
                {
                    SourceCreature.Health.HealthPercentage = 1f;
                    SourceCreature.Energy.Energy = 1f;
                    SourceCreature.Age.Start();
                };
            }

            Health.OnValueChanged += InformHealth;
            Energy.OnValueChanged += InformEnergy;
            Age.OnValueChanged += InformAge;
        }

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
        private void SetInfo<T>(T value, Action<T> nF, Action<T> nnF)
        {
            if (SetupGame.IsNetworkedGame)
            {
                nF.Invoke(value);
            }
            else
            {
                nnF.Invoke(value);
            }
        }

        private void SetHealth(float health)
        {
            Health.Value = Mathf.InverseLerp(SourceCreature.Health.MinMaxHealth.min, SourceCreature.Health.MinMaxHealth.max, health);
        }
        private void SetEnergy(float energy)
        {
            Energy.Value = energy;
        }
        private void SetAge(int age)
        {
            Age.Value = age;
        }

        [ServerRpc]
        private void SetHealthServerRpc(float health)
        {
            SetHealth(health);
        }
        [ServerRpc]
        private void SetEnergyServerRpc(float energy)
        {
            SetEnergy(energy);
        }
        [ServerRpc]
        private void SetAgeServerRpc(int age)
        {
            SetAge(age);
        }
        
        private void InformHealth(float oldHealth, float newHealth)
        {
            TargetCreature.Informer.Information.Health = newHealth;
        }
        private void InformEnergy(float oldEnergy, float newEnergy)
        {
            TargetCreature.Informer.Information.Energy = newEnergy;
        }
        private void InformAge(int oldAge, int newAge)
        {
            TargetCreature.Informer.Information.Age = newAge;
        }
        #endregion
        #endregion
    }
}