// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureSelectable))]
    public class CreatureNonPlayer : CreatureBase
    {
        #region Fields
        [SerializeField] private CreatureSelectable selectable;
        [SerializeField] private Commandable commandable;
        #endregion

        #region Properties
        public CreatureSelectable Selectable => selectable;
        public Commandable Commandable => commandable;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            selectable = GetComponent<CreatureSelectable>();
            commandable = GetComponent<Commandable>();
        }
#endif
        private IEnumerator Start()
        {
            yield return new WaitUntilSetup(Player.Instance);
            Setup();
        }

        public override void Setup()
        {
            base.Setup();
            Selectable.Setup();
        }

        public override void OnDie()
        {
            base.OnDie();
            Selectable.SetSelected(false);
        }
        public override void OnHide()
        {
            base.OnHide();
            Collider.enabled = false;
            Animator.IsAnimated = false;
        }
        #endregion
    }
}