// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Tilt : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float duration;
        [SerializeField] private bool reverse;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.InvokeOverTime(delegate (float p)
            {
            },
            duration);
        }
        #endregion
    }
}