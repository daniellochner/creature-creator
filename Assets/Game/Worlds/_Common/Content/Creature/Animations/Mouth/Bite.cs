// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Bite : CreatureAnimation
    {
        #region Fields
        [SerializeField] private float timeR2O;
        [SerializeField] private float timeO2C;
        [SerializeField] private float timeC2R;
        #endregion

        #region Properties
        public Action<MouthAnimator> OnBite { get; set; }
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (MouthAnimator mouth in Creature.Mouths)
            {
                Creature.StartCoroutine(BiteRoutine(mouth));
            }
        }

        private IEnumerator BiteRoutine(MouthAnimator mouth)
        {
            // Rest -> Open
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
            {
                mouth.SetOpen(0.5f + (p * 0.5f));
            },
            timeR2O);

            // Bite
            OnBite?.Invoke(mouth);
            
            // Open -> Close
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
            {
                mouth.SetOpen(1f - p);
            },
            timeO2C);

            // Close -> Rest
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
            {
                mouth.SetOpen(p * 0.5f);
            }, 
            timeC2R);
        }
        #endregion
    }
}