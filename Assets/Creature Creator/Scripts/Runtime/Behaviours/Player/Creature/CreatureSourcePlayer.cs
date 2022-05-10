// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureEditor))]
    [RequireComponent(typeof(CreatureAbilities))]
    [RequireComponent(typeof(CreatureMover))]
    [RequireComponent(typeof(CreatureInteractor))]
    public class CreatureSourcePlayer : CreatureSource
    {
        #region Fields
        [SerializeField] private CreatureEditor editor;
        [SerializeField] private CreatureAbilities abilities;
        [SerializeField] private CreatureMover mover;
        [SerializeField] private CreatureInteractor interactor;
        #endregion

        #region Properties
        public CreatureEditor Editor => editor;
        public CreatureAbilities Abilities => abilities;
        public CreatureMover Mover => mover;
        public CreatureInteractor Interactor => interactor;

        public CreatureLife Life => Health as CreatureLife;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            editor = GetComponent<CreatureEditor>();
            abilities = GetComponent<CreatureAbilities>();
            mover = GetComponent<CreatureMover>();
            interactor = GetComponent<CreatureInteractor>();
        }
#endif

        public override void Setup()
        {
            base.Setup();
            Editor.Setup();
        }
        #endregion
    }
}