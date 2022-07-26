// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureRagdoll))]
    [RequireComponent(typeof(CreatureAnimator))]
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureKiller : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Behaviour[] disabled;
        #endregion

        #region Properties
        public CreatureAnimator CreatureAnimator { get; private set; }
        public CreatureConstructor CreatureConstructor { get; private set; }
        public CreatureRagdoll CreatureRagdoll { get; private set; }

        public GameObject Corpse { get; private set; }

        public Action OnKill { get; set; }
        public Action OnRespawn { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            CreatureConstructor = GetComponent<CreatureConstructor>();
            CreatureAnimator = GetComponent<CreatureAnimator>();
            CreatureRagdoll = GetComponent<CreatureRagdoll>();
        }

        public void Kill()
        {
            Corpse = CreatureRagdoll.Generate().gameObject;
            foreach (Behaviour behaviour in disabled)
            {
                behaviour.enabled = false;
            }

            OnKill?.Invoke();
        }
        public void Respawn()
        {
            Destroy(Corpse);
            foreach (Behaviour behaviour in disabled)
            {
                behaviour.enabled = true;
            }

            OnRespawn?.Invoke();
        }
        #endregion
    }
}