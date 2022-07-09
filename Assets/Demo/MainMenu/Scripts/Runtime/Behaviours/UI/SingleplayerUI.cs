// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SingleplayerUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private string map;
        #endregion

        #region Methods
        public void PlayDefault()
        {
            LoadingManager.Instance.Load(map);
        }
        public void PlaySandbox()
        {
            if (!ProgressManager.Instance.IsComplete)
            {
                InformationDialog.Inform("Sandbox Locked", "You must collect all parts and patterns before you may access the sandbox!");
            }
            else
            {
                LoadingManager.Instance.Load("Sandbox");
            }
        }
        #endregion
    }
}