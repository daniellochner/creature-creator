// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Bark : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float barkTime = 0.5f;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (MouthAnimator mouth in m_MonoBehaviour.Mouths)
            {
                m_MonoBehaviour.StartCoroutine(BarkRoutine(mouth));
            }
        }

        private IEnumerator BarkRoutine(MouthAnimator mouth)
        {
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
            {
                mouth.SetOpen(0.5f + 0.5f * Mathf.Sin(p * Mathf.PI));
            },
            barkTime);
        }
        #endregion
    }
}