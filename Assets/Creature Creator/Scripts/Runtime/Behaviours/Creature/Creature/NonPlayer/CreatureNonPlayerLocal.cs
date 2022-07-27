// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHungerDepleter))]
    [RequireComponent(typeof(CreatureAger))]
    public class CreatureNonPlayerLocal : CreatureNonPlayer
    {
        #region Fields
        [SerializeField] private CreatureHungerDepleter hungerDepleter;
        [SerializeField] private CreatureAger ager;
        #endregion

        #region Properties
        public CreatureHungerDepleter HungerDepleter => hungerDepleter;
        public CreatureAger Ager => ager;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            hungerDepleter = GetComponent<CreatureHungerDepleter>();
            ager = GetComponent<CreatureAger>();
        }
#endif
        #endregion
    }
}