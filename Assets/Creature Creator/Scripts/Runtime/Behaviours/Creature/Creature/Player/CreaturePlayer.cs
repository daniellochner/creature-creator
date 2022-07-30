// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreaturePlayer : CreatureBase
    {
        [SerializeField] private CreatureNamer namer;
        [SerializeField] private PlayerDeathMessenger deathMessenger;

        public CreatureNamer Namer => namer;
        public PlayerDeathMessenger DeathMessenger => deathMessenger;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            namer = GetComponent<CreatureNamer>();
            deathMessenger = GetComponent<PlayerDeathMessenger>();
        }
#endif
        
        public override void OnDie()
        {
            base.OnDie();

            Namer.enabled = false;
            DeathMessenger.enabled = false;
        }
    }
}