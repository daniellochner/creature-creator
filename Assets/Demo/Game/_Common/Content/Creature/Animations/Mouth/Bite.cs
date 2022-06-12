// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Bite : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float biteRadius;
        [SerializeField] private MinMax biteDamage;
        [SerializeField] private CreatureEffector.Sound[] biteSounds;
        [Space]
        [SerializeField] private float timeR2O;
        [SerializeField] private float timeO2C;
        [SerializeField] private float timeC2R;

        private bool hasDealtDamage;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            hasDealtDamage = false;
            foreach (MouthAnimator mouth in m_MonoBehaviour.Mouths)
            {
                m_MonoBehaviour.StartCoroutine(BiteRoutine(mouth));
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
            if (!hasDealtDamage)
            {
                Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, biteRadius);
                foreach (Collider collider in colliders)
                {
                    CreatureBase creature = collider.GetComponent<CreatureBase>();
                    if (creature != null && creature.Animator != m_MonoBehaviour) // biting creature shouldn't damage itself
                    {
                        creature.Health.TakeDamage(biteDamage.Random);
                        hasDealtDamage = true;
                    }
                }
            }
            m_MonoBehaviour.Effector.PlaySound(biteSounds);

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