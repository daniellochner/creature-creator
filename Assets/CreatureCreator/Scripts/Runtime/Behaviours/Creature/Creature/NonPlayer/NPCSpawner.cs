using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class NPCSpawner : MonoBehaviour
    {
        public NetworkObject npcPrefab;

        public static List<NPCSpawner> Spawners { get; set; } = new List<NPCSpawner>();

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
            NetworkObject npc = Instantiate(npcPrefab, transform.position, transform.rotation);
            npc.Spawn();
            Setup(npc);
        }

        public virtual void Setup(NetworkObject npc) { }
    }
}