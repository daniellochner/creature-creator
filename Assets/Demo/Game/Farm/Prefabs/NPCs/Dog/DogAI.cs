// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DogAI : AnimalAI
    {
        public override void Reset()
        {
            base.Reset();
            AddState(new Barking(this));
            AddState(new Scurrying(this));
            AddState(new Biting(this));
        }

        #region States
        [Serializable]
        public class Barking : BaseState
        {
            private float barkTimeLeft;

            public DogAI DogAI => StateMachine as DogAI;

            public Barking(DogAI dogAI) : base(dogAI) { }

            public override void UpdateLogic()
            {

            }
        }

        [Serializable]
        public class Scurrying : BaseState
        {
            public DogAI DogAI => StateMachine as DogAI;

            public Scurrying(DogAI dogAI) : base(dogAI) { }

            public override void UpdateLogic()
            {
            }
        }

        [Serializable]
        public class Biting : BaseState
        {
            public DogAI DogAI => StateMachine as DogAI;

            public Biting(DogAI dogAI) : base(dogAI) { }

            public override void UpdateLogic()
            {
            }
        }
        #endregion
    }
}