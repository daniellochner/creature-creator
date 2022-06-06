// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CowAI : AnimalAI
    {
        #region Inner Classes
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