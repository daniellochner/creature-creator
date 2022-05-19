using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(TMP_InputField))]
    public class CodeField : MonoBehaviour
    {
        #region Fields
        [SerializeField, ReadOnly] private string code;
        private TMP_InputField inputField;
        #endregion

        #region Properties
        public bool IsVisible { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        public void Setup(string code, Action<string> onSelect = null, Action<string> onDeselect = null)
        {
            inputField.text = this.code = code;

            inputField.onSelect.AddListener(delegate (string c)
            {
                Show();
                onSelect?.Invoke(c);
            });
            inputField.onDeselect.AddListener(delegate (string c)
            {
                Hide();
                onDeselect?.Invoke(c);
            });

            Hide();
        }

        private void Show()
        {
            inputField.text = code;
            inputField.textComponent.rectTransform.anchoredPosition = Vector2.zero;
            IsVisible = true;
        }
        private void Hide()
        {
            inputField.text = new string('•', code.Length);
            inputField.textComponent.rectTransform.anchoredPosition = Vector2.zero;
            IsVisible = false;
        }
        #endregion
    }
}