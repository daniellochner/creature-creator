// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureRagdoll))]
    [RequireComponent(typeof(CreatureHider))]
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureKiller : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Behaviour[] disabled;
        #endregion

        #region Properties
        public CreatureConstructor CreatureConstructor { get; private set; }
        public CreatureRagdoll CreatureRagdoll { get; private set; }
        public CreatureHider CreatureHider { get; private set; }

        public GameObject Corpse { get; private set; }

        public Action OnKill { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            CreatureConstructor = GetComponent<CreatureConstructor>();
            CreatureRagdoll = GetComponent<CreatureRagdoll>();
        }

        public void Kill()
        {
            Corpse = CreatureRagdoll.Generate().gameObject;
            Corpse.AddComponent<SelfDestructor>().Lifetime = 10f;

            CreatureConstructor.Body.gameObject.SetActive(false);

            foreach (Behaviour behaviour in disabled)
            {
                behaviour.enabled = false;
            }

            OnKill?.Invoke();
        }
        #endregion
    }
}