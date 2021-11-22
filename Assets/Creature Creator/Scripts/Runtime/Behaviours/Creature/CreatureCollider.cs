// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor), typeof(CapsuleCollider))]
    public class CreatureCollider : MonoBehaviour
    {
        #region Fields
        private CapsuleCollider capsuleCollider;
        #endregion

        #region Properties
        public CreatureConstructor CreatureConstructor { get; private set; }

        public float Radius => capsuleCollider.radius;
        public float Height => capsuleCollider.height;
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
            Setup();
        }

        private void Initialize()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();

            CreatureConstructor = GetComponent<CreatureConstructor>();
        }
        private void Setup()
        {
            CreatureConstructor.OnConstructBody += delegate ()
            {
                UpdateDimensions();
            };
        }

        public void UpdateDimensions()
        {
            float maxRadius = 0, maxHeight = 0;

            foreach (Transform bone in CreatureConstructor.Bones)
            {
                float height = transform.InverseTransformPoint(bone.position).y;
                if (height > maxHeight)
                {
                    maxHeight = height;
                }

                float radius = Mathf.Abs(CreatureConstructor.Body.InverseTransformPoint(bone.position).z);
                if (radius > maxRadius)
                {
                    maxRadius = radius;
                }
            }

            //capsuleCollider.radius = maxRadius;
            capsuleCollider.height = maxHeight;
            capsuleCollider.center = Vector3.up * maxHeight / 2f;
        }
        #endregion
    }
}