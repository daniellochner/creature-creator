// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

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
            EditorManager.Instance.IsVisible = false;

            Killer.Kill();

            InformationDialog.Inform("You Died!", $"Press the button below to respawn at the last platform where you edited your creature.", "Respawn", Respawn);
        }
        protected override void OnRespawn()
        {
            Mover.TeleportToPlatform();
            Killer.Respawn();

            CreatureInformationManager.Instance.Respawn();

            EditorManager.Instance.IsVisible = true;
        }
        #endregion
    }
}