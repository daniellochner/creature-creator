using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class NPCSpawner : MonoBehaviour
    {
        public NetworkObject npcPrefab;

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkObject npc = Instantiate(npcPrefab, transform.position, transform.rotation);
                npc.Spawn();
                Setup(npc);
            }
        }
        public virtual void Setup(NetworkObject npc) { }
    }
}