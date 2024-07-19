// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DanielLochner.Assets.CreatureCreator.Animations;
using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SpiderAI : AnimalAI
    {
        #region Fields
        [SerializeField] protected TrackRegion trackRegion;
        #endregion

        #region Methods
        public override void Start()
        {
            base.Start();

            if (PVE)
            {
                SetupTrackRegionBuffer(trackRegion);

                trackRegion.OnTrack += delegate (Collider col)
                {
                    if (currentStateId == "WAN")
                    {
                        ChangeState("BIT");
                    }
                };
                trackRegion.OnLoseTrackOf += delegate
                {
                    if (Creature.Health.Health <= 0) return;

                    if (trackRegion.tracked.Count == 0)
                    {
                        ChangeState("WAN");
                    }
                };
            }
        }
        #endregion

        #region Nested
        [Serializable]
        public class Biting : Targeting
        {
            [SerializeField] private float maxBiteDistance;
            [SerializeField] private float minBiteAngle;
            [SerializeField] private MinMax biteDelay;
            [SerializeField] private float biteRadius;
            [SerializeField] private MinMax biteDamage;
            [SerializeField] private PlayerEffects.Sound[] biteSounds;
            private Coroutine bitingCoroutine;
            private bool hasDealtDamage;

            public SpiderAI SpiderAI => StateMachine as SpiderAI;
            
            public override void Enter()
            {
                base.Enter();
                SpiderAI.ResetPath();

                SpiderAI.StopStartCoroutine(BitingRoutine(), ref bitingCoroutine);
            }
            public override void UpdateLogic()
            {
                if (!SpiderAI.IsAnimationState("Strike"))
                {
                    UpdateLookDir();

                    Vector3 offset = lookDir * GetTargetDistance(SpiderAI.Creature, target);
                    SpiderAI.Agent.SetDestination(target.transform.position - offset);

                    HandleLookAt();
                }
            }
            public override void Exit()
            {
                base.Exit();
                SpiderAI.TryStopCoroutine(bitingCoroutine);
            }

            private IEnumerator BitingRoutine()
            {
                while (IsActive)
                {
                    // Move Closer.
                    float angle = Mathf.Infinity, distance = Mathf.Infinity;
                    while (angle > minBiteAngle || distance > GetTargetDistance(SpiderAI.Creature, target, maxBiteDistance))
                    {
                        UpdateTarget();
                        angle = Vector3.Angle(SpiderAI.transform.forward, lookDir);
                        distance = Vector3.Distance(target.transform.position, SpiderAI.transform.position);
                        yield return null;
                    }

                    // Strike!
                    hasDealtDamage = false;
                    Vector3 head = SpiderAI.Creature.Animator.Mouths[0].transform.position;
                    Vector3 displacement = Vector3.ProjectOnPlane(target.transform.position - head, SpiderAI.Creature.transform.up);
                    float d = displacement.magnitude;

                    SpiderAI.Animator.GetBehaviour<Bite>().OnBiteMouth = OnBiteMouth;
                    SpiderAI.Animator.GetBehaviour<Bite>().OnBite = OnBite;

                    SpiderAI.Params.SetTriggerWithValue("Body_Strike", "Body_Strike_Distance", d);

                    // Wait...
                    yield return new WaitForSeconds(biteDelay.Random);
                }
            }

            private void OnBiteMouth(MouthAnimator mouth)
            {
                if (!hasDealtDamage)
                {
                    Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, biteRadius);
                    foreach (Collider collider in colliders)
                    {
                        CreatureBase creature = collider.GetComponent<CreatureBase>();
                        if (creature != null && creature != SpiderAI.Creature)
                        {
                            creature.Health.TakeDamage(biteDamage.Random, DamageReason.Bite);
                            hasDealtDamage = true;
                        }
                    }
                }
            }
            private void OnBite()
            {
                SpiderAI.Creature.Effects.PlaySound(biteSounds);
            }
        }
        #endregion
    }
}