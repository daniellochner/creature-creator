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
        public static void Inform(string title, string informationMessage, string okay = null, bool closeable = true, UnityAction onOkay = null)
        {
            if (okay == null)
            {
                okay = LocalizationUtility.Localize("inform_okay");
            }

            if (Instance.IsOpen)
            {
                Instance.ignoreButton.onClick.Invoke();
            }

            Instance.titleText.text = title;
            Instance.informationMessageText.text = informationMessage;
            Instance.okayText.text = okay;
            Instance.titleText.text = title;

            Instance.closeButton.gameObject.SetActive(closeable);

            Instance.okayButton.onClick.RemoveAllListeners();
            Instance.okayButton.onClick.AddListener(delegate
            {
                Instance.Close();
                onOkay?.Invoke();
            });
            Instance.ignoreButton.onClick = Instance.closeButton.onClick = Instance.okayButton.onClick;

            Instance.Open();
        }
        #endregion
    }
}