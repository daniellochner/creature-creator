// Menus
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ControlRebindDialog : MenuSingleton<ControlRebindDialog>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI messageText;
        #endregion

        #region Methods
        public static void Rebind(KeybindUI controlUI)
        {
            Instance.messageText.text = $"Press any key to rebind \"{controlUI.Action}\"\n.Press ESC to cancel.";
            Instance.StartCoroutine(Instance.RebindRoutine(controlUI));
        }
        private IEnumerator RebindRoutine(KeybindUI controlUI)
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
                                controlUI.Rebind(key);
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