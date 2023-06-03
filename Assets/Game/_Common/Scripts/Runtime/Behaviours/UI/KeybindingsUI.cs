// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

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
        [SerializeField] private KeybindUI dropKUI;
        [SerializeField] private KeybindUI talkKUI;
        [SerializeField] private KeybindUI viewPlayersKUI;
        [SerializeField] private KeybindUI freeLookKUI;
        [SerializeField] private KeybindUI stopMoveKUI;
        [SerializeField] private KeybindUI respawnKUI;
        [SerializeField] private KeybindUI toggleUIKUI;
        [SerializeField] private KeybindUI biteKUI;
        [SerializeField] private KeybindUI shootKUI;
        [SerializeField] private KeybindUI spitKUI;
        [SerializeField] private KeybindUI growlKUI;
        [SerializeField] private KeybindUI nightVisionKUI;
        [SerializeField] private KeybindUI danceKUI;
        [SerializeField] private KeybindUI spinKUI;

        [Header("Build")]
        [SerializeField] private KeybindUI copyKUI;
        [SerializeField] private KeybindUI undoKUI;
        [SerializeField] private KeybindUI redoKUI;
        [SerializeField] private KeybindUI flipKUI;

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
            KeybindingsManager.Instance?.Save();
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
                KeybindingsManager.Instance.RebindJump(key);
            });

            // Flap
            flapKUI.Rebind(KeybindingsManager.Data.Flap, false);
            flapKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Instance.RebindFlap(key);
            });

            // Sprint
            sprintKUI.Rebind(KeybindingsManager.Data.Sprint, false);
            sprintKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Instance.RebindSprint(key);
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
                KeybindingsManager.Instance.RebindViewPlayers(key);
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

            // Bite
            biteKUI.Rebind(KeybindingsManager.Data.Bite, false);
            biteKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Instance.RebindBite(key);
            });

            // Drop
            dropKUI.Rebind(KeybindingsManager.Data.Drop, false);
            dropKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Instance.RebindDrop(key);
            });

            // Shoot
            shootKUI.Rebind(KeybindingsManager.Data.Shoot, false);
            shootKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Instance.RebindShoot(key);
            });

            // Spit
            spitKUI.Rebind(KeybindingsManager.Data.Spit, false);
            spitKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Instance.RebindSpit(key);
            });

            // Growl
            growlKUI.Rebind(KeybindingsManager.Data.Growl, false);
            growlKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Instance.RebindGrowl(key);
            });

            // Night Vision
            nightVisionKUI.Rebind(KeybindingsManager.Data.NightVision, false);
            nightVisionKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Instance.RebindNightVision(key);
            });

            // Dance
            danceKUI.Rebind(KeybindingsManager.Data.Dance, false);
            danceKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Instance.RebindDance(key);
            });

            // Spin
            spinKUI.Rebind(KeybindingsManager.Data.Spin, false);
            spinKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Instance.RebindSpin(key);
            });
            #endregion

            #region Build
            // Copy
            copyKUI.Rebind(KeybindingsManager.Data.Copy, false);
            copyKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Copy = key;
            });

            // Undo
            undoKUI.Rebind(KeybindingsManager.Data.Undo, false);
            undoKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Undo = key;
            });

            // Redo
            redoKUI.Rebind(KeybindingsManager.Data.Redo, false);
            redoKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Redo = key;
            });

            // Flip
            flipKUI.Rebind(KeybindingsManager.Data.Flip, false);
            flipKUI.OnRebind.AddListener(delegate (Keybind key)
            {
                KeybindingsManager.Data.Flip = key;
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