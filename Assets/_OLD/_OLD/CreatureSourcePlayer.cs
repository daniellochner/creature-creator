// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureEditor))]
    [RequireComponent(typeof(CreatureHealthEditor))]
    [RequireComponent(typeof(CreatureAbilities))]
    [RequireComponent(typeof(CreatureMover))]
    [RequireComponent(typeof(CreatureInteractor))]
    public class CreatureSourcePlayer : CreatureSource
    {
        #region Fields
        [SerializeField] private CreatureEditor editor;
        [SerializeField] private CreatureHealthEditor healthEditor;
        [SerializeField] private CreatureAbilities abilities;
        [SerializeField] private CreatureMover mover;
        [SerializeField] private CreatureInteractor interactor;
        [Space]
        [SerializeField] private CameraOrbit cameraOrbit;
        #endregion

        #region Properties
        public CreatureEditor Editor => editor;
        public CreatureHealthEditor HealthEditor => healthEditor;
        public CreatureAbilities Abilities => abilities;
        public CreatureMover Mover => mover;
        public CreatureInteractor Interactor => interactor;

        public CameraOrbit CameraOrbit => cameraOrbit;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            editor = GetComponent<CreatureEditor>();
            healthEditor = GetComponent<CreatureHealthEditor>();
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