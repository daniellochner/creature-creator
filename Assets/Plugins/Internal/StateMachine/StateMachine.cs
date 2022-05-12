// State Machine
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class StateMachine<T> : MonoBehaviour where T : StateMachine<T>
    {
        #region Fields
        private BaseState<T> currentState;
        #endregion

        #region Properties
        protected virtual Dictionary<string, BaseState<T>> States { get; set; } = new Dictionary<string, BaseState<T>>();

        protected virtual string StartState { get; }
        #endregion

        #region Methods
        public virtual void Awake()
        {
            Initialize();
            ChangeState(StartState);
        }
        public virtual void Update()
        {
            currentState?.UpdateLogic();
        }
        public virtual void FixedUpdate()
        {
            currentState?.UpdatePhysics();
        }

        protected virtual void Initialize()
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