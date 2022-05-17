// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureSelector))]
    [RequireComponent(typeof(CreatureTracker))]
    public class CreatureSourceNonPlayer : CreatureSource
    {
        #region Fields
        [SerializeField] private CreatureSelector selector;
        [SerializeField] private CreatureTracker tracker;
        #endregion

        #region Properties
        public CreatureSelector Selector => selector;
        public CreatureTracker Tracker => tracker;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            selector = GetComponent<CreatureSelector>();
            tracker = GetComponent<CreatureTracker>();
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