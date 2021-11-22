// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureRagdoll))]
    public class CreatureKiller : MonoBehaviour
    {
        #region Properties
        public CreatureRagdoll CreatureRagdoll { get; private set; }

        public GameObject Corpse { get; private set; }

        public Action OnKill { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            CreatureRagdoll = GetComponent<CreatureRagdoll>();
        }

        public void Kill()
        {
            Corpse = CreatureRagdoll.Generate().gameObject;
            gameObject.SetActive(false);

            OnKill?.Invoke();
        }
        #endregion
    }
}