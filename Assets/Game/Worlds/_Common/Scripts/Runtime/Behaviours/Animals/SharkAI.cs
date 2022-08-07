// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DanielLochner.Assets.CreatureCreator.Animations;
using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SharkAI : AnimalAI
    {
        #region Fields
        [SerializeField] protected TrackRegion trackRegion;
        #endregion

        #region Methods
        public void Attack(Collider col)
        {
            ChangeState("BIT");
        }
        #endregion

        #region Nested
        [Serializable]
        public class Biting : Targeting
        {
            [SerializeField] private float biteOffset;
            [SerializeField] private float minBiteAngle;
            [SerializeField] private MinMax biteDelay;
            [SerializeField] private float biteRadius;
            [SerializeField] private MinMax biteDamage;
            [SerializeField] private PlayerEffects.Sound[] biteSounds;
            private Coroutine bitingCoroutine;
            private bool hasDealtDamage;

            public SharkAI SharkAI => StateMachine as SharkAI;

            private float TargetDistance => SharkAI.Creature.Constructor.Dimensions.radius + target.Constructor.Dimensions.radius;
            
            public override void Enter()
            {
                bitingCoroutine = SharkAI.StartCoroutine(BitingRoutine());
                SharkAI.Animator.GetBehaviour<Bite>().OnBite += OnBite;
            }
            public override void UpdateLogic()
            {
                if (!SharkAI.Animator.GetCurrentAnimatorStateInfo(0).IsName("Strike"))
                {
                    UpdateLookDir();

                    Vector3 offset = lookDir * TargetDistance;
                    SharkAI.Agent.SetDestination(target.transform.position - offset);

                    if (!SharkAI.IsMovingToPosition)
                    {
                        HandleLookAt();
                    }
                }
            }
            public override void Exit()
            {
                SharkAI.StopCoroutine(bitingCoroutine);
                SharkAI.Animator.GetBehaviour<Bite>().OnBite -= OnBite;
            }

            private IEnumerator BitingRoutine()
            {
                while (IsActive)
                {
                    // Move Closer.
                    float angle = Mathf.Infinity, distance = Mathf.Infinity;
                    while (angle > minBiteAngle || distance > (TargetDistance + biteOffset))
                    {
                        UpdateTarget();
                        angle = Vector3.Angle(SharkAI.transform.forward, lookDir);
                        distance = Vector3.Distance(target.transform.position, SharkAI.transform.position);
                        yield return null;
                    }

                    // Strike!
                    hasDealtDamage = false;
                    float d = Vector3.Distance(target.transform.position, SharkAI.transform.position);
                    SharkAI.Params.SetTriggerWithValue("Body_Strike", "Body_Strike_Distance", d);

                    // Wait...
                    yield return new WaitForSeconds(biteDelay.Random);
                }
            }

            private void OnBite(MouthAnimator mouth)
            {
                SharkAI.Creature.Effects.PlaySound(biteSounds);
                if (!hasDealtDamage)
                {
                    Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, biteRadius);
                    foreach (Collider collider in colliders)
                    {
                        CreatureBase creature = collider.GetComponent<CreatureBase>();
                        if (creature != null && creature.Animator != SharkAI.Creature)
                        {
                            creature.Health.TakeDamage(biteDamage.Random);
                            hasDealtDamage = true;
                        }
                    }
                }
            }
        }
        #endregion
    }
}