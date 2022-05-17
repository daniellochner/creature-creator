// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PigAI : AnimalAI
    {
        #region States
        public override void Reset()
        {
            base.Reset();
            states.Add(new Rolling(this));
        }

        [Serializable]
        public class Rolling : BaseState
        {
            public Rolling(PigAI pigAI) : base(pigAI) { }

            public override void UpdateLogic()
            {

            }
        }
        #endregion
    }
}