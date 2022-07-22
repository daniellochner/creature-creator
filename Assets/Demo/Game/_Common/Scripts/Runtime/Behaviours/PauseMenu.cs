// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PauseMenu : MenuSingleton<PauseMenu>
    {

        #region Properties
        private bool CanPause => !ConfirmationDialog.Instance.IsOpen && !InformationDialog.Instance.IsOpen && !InputDialog.Instance.IsOpen;
        #endregion

        #region Methods
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && CanPause)
            {

            }
        }

        public void Leave()
        {
            ConfirmationDialog.Confirm("Leave?", "Are you sure you want to leave the current game, and return to the main menu?", onYes: delegate
            {
                NetworkConnectionManager.Instance.Leave();
            });
        }
        #endregion
    }
}