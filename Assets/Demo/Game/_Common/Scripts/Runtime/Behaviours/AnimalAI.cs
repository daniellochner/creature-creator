// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
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

        protected CreatureSource creature;
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
            creature = GetComponentInParent<CreatureSource>();
            agent = GetComponent<NavMeshAgent>();
        }
        public virtual void Start()
        {
            creature.Setup();
            creature.Constructor.Construct(JsonUtility.FromJson<CreatureData>(data.text));
            creature.Animator.IsAnimated = true;
        }

        public override void Reset()
        {
            base.Reset();
            AddState(new Idling(this));
            AddState(new Wandering(this));
        }
        #endregion

        #region Inner Classes
        [Serializable]
        public class Idling : BaseState
        {
            [SerializeField] public string[] noises;
            [SerializeField] public MinMax noiseCooldown;
            [SerializeField, ReadOnly] private float silentTimeLeft;

            public AnimalAI AnimalAI => StateMachine as AnimalAI;

            public Idling(AnimalAI animalAI) : base(animalAI) { }

            public override void Enter()
            {
                silentTimeLeft = noiseCooldown.Random;
            }
            public override void UpdateLogic()
            {
                if (noises.Length > 0)
                {
                    TimerUtility.OnTimer(ref silentTimeLeft, noiseCooldown.Random, Time.deltaTime, MakeRandomAmbientNoise);
                }
            }

            private void MakeRandomAmbientNoise()
            {
                AnimalAI.creature.Effector.PlaySound(noises[UnityEngine.Random.Range(0, noises.Length)]);
            }
        }

        [Serializable]
        public class Wandering : Idling
        {
            [SerializeField] private MinMax wanderCooldown;
            [SerializeField] private Bounds wanderBounds;
            [SerializeField, ReadOnly] private float idleTimeLeft;

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
        #endregion
    }
}