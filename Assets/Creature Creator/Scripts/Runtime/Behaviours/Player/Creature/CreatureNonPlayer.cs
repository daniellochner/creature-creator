// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureInformer))]
    public class CreatureNonPlayer : CreatureBase
    {
        #region Fields
        [SerializeField] private CreatureInformer informer;
        #endregion

        #region Properties
        public CreatureInformer Informer => informer;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            informer = GetComponent<CreatureInformer>();
        }
#endif
        #endregion
    }
}