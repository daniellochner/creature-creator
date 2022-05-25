// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor), typeof(CapsuleCollider))]
    public class CreatureCollider : MonoBehaviour
    {
        #region Fields
        private CapsuleCollider capsule;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; private set; }

        public float Radius => capsule.radius;
        public float Height => capsule.height;
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
            Setup();
        }

        private void Initialize()
        {
            capsule = GetComponent<CapsuleCollider>();

            Constructor = GetComponent<CreatureConstructor>();
        }
        private void Setup()
        {
            Constructor.OnConstructCreature += delegate ()
            {
                UpdateCollider();
            };
        }

        public void UpdateCollider()
        {
            capsule.radius = Constructor.Dimensions.radius;
            capsule.height = Constructor.Dimensions.height;
            capsule.center = Vector3.up * (Mathf.Max(capsule.height, capsule.radius) / 2f);
        }
        #endregion
    }
}