// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureRagdoll))]
    [RequireComponent(typeof(CreatureAnimator))]
    public class CreatureKiller : MonoBehaviour
    {
        #region Properties
        public CreatureAnimator CreatureAnimator { get; private set; }
        public CreatureRagdoll CreatureRagdoll { get; private set; }

        public GameObject Corpse { get; private set; }

        public Action OnKill { get; set; }
        public Action OnRespawn { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            CreatureAnimator = GetComponent<CreatureAnimator>();
            CreatureRagdoll = GetComponent<CreatureRagdoll>();
        }

        public void Kill()
        {
            Corpse = CreatureRagdoll.Generate().gameObject;
            gameObject.SetActive(false);

            OnKill?.Invoke();
        }
        public void Respawn()
        {
            Destroy(Corpse);
            gameObject.SetActive(true);

            CreatureAnimator.IsAnimated = false;
            CreatureAnimator.IsAnimated = true;

            OnRespawn?.Invoke();
        }
        #endregion
    }
}