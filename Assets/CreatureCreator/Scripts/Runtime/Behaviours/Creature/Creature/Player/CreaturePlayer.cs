// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreaturePlayer : CreatureBase
    {
        #region Fields
        [SerializeField] private CreatureNamer namer;
        [SerializeField] private PlayerDeathMessenger deathMessenger;
        [SerializeField] private PlayerMessenger messenger;
        [SerializeField] private CreatureSpeedup speedup;
        #endregion

        #region Properties
        public CreatureNamer Namer => namer;
        public PlayerDeathMessenger DeathMessenger => deathMessenger;
        public PlayerMessenger Messenger => messenger;
        public CreatureSpeedup Speedup => speedup;

        public static List<CreaturePlayer> Players { get; } = new List<CreaturePlayer>();
        #endregion

        #region Methods
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

        protected override void OnEnable()
        {
            base.OnEnable();
            Players.Add(this);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            Players.Remove(this);
        }

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
        #endregion
    }
}