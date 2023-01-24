using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class NPCSpawner : NetworkBehaviour
    {
        [SerializeField] private NetworkObject npcPrefab;
        [SerializeField] public bool spawnOnStart = true;

        public static List<NPCSpawner> Spawners { get; set; } = new List<NPCSpawner>();

        public NetworkObject SpawnedNPC { get; private set; }

        private void OnEnable()
        {
            Spawners.Add(this);
        }
        private void OnDisable()
        {
            Spawners.Remove(this);
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