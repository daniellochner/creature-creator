// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureSelectable))]
    public class CreatureTarget : CreatureTargetBase
    {
        #region Fields
        [SerializeField] private CreatureSelectable selector;
        #endregion

        #region Properties
        public CreatureSelectable Selector => selector;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            selector = GetComponent<CreatureSelectable>();
        }
#endif

        public override void Setup()
        {
            base.Setup();
            Selector.Setup();
        }
        #endregion
    }
}