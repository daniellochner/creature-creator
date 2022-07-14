// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureEnergyDepleter))]
    [RequireComponent(typeof(CreatureAger))]
    public class CreatureNonPlayerLocal : CreatureNonPlayer
    {
        #region Fields
        [SerializeField] private CreatureEnergyDepleter energyDepleter;
        [SerializeField] private CreatureAger ager;
        #endregion

        #region Properties
        public CreatureEnergyDepleter EnergyDepleter => energyDepleter;
        public CreatureAger Ager => ager;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            energyDepleter = GetComponent<CreatureEnergyDepleter>();
            ager = GetComponent<CreatureAger>();
        }
#endif
        #endregion
    }
}