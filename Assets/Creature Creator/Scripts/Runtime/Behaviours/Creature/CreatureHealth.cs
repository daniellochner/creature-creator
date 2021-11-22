// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureKiller))]
    public class CreatureHealth : PlayerHealth
    {
        #region Properties
        public CreatureConstructor Constructor { get; private set; }
        public CreatureKiller Killer { get; private set; }
        public CreatureMover Mover { get; private set; }

        public Action OnRespawn { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Killer = GetComponent<CreatureKiller>();
            Mover = GetComponent<CreatureMover>();
        }

        protected override void OnDie()
        {
            Killer.Kill();

            EditorManager.Instance.IsVisible = false;
        }

        [ContextMenu("Respawn")]
        public void Respawn()
        {
            Destroy(Killer.Corpse);

            transform.position = Mover.Platform.position;
            Constructor.Body.rotation = transform.rotation;
            gameObject.SetActive(true);

            EditorManager.Instance.Build();
            EditorManager.Instance.IsVisible = true;

            OnRespawn?.Invoke();
        }
        #endregion
    }
}