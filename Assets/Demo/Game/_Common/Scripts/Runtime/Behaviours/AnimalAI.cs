// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;
using UnityEngine.AI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AnimalAI : StateMachine
    {
        #region Fields
        [Header("Animal")]
        [SerializeField] private TextAsset data;

        protected CreatureSourceNonPlayer creature;
        protected NavMeshAgent agent;
        #endregion

        #region Properties
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
            creature = GetComponentInParent<CreatureSourceNonPlayer>();
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
            creature.Animator.InteractTarget = target;
            ChangeState("FOL");
        }
        public virtual void StopFollowing()
        {
            ChangeState(startStateID);
        }

        [ContextMenu("Debug/FollowPlayer")]
        public void FollowPlayer()
        {
            Follow(Player.Instance.Creature.transform);
        }
        #endregion

        #region States
        public override void Reset()
        {
            states.Add(new Idling(this));
            states.Add(new Wandering(this));
            states.Add(new Following(this));
        }

        [Serializable]
        public class Idling : BaseState
        {
            [SerializeField] public string[] noises;
            [SerializeField] public MinMax noiseCooldown;
            private float silentTimeLeft;

            public AnimalAI AnimalAI => StateMachine as AnimalAI;

            public Idling(AnimalAI animalAI) : base(animalAI) { }

            public override void Enter()
            {
                silentTimeLeft = noiseCooldown.Random;
            }
            public override void UpdateLogic()
            {
                TimerUtility.OnTimer(ref silentTimeLeft, noiseCooldown.Random, Time.deltaTime, MakeRandomAmbientNoise);
            }

            private void MakeRandomAmbientNoise()
            {
                AnimalAI.creature.Effector.PlaySound(noises);
            }
        }
        
        [Serializable]
        public class Wandering : Idling
        {
            [SerializeField] private MinMax wanderCooldown;
            [SerializeField] private Bounds wanderBounds;
            private float idleTimeLeft;

            public Wandering(AnimalAI animalAI) : base(animalAI) { }

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

            public Following(AnimalAI animalAI) : base(animalAI) { }

            public override void UpdateLogic()
            {
                Vector3 displacement = AnimalAI.creature.Animator.InteractTarget.position - AnimalAI.transform.position;
                if (displacement.magnitude > FollowOffset)
                {
                    Vector3 offset = 0.99f * FollowOffset * displacement.normalized; // offset slightly closer to target
                    Vector3 target = AnimalAI.transform.position + (displacement - offset);
                    AnimalAI.agent.SetDestination(target);
                }
            }
            public override void Exit()
            {
                AnimalAI.creature.Animator.InteractTarget = null;
            }
        }
        #endregion
    }
}