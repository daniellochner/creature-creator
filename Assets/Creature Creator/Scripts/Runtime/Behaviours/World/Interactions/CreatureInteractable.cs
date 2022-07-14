// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureInteractable : Interactable
    {
        #region Fields
        [SerializeField] private Ability reqAbility;
        #endregion

        #region Methods
        public override bool CanInteract(Interactor interactor)
        {
            bool hasReqAbility = (reqAbility == null);
            if (reqAbility != null)
            {
                hasReqAbility = interactor.GetComponent<CreatureAbilities>().Abilities.Contains(reqAbility);
            }
            return base.CanInteract(interactor) && EditorManager.Instance.IsPlaying && hasReqAbility;
        }
        #endregion
    }
}