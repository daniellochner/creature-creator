using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AnimalSpawner : NPCSpawner
    {
        [Header("Scene References")]
        [SerializeField] private Bounds wanderBounds;
        [SerializeField] private float scale = 1f;

        public override void Setup(NetworkObject npc)
        {
            AnimalAI animal = npc.GetComponent<AnimalAI>();

            animal.Creature.Scaler.Scale(scale);

            AnimalAI.Wandering wandering = animal.GetState("WAN") as AnimalAI.Wandering;
            if (wandering != null)
            {
                wandering.wanderBounds = wanderBounds;
            }
        }
    }
}