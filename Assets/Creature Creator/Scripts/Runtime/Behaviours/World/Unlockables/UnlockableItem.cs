// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
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
            if (IsUnlocked)
            {
                Destroy(gameObject);
            }
            else
            {
                OnSpawn();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player/Local") && isUnlockable)
            {
                OnUnlock();
                ProgressManager.Instance.Save();
                Destroy(gameObject);
            }
        }

        protected abstract void OnUnlock();
        protected abstract void OnSpawn();
        #endregion
    }
}