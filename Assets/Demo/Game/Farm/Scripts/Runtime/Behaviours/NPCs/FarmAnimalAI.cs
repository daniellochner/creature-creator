// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;
using UnityEngine.AI;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class FarmAnimalAI<T> : AnimalAI<T> where T : StateMachine<T>
    {
        #region Fields
        [SerializeField] private WanderInfo wander;

        private NavMeshAgent agent;
        #endregion

        #region Methods
        public override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, wander.radius);
        }
        #endregion

        #region Inner Classes
        public class Wandering<S> : BaseState<S> where S : FarmAnimalAI<S>
        {
            private float timeLeft;

            public Wandering(S sm) : base("Wandering", sm) { }

            public override void UpdateLogic()
            {
                TimerUtility.OnTimer(ref timeLeft, StateMachine.wander.cooldown.Random, Time.deltaTime, WanderToRandomPosition);
            }

            private void WanderToRandomPosition()
            {
                Vector3 direction = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up) * StateMachine.transform.forward;
                float distance = UnityEngine.Random.Range(0f, StateMachine.wander.radius);

                Vector3 position = StateMachine.wander.center.position;
                if (NavMesh.SamplePosition(position + (direction * distance), out NavMeshHit hit, 10f, NavMesh.AllAreas))
                {
                    position = hit.position;
                }
                StateMachine.agent.SetDestination(position);
            }
        }

        [Serializable]
        public class WanderInfo
        {
            public float radius;
            public MinMax cooldown;
            public Transform center;
        }
        #endregion
    }
}