// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Turning : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float turnThreshold = 0.9f;
        [SerializeField] private int direction = 1;
        #endregion

        #region Methods
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            if (m_MonoBehaviour.Legs.Length > 0)
            {
                
            }
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.zero;
        }

        protected void HandleLeg(LegAnimator leg)
        {
            //float dot = Vector3.Dot(leg.Anchor.forward, m_MonoBehaviour.CreatureConstructor.Body.forward);
            //if (dot < 0.75f)
            //{
            //    return true;
            //}


            //if (Vector3.Dot(leg.Anchor.forward, leg.LegConstructor.Extremity.forward) < turnThreshold && !leg.FlippedLeg.IsMovingToTarget)
            //{
            //    Vector3 position = m_MonoBehaviour.CreatureConstructor.Body.TransformPoint(leg.DefaultFootPosition);
            //    Quaternion rotation = m_MonoBehaviour.CreatureConstructor.Body.rotation;
            //    float timeToMove = 0.25f;
            //    float liftHeight = 0.25f;

            //    leg.MoveToTarget(position, rotation, timeToMove, liftHeight);
            //}
        }
        #endregion
    }
}