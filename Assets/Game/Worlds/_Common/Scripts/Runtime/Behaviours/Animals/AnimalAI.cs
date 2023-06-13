// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AnimalAI : StateMachine
    {
        #region Fields
        [SerializeField] private TrackRegion ignoreRegion;

        private List<TrackRegion> trackRegions;
        #endregion

        #region Properties
        public AnimatorParams Params => Creature.Animator.Params;
        public Animator Animator => Creature.Animator.Animator;

        public CreatureNonPlayerLocal Creature { get; set; }
        public NavMeshAgent Agent { get; set; }

        public Transform Target
        {
            get;
            set;
        }
        public Battle Battle
        {
            get;
            set;
        }
        public bool PVE
        {
            get;
            set;
        }
        public bool CanFollow
        {
            get => currentState is Idling || currentState is Following;
        }
        public bool IsMovingToPosition
        {
            get
            {
                if (Agent.isPathStale || Agent.isStopped)
                {
                    return false;
                }
                else if (Agent.pathPending)
                {
                    return true;
                }

                return (Agent.remainingDistance > Agent.stoppingDistance);
            }
        }
        #endregion

        #region Methods
        public void Awake()
        {
            Creature = GetComponent<CreatureNonPlayerLocal>();
            Agent = GetComponent<NavMeshAgent>();

            if (ignoreRegion != null)
            {
                trackRegions = new List<TrackRegion>(GetComponentsInChildren<TrackRegion>(true));
                trackRegions.Remove(ignoreRegion);
            }
        }

        public void Setup()
        {
            Agent.speed *= Creature.Constructor.Statistics.Speed;
        }

        public void SetupTrackRegionBuffer(TrackRegion region)
        {
            region.OnTrack += delegate
            {
                if (region.tracked.Count == 1)
                {
                    (region.Region as SphereCollider).radius *= 1.5f;
                }
            };
            region.OnLoseTrackOf += delegate
            {
                if (region.tracked.Count == 0)
                {
                    (region.Region as SphereCollider).radius /= 1.5f;
                }
            };
        }

        public virtual void Follow(Transform target)
        {
            Target = target;
            ChangeState("FOL");
        }
        public virtual void StopFollowing()
        {
            ChangeState(startStateID);
            Target = null;
        }
 
        public void UpdateIgnored()
        {
            List<string> ignored = new List<string>();
            foreach (var client in NetworkManager.Singleton.ConnectedClients)
            {
                CreaturePlayer player = client.Value.PlayerObject.GetComponent<CreaturePlayer>();
                if (Creature.Comparer.CompareTo(player.Constructor))
                {
                    ignored.Add(player.name);
                }
            }

            foreach (TrackRegion region in trackRegions)
            {
                region.ignored = ignored;
            }
        }

        public Transform MoveToRandomPlayer()
        {
            if (Battle.Players.Count > 0)
            {
                Transform player = Battle.Players[UnityEngine.Random.Range(0, Battle.Players.Count - 1)].transform;
                MoveToPosition(player.position);
                return player;
            }
            return null;
        }
        public void MoveToPosition(Vector3 position)
        {
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                position = hit.position;
            }
            Agent.SetDestination(position);
        }

        public bool IsAnimationState(string state)
        {
            return Animator.GetCurrentAnimatorStateInfo(0).IsName(state);
        }

        public static float GetTargetDistance(CreatureBase creature, CreatureBase other, float offset = 0f)
        {
            return (creature.Constructor.Dimensions.Radius + other.Constructor.Dimensions.Radius) / 2f + offset;
        }
        public static float GetTargetDistance(CreatureBase creature, float offset = 0f)
        {
            return (creature.Constructor.Dimensions.Radius / 2f) + offset;
        }

        #region Debug
        [ContextMenu("Debug/Follow/Player")]
        public void FollowPlayer()
        {
            Follow(Player.Instance.transform);
        }
        #endregion
        #endregion

        #region Nested
        [Serializable]
        public class Idling : BaseState
        {
            [SerializeField] private MinMax ambientCooldown;
            [SerializeField] private MinMax actionCooldown;
            [SerializeField] private PlayerEffects.Sound[] ambientSounds;
            [SerializeField] private string[] actions;
            protected float silentTimeLeft, actionTimeLeft;

            public AnimalAI AnimalAI => StateMachine as AnimalAI;

            public override void Enter()
            {
                silentTimeLeft = ambientCooldown.Random;
                actionTimeLeft = actionCooldown.Random;
            }
            public override void UpdateLogic()
            {
                if (ambientSounds.Length > 0)
                {
                    TimerUtility.OnTimer(ref silentTimeLeft, ambientCooldown.Random, Time.deltaTime, MakeSound);
                }
                if (actions.Length > 0 && AnimalAI.IsAnimationState("Idling"))
                {
                    TimerUtility.OnTimer(ref actionTimeLeft, actionCooldown.Random, Time.deltaTime, PerformAction);
                }
            }

            private void MakeSound()
            {
                AnimalAI.StartCoroutine(MakeSoundRoutine());
            }
            private IEnumerator MakeSoundRoutine()
            {
                PlayerEffects.Sound sound = ambientSounds[UnityEngine.Random.Range(0, ambientSounds.Length)];

                // Open
                AnimalAI.Params.SetBool("Mouth_IsOpen", true);
                AnimalAI.Creature.Effects.PlaySound(sound.name, sound.volume);

                // Hold (to make the sound)...
                yield return new WaitForSeconds(AnimalAI.Creature.Effects.SoundFX[sound.name].length);

                // Close
                AnimalAI.Params.SetBool("Mouth_IsOpen", false);
            }

            private void PerformAction()
            {
                string action = actions[UnityEngine.Random.Range(0, actions.Length)];
                AnimalAI.Params.SetTrigger(action);
            }
        }

        [Serializable]
        public class Wandering : Idling
        {
            [SerializeField] private MinMax wanderCooldown;
            [SerializeField] public Bounds wanderBounds;
            private float idleTimeLeft;

            public override void Enter()
            {
                base.Enter();
                AnimalAI.Agent.ResetPath();
                idleTimeLeft = wanderCooldown.Random;
                AnimalAI.Creature.Health.OnTakeDamage += OnTakeDamage;
            }
            public override void UpdateLogic()
            {
                base.UpdateLogic();

                if (AnimalAI.Battle == null || AnimalAI.Battle.Players.Count == 0)
                {
                    if (!AnimalAI.IsMovingToPosition)
                    {
                        TimerUtility.OnTimer(ref idleTimeLeft, wanderCooldown.Random, Time.deltaTime, delegate
                        {
                            WanderToRandomPosition();
                        });
                    }
                }
                else
                {
                    if (AnimalAI.Target != null)
                    {
                        AnimalAI.Agent.SetDestination(AnimalAI.Target.position);
                    }
                    else 
                    if (AnimalAI.Battle.Players.Count > 0)
                    {
                        AnimalAI.Target = AnimalAI.MoveToRandomPlayer();
                    }
                }
            }
            public override void Exit()
            {
                AnimalAI.Creature.Health.OnTakeDamage -= OnTakeDamage;
            }

            public void WanderToRandomPosition()
            {
                AnimalAI.MoveToPosition(wanderBounds?.RandomPointInBounds ?? AnimalAI.transform.position);
            }

            private void OnTakeDamage(float damage, DamageReason reason, string inflicter)
            {
                idleTimeLeft = 0;
            }
        }

        [Serializable]
        public class Following : BaseState
        {
            [SerializeField] private float baseFollowOffset;
            [SerializeField] private UnityEvent onFollow;
            [SerializeField] private UnityEvent onStopFollowing;

            public AnimalAI AnimalAI => StateMachine as AnimalAI;

            private float FollowOffset => GetTargetDistance(AnimalAI.Creature, baseFollowOffset);

            public override void Enter()
            {
                onFollow.Invoke();
            }
            public override void UpdateLogic()
            {
                if (AnimalAI.Target != null)
                {
                    Vector3 displacement = AnimalAI.Target.transform.position - AnimalAI.transform.position;
                    if (displacement.magnitude > FollowOffset)
                    {
                        Vector3 offset = 0.99f * FollowOffset * displacement.normalized; // offset slightly closer to target
                        Vector3 target = AnimalAI.transform.position + (displacement - offset);
                        AnimalAI.Agent.SetDestination(target);
                    }
                }
            }
            public override void Exit()
            {
                onStopFollowing.Invoke();
            }
        }

        public class Targeting : BaseState
        {
            [SerializeField] protected TrackRegion trackRegion;
            [SerializeField] protected float lookAtSmoothing;

            protected CreatureBase target;
            protected Vector3 lookDir;

            public AnimalAI AnimalAI => StateMachine as AnimalAI;

            protected void UpdateTarget()
            {
                Transform nearest = trackRegion.Nearest.transform;
                if (target == null || target.transform != nearest)
                {
                    target = nearest.GetComponent<CreatureBase>();
                }
            }
            protected void HandleLookAt()
            {
                StateMachine.transform.rotation = Quaternion.Slerp(StateMachine.transform.rotation, Quaternion.LookRotation(lookDir), lookAtSmoothing * Time.deltaTime);
            }
            protected void UpdateLookDir()
            {
                lookDir = Vector3.ProjectOnPlane(target.transform.position - StateMachine.transform.position, StateMachine.transform.up).normalized;
            }

            public override void Enter()
            {
                base.Enter();
                AnimalAI.Agent.updateRotation = false;
            }

            public override void Exit()
            {
                base.Exit();
                AnimalAI.Agent.updateRotation = true;
            }
        }
        #endregion
    }
}