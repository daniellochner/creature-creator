// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PauseMenuManager : MonoBehaviourSingleton<PauseMenuManager>
    {
        #region Methods
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ConfirmationDialog.Confirm("Quit?", "Are you sure you want to leave the current game, and return to the main menu?", onYes: Leave);
            }
        }

        public void Leave()
        {
            NetworkConnectionManager.Instance.Leave();
        }
        #endregion
    }
}