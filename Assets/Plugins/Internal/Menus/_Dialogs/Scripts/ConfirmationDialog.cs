// Menus
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ConfirmationDialog : Dialog<ConfirmationDialog>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI confirmationMessageText;
        [SerializeField] private TextMeshProUGUI yesText;
        [SerializeField] private TextMeshProUGUI noText;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        #endregion

        #region Methods
        public static void Confirm(string title, string confirmationMessage, string yes = null, string no = null, bool closeable = true, UnityAction onYes = null, UnityAction onNo = null)
        {
            if (yes == null)
            {
                yes = LocalizationUtility.Localize("confirm_yes");
            }
            if (no == null)
            {
                no = LocalizationUtility.Localize("confirm_no");
            }

            if (Instance.IsOpen)
            {
                Instance.ignoreButton.onClick.Invoke();
            }

            Instance.titleText.text = title;
            Instance.confirmationMessageText.text = confirmationMessage;
            Instance.yesText.text = yes;
            Instance.noText.text = no;
            Instance.titleText.text = title;

            Instance.closeButton.gameObject.SetActive(closeable);

            Instance.yesButton.onClick.RemoveAllListeners();
            Instance.noButton.onClick.RemoveAllListeners();
            Instance.yesButton.onClick.AddListener(delegate
            {
                Instance.Close();
                onYes?.Invoke();
            });
            Instance.noButton.onClick.AddListener(delegate
            {
                Instance.Close();
                onNo?.Invoke();
            });
            Instance.ignoreButton.onClick = Instance.closeButton.onClick = Instance.noButton.onClick;

            Instance.Open();
        }
        #endregion
    }
}