using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DogSpawner : AnimalSpawner
    {
        [SerializeField] private GameObject doghouse;

        public override void Setup(NetworkObject npc)
        {
            base.Setup(npc);

            DogAI dog = npc.GetComponent<DogAI>();

            DogAI.Scurrying scurrying = dog.GetState<DogAI.Scurrying>("SCU");
            if (scurrying != null)
            {
                scurrying.doghouse = doghouse;
            }
        }
    }
}