using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class NPCSpawner : MonoBehaviour
    {
        public NetworkObject npcPrefab;

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer || !NetworkConnectionManager.IsConnected)
            {
                NetworkObject npc = Instantiate(npcPrefab, transform.position, transform.rotation);

                if (NetworkConnectionManager.IsConnected)
                {
                    npc.Spawn();
                }
                Setup(npc);
            }
        }
        public virtual void Setup(NetworkObject npc) { }
    }
}