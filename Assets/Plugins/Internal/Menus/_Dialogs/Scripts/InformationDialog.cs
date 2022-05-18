// Menus
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class InformationDialog : Dialog<InformationDialog>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI informationMessageText;
        [SerializeField] private TextMeshProUGUI okayText;
        [SerializeField] private Button okayButton;
        #endregion

        #region Methods
        public static void Inform(string title = "Title", string informationMessage = "Message", string okay = "Okay", bool closeable = true, UnityAction okayEvent = null)
        {
            Instance.titleText.text = title;
            Instance.informationMessageText.text = informationMessage;
            Instance.okayText.text = okay;
            Instance.titleText.text = title;

            Instance.closeButton.gameObject.SetActive(closeable);

            Instance.okayButton.onClick.RemoveAllListeners();
            Instance.okayButton.onClick.AddListener(delegate
            {
                Instance.Close();
                okayEvent?.Invoke();
            });
            Instance.ignoreButton.onClick = Instance.closeButton.onClick = Instance.okayButton.onClick;

            Instance.Open();
        }
        #endregion
    }
}