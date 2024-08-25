// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Seal : CreatureAnimation
    {
        #region Fields
        [SerializeField] private float s;
        private float x = 0f;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Creature.Constructor.Root);
            }
            x = 0f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.Constructor.Root.localRotation = Quaternion.Euler(0f, x, 0f);
            x += Time.deltaTime * s;
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.Constructor.Root.localRotation = Quaternion.identity;
            Creature.Effector.StopSoundsSelf();

            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Dynamic.Transform);
            }
        }
        #endregion
    }
}