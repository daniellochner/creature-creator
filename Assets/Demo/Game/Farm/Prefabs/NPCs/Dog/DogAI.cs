// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DogAI : AnimalAI
    {
        #region States
        public override void Reset()
        {
            base.Reset();
            states.Add(new Barking(this));
            states.Add(new Scurrying(this));
            states.Add(new Biting(this));
        }

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