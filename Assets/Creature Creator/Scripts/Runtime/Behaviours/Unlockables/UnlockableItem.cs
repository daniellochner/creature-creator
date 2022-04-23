// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class UnlockableItem : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool isUnlockable = true;
        [SerializeField] private GameObject item;
        #endregion

        #region Properties
        public abstract bool IsUnlocked { get; }
        #endregion

        #region Methods
        private void Start()
        {
            if (IsUnlocked)
            {
                item.SetActive(false);
            }
            else
            {
                OnSpawn();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && isUnlockable)
            {
                OnUnlock();
                item.SetActive(false);
                ProgressManager.Instance.Save();
            }
        }

        protected abstract void OnUnlock();
        protected abstract void OnSpawn();
        #endregion
    }
}