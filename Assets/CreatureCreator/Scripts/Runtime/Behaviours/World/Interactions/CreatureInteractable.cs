// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureInteractable : Interactable
    {
        #region Fields
        [SerializeField] private Ability reqAbility;

        private Outline outline;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            outline = GetComponent<Outline>();
        }

        public override bool CanInteract(Interactor interactor)
        {
            bool hasReqAbility = (reqAbility == null);
            if (reqAbility != null)
            {
                hasReqAbility = interactor.GetComponent<CreatureAbilities>().Abilities.Contains(reqAbility);
            }
            return base.CanInteract(interactor) && EditorManager.Instance.IsPlaying && hasReqAbility;
        }
        public override bool CanHighlight(Interactor interactor)
        {
            return base.CanHighlight(interactor) && EditorManager.Instance.IsPlaying;
        }

        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
        protected override void OnHighlight(Interactor interactor, bool isHighlighted)
        {
            base.OnHighlight(interactor, isHighlighted);
            if (outline != null)
            {
                outline.enabled = isHighlighted;
            }

            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                (interactor as CreatureInteractor).CreatureCamera.CameraOrbit.SetFrozen(isHighlighted);
            }
        }
        #endregion
    }
}