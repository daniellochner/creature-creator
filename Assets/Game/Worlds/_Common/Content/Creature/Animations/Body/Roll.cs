// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Roll : CreatureAnimation
    {
        #region Fields
        [SerializeField] private float rollTime;
        [SerializeField] private float rollDistance;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.StartCoroutine(RollRoutine(1));
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.Constructor.Root.localRotation = Quaternion.identity;
            Creature.Constructor.Root.localPosition = Vector3.zero;
        }

        private IEnumerator RollRoutine(int dir)
        {
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                Creature.Constructor.Root.localRotation = Quaternion.Euler(0f, 0f, (1f - t) * 360f);
                Creature.Constructor.Root.localPosition = Vector3.Lerp(Vector3.zero, dir * Vector3.right * rollDistance, t);
            },
            rollTime);

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                Creature.Constructor.Root.localRotation = Quaternion.Euler(0f, 0f, t * 360f);
                Creature.Constructor.Root.localPosition = Vector3.Lerp(dir * Vector3.right * rollDistance, Vector3.zero, t);
            },
            rollTime);
        }
        #endregion
    }
}