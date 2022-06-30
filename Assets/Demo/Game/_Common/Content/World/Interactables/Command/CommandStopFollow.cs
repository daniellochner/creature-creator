using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CommandStopFollow : CreatureInteractable
    {
        private AnimalAI animalAI;

        private void Awake()
        {
            animalAI = GetComponent<AnimalAI>();
        }

        protected override void OnInteract(Interactor interactor)
        {
            animalAI.StopFollowing();
        }
    }
}