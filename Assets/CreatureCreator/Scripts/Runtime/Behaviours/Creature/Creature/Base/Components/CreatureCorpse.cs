// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureRagdoll))]
    [RequireComponent(typeof(CreatureLoader))]
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreatureHealth))]
    public class CreatureCorpse : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Edible fleshPrefab;
        [SerializeField] private MinimapIcon iconPrefab;
        #endregion

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

            Health.OnDie += Kill;
        }

        public void Kill()
        {
            CreatureConstructor creature = Ragdoll.Generate(Constructor.Body.position);

            Corpse = creature.gameObject;
            Corpse.AddComponent<SelfDestructor>().Lifetime = 30f;

            foreach (Transform bone in creature.Bones)
            {
                SphereCollider prev = bone.GetComponent<SphereCollider>();

                Edible flesh = Instantiate(fleshPrefab, bone);
                flesh.GetComponent<SphereCollider>().radius = prev.radius;
                flesh.OnEat.AddListener(delegate
                {
                    Destroy(bone.gameObject);
                });

                Destroy(prev);
            }

            Instantiate(iconPrefab, Corpse.transform);
        }
        #endregion
    }
}