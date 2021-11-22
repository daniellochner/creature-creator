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
        public static void Confirm(string title = "Title", string confirmationMessage = "Message", string yes = "Yes", string no = "No", UnityAction yesEvent = null, UnityAction noEvent = null)
        {
            Instance.titleText.text = title;
            Instance.confirmationMessageText.text = confirmationMessage;
            Instance.yesText.text = yes;
            Instance.noText.text = no;
            Instance.titleText.text = title;

            Instance.yesButton.onClick.RemoveAllListeners();
            Instance.noButton.onClick.RemoveAllListeners();
            Instance.yesButton.onClick.AddListener(delegate
            {
                Instance.Close();
                yesEvent?.Invoke();
            });
            Instance.noButton.onClick.AddListener(delegate
            {
                Instance.Close();
                noEvent?.Invoke();
            });
            Instance.ignoreButton.onClick = Instance.noButton.onClick;

            Instance.Open();
        }
        #endregion
    }
}