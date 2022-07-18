// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureRagdoll))]
    [RequireComponent(typeof(CreatureAnimator))]
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureKiller : MonoBehaviour
    {
        #region Properties
        public CreatureAnimator CreatureAnimator { get; private set; }
        public CreatureConstructor CreatureConstructor { get; private set; }
        public CreatureRagdoll CreatureRagdoll { get; private set; }

        public GameObject Corpse { get; private set; }

        public Action OnKill { get; set; }
        public Action OnRespawn { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            CreatureConstructor = GetComponent<CreatureConstructor>();
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

            CreatureAnimator.Rebuild();

            OnRespawn?.Invoke();
        }


        #endregion
    }
}