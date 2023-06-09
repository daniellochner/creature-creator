using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAge))]
    [RequireComponent(typeof(CreatureHealth))]
    [RequireComponent(typeof(CreatureHunger))]
    public class CreatureSpawner : NetworkBehaviour
    {
        public CreatureAge Age { get; private set; }
        public CreatureHunger Hunger { get; private set; }
        public CreatureHealth Health { get; private set; }
        
        public Action OnSpawn { get; set; }
        public Action OnDespawn { get; set; }

        private void Awake()
        {
            Age = GetComponent<CreatureAge>();
            Hunger = GetComponent<CreatureHunger>();
            Health = GetComponent<CreatureHealth>();
        }

        public void Spawn()
        {
            SpawnServerRpc(Health.MaxHealth);
        }
        [ServerRpc]
        private void SpawnServerRpc(float maxHealth)
        {
            Health.Health = maxHealth;
            Age.StartAging();
            Hunger.StartHunger();

            SpawnClientRpc();
        }
        [ClientRpc]
        private void SpawnClientRpc()
        {
            OnSpawn?.Invoke();
        }

        public void Despawn()
        {
            DespawnServerRpc();
        }
        [ServerRpc]
        public void DespawnServerRpc()
        {
            Age.StopAging();
            Hunger.StopHunger();

            DespawnClientRpc();
        }
        [ClientRpc]
        private void DespawnClientRpc()
        {
            OnDespawn?.Invoke();
        }
    }
}