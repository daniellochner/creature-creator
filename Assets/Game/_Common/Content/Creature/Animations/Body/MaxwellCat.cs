// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class MaxwellCat : CreatureAnimation
    {
        #region Fields
        private Coroutine danceCoroutine;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            danceCoroutine = Creature.StartCoroutine(DanceRoutine());
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.Constructor.Root.localPosition = Vector3.zero;
            Creature.Constructor.Root.localRotation = Quaternion.identity;

            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Dynamic.Transform);
            }

            Creature.Effector.StopMySounds();
            Creature.StopCoroutine(danceCoroutine);
        }

        private IEnumerator DanceRoutine()
        {
            for (int i = 0; i < 2; i++)
            {
                yield return SwayRoutine(3.5f);

                Creature.Constructor.Root.localPosition = Vector3.zero;
                Creature.Constructor.Root.localRotation = Quaternion.identity;

                yield return SpinRoutine(3.5f);

                Creature.Constructor.Root.localPosition = Vector3.zero;
                Creature.Constructor.Root.localRotation = Quaternion.identity;
            }
        }
        private IEnumerator SwayRoutine(float duration)
        {
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
            {
                float a = Mathf.Cos(8f * p * Mathf.PI) * 120f;
                Creature.Constructor.Root.RotateAround(Creature.transform.position, Creature.transform.forward, a * Mathf.Deg2Rad);
            },
            duration);
        }
        private IEnumerator SpinRoutine(float duration)
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Creature.Constructor.Root);
            }
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
            {
                float d = p * 2 * 360f;
                Creature.Constructor.Root.localRotation = Quaternion.Euler(0f, d, 0f);
            },
            duration);
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Dynamic.Transform);
            }
        }
        #endregion
    }
}