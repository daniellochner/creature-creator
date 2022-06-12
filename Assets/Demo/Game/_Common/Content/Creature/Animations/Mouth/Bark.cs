// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Bark : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float duration;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.InvokeOverTime(delegate (float p)
            {
                foreach (MouthAnimator mouth in m_MonoBehaviour.Mouths)
                {
                    mouth.SetOpen(0.5f + 0.5f * Mathf.Sin(p * Mathf.PI));
                }
            },
            duration);
        }
        #endregion
    }
}