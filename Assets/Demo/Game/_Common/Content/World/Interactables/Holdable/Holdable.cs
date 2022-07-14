// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class Holdable : CreatureInteractable
    {
        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && interactor.GetComponent<CreatureAnimator>().Arms.Count > 0;
        }
    }
}