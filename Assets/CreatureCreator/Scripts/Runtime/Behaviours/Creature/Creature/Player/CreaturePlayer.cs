// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreaturePlayer : CreatureBase
    {
        #region Fields
        [SerializeField] private PlayerDeathMessenger deathMessenger;
        [SerializeField] private PlayerMessenger messenger;
        [SerializeField] private CreatureSpeedup speedup;
        [SerializeField] private PlayerDataContainer dataContainer;
        [SerializeField] private PlayerRecolour recolour;
        [SerializeField] private PlayerVIP vip;
        [SerializeField] private PlayerVerified verified;
        #endregion

        #region Properties
        public PlayerDeathMessenger DeathMessenger => deathMessenger;
        public PlayerMessenger Messenger => messenger;
        public CreatureSpeedup Speedup => speedup;
        public PlayerDataContainer DataContainer => dataContainer;
        public PlayerRecolour Recolour => recolour;
        public PlayerVIP VIP => vip;
        public PlayerVerified Verified => verified;

        public static List<CreaturePlayer> Players { get; } = new List<CreaturePlayer>();
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            deathMessenger = GetComponent<PlayerDeathMessenger>();
            messenger = GetComponent<PlayerMessenger>();
            speedup = GetComponent<CreatureSpeedup>();
            dataContainer = GetComponent<PlayerDataContainer>();
            recolour = GetComponent<PlayerRecolour>();
            vip = GetComponent<PlayerVIP>();
            verified = GetComponent<PlayerVerified>();
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

        public override void Setup()
        {
            base.Setup();

            VIP.Setup();
            Verified.Setup();
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