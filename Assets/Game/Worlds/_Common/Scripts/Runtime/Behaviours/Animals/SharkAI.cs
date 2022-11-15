// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DanielLochner.Assets.CreatureCreator.Animations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SharkAI : AnimalAI
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

            public SharkAI SharkAI => StateMachine as SharkAI;

            public override void UpdateLogic()
            {
                base.UpdateLogic();
                if (!AnimalAI.IsMovingToPosition)
                {
                    current = (current + 1) % waypoints.Length;
                    SharkAI.Agent.SetDestination(waypoints[current].position);
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

            public SharkAI SharkAI => StateMachine as SharkAI;

            private float TargetDistance => SharkAI.Creature.Constructor.Dimensions.radius + target.Constructor.Dimensions.radius;
            
            public override void Enter()
            {
                base.Enter();
                bitingCoroutine = SharkAI.StartCoroutine(BitingRoutine());
                SharkAI.Animator.GetBehaviour<Bite>().OnBite += OnBite;
            }
            public override void UpdateLogic()
            {
                if (!SharkAI.IsAnimationState("Strike"))
                {
                    UpdateLookDir();

                    Vector3 offset = lookDir * TargetDistance;
                    SharkAI.Agent.SetDestination(target.transform.position - offset);

                    HandleLookAt();

                    NavMeshPath path = new NavMeshPath();
                    SharkAI.Agent.CalculatePath(target.transform.position, path);
                    if (path.status != NavMeshPathStatus.PathComplete)
                    {
                        SharkAI.trackRegion.enabled = false;
                        SharkAI.ChangeState("SWI");
                        SharkAI.Invoke(delegate { SharkAI.trackRegion.enabled = true; }, 5f);
                    }
                }
            }
            public override void Exit()
            {
                base.Exit();
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
                    Vector3 head = SharkAI.Creature.Animator.Mouths[0].transform.position;
                    Vector3 displacement = Vector3.ProjectOnPlane(target.transform.position - head, SharkAI.Creature.transform.up);
                    float d = displacement.magnitude;
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
                        if (creature != null && creature != SharkAI.Creature)
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