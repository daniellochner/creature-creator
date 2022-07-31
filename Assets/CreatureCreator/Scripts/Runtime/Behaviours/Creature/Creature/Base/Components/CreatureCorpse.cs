// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureRagdoll))]
    [RequireComponent(typeof(CreatureHider))]
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreatureHealth))]
    public class CreatureCorpse : NetworkBehaviour
    {
        #region Properties
        public CreatureConstructor Constructor { get; private set; }
        public CreatureRagdoll Ragdoll { get; private set; }
        public CreatureHealth Health { get; private set; }

        public GameObject Corpse { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Ragdoll = GetComponent<CreatureRagdoll>();
            Health = GetComponent<CreatureHealth>();
        }
        private void Start()
        {
            Health.OnDie += Kill;
        }

        public void Kill()
        {
            Corpse = Ragdoll.Generate().gameObject;
            Corpse.AddComponent<SelfDestructor>().Lifetime = 10f;
        }
        #endregion
    }
}