// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureHealth : PlayerHealth
    {
        public CreatureConstructor Constructor { get; private set; }

        public override float MaxHealth => Constructor.Statistics.Health;

        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }
    }
}