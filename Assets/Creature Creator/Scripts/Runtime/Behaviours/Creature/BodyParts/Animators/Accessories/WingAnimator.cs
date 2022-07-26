// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class WingAnimator : BodyPartAnimator
    {
        private Animator animator;

        protected override void Initialize()
        {
            base.Initialize();
            animator = GetComponent<Animator>();
        }

        public void Flap()
        {
            animator.SetTrigger("Flap");
        }
    }
}