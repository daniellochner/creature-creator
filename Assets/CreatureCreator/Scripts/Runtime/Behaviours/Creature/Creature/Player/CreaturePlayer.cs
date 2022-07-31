// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreaturePlayer : CreatureBase
    {
        [SerializeField] private CreatureNamer namer;
        [SerializeField] private PlayerDeathMessenger deathMessenger;
        [SerializeField] private PlayerMessenger messenger;

        public CreatureNamer Namer => namer;
        public PlayerDeathMessenger DeathMessenger => deathMessenger;
        public PlayerMessenger Messenger => messenger;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            namer = GetComponent<CreatureNamer>();
            deathMessenger = GetComponent<PlayerDeathMessenger>();
            messenger = GetComponent<PlayerMessenger>();
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