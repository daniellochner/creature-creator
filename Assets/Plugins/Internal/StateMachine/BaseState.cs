// State Machine
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets
{
    public abstract class BaseState
    {
        #region Properties
        public string Name { get; private set; }
        public StateMachine StateMachine { get; private set; }
        #endregion

        #region Methods
        public BaseState(string name, StateMachine stateMachine)
        {
            Name = name;
            StateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void UpdateLogic() { }
        public virtual void UpdatePhysics() { }
        public virtual void Exit() { }
        #endregion
    }
}