// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AnimalAI : StateMachine
    {
        #region Fields
        [Header("Animal")]
        [SerializeField] private TextAsset data;

        public Transform followTarget;
        #endregion

        #region Properties
        public Animator Animator => Creature.Animator.Animator;

        protected CreatureSourceNonPlayer Creature { get; set; }
        protected NavMeshAgent Agent { get; set; }

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
            Creature = GetComponent<CreatureSourceNonPlayer>();
            Agent = GetComponent<NavMeshAgent>();
        }
        public override void Start()
        {
            base.Start();
            Creature.Constructor.Construct(JsonUtility.FromJson<CreatureData>(data.text));
        }

        public virtual void Follow(Transform target)
        {
            followTarget = target;
            ChangeState("FOL");
        }
        public virtual void StopFollowing()
        {
            ChangeState(startStateID);
            followTarget = null;
        }

        #region Debug
        [ContextMenu("Debug/Follow/Player")]
        public void FollowPlayer()
        {
            Follow(Player.Instance.Creature.transform);
        }
        #endregion
        #endregion

        #region Nested
        [Serializable]
        public class Idling : BaseState
        {
            [SerializeField] private MinMax ambientCooldown;
            [SerializeField] private MinMax actionCooldown;
            [SerializeField] private CreatureEffector.Sound[] ambientSounds;
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
                if (actions.Length > 0)
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
                CreatureEffector.Sound sound = ambientSounds[UnityEngine.Random.Range(0, ambientSounds.Length)];

                // Open
                AnimalAI.Animator.SetBool("Mouth_IsOpened", true);
                AnimalAI.Creature.Effector.PlaySound(sound.name, sound.volume);

                // Hold (to make the sound)...
                yield return new WaitForSeconds(AnimalAI.Creature.Effector.SoundFX[sound.name].length);

                // Close
                AnimalAI.Animator.SetBool("Mouth_IsOpened", false);
            }

            private void PerformAction()
            {
                string action = actions[UnityEngine.Random.Range(0, actions.Length)];
                AnimalAI.Animator.SetTrigger(action);
            }
        }

        [Serializable]
        public class Wandering : Idling
        {
            [SerializeField] private MinMax wanderCooldown;
            [SerializeField] private Bounds wanderBounds;
            private float idleTimeLeft;

            public override void Enter()
            {
                base.Enter();
                idleTimeLeft = wanderCooldown.Random;
                AnimalAI.Agent.SetDestination(AnimalAI.transform.position);
            }
            public override void UpdateLogic()
            {
                base.UpdateLogic();
                if (!AnimalAI.IsMovingToPosition)
                {
                    TimerUtility.OnTimer(ref idleTimeLeft, wanderCooldown.Random, Time.deltaTime, delegate
                    {
                        WanderToPosition(wanderBounds?.RandomPointInBounds ?? AnimalAI.transform.position);
                    });
                }
            }
            public override void Exit()
            {
                AnimalAI.Agent.SetDestination(AnimalAI.transform.position);
            }

            private void WanderToPosition(Vector3 position)
            {
                if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                {
                    position = hit.position;
                }
                AnimalAI.Agent.SetDestination(position);
            }
        }

        [Serializable]
        public class Following : BaseState
        {
            [SerializeField] private float baseFollowOffset;

            public AnimalAI AnimalAI => StateMachine as AnimalAI;

            private float FollowOffset => AnimalAI.Creature.Constructor.Dimensions.radius + baseFollowOffset;

            public override void UpdateLogic()
            {
                Vector3 displacement = AnimalAI.followTarget.position - AnimalAI.transform.position;
                if (displacement.magnitude > FollowOffset)
                {
                    Vector3 offset = 0.99f * FollowOffset * displacement.normalized; // offset slightly closer to target
                    Vector3 target = AnimalAI.transform.position + (displacement - offset);
                    AnimalAI.Agent.SetDestination(target);
                }
            }
        }
        #endregion
    }
}