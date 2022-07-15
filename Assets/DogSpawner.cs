using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DogSpawner : AnimalSpawner
    {
        [SerializeField] private GameObject dogHouse;

        public override void Setup(NetworkObject npc)
        {
            base.Setup(npc);

            
        }
    }
}