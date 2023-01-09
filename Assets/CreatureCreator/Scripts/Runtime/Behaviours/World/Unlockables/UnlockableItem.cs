// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class UnlockableItem : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool isUnlockable = true;
        [SerializeField] private GameObject unlockFX;
        [SerializeField] public UnityEvent onUnlock;
        private bool hasSpawned = false;
        #endregion

        #region Properties
        public abstract bool IsUnlocked { get; }
        #endregion

        #region Methods
        public void Setup()
        {
            if (IsUnlocked)
            {
                Destroy(gameObject);
            }
            else if (!hasSpawned)
            {
                OnSpawn();
                hasSpawned = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player/Local") && isUnlockable)
            {
                OnUnlock();
                onUnlock.Invoke();
                ProgressManager.Instance.Save();
                Instantiate(unlockFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        protected abstract void OnUnlock();
        protected abstract void OnSpawn();
        #endregion
    }
}