using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class NPCSpawner : MonoBehaviour
    {
        public NetworkObject npcPrefab;

        public void Spawn()
        {
            NetworkObject npc = Instantiate(npcPrefab, transform.position, transform.rotation);
            npc.Spawn();
            Setup(npc);
        }

        public virtual void Setup(NetworkObject npc) { }
    }
}