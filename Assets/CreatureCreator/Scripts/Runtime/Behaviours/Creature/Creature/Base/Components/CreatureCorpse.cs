// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
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

        public void Kill(DamageReason reason)
        {
            bool dismember = reason == DamageReason.Acid;

            CreatureConstructor corpse = Ragdoll.Generate(Constructor.Body.position, dismember);

            Corpse = corpse.gameObject;
            Corpse.AddComponent<SelfDestructor>().Lifetime = 30f;

            List<Transform> transforms = new List<Transform>(corpse.Bones);
            if (dismember)
            {
                foreach (BodyPartConstructor bpc in corpse.BodyParts)
                {
                    if (bpc.IsVisible)
                    {
                        transforms.Add(bpc.transform);
                    }
                    if (bpc.Flipped.IsVisible)
                    {
                        transforms.Add(bpc.Flipped.transform);
                    }
                }
            }

            foreach (Transform t in transforms)
            {
                BuoyantObject buoyantObject = t.gameObject.AddComponent<BuoyantObject>();
                buoyantObject.floatingPoints = new Transform[]
                {
                    t
                };
                buoyantObject.floatingPower = 100;
                buoyantObject.underwaterAngularDrag = 3;
                buoyantObject.underwaterDrag = 1;
                buoyantObject.airAngularDrag = 0;
                buoyantObject.airDrag = 0.05f;

                Edible flesh = Instantiate(fleshPrefab, t);
                flesh.GetComponent<SphereCollider>().radius = t.GetComponent<SphereCollider>().radius;
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