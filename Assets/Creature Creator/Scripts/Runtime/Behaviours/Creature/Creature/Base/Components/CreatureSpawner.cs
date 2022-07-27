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
        public void Respawn()
        {
            RespawnServerRpc();
        }

        [ServerRpc]
        public void SpawnServerRpc()
        {
            OnSpawn?.Invoke();
        }

        [ServerRpc]
        public void DespawnServerRpc()
        {
            OnDespawn?.Invoke();
        }

        [ServerRpc]
        public void RespawnServerRpc()
        {
            OnDespawn?.Invoke();
            OnSpawn?.Invoke();
        }
    }
}