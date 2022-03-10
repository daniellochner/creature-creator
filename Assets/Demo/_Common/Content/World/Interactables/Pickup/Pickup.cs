// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class Pickup : CreatureInteractable
    {
        public override bool CanInteract(CreatureInteractor creatureInteractor)
        {
            return base.CanInteract(creatureInteractor) && creatureInteractor.Creature.Animator.Arms.Count > 0;
        }
    }
}