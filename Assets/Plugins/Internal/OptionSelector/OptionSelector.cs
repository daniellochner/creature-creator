using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class OptionSelector : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI selectedOptionText;
        [SerializeField] private Option[] options;
        [SerializeField] private int defaultOptionIndex;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int currentOptionIndex;
        #endregion

        #region Methods
        private void Start()
        {
            Select(defaultOptionIndex, false);
        }

        public void Next()
        {
            int nextOptionIndex = currentOptionIndex + 1;
            if (nextOptionIndex > options.Length - 1)
            {
                nextOptionIndex = 0;
            }
            Select(nextOptionIndex);
        }
        public void Previous()
        {
            int nextOptionIndex = currentOptionIndex - 1;
            if (nextOptionIndex < 0)
            {
                nextOptionIndex = options.Length - 1;
            }
            Select(nextOptionIndex);
        }

        public void Select(int optionIndex, bool notify = true)
        {
            currentOptionIndex = Math.Max(0, Math.Min(optionIndex, options.Length - 1));

            Option selectedOption = options[currentOptionIndex];
            selectedOptionText.text = selectedOption.Name;
            if (notify)
            {
                selectedOption.OnSelected?.Invoke();
            }
        }
        #endregion

        #region Structs
        [Serializable]
        public struct Option
        {
            public string Name;
            public UnityEvent OnSelected;
        }
        #endregion
    }
}