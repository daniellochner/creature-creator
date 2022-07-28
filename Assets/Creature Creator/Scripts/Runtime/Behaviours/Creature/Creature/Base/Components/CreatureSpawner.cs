using System;
using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureSpawner : NetworkBehaviour
    {
        public Action OnSpawn { get; set; }
        public Action OnDespawn { get; set; }

        public void Spawn()
        {
            SpawnServerRpc();
        }
        public void Despawn()
        {
            DespawnServerRpc();
        }

        [ServerRpc]
        public void SpawnServerRpc()
        {
            OnDespawn?.Invoke();
            OnSpawn?.Invoke();
        }

        [ServerRpc]
        public void DespawnServerRpc()
        {
            OnDespawn?.Invoke();
        }
    }
}