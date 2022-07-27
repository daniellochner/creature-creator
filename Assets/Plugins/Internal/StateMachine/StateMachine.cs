// State Machine
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class StateMachine : MonoBehaviour
    {
        #region Fields
        [SerializeField] protected string startStateID;
        [SerializeField, ReadOnly] protected string currentStateId;
        [SerializeReference] protected List<BaseState> states;

        protected BaseState currentState;
        #endregion

        #region Properties
        public List<BaseState> States => states;
        #endregion

        #region Methods
        public virtual void Start()
        {
            ChangeState(startStateID);
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

            currentState?.InternalExit();
            currentState = states.Find(x => x.ID == id);
            currentStateId = id;
            currentState?.InternalEnter();
        }
        public BaseState GetState(string id)
        {
            return States.Find(x => x.ID == id);
        }
        public T GetState<T>(string id) where T : BaseState
        {
            return GetState(id) as T;
        }
        #endregion
    }
}