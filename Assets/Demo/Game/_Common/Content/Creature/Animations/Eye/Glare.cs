// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Glare : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float duration;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.InvokeOverTime(delegate (float p)
            {
                float t = (stateInfo.speed > 0) ? p : (1f - p);
                foreach (EyeAnimator eye in m_MonoBehaviour.Eyes)
                {
                    eye.BodyPartConstructor.SkinnedMeshRenderer.SetBlendShapeWeight(1, t * 100f);
                }
            },
            duration);
        }
        #endregion
    }
}