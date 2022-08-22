// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Strike : CreatureAnimation
    {
        #region Fields
        [SerializeField] private string strikeAction;
        [SerializeField] private float strikeTime;
        [SerializeField] private float returnTime;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Vector3 displacement = (animator.GetFloat("Body_Strike_Distance") * Creature.transform.forward);
            Creature.StartCoroutine(StrikeRoutine(displacement));
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.Constructor.Root.localPosition = Vector3.zero;
        }

        private IEnumerator StrikeRoutine(Vector3 displacement)
        {
            Vector3 localDisplacement = Creature.Constructor.Root.InverseTransformVector(displacement);

            Creature.Animator.SetTrigger(strikeAction);
            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                Creature.Constructor.Root.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.LookRotation(localDisplacement), t);
                Creature.Constructor.Root.localPosition = Vector3.Lerp(Vector3.zero, localDisplacement, EasingFunction.EaseOutExpo(0f, 1f, t));
            },
            strikeTime);

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                Creature.Constructor.Root.localRotation = Quaternion.Slerp(Quaternion.LookRotation(localDisplacement), Quaternion.identity, t);
                Creature.Constructor.Root.localPosition = Vector3.Lerp(localDisplacement, Vector3.zero, t);
            }, 
            returnTime);
        }
        #endregion
    }
}