using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SharkSpawner : AnimalSpawner
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private int start;

        public override void Setup(NetworkObject npc)
        {
            base.Setup(npc);

            SharkAI shark = npc.GetComponent<SharkAI>();

            SharkAI.Swimming swimming = shark.GetState("SWI") as SharkAI.Swimming;
            if (swimming != null)
            {
                swimming.current = start;
                swimming.waypoints = waypoints;
            }
        }
    }
}