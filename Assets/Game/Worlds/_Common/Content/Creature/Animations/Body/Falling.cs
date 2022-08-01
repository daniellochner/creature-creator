// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Falling : CreatureAnimation
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Creature.Constructor.Root);
                leg.Anchor.SetPositionAndRotation(Creature.Constructor.transform.L2WSpace(leg.DefaultFootLocalPos), Quaternion.identity);
            }
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Dynamic.Transform);
            }
        }
    }
}