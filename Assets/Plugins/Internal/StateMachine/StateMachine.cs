// State Machine
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class StateMachine : MonoBehaviour
    {
        #region Fields
        [SerializeField] private string startState;
        [SerializeReference] private List<BaseState> states;

        [SerializeField, ReadOnly] private BaseState currentState;
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

        public void AddState(BaseState state)
        {
            states.Add(state);
        }
        public void ChangeState(string id)
        {
            currentState?.Exit();
            currentState = states.Find(x => x.ID == id);
            currentState?.Enter();
        }
        #endregion
    }
}