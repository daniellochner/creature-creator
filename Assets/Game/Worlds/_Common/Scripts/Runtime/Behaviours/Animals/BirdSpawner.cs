using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BirdSpawner : AnimalSpawner
    {
        [SerializeField] private Transform perchPoints;

        public override void Setup(NetworkObject npc)
        {
            base.Setup(npc);

            BirdAI bird = npc.GetComponent<BirdAI>();

            BirdAI.Flying flying = bird.GetState("FLY") as BirdAI.Flying;
            if (flying != null)
            {
                flying.perchPoints = perchPoints;
            }
        }
    }
}