// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHungerDepleter))]
    [RequireComponent(typeof(CreatureAger))]
    [RequireComponent(typeof(CreatureCamera))]
    public class CreaturePlayerLocal : CreaturePlayer, ISetupable
    {
        #region Fields
        [SerializeField] private CreatureHungerDepleter energyDepleter;
        [SerializeField] private CreatureAger ager;

        [SerializeField] private CreatureEditor editor;
        [SerializeField] private CreatureHealthEditor healthEditor;
        [SerializeField] private CreatureAbilities abilities;
        [SerializeField] private CreatureMover mover;
        [SerializeField] private CreatureInteractor interactor;
        [SerializeField] private new CreatureCamera camera;
        #endregion

        #region Properties
        public CreatureHungerDepleter EnergyDepleter => energyDepleter;
        public CreatureAger Ager => ager;

        public CreatureEditor Editor => editor;
        public CreatureHealthEditor HealthEditor => healthEditor;
        public CreatureAbilities Abilities => abilities;
        public CreatureMover Mover => mover;
        public CreatureInteractor Interactor => interactor;
        public CreatureCamera Camera => camera;

        public bool IsSetup { get; set; }
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            energyDepleter = GetComponent<CreatureHungerDepleter>();
            ager = GetComponent<CreatureAger>();

            editor = GetComponent<CreatureEditor>();
            healthEditor = GetComponent<CreatureHealthEditor>();
            abilities = GetComponent<CreatureAbilities>();
            mover = GetComponent<CreatureMover>();
            interactor = GetComponent<CreatureInteractor>();
            camera = GetComponent<CreatureCamera>();
        }
#endif

        public override void Setup()
        {
            Camera.Setup();
            base.Setup();
            Editor.Setup();
            Interactor.Setup();

            IsSetup = true;
        }
        #endregion
    }
}