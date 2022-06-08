// State Machine
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    [Serializable]
    public abstract class BaseState
    {
        #region Fields
        [SerializeField] private string id;
        [SerializeField, HideInInspector] private StateMachine sm;
        #endregion

        #region Properties
        public string ID
        {
            get => id;
            set => id = value;
        }
        public StateMachine StateMachine
        {
            get => sm;
            set => sm = value;
        }

        public bool IsActive
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public BaseState()
        {

        }
        public BaseState(StateMachine sm)
        {
            StateMachine = sm;
        }
        public BaseState(string id, StateMachine sm)
        {
            ID = id;
            StateMachine = sm;
        }

        public virtual void Enter() { }
        public virtual void UpdateLogic() { }
        public virtual void UpdatePhysics() { }
        public virtual void Exit() { }

        internal void InternalEnter()
        {
            IsActive = true;
            Enter();
        }
        internal void InternalExit()
        {
            IsActive = false;
            Exit();
        }
        #endregion
    }
}