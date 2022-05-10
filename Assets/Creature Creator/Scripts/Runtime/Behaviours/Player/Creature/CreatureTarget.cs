// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureSelector))]
    public class CreatureTarget : CreatureTargetBase
    {
        #region Fields
        [SerializeField] private CreatureSelector selector;
        #endregion

        #region Properties
        public CreatureSelector Selector => selector;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            selector = GetComponent<CreatureSelector>();
        }
#endif

        public override void Setup()
        {
            Selector.Setup();
            base.Setup();
        }
        #endregion
    }
}