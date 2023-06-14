// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DanielLochner.Assets.CreatureCreator.Animations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FishAI : AnimalAI
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

                trackRegion.OnLoseTrackOf += delegate
                {
                    if (Creature.Health.Health <= 0) return;

                    if (trackRegion.tracked.Count == 0)
                    {
                        ChangeState("SWI");
                    }
                };
            }
        }
        public override void Update()
        {
            base.Update();
            if (PVE)
            {
                if (trackRegion.tracked.Count > 0 && !(currentState is Biting) && !(currentState is Following))
                {
                    ChangeState("BIT");
                }
            }
        }
        #endregion

        #region Nested
        [Serializable]
        public class Swimming : Idling
        {
            public Transform[] waypoints;
            public int current;

            public FishAI FishAI => StateMachine as FishAI;

            public override void Enter()
            {
                base.Enter();
                AnimalAI.Agent.ResetPath();
            }
            public override void UpdateLogic()
            {
                base.UpdateLogic();

                if (AnimalAI.Region == null || AnimalAI.Region.tracked.Count == 0)
                {
                    if (!AnimalAI.IsMovingToPosition)
                    {
                        current = (current + 1) % waypoints.Length;
                        FishAI.Agent.SetDestination(waypoints[current].position);
                    }
                }
                else
                {
                    if (AnimalAI.Target != null)
                    {
                        AnimalAI.Agent.SetDestination(AnimalAI.Target.position);
                    }
                    else
                    if (AnimalAI.Region.tracked.Count > 0)
                    {
                        AnimalAI.Target = AnimalAI.MoveToRandomPlayer();
                    }
                }
            }
        }

        [Serializable]
        public class Biting : Targeting
        {
            [SerializeField] private float biteMaxDistance;
            [SerializeField] private float minBiteAngle;
            [SerializeField] private MinMax biteDelay;
            [SerializeField] private float biteRadius;
            [SerializeField] private MinMax biteDamage;
            [SerializeField] private PlayerEffects.Sound[] biteSounds;
            private Coroutine bitingCoroutine;
            private bool hasDealtDamage;

            public FishAI FishAI => StateMachine as FishAI;
            
            public override void Enter()
            {
                base.Enter();
                UpdateTarget();

                FishAI.Agent.ResetPath();
                bitingCoroutine = FishAI.StartCoroutine(BitingRoutine());
            }
            public override void UpdateLogic()
            {
                if (!FishAI.IsAnimationState("Strike"))
                {
                    UpdateLookDir();

                    Vector3 offset = lookDir * GetTargetDistance(FishAI.Creature, target);
                    FishAI.Agent.SetDestination(target.transform.position - offset);

                    HandleLookAt();

                    NavMeshPath path = new NavMeshPath();
                    FishAI.Agent.CalculatePath(target.transform.position, path);
                    if (path.status != NavMeshPathStatus.PathComplete)
                    {
                        FishAI.ChangeState("SWI");
                    }
                }
            }
            public override void Exit()
            {
                base.Exit();
                FishAI.StopCoroutine(bitingCoroutine);
            }

            private IEnumerator BitingRoutine()
            {
                while (IsActive)
                {
                    // Move Closer.
                    float angle = Mathf.Infinity, distance = Mathf.Infinity;
                    while (angle > minBiteAngle || distance > GetTargetDistance(FishAI.Creature, target, biteMaxDistance))
                    {
                        UpdateTarget();
                        angle = Vector3.Angle(FishAI.transform.forward, lookDir);
                        distance = Vector3.Distance(target.transform.position, FishAI.transform.position);
                        yield return null;
                    }

                    // Strike!
                    hasDealtDamage = false;
                    Vector3 head = FishAI.Creature.Animator.Mouths[0].transform.position;
                    Vector3 displacement = Vector3.ProjectOnPlane(target.transform.position - head, FishAI.Creature.transform.up);
                    float d = displacement.magnitude;

                    FishAI.Animator.GetBehaviour<Bite>().OnBiteMouth = OnBiteMouth;
                    FishAI.Animator.GetBehaviour<Bite>().OnBite = OnBite;

                    FishAI.Params.SetTriggerWithValue("Body_Strike", "Body_Strike_Distance", d);

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
                        if (creature != null && creature != FishAI.Creature)
                        {
                            creature.Health.TakeDamage(biteDamage.Random, DamageReason.BiteAttack, FishAI.Creature.Constructor.Data.Name);
                            hasDealtDamage = true;
                        }
                    }
                }
            }
            private void OnBite()
            {
                FishAI.Creature.Effects.PlaySound(biteSounds);
            }
        }
        #endregion
    }
}