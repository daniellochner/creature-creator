// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureSelectable))]
    public class CreaturePlayerRemote : CreaturePlayer
    {
        #region Fields
        [SerializeField] private CreatureSelectable selectable;
        #endregion

        #region Properties
        public CreatureSelectable Selectable => selectable;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            selectable = GetComponent<CreatureSelectable>();
        }
#endif
        private void Start()
        {
            Setup();
        }

        public override void Setup()
        {
            base.Setup();
            Selectable.Setup();
        }
        #endregion
    }
}