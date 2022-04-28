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

        private float timeToFlap, flapTime;
        private WingAnimator[] wings;
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
            flapTime = 0f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (timeToFlap <= 0f)
            {
                foreach (WingAnimator wing in wings)
                {
                    wing.Flap();
                }
                timeToFlap = flapCooldown;
            }
            timeToFlap -= Time.deltaTime;

            float c = flapCooldown / 4f;
            float a = (flapTime - ((c * Mathf.PI) / 2f)) / c;
            float y = flapHeight * Mathf.Sin(a) + flapHeight;
            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.up * y;
            flapTime += Time.deltaTime;
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (WingAnimator wing in wings)
            {
                wing.IsPrepared = false;
            }
            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.zero;
        }
        #endregion
    }
}