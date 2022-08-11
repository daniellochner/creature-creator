// Menus
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class InputDialog : Dialog<InputDialog>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI placeholderText;
        [SerializeField] private TextMeshProUGUI submitText;
        [SerializeField] private TextMeshProUGUI cancelText;
        [SerializeField] private TMP_InputField inputFieldText;
        [SerializeField] private Image inputBackgroundImage;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button cancelButton;
        #endregion

        #region Methods
        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (IsOpen)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
                {
                    submitButton.onClick.Invoke();
                }
            }
        }

        public static void Input(string title = "Title", string placeholder = "Enter text...", string submit = "Submit", string cancel = "Cancel", bool closeable = true, UnityAction<string> onSubmit = null, UnityAction<string> onCancel = null, int maxCharacters = -1)
        {
            if (Instance.IsOpen)
            {
                Instance.ignoreButton.onClick.Invoke();
            }

            Instance.titleText.text = title;
            Instance.placeholderText.text = placeholder;
            Instance.submitText.text = submit;
            Instance.cancelText.text = cancel;
            Instance.titleText.text = title;
            Instance.inputFieldText.text = "";

            Instance.closeButton.gameObject.SetActive(closeable);

            Instance.inputFieldText.onValueChanged.RemoveAllListeners();
            Instance.inputFieldText.onValueChanged.AddListener(delegate (string text)
            {
                if (text.Length > maxCharacters && maxCharacters != -1)
                {
                    Instance.inputBackgroundImage.color = Color.red;
                }
                else
                {
                    Instance.inputBackgroundImage.color = Color.white;
                }
            });

            Instance.submitButton.onClick.RemoveAllListeners();
            Instance.cancelButton.onClick.RemoveAllListeners();
            Instance.submitButton.onClick.AddListener(delegate
            {
                if (Instance.inputFieldText.text.Length <= maxCharacters || maxCharacters == -1)
                {
                    Instance.Close();
                    onSubmit?.Invoke(Instance.inputFieldText.text);
                }
            });
            Instance.cancelButton.onClick.AddListener(delegate
            {
                Instance.Close();
                onCancel?.Invoke(Instance.inputFieldText.text);
            });
            Instance.ignoreButton.onClick = Instance.closeButton.onClick = Instance.cancelButton.onClick;

            Instance.Open();
        }

        public static async Task<string> InputAsync(string title = "Title", string placeholder = "Enter text...", string submit = "Submit", string cancel = "Cancel", bool closeable = true, string error = "No input was provided.")
        {
            var promise = new TaskCompletionSource<string>();

            Input(title, placeholder, submit, cancel, closeable, delegate (string text)
            {
                promise.SetResult(text);
            }, 
            delegate
            {
                promise.SetException(new Exception(error));
            });

            return await promise.Task;
        }
        
        public override void OnEndOpen()
        {
            Instance.inputFieldText.Select();
            Instance.inputFieldText.ActivateInputField();
        }
        #endregion
    }
}