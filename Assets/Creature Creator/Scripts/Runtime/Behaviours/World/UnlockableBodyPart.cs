// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class UnlockableBodyPart : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Material unlockedMat;
        [SerializeField] private Material lockedMat;
        [Space]
        [SerializeField] private bool isUnlockable;
        [SerializeField] public string bodyPartID;
        #endregion

        #region Properties
        public bool IsUnlockable
        {
            get => isUnlockable;
            set
            {
                isUnlockable = value;

                foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.material = isUnlockable ? unlockedMat : lockedMat;
                }
            }
        }

        public BodyPart BodyPart => DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartID);
        #endregion

        #region Methods
        private void Start()
        {
            Instantiate(BodyPart.Prefab.constructible, transform);
            IsUnlockable = isUnlockable;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && IsUnlockable)
            {
                EditorManager.Instance.UnlockBodyPart(bodyPartID);
            }
        }
        #endregion
    }
}