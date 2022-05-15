// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SnakeAI : AnimalAI
    {
        public override void Reset()
        {
            base.Reset();
            AddState(new Striking(this));
        }

        public void Strike(Collider other)
        {
            Debug.Log("STRIKE");
        }

        [Serializable]
        public class Striking : BaseState
        {
            public Striking(SnakeAI snakeAI) : base(snakeAI) { }

            public override void UpdateLogic()
            {

            }
        }
    }
}