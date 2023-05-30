// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Spin : CreatureAnimation
    {
        #region Fields
        [SerializeField] private float spinTime;
        [SerializeField] private GameObject spinTrailPrefab;

        private List<GameObject> spinTrails = new List<GameObject>();
        #endregion

        #region Properties
        public Action<ArmAnimator> OnSpinArm { get; set; }
        public Action OnSpin { get; set; }
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.StartCoroutine(SpinRoutine());
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.Constructor.Root.localRotation = Quaternion.identity;

            OnSpinArm = null;
            OnSpin = null;

            ClearTrails();
        }

        private IEnumerator SpinRoutine()
        {
            foreach (ArmAnimator arm in Creature.Arms)
            {
                spinTrails.Add(Instantiate(spinTrailPrefab, arm.LimbConstructor.Extremity, false));
            }

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                Creature.Constructor.Root.localRotation = Quaternion.Euler(0f, Mathf.Lerp(0f, 360f, EasingFunction.EaseInExpo(0f, 1f, t)), 0f);
            },
            spinTime);
            
            OnSpin?.Invoke();

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                Creature.Constructor.Root.localRotation = Quaternion.Euler(0f, Mathf.Lerp(0f, 360f, EasingFunction.EaseOutExpo(0f, 1f, t)), 0f);

                foreach (ArmAnimator arm in Creature.Arms)
                {
                    OnSpinArm?.Invoke(arm);
                }
            },
            spinTime);

            ClearTrails();
        }

        private void ClearTrails()
        {
            foreach (GameObject spinTrail in spinTrails)
            {
                Destroy(spinTrail);
            }
            spinTrails.Clear();
        }
        #endregion
    }
}