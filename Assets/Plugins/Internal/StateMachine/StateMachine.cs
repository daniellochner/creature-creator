// State Machine
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class StateMachine : MonoBehaviour
    {
        #region Fields
        [SerializeField] protected string startState;
        [SerializeReference] protected List<BaseState> states;

        [SerializeField, ReadOnly] protected BaseState currentState;
        #endregion

        #region Methods
        public virtual void Awake()
        {
            ChangeState(startState);
        }
        public virtual void Reset()
        {
        }

        public virtual void Update()
        {
            currentState?.UpdateLogic();
        }
        public virtual void FixedUpdate()
        {
            currentState?.UpdatePhysics();
        }

        public void ChangeState(string id)
        {
            if (currentState != null && currentState.ID == id) return;

            currentState?.Exit();
            currentState = states.Find(x => x.ID == id);
            currentState?.Enter();
        }
        #endregion
    }
}