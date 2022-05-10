// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class Pickup : CreatureInteractable
    {
        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && (interactor as CreatureInteractor).Creature.Animator.Arms.Count > 0;
        }
    }
}