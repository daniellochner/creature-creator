using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public static class AnimatorUtility
    {
        public static bool CanTransitionTo(this Animator animator, string state)
        {
            return !animator.IsInTransition(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName(state);
        }
    }
}