// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.AI;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class FarmAnimalAI<T> : AnimalAI<T>
    {
        #region Fields
        protected NavMeshAgent agent;
        #endregion

        #region Methods
        public override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
        }
        #endregion

        #region States
        #endregion
    }
}