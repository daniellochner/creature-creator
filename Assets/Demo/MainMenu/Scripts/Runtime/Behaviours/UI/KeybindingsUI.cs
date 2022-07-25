// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DanielLochner.Assets.CreatureCreator.Abilities;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class KeybindingsUI : MonoBehaviour
    {
        #region Fields
        [Header("Play")]
        [SerializeField] private KeybindUI walkForwardsKUI;
        [SerializeField] private KeybindUI walkBackwardsKUI;
        [SerializeField] private KeybindUI walkLeftKUI;
        [SerializeField] private KeybindUI walkRightKUI;
        [SerializeField] private KeybindUI jumpKUI;
        [SerializeField] private KeybindUI flapKUI;
        [SerializeField] private KeybindUI sprintKUI;
        [SerializeField] private KeybindUI interactKUI;
        [SerializeField] private KeybindUI dropKUI;
        [SerializeField] private KeybindUI talkKUI;
        [SerializeField] private KeybindUI viewPlayersKUI;
        [SerializeField] private KeybindUI freeLookKUI;
        [SerializeField] private KeybindUI stopMoveKUI;
        [SerializeField] private KeybindUI respawnKUI;
        [SerializeField] private KeybindUI toggleUIKUI;
        [SerializeField] private NetworkPlayersMenu networkPlayersMenu;
        [SerializeField] private Jump[] jumpAbilities;
        [SerializeField] private Flap[] flapAbilities;
        [SerializeField] private Sprint[] sprintAbilities;

        [Header("Build")]
        [SerializeField] private KeybindUI copyKUI;

        [Header("General")]
        [SerializeField] private KeybindUI saveKUI;
        [SerializeField] private KeybindUI loadKUI;
        [SerializeField] private KeybindUI clearKUI;
        [SerializeField] private KeybindUI importKUI;
        [SerializeField] private KeybindUI exportKUI;
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void OnDestroy()
        {
            KeybindingsManager.Instance.Save();
        }

        private void Setup()
        {
            #region Play
            // Walk Forwards
            walkForwardsKUI.Rebind(KeybindingsManager.Data.WalkForwards, false);
            walkForwardsKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.WalkForwards = key;
            });

            // Walk Backwards
            walkBackwardsKUI.Rebind(KeybindingsManager.Data.WalkBackwards, false);
            walkBackwardsKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.WalkBackwards = key;
            });

            // Walk Left
            walkLeftKUI.Rebind(KeybindingsManager.Data.WalkLeft, false);
            walkLeftKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.WalkLeft = key;
            });

            // Walk Right
            walkRightKUI.Rebind(KeybindingsManager.Data.WalkRight, false);
            walkRightKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.WalkRight = key;
            });

            // Jump
            jumpKUI.Rebind(KeybindingsManager.Data.Jump, false);
            jumpKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                foreach (Jump jumpAbility in jumpAbilities)
                {
                    jumpAbility.PerformKeybind = key;
                }
                KeybindingsManager.Data.Jump = key;
            });

            // Flap
            flapKUI.Rebind(KeybindingsManager.Data.Flap, false);
            flapKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                foreach (Flap flapAbility in flapAbilities)
                {
                    flapAbility.PerformKeybind = key;
                }
                KeybindingsManager.Data.Flap = key;
            });

            // Sprint
            sprintKUI.Rebind(KeybindingsManager.Data.Sprint, false);
            sprintKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                foreach (Sprint sprintAbility in sprintAbilities)
                {
                    sprintAbility.PerformKeybind = key;
                }
                KeybindingsManager.Data.Sprint = key;
            });

            // Interact
            interactKUI.Rebind(KeybindingsManager.Data.Interact, false);
            interactKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Interact = key;
            });

            // Drop
            dropKUI.Rebind(KeybindingsManager.Data.Drop, false);
            dropKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Drop = key;
            });

            // Talk
            talkKUI.Rebind(KeybindingsManager.Data.Talk, false);
            talkKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Talk = key;
            });

            // View Players
            viewPlayersKUI.Rebind(KeybindingsManager.Data.ViewPlayers, false);
            viewPlayersKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.ViewPlayers = networkPlayersMenu.Keybind = key;
            });

            // Free Look
            freeLookKUI.Rebind(KeybindingsManager.Data.FreeLook, false);
            freeLookKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.FreeLook = key;
            });

            // Stop Move
            stopMoveKUI.Rebind(KeybindingsManager.Data.StopMove, false);
            stopMoveKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.StopMove = key;
            });

            // Respawn
            respawnKUI.Rebind(KeybindingsManager.Data.Respawn, false);
            respawnKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Respawn = key;
            });

            // Toggle UI
            toggleUIKUI.Rebind(KeybindingsManager.Data.ToggleUI, false);
            toggleUIKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.ToggleUI = key;
            });
            #endregion

            #region Build
            // Copy
            copyKUI.Rebind(KeybindingsManager.Data.Copy, false);
            copyKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Copy = key;
            });
            #endregion

            #region General
            // Save
            saveKUI.Rebind(KeybindingsManager.Data.Save, false);
            saveKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Save = key;
            });

            // Load
            loadKUI.Rebind(KeybindingsManager.Data.Load, false);
            loadKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Load = key;
            });

            // Clear
            clearKUI.Rebind(KeybindingsManager.Data.Clear, false);
            clearKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Clear = key;
            });

            // Import
            importKUI.Rebind(KeybindingsManager.Data.Import, false);
            importKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Import = key;
            });

            // Export
            exportKUI.Rebind(KeybindingsManager.Data.Export, false);
            exportKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Export = key;
            });
            #endregion
        }
        #endregion
    }
}