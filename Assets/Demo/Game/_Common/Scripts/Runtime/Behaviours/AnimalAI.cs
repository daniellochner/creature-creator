// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
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

        protected CreatureSourceNonPlayer creature;
        protected NavMeshAgent agent;
        protected Transform followTarget;
        #endregion

        #region Properties
        public Animator Animator => creature.Animator.Animator;

        public bool IsMovingToPosition
        {
            get
            {
                if (agent.isPathStale || agent.isStopped)
                {
                    return false;
                }
                else if (agent.pathPending)
                {
                    return true;
                }

                return (agent.remainingDistance > agent.stoppingDistance);
            }
        }
        #endregion

        #region Methods
        public override void Awake()
        {
            base.Awake();
            creature = GetComponent<CreatureSourceNonPlayer>();
            agent = GetComponent<NavMeshAgent>();
        }
        public virtual void Start()
        {
            creature.Setup();
            creature.Constructor.Construct(JsonUtility.FromJson<CreatureData>(data.text));
            creature.Animator.IsAnimated = true;
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

        #region Inner Classes
        [Serializable]
        public class Idling : BaseState
        {
            [SerializeField] public MinMax ambientCooldown;
            [SerializeField] public CreatureEffector.Sound[] ambientSounds;
            private float silentTimeLeft;

            public AnimalAI AnimalAI => StateMachine as AnimalAI;

            public override void Enter()
            {
                silentTimeLeft = ambientCooldown.Random;
            }
            public override void UpdateLogic()
            {
                if (ambientSounds.Length > 0)
                {
                    TimerUtility.OnTimer(ref silentTimeLeft, ambientCooldown.Random, Time.deltaTime, MakeRandomAmbientSound);
                }
            }

            private void MakeRandomAmbientSound()
            {
                AnimalAI.creature.Effector.PlaySound(ambientSounds);
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
                AnimalAI.agent.SetDestination(AnimalAI.transform.position);
            }

            private void WanderToPosition(Vector3 position)
            {
                if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                {
                    position = hit.position;
                }
                AnimalAI.agent.SetDestination(position);
            }
        }

        [Serializable]
        public class Following : BaseState
        {
            [SerializeField] private float baseFollowOffset;

            public AnimalAI AnimalAI => StateMachine as AnimalAI;

            private float FollowOffset => AnimalAI.creature.Constructor.Dimensions.radius + baseFollowOffset;

            public override void UpdateLogic()
            {
                Vector3 displacement = AnimalAI.followTarget.position - AnimalAI.transform.position;
                if (displacement.magnitude > FollowOffset)
                {
                    Vector3 offset = 0.99f * FollowOffset * displacement.normalized; // offset slightly closer to target
                    Vector3 target = AnimalAI.transform.position + (displacement - offset);
                    AnimalAI.agent.SetDestination(target);
                }
            }
        }
        #endregion
    }
}