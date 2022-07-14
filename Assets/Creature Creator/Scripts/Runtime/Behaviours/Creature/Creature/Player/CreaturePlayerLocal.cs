// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureEnergyDepleter))]
    [RequireComponent(typeof(CreatureAger))]
    public class CreaturePlayerLocal : CreaturePlayer
    {
        #region Fields
        [SerializeField] private CreatureEnergyDepleter energyDepleter;
        [SerializeField] private CreatureAger ager;

        [SerializeField] private CreatureEditor editor;
        [SerializeField] private CreatureHealthEditor healthEditor;
        [SerializeField] private CreatureAbilities abilities;
        [SerializeField] private CreatureMover mover;
        [SerializeField] private CreatureInteractor interactor;
        [Space]
        [SerializeField] private CameraOrbit cameraOrbit;
        #endregion

        #region Properties
        public CreatureEnergyDepleter EnergyDepleter => energyDepleter;
        public CreatureAger Ager => ager;

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

            energyDepleter = GetComponent<CreatureEnergyDepleter>();
            ager = GetComponent<CreatureAger>();

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