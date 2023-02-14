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

            public override void UpdateLogic()
            {
                base.UpdateLogic();
                if (!AnimalAI.IsMovingToPosition)
                {
                    current = (current + 1) % waypoints.Length;
                    FishAI.Agent.SetDestination(waypoints[current].position);
                }
            }
        }

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

            public FishAI FishAI => StateMachine as FishAI;

            private float TargetDistance => FishAI.Creature.Constructor.Dimensions.radius + target.Constructor.Dimensions.radius;
            
            public override void Enter()
            {
                base.Enter();
                bitingCoroutine = FishAI.StartCoroutine(BitingRoutine());
                FishAI.Animator.GetBehaviour<Bite>().OnBite += OnBite;
            }
            public override void UpdateLogic()
            {
                if (!FishAI.IsAnimationState("Strike"))
                {
                    UpdateLookDir();

                    Vector3 offset = lookDir * TargetDistance;
                    FishAI.Agent.SetDestination(target.transform.position - offset);

                    HandleLookAt();

                    NavMeshPath path = new NavMeshPath();
                    FishAI.Agent.CalculatePath(target.transform.position, path);
                    if (path.status != NavMeshPathStatus.PathComplete)
                    {
                        FishAI.trackRegion.enabled = false;
                        FishAI.ChangeState("SWI");
                        FishAI.Invoke(delegate { FishAI.trackRegion.enabled = true; }, 5f);
                    }
                }
            }
            public override void Exit()
            {
                base.Exit();
                FishAI.StopCoroutine(bitingCoroutine);
                FishAI.Animator.GetBehaviour<Bite>().OnBite -= OnBite;
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
                        angle = Vector3.Angle(FishAI.transform.forward, lookDir);
                        distance = Vector3.Distance(target.transform.position, FishAI.transform.position);
                        yield return null;
                    }

                    // Strike!
                    hasDealtDamage = false;
                    Vector3 head = FishAI.Creature.Animator.Mouths[0].transform.position;
                    Vector3 displacement = Vector3.ProjectOnPlane(target.transform.position - head, FishAI.Creature.transform.up);
                    float d = displacement.magnitude;
                    FishAI.Params.SetTriggerWithValue("Body_Strike", "Body_Strike_Distance", d);
                    
                    // Wait...
                    yield return new WaitForSeconds(biteDelay.Random);
                }
            }

            private void OnBite(MouthAnimator mouth)
            {
                FishAI.Creature.Effects.PlaySound(biteSounds);
                if (!hasDealtDamage)
                {
                    Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, biteRadius);
                    foreach (Collider collider in colliders)
                    {
                        CreatureBase creature = collider.GetComponent<CreatureBase>();
                        if (creature != null && creature != FishAI.Creature)
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