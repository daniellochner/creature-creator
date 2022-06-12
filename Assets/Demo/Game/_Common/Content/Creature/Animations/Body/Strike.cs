// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Strike : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private string strikeAction;
        [SerializeField] private float strikeTime;
        [SerializeField] private float returnTime;
        [SerializeField] private float headOffset;
        #endregion

        #region Properties
        private Transform Head
        {
            get => m_MonoBehaviour.Constructor.Bones[m_MonoBehaviour.Constructor.Bones.Count - 1];
        }
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Vector3 displacement = Vector3.ProjectOnPlane(m_MonoBehaviour.InteractTarget.position - Head.position, m_MonoBehaviour.transform.up);
            Vector3 offset = headOffset * m_MonoBehaviour.transform.forward;
            m_MonoBehaviour.StartCoroutine(StrikeRoutine(displacement - offset));
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Constructor.Root.localPosition = Vector3.zero;
        }

        private IEnumerator StrikeRoutine(Vector3 displacement)
        {
            Vector3 localDisplacement = m_MonoBehaviour.Constructor.Root.InverseTransformVector(displacement);

            m_MonoBehaviour.Animator.SetTrigger(strikeAction);
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                m_MonoBehaviour.Constructor.Root.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.LookRotation(localDisplacement), t);
                m_MonoBehaviour.Constructor.Root.localPosition = Vector3.Lerp(Vector3.zero, localDisplacement, EasingFunction.EaseOutExpo(0f, 1f, t));
            },
            strikeTime);

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                m_MonoBehaviour.Constructor.Root.localRotation = Quaternion.Slerp(Quaternion.LookRotation(localDisplacement), Quaternion.identity, t);
                m_MonoBehaviour.Constructor.Root.localPosition = Vector3.Lerp(localDisplacement, Vector3.zero, t);
            }, 
            returnTime);
        }
        #endregion
    }
}