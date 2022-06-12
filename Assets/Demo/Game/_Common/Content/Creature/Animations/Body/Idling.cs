// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Idling : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float breatheBody;
        [SerializeField] private float breatheMouth;
        [SerializeField] private float breatheTime;
        private float x;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            x = 0f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            float t = (2f * Mathf.PI * x) / breatheTime;

            float b = (breatheBody / 2f) * (Mathf.Cos(t) - 1f);
            float m = 0.5f + Mathf.Lerp(-breatheMouth, breatheMouth, Mathf.InverseLerp(-breatheBody, 0f, b));

            if (m_MonoBehaviour.Legs.Count > 0)
            {
                m_MonoBehaviour.Constructor.Root.localPosition = Vector3.up * b;
            }
            if (m_MonoBehaviour.Mouths.Count > 0)
            {
                animator.SetFloat("Mouth_Breathe", m);
            }

            x += Time.deltaTime;
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Constructor.Root.localPosition = Vector3.zero;
            animator.SetFloat("Mouth_Breathe", 0.5f);
        }
        #endregion
    }
}