// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Idling : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float d = 0.025f;
        [SerializeField] private float t = 0.5f;
        [SerializeField, ReadOnly] private float idlingTime;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            idlingTime = 0f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            if (m_MonoBehaviour.CreatureConstructor.Legs.Count > 0)
            {
                m_MonoBehaviour.CreatureConstructor.Root.localPosition = new Vector3(0, d * Mathf.Cos(idlingTime / t) - d, 0);
                idlingTime += Time.deltaTime;
            }
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.zero;
        }
        #endregion
    }
}