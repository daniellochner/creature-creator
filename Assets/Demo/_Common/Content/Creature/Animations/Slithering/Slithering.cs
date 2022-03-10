// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Slithering : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float width = 0.2f;
        [SerializeField] private float maxK = 3;
        [SerializeField, ReadOnly] private float slitherTime;
        [SerializeField] private float movementSpeed = 0.8f;

        private Vector3 headPos;
        #endregion

        #region Methods
        public override void OnStart(Animator animator)
        {
            if (m_MonoBehaviour.IsAnimated)
            {
                headPos = m_MonoBehaviour.CreatureConstructor.Body.W2LSpace(m_MonoBehaviour.CreatureConstructor.Bones[m_MonoBehaviour.CreatureConstructor.Bones.Count - 1].position);
                slitherTime = 0f;
            }
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float t = animator.GetFloat("%LSpeed");
            float x = t * width * Mathf.Sin(slitherTime);

            m_MonoBehaviour.CreatureConstructor.Root.localPosition = new Vector3(x, 0, 0);
            slitherTime += Time.deltaTime;

            Vector3 pos = m_MonoBehaviour.CreatureConstructor.Body.L2WSpace(headPos) + m_MonoBehaviour.CreatureConstructor.Body.forward * movementSpeed;
            m_MonoBehaviour.CreatureConstructor.Bones[m_MonoBehaviour.CreatureConstructor.Bones.Count - 1].LookAt(pos, m_MonoBehaviour.CreatureConstructor.Body.up);
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.zero;
        }
        #endregion
    }
}