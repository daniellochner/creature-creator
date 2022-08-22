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
        [SerializeField] private CreatureSpeedup speedup;

        public CreatureNamer Namer => namer;
        public PlayerDeathMessenger DeathMessenger => deathMessenger;
        public PlayerMessenger Messenger => messenger;
        public CreatureSpeedup Speedup => speedup;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            namer = GetComponent<CreatureNamer>();
            deathMessenger = GetComponent<PlayerDeathMessenger>();
            messenger = GetComponent<PlayerMessenger>();
            speedup = GetComponent<CreatureSpeedup>();
        }
#endif

        public override void OnShow()
        {
            base.OnShow();

            Messenger.enabled = true;
        }

        public override void OnHide()
        {
            base.OnHide();

            Messenger.enabled = false;
        }
    }
}