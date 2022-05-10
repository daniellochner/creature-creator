// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHealth))]
    [RequireComponent(typeof(CreatureEnergy))]
    [RequireComponent(typeof(CreatureAge))]
    public class CreatureSource : CreatureTargetBase
    {
        #region Fields
        [SerializeField] private CreatureHealth health;
        [SerializeField] private CreatureEnergy energy;
        [SerializeField] private CreatureAge age;
        #endregion

        #region Properties
        public CreatureHealth Health => health;
        public CreatureEnergy Energy => energy;
        public CreatureAge Age => age;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            health = GetComponent<CreatureHealth>();
            energy = GetComponent<CreatureEnergy>();
            age = GetComponent<CreatureAge>();
        }
#endif
        #endregion
    }
}