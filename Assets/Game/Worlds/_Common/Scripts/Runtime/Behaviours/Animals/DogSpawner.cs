using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DogSpawner : AnimalSpawner
    {
        [SerializeField] private GameObject doghouse;
        [SerializeField] private Quest quest;

        public override void Setup(NetworkObject npc)
        {
            base.Setup(npc);

            DogAI dog = npc.GetComponent<DogAI>();

            DogAI.Scurrying scurrying = dog.GetState("SCU") as DogAI.Scurrying;
            if (scurrying != null)
            {
                scurrying.doghouse = doghouse;
            }

            if (quest != null)
            {
                quest.transform.SetZeroParent(dog.transform);
            }
        }
    }
}