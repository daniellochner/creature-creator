// Keybind
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class KeybindingsDialog : MenuSingleton<KeybindingsDialog>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI messageText;
        #endregion

        #region Methods
        public static void Rebind(KeybindUI keybindUI)
        {
            Instance.messageText.text = $"Press any key to rebind to \"{keybindUI.Action}\"\n.Press ESC to cancel.";
            Instance.StartCoroutine(Instance.RebindRoutine(keybindUI));
        }
        private IEnumerator RebindRoutine(KeybindUI keybindUI)
        {
            Open();

            while (IsOpen)
            {
                if (Input.anyKeyDown)
                {
                    if (!Input.GetKeyDown(KeyCode.Escape))
                    {
                        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                        {
                            if (Input.GetKeyDown(key))
                            {
                                keybindUI.Rebind(key);
                                break;
                            }
                        }
                    }
                    Close();
                }
                yield return null;
            }
        }
        #endregion
    }
}