// State Machine
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets
{
    public abstract class BaseState<T> where T : StateMachine<T>
    {
        #region Properties
        public virtual string Name { get; private set; }
        public T StateMachine { get; private set; }
        #endregion

        #region Methods
        public BaseState(string name, T stateMachine)
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