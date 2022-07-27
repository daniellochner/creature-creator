using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
            CommandServerRpc(interactor.NetworkObject);
        }

        [ServerRpc]
        private void CommandServerRpc(NetworkObjectReference commander)
        {
            if (commander.TryGet(out NetworkObject obj))
            {
                if (animalAI.GetState<AnimalAI.Following>("FOL").Target == null)
                {
                    animalAI.Follow(obj.transform);
                }
                else
                {
                    animalAI.StopFollowing();
                }
            }
        }
    }
}