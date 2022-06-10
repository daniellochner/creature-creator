// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureCloner : MonoBehaviour
    {
        #region Fields
        [SerializeField] private CreatureConstructor baseCreaturePrefab;
        #endregion

        #region Properties
        public CreatureConstructor CreatureConstructor { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            CreatureConstructor = GetComponent<CreatureConstructor>();
        }

        public CreatureConstructor Clone(CreatureData creatureData = null, Vector3? position = null, Quaternion? rotation = null, Transform parent = null)
        {
            if (creatureData == null) creatureData = CreatureConstructor.Data;

            if (position == null) position = transform.position;
            if (rotation == null) rotation = transform.rotation;
            if (position == null) parent = transform;

            CreatureConstructor clone = Instantiate(baseCreaturePrefab, (Vector3)position, (Quaternion)rotation, parent);
            clone.Construct(creatureData);

            return clone;
        }
        #endregion
    }
}