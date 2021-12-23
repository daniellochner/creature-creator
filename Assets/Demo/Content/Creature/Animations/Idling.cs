// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Idling : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float breatheHeight = 0.02f;
        [SerializeField] private float breatheTime = 0.5f;
        [SerializeField] private float turnThreshold = 0.9f;
        [SerializeField, ReadOnly] private float idlingTime;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            idlingTime = 0f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            if (m_MonoBehaviour.Legs.Length > 0)
            {
                m_MonoBehaviour.CreatureConstructor.Root.localPosition = new Vector3(0, breatheHeight * Mathf.Cos(idlingTime / breatheTime) - breatheHeight, 0);
                idlingTime += Time.deltaTime;

                for (int i = 0; i < m_MonoBehaviour.Legs.Length / 2; ++i)
                {
                    LegAnimator lleg = m_MonoBehaviour.LLegs[i];
                    LegAnimator rleg = m_MonoBehaviour.RLegs[i];

                    if (m_MonoBehaviour.IsTurningRight && (i % 2) == 0)
                    {
                        HandleLeg(rleg);
                        HandleLeg(lleg);
                    }
                    else
                    {
                        HandleLeg(lleg);
                        HandleLeg(rleg);
                    }
                }
            }
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.zero;
        }

        private void HandleLeg(LegAnimator leg)
        {
            if (Vector3.Dot(leg.Anchor.forward, leg.LegConstructor.Extremity.forward) < turnThreshold && !leg.FlippedLeg.IsMovingToTarget)
            {
                leg.MoveToTarget(m_MonoBehaviour.CreatureConstructor.Body.TransformPoint(leg.DefaultFootPosition), 0.25f, 0.2f);
            }
        }
        #endregion
    }
}