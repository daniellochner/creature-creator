// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Roll : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float rollTime;
        [SerializeField] private float rollDistance;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.StartCoroutine(RollRoutine());
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Constructor.Root.localRotation = Quaternion.identity;
            m_MonoBehaviour.Constructor.Root.localPosition = Vector3.zero;
        }

        private IEnumerator RollRoutine()
        {
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                m_MonoBehaviour.Constructor.Root.localRotation = Quaternion.Euler(0f, 0f, (1f - t) * 360f);
                m_MonoBehaviour.Constructor.Root.localPosition = Vector3.Lerp(Vector3.zero, Vector3.right * rollDistance, t);
            },
            rollTime);

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                m_MonoBehaviour.Constructor.Root.localRotation = Quaternion.Euler(0f, 0f, t * 360f);
                m_MonoBehaviour.Constructor.Root.localPosition = Vector3.Lerp(Vector3.right * rollDistance, Vector3.zero, t);
            },
            rollTime);
        }
        #endregion
    }
}