using System.Collections.Generic;
using Unity.Netcode;

namespace DanielLochner.Assets
{
    public class SpawnedNPC : NetworkBehaviour
    {
        public NetworkVariable<ulong> spawnerId = new NetworkVariable<ulong>();

        public static List<SpawnedNPC> spawned = new List<SpawnedNPC>();

        private void OnEnable()
        {
            spawned.Add(this);
        }
        private void OnDisable()
        {
            spawned.Remove(this);
        }
        public override void OnDestroy()
        {
            spawned.Remove(this);
            base.OnDestroy();
        }
    }
}