// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class UnlockableItem : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool isUnlockable = true;
        #endregion

        #region Properties
        public abstract bool IsUnlocked { get; }
        #endregion

        #region Methods
        private void Start()
        {
            //if (IsUnlocked)
            //{
            //    gameObject.SetActive(false);
            //}
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && isUnlockable)
            {
                OnUnlock();
                gameObject.SetActive(false);
            }
        }

        protected abstract void OnUnlock();
        #endregion
    }
}