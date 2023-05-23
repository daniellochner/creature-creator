using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AnimalSpawner : NPCSpawner
    {
        [Header("Scene References")]
        [SerializeField] public Bounds wanderBounds;
        [SerializeField] public float scale = 1f;

        public override void Setup(NetworkObject npc)
        {
            base.Setup(npc);

            AnimalAI animal = npc.GetComponent<AnimalAI>();

            animal.Creature.Scaler.Scale(scale);

            AnimalAI.Wandering wandering = animal.GetState<AnimalAI.Wandering>("WAN");
            if (wandering != null)
            {
                wandering.wanderBounds = wanderBounds;
            }
        }
    }
}