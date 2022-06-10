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
        [SerializeField, ReadOnly] private float idleTime;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            idleTime = 0f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            if (animator.GetBool("HasLegs"))
            {
                m_MonoBehaviour.Constructor.Root.localPosition = new Vector3(0, breatheHeight * Mathf.Cos(idleTime / breatheTime) - breatheHeight, 0);
                idleTime += Time.deltaTime;
            }
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Constructor.Root.localPosition = Vector3.zero;
        }
        #endregion
    }
}