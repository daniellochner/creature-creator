// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureKiller))]
    [RequireComponent(typeof(CreatureMover))]
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

            CreatureInformation info = CreatureInformationManager.Instance.Information;
            string name = info.Name.Equals("Unnamed") ? "You" : info.Name;
            string age = info.FormattedAge;
            
            InformationDialog.Inform("You Died!", $"{name} died after {age}. Press the button below to respawn at your previous editing platform.", "Respawn", Respawn);
        }
        protected override void OnRespawn()
        {
            Mover.Teleport(Mover.Platform);
            Killer.Respawn();

            CreatureInformationManager.Instance.Respawn();

            EditorManager.Instance.IsVisible = true;
        }
        #endregion
    }
}