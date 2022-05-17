// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CowAI : AnimalAI
    {
        #region States
        public override void Reset()
        {
            base.Reset();
            states.Add(new Charging(this));
        }

        [Serializable]
        public class Charging : BaseState
        {
            public Charging(CowAI cowAI) : base(cowAI) { }

            public override void UpdateLogic()
            {

            }
        }
        #endregion
    }
}