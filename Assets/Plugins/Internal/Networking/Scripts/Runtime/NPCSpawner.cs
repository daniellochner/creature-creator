using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class NPCSpawner : NetworkBehaviour
    {
        [SerializeField] private NetworkObject npcPrefab;
        [SerializeField] private bool ignoreFromSpawners;

        public static List<NPCSpawner> Spawners { get; set; } = new List<NPCSpawner>();

        public NetworkObject SpawnedNPC { get; private set; }

        private void OnEnable()
        {
            if (!ignoreFromSpawners)
            {
                Spawners.Add(this);
            }
        }
        private void OnDisable()
        {
            if (!ignoreFromSpawners)
            {
                Spawners.Remove(this);
            }
        }

        public void Spawn()
        {
            SpawnedNPC = Instantiate(npcPrefab, transform.position, transform.rotation);
            SpawnedNPC.Spawn(true);
            Setup(SpawnedNPC);
        }
        public virtual void Setup(NetworkObject npc)
        {
            npc.GetComponent<SpawnedNPC>().spawnerId.Value = NetworkObject.NetworkObjectId;
        }
    }
}