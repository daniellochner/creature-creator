// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor), typeof(CapsuleCollider))]
    public class CreatureCollider : MonoBehaviour
    {
        #region Fields
        [SerializeField] private MinMax minMaxRadius;
        [SerializeField] private MinMax minMaxHeight;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; private set; }

        public float Radius => Hitbox.radius;
        public float Height => Hitbox.height;

        public CapsuleCollider Hitbox { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        private void OnEnable()
        {
            Hitbox.enabled = true;
            UpdateCollider();
        }
        private void OnDisable()
        {
            Hitbox.enabled = false;
        }

        private void Initialize()
        {
            Hitbox = GetComponent<CapsuleCollider>();
            Constructor = GetComponent<CreatureConstructor>();
        }

        public void UpdateCollider()
        {
            Hitbox.height = minMaxHeight.Clamp(Constructor.Dimensions.height);
            Hitbox.radius = minMaxRadius.Clamp(Constructor.Dimensions.radius);
            Hitbox.center = Vector3.up * Mathf.Max(Hitbox.radius, Hitbox.height / 2f);
        }
        #endregion
    }
}