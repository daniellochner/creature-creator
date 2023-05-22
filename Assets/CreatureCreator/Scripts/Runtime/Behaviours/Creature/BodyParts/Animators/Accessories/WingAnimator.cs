// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class WingAnimator : BodyPartAnimator
    {
        #region Fields
        private Animator animator;
        #endregion

        #region Methods
        protected override void Initialize()
        {
            base.Initialize();
            animator = GetComponent<Animator>();
        }

        public void Flap()
        {
            animator.SetTrigger("Flap");
        }
        #endregion
    }
}