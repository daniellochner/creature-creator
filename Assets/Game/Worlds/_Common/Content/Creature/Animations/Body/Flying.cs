// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Flying : CreatureAnimation
    {
        #region Fields
        [SerializeField] private float flapCooldown;
        [SerializeField] private float flapHeight;

        private WingAnimator[] wings;
        private float timeLeft;
        #endregion

        #region Properties
        public Action OnFlap { get; set; }
        #endregion

        #region Methods
        public override void Setup()
        {
            wings = Creature.GetComponentsInChildren<WingAnimator>();
        }
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Creature.Constructor.Root);
                leg.Anchor.SetPositionAndRotation(Creature.Constructor.transform.L2WSpace(leg.DefaultFootLocalPos), Quaternion.identity);
            }
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            TimerUtility.OnTimer(ref timeLeft, flapCooldown, Time.deltaTime, Flap);
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.Constructor.Root.localPosition = Vector3.zero;

            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Dynamic.Transform);
            }
        }

        private void Flap()
        {
            foreach (WingAnimator wing in wings)
            {
                wing.Flap();
            }
            OnFlap?.Invoke();

            Creature.InvokeOverTime(delegate (float progress)
            {
                float y = flapHeight * Mathf.Sin(progress * Mathf.PI);
                Creature.Constructor.Root.localPosition = Vector3.up * y;
            }, 
            flapCooldown / 2f);
        }
        #endregion
    }
}