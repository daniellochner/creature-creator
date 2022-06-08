// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Flying : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float flapCooldown;
        [SerializeField] private float flapHeight;
        [SerializeField] private CreatureEffector.Sound[] flapSounds;

        private WingAnimator[] wings;
        private float timeLeft;
        #endregion

        #region Methods
        public override void OnStart(Animator animator)
        {
            base.OnStart(animator);
            wings = m_MonoBehaviour.GetComponentsInChildren<WingAnimator>();
        }
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (WingAnimator wing in wings)
            {
                wing.IsPrepared = true;
            }

            foreach (LegAnimator leg in m_MonoBehaviour.Legs)
            {
                leg.Anchor.SetParent(m_MonoBehaviour.Constructor.Root);
                leg.Anchor.SetPositionAndRotation(m_MonoBehaviour.Constructor.Body.L2WSpace(leg.DefaultFootPosition), Quaternion.identity);
            }
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            TimerUtility.OnTimer(ref timeLeft, flapCooldown, Time.deltaTime, Flap);
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (WingAnimator wing in wings)
            {
                wing.IsPrepared = false;
            }
            m_MonoBehaviour.Constructor.Root.localPosition = Vector3.zero;

            foreach (LegAnimator leg in m_MonoBehaviour.Legs)
            {
                leg.Anchor.SetParent(Dynamic.Transform);
            }
        }

        private void Flap()
        {
            foreach (WingAnimator wing in wings)
            {
                wing.Flap();

                m_MonoBehaviour.Effector.PlaySound(flapSounds);
            }
            m_MonoBehaviour.InvokeOverTime(delegate (float progress)
            {
                float y = flapHeight * Mathf.Sin(progress * Mathf.PI);
                m_MonoBehaviour.Constructor.Root.localPosition = Vector3.up * y;
            }, 
            flapCooldown / 2f);
        }
        #endregion
    }
}