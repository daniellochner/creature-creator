// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

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
            CreatureConstructor corpse = Ragdoll.Generate(Constructor.Body.position);

            Corpse = corpse.gameObject;
            Corpse.AddComponent<SelfDestructor>().Lifetime = 30f;

            foreach (Transform bone in corpse.Bones)
            {
                Edible flesh = Instantiate(fleshPrefab, bone);
                flesh.GetComponent<SphereCollider>().radius = bone.GetComponent<SphereCollider>().radius;
                flesh.OnEat.AddListener(delegate
                {
                    Destroy(corpse.gameObject);
                });
            }

            Instantiate(iconPrefab, Corpse.transform);
        }
        #endregion
    }
}