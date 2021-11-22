// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureInteractable : Interactable
    {
        #region Methods
        public sealed override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && CanInteract(interactor as CreatureInteractor);
        }
        public sealed override bool CanHighlight(Interactor interactor)
        {
            return base.CanHighlight(interactor) && CanHighlight(interactor as CreatureInteractor);
        }

        public virtual bool CanInteract(CreatureInteractor creatureInteractor)
        {
            return EditorManager.Instance.IsPlaying;
        }
        public virtual bool CanHighlight(CreatureInteractor creatureInteractor)
        {
            return EditorManager.Instance.IsPlaying;
        }
        #endregion
    }
}