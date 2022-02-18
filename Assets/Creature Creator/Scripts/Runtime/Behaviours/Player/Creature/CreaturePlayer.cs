// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureEditor))]
    [RequireComponent(typeof(CreatureAbilities))]
    [RequireComponent(typeof(CreatureMover))]
    [RequireComponent(typeof(CreatureHealth))]
    [RequireComponent(typeof(CreatureEnergy))]
    [RequireComponent(typeof(CreatureAge))]
    [RequireComponent(typeof(CreatureInteractor))]
    public class CreaturePlayer : CreatureBase
    {
        #region Fields
        [SerializeField] private CreatureEditor editor;
        [SerializeField] private CreatureAbilities abilities;
        [SerializeField] private CreatureMover mover;
        [SerializeField] private CreatureHealth health;
        [SerializeField] private CreatureEnergy energy;
        [SerializeField] private CreatureAge age;
        [SerializeField] private CreatureInteractor interactor;
        #endregion

        #region Properties
        public CreatureEditor Editor => editor;
        public CreatureAbilities Abilities => abilities;
        public CreatureMover Mover => mover;
        public CreatureHealth Health => health;
        public CreatureEnergy Energy => energy;
        public CreatureAge Age => age;
        public CreatureInteractor Interactor => interactor;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            editor = GetComponent<CreatureEditor>();
            abilities = GetComponent<CreatureAbilities>();
            mover = GetComponent<CreatureMover>();
            health = GetComponent<CreatureHealth>();
            energy = GetComponent<CreatureEnergy>();
            age = GetComponent<CreatureAge>();
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