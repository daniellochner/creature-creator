// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureInformer))]
    [RequireComponent(typeof(CreatureKiller))]
    public class CreatureHealth : PlayerHealth
    {
        #region Properties
        public CreatureConstructor Constructor { get; private set; }
        public CreatureKiller Killer { get; private set; }
        public CreatureInformer Informer { get; private set; }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Killer = GetComponent<CreatureKiller>();
            Informer = GetComponent<CreatureInformer>();
        }

        protected override void OnDie()
        {
            Killer.Kill();
        }
        protected override void OnRespawn()
        {
            Killer.Respawn();

            Informer.Respawn();
        }
        #endregion
    }
}