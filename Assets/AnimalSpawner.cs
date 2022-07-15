using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AnimalSpawner : NPCSpawner
    {
        [SerializeField] private Bounds bounds;

        public override void Setup(NetworkObject npc)
        {
            AnimalAI animal = npc.GetComponent<AnimalAI>();

            //AnimalAI.Wandering wandering = animal.GetState("WAN") as AnimalAI.Wandering;
            //if (wandering != null)
            //{
            //    wandering.wanderBounds = bounds;
            //}
        }
    }
}