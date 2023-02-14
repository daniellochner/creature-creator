using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FishSpawner : AnimalSpawner
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private int start;

        public override void Setup(NetworkObject npc)
        {
            base.Setup(npc);

            FishAI fish = npc.GetComponent<FishAI>();

            FishAI.Swimming swimming = fish.GetState("SWI") as FishAI.Swimming;
            if (swimming != null)
            {
                swimming.current = start;
                swimming.waypoints = waypoints;
            }
        }
    }
}