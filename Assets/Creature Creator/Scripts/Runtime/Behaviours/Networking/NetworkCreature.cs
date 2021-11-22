// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(NetworkObject), typeof(NetworkTransform))]
    public class NetworkCreature : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Player player;
        [SerializeField] private CreatureNonPlayer nonPlayerCreature;

        private NetworkObject networkObject;
        private NetworkTransform networkTransform;
        #endregion

        #region Properties
        public NetworkVariable<float> Health { get; private set; } = new NetworkVariable<float>();
        public NetworkVariable<float> Energy { get; private set; } = new NetworkVariable<float>();
        public NetworkVariable<int> Age { get; private set; } = new NetworkVariable<int>();
        public NetworkVariable<bool> IsHidden { get; private set; } = new NetworkVariable<bool>();

        public Player Player => player;
        public CreaturePlayer PlayerCreature => player.Creature;
        public CreatureNonPlayer NonPlayerCreature => nonPlayerCreature;
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        public override void OnNetworkSpawn()
        {
            Setup();
        }

        private void Initialize()
        {
            networkObject = GetComponent<NetworkObject>();
            networkTransform = GetComponent<NetworkTransform>();
        }
        private void Setup()
        {
            if (IsOwner)
            {
                player.Creature.Health.OnRespawn += RespawnServerRpc;
                player.Creature.Health.OnHealthChanged += SetHealthServerRpc;
                player.Creature.Energy.OnEnergyChanged += SetEnergyServerRpc;
                player.Creature.Age.OnAgeChanged += SetAgeServerRpc;

                if (player.Creature.Mover.RequestToMove)
                {
                    player.Creature.Mover.OnRotateRequest += RequestToRotate;
                    player.Creature.Mover.OnMoveRequest += RequestToMove;
                }
            }
            else
            {
                Health.OnValueChanged += delegate (float oldHealth, float newHealth)
                {
                    NonPlayerCreature.Informer.Information.Health = newHealth;

                    if (newHealth <= 0)
                    {
                        NonPlayerCreature.Killer.Kill();
                    }
                };
                Energy.OnValueChanged += delegate (float oldEnergy, float newEnergy)
                {
                    NonPlayerCreature.Informer.Information.Energy = newEnergy;
                };
                Age.OnValueChanged += delegate (int oldAge, int newAge)
                {
                    NonPlayerCreature.Informer.Information.Age = newAge;
                };
            }
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
            if (!IsOwner)
            {
                NonPlayerCreature.gameObject.SetActive(false);
            }
        }
        public void Hide()
        {
            HideServerRpc();
            //networkTransform.Capture = false;
        }
        #endregion

        #region Load Player
        [ClientRpc]
        public void LoadPlayerClientRpc(PlayerData playerData, string creatureData, ClientRpcParams clientRpcParams)
        {
            NetworkCreaturesMenu.Instance.AddPlayer(playerData);
            ReconstructAndShowClientRpc(creatureData);
        }
        #endregion

        #region Move
        [ServerRpc]
        private void RequestToMoveServerRpc(Vector3 direction, ulong clientId)
        {
            RequestToMoveClientRpc(direction, NetworkUtils.SendTo(clientId));
        }
        [ClientRpc]
        private void RequestToMoveClientRpc(Vector3 direction, ClientRpcParams clientRpcParams)
        {
            player.Creature.Mover.Move(direction);
        }
        private void RequestToMove(Vector3 direction)
        {
            RequestToMoveServerRpc(direction, OwnerClientId);
        }
        #endregion

        #region Reconstruct And Show
        [ServerRpc]
        private void ReconstructAndShowServerRpc(string creatureData)
        {
            ReconstructAndShowClientRpc(creatureData);
            IsHidden.Value = false;
        }
        [ClientRpc]
        private void ReconstructAndShowClientRpc(string creatureData, ClientRpcParams clientRpcParams = default)
        {
            CreatureData data = JsonUtility.FromJson<CreatureData>(creatureData);
            if (!IsOwner)
            {
                NonPlayerCreature.Constructor.Demolish();
                NonPlayerCreature.gameObject.SetActive(true);
                NonPlayerCreature.Constructor.Construct(data);
            }
            NetworkCreaturesMenu.Instance.SetName(OwnerClientId, data.Name);
        }
        public void ReconstructAndShow()
        {
            ReconstructAndShowServerRpc(JsonUtility.ToJson(PlayerCreature.Constructor.Data));
            //networkTransform.Capture = true;
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
                Destroy(NonPlayerCreature.Killer.Corpse);
            }
        }
        #endregion

        #region Rotate
        [ServerRpc]
        private void RequestToRotateServerRpc(Quaternion rotation, ulong clientId)
        {
            RequestToRotateClientRpc(rotation, NetworkUtils.SendTo(clientId));
        }
        [ClientRpc]
        private void RequestToRotateClientRpc(Quaternion rotation, ClientRpcParams clientRpcParams)
        {
            player.Creature.Mover.Rotate(rotation);
        }
        private void RequestToRotate(Quaternion rotation)
        {
            RequestToRotateServerRpc(rotation, OwnerClientId);
        }
        #endregion

        #region Set Information
        [ServerRpc]
        private void SetHealthServerRpc(float health)
        {
            Health.Value = Mathf.InverseLerp(player.Creature.Health.MinMaxHealth.min, player.Creature.Health.MinMaxHealth.max, health);
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
        #endregion
        #endregion
    }
}