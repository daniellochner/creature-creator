// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SingleplayerUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private OptionSelector mapOS;
        #endregion

        #region Methods
        public void Play()
        {
            string map = mapOS.Options[mapOS.Selected].Name;
            if (map.Equals("Sandbox") && !ProgressManager.Instance.IsComplete)
            {
                InformationDialog.Inform("Sandbox Locked", "You must collect all parts and patterns before you may access the sandbox!");
            }
            else
            {
                SceneManager.LoadScene(map);
            }
        }
        #endregion
    }
}