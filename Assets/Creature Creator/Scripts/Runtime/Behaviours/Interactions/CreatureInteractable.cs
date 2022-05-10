// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureInteractable : Interactable
    {
        #region Methods
        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && EditorManager.Instance.IsPlaying;
        }
        #endregion
    }
}