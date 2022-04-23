// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class KeybindingsUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private KeybindUI walkForwardsKUI;
        [SerializeField] private KeybindUI walkBackwardsKUI;
        [SerializeField] private KeybindUI walkLeftKUI;
        [SerializeField] private KeybindUI walkRightKUI;
        [SerializeField] private KeybindUI jumpKUI;
        [SerializeField] private KeybindUI sprintKUI;
        [SerializeField] private KeybindUI interactKUI;
        [SerializeField] private KeybindUI dropKUI;
        [SerializeField] private KeybindUI talkKUI;
        [SerializeField] private KeybindUI viewPlayersKUI;
        [SerializeField] private KeybindUI copyKUI;
        [SerializeField] private KeybindUI viewMapKUI;
        [SerializeField] private KeybindUI freeLookKUI;
        [SerializeField] private KeybindUI respawnKUI;
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void Setup()
        {
            // Walk Forwards
            walkForwardsKUI.Rebind(KeybindingsManager.Data.WalkForwards, false);
            walkForwardsKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.WalkForwards = key;
            });

            // Walk Backwards
            walkBackwardsKUI.Rebind(KeybindingsManager.Data.WalkBackwards, false);
            walkBackwardsKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.WalkBackwards = key;
            });

            // Walk Left
            walkLeftKUI.Rebind(KeybindingsManager.Data.WalkLeft, false);
            walkLeftKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.WalkLeft = key;
            });

            // Walk Right
            walkRightKUI.Rebind(KeybindingsManager.Data.WalkRight, false);
            walkRightKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.WalkRight = key;
            });

            // Jump
            jumpKUI.Rebind(KeybindingsManager.Data.Jump, false);
            jumpKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.Jump = key;
            });

            // Sprint
            sprintKUI.Rebind(KeybindingsManager.Data.Sprint, false);
            sprintKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.Sprint = key;
            });

            // Interact
            interactKUI.Rebind(KeybindingsManager.Data.Interact, false);
            interactKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.Interact = key;
            });

            // Drop
            dropKUI.Rebind(KeybindingsManager.Data.Drop, false);
            dropKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.Drop = key;
            });

            // Talk
            talkKUI.Rebind(KeybindingsManager.Data.Talk, false);
            talkKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.Talk = key;
            });

            // View Players
            viewPlayersKUI.Rebind(KeybindingsManager.Data.ViewPlayers, false);
            viewPlayersKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.ViewPlayers = key;
            });

            // Copy
            copyKUI.Rebind(KeybindingsManager.Data.Copy, false);
            copyKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.Copy = key;
            });

            // View Map
            viewMapKUI.Rebind(KeybindingsManager.Data.ViewMap, false);
            viewMapKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.ViewMap = key;
            });

            // Free Look
            freeLookKUI.Rebind(KeybindingsManager.Data.FreeLook, false);
            freeLookKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.FreeLook = key;
            });

            // Respawn
            respawnKUI.Rebind(KeybindingsManager.Data.Respawn, false);
            respawnKUI.OnRebind.AddListener(delegate (KeyCode key)
            {
                KeybindingsManager.Data.Respawn = key;
            });
        }
        #endregion
    }
}