// State Machine
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class StateMachine<T> : MonoBehaviour
    {
        #region Fields
        private BaseState<T> currentState;
        #endregion

        #region Properties
        protected virtual Dictionary<string, BaseState<T>> States { get; set; }
        #endregion

        #region Methods
        public virtual void Awake()
        {
            InitializeStates();
        }
        public virtual void Update()
        {
            currentState?.UpdateLogic();
        }
        public virtual void FixedUpdate()
        {
            currentState?.UpdatePhysics();
        }

        protected virtual void InitializeStates()
        {
        }
        public void ChangeState(string stateID)
        {
            currentState?.Exit();
            currentState = States[stateID];
            currentState.Enter();
        }
        #endregion
    }
}