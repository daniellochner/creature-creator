// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Shake : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float shakeTime;
        [SerializeField] private float shakeAmplitude;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.StartCoroutine(ShakeRoutine());
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Constructor.Root.localRotation = Quaternion.identity;
        }

        private IEnumerator ShakeRoutine()
        {
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
            {
                float x = p * 5f;
                float z = shakeAmplitude * Mathf.Exp(-x) * Mathf.Sin(2 * Mathf.PI * x);
                m_MonoBehaviour.Constructor.Root.localRotation = Quaternion.Euler(0f, 0f, z);
            },
            shakeTime);
        }
        #endregion
    }
}