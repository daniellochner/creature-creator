// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureInformer))]
    [RequireComponent(typeof(CreatureKiller))]
    [RequireComponent(typeof(CreatureEnergy))]
    [RequireComponent(typeof(CreatureAge))]
    public class CreatureHealth : PlayerHealth
    {
        #region Properties
        public CreatureKiller Killer { get; private set; }
        public CreatureInformer Informer { get; private set; }
        public CreatureEnergy Energy { get; private set; }
        public CreatureAge Age { get; private set; }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            Killer = GetComponent<CreatureKiller>();
            Informer = GetComponent<CreatureInformer>();
            Energy = GetComponent<CreatureEnergy>();
            Age = GetComponent<CreatureAge>();
        }

        public override void Die()
        {
            base.Die();
            Killer.Kill();
        }
        public override void Respawn()
        {
            base.Respawn();
            Killer.Respawn();
            Informer.Respawn();

            Energy.Energy = 1f;
            Age.Age = 0;
        }
        #endregion
    }
}