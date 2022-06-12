// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public abstract class Animate<T> : SceneLinkedSMB<CreatureAnimator> where T : BodyPartAnimator
    {
        #region Fields
        [SerializeField] private int index;
        [Space]
        [SerializeField] private float duration;
        [SerializeField] private AnimationCurve curve;
        #endregion

        #region Properties
        public abstract List<T> BodyParts { get; }
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.InvokeOverTime(delegate (float p)
            {
                foreach (T bpa in BodyParts)
                {
                    bpa.BodyPartConstructor.SkinnedMeshRenderer.SetBlendShapeWeight(index, curve.Evaluate(p) * 100f);
                }
            },
            duration);
        }
        #endregion
    }
}