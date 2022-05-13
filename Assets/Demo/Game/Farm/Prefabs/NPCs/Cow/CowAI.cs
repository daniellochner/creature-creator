// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CowAI : FarmAnimalAI<CowAI>
    {
        protected override void Initialize()
        {
            States.Add("WAN", new Wandering(this));
            States.Add("REP", new Repositioning(this));
        }
    }
}