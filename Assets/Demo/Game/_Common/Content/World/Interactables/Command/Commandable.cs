using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Commandable : CreatureInteractable
    {
        private AnimalAI animalAI;

        private void Awake()
        {
            animalAI = GetComponent<AnimalAI>();
        }

        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && animalAI.CanFollow;
        }
        protected override void OnInteract(Interactor interactor)
        {
            if (animalAI.FollowTarget == null)
            {
                animalAI.Follow(interactor.transform);
            }
            else
            {
                animalAI.StopFollowing();
            }
        }
    }
}