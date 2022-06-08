// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Strike : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float strikeTime;
        [SerializeField] private float strikeRadius;
        [SerializeField] private float strikeDamage;
        [SerializeField] private CreatureEffector.Sound[] strikeSounds;
        [Space]
        [SerializeField] private float returnTime;
        #endregion

        #region Properties
        private Transform Head
        {
            get => m_MonoBehaviour.Constructor.Bones[m_MonoBehaviour.Constructor.Bones.Count - 1];
        }
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Vector3 displacement = Vector3.ProjectOnPlane(m_MonoBehaviour.InteractTarget.position - Head.position, m_MonoBehaviour.transform.up);
            m_MonoBehaviour.StartCoroutine(StrikeRoutine(displacement));

            m_MonoBehaviour.Effector.PlaySound(strikeSounds);
        }

        private IEnumerator StrikeRoutine(Vector3 displacement)
        {
            Vector3 startPosition  = m_MonoBehaviour.transform.position;
            Vector3 targetPosition = startPosition + displacement;

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                m_MonoBehaviour.Constructor.Body.position = Vector3.Lerp(startPosition, targetPosition, EasingFunction.EaseOutExpo(0f, 1f, t));
            },
            strikeTime);

            Collider[] colliders = Physics.OverlapSphere(Head.position, strikeRadius);
            foreach (Collider collider in colliders)
            {
                CreatureBase creature = collider.GetComponent<CreatureBase>();
                if (creature != null && creature.Animator != m_MonoBehaviour) // striking creature shouldn't damage itself
                {
                    creature.Health.TakeDamage(strikeDamage);
                }
            }

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float t)
            {
                m_MonoBehaviour.Constructor.Body.position = Vector3.Lerp(targetPosition, startPosition, t);
            }, 
            returnTime);
        }
        #endregion
    }
}