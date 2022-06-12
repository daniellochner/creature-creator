// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Open : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float duration = 0.5f;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.InvokeOverTime(delegate (float p)
            {
                float t = 0.5f * (1f + ((stateInfo.speed > 0) ? p : (1f - p)));
                foreach (MouthAnimator mouth in m_MonoBehaviour.Mouths)
                {
                    mouth.SetOpen(t);
                }
            },
            duration);
        }
        #endregion
    }
}