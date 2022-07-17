// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Player : MonoBehaviourSingleton<CreaturePlayerLocal>
    {
        protected override void Awake()
        {
            SetSingleton(GetComponent<CreaturePlayerLocal>());
        }
    }
}