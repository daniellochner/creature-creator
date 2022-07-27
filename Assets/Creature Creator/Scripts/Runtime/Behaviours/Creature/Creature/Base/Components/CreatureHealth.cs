// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureKiller))]
    public class CreatureHealth : PlayerHealth
    {
        #region Properties
        public CreatureKiller Killer { get; private set; }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            Killer = GetComponent<CreatureKiller>();
        }

        public override void Die()
        {
            base.Die();
            Killer.Kill();

            Debug.Log("KILL");
        }
        #endregion
    }
}