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

        private readonly KeyCode[] MODIFIER_KEYS = new KeyCode[]
        {
            KeyCode.LeftShift,
            KeyCode.LeftControl,
            KeyCode.LeftCommand,
            KeyCode.LeftAlt,
            KeyCode.RightShift,
            KeyCode.RightControl,
            KeyCode.RightCommand,
            KeyCode.RightAlt
        };
        #endregion

        #region Methods
        public static void Rebind(KeybindUI keybindUI)
        {
            Instance.messageText.text = $"Press any key(s) to rebind to \"{keybindUI.Action}\"\n.Press ESC to cancel.";
            Instance.StartCoroutine(Instance.RebindRoutine(keybindUI));
        }
        private IEnumerator RebindRoutine(KeybindUI keybindUI)
        {
            Open();

            KeyCode modifierKey = KeyCode.None;

            while (IsOpen)
            {
                if (!Input.GetMouseButton(0))
                {
                    if (Input.anyKey)
                    {
                        if (!Input.GetKeyDown(KeyCode.Escape))
                        {
                            foreach (KeyCode key in MODIFIER_KEYS)
                            {
                                if (Input.GetKeyDown(key))
                                {
                                    modifierKey = key;
                                    break;
                                }
                            }

                            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                            {
                                if (Input.GetKeyDown(key) && key != modifierKey)
                                {
                                    keybindUI.Rebind(new Keybind(key, modifierKey));
                                    Close();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Close();
                        }
                    }
                    else if (modifierKey != KeyCode.None)
                    {
                        keybindUI.Rebind(new Keybind(modifierKey, KeyCode.None));
                        Close();
                    }
                }

                yield return null;
            }
        }
        #endregion
    }
}