// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Blinking : CreatureAnimation
    {
        #region Fields
        [SerializeField] private MinMax blinkCooldown;
        [SerializeField] private float blinkTime;
        private float blinkTimeLeft;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            blinkTimeLeft = blinkCooldown.Random;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            TimerUtility.OnTimer(ref blinkTimeLeft, blinkCooldown.Random, Time.deltaTime, Blink);
        }

        private void Blink()
        {
            Creature.InvokeOverTime(delegate (float t)
            {
                foreach (EyeAnimator eye in Creature.Eyes)
                {
                    eye.SetClose(Mathf.Sin(t * Mathf.PI));
                }
            }, 
            blinkTime);
        }
        #endregion
    }
}