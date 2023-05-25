// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureMessenger : PlayerMessenger
    {
        #region Properties
        public CreatureConstructor Constructor { get; set; }

        public override bool CanSend => base.CanSend && !CinematicManager.Instance.IsInCinematic && !EditorManager.Instance.IsEditing && SettingsManager.Data.WorldChat && GameSetup.Instance.IsMultiplayer;
        public override bool CanReceive => base.CanReceive && SettingsManager.Data.WorldChat;

        public override bool TryOpen => InputUtility.GetKeyDown(KeybindingsManager.Data.Talk);
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }

        protected override void ReceiveMessage(string message)
        {
            base.ReceiveMessage(message);
            messageGO.transform.localPosition = Vector3.up * (Constructor.Height + height); // this.height functions as an offset in this case
        }
        #endregion
    }
}