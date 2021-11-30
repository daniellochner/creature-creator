using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class OptionSelector : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI selectedOptionText;
        [Space]
        [SerializeField] private List<Option> options;
        [SerializeField] private UnityEvent<int> onSelected;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int currentOptionIndex;

        private int previousOptionIndex = -1;
        #endregion

        #region Properties
        public List<Option> Options => options;
        public UnityEvent<int> OnSelected => onSelected;
        #endregion

        #region Methods
        public void Next()
        {
            int nextOptionIndex = currentOptionIndex + 1;
            if (nextOptionIndex > options.Count - 1)
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
                nextOptionIndex = options.Count - 1;
            }
            Select(nextOptionIndex);
        }

        public void SetupUsingEnum<T>()
        {
            foreach (var type in Enum.GetValues(typeof(T)))
            {
                options.Add(new Option()
                {
                    Name = type.ToString()
                });
            }
        }
        public void Select(int optionIndex, bool notify = true)
        {
            currentOptionIndex = Math.Max(0, Math.Min(optionIndex, options.Count - 1));

            Option currentOption = options[currentOptionIndex];
            selectedOptionText.text = currentOption.Name;
            if (notify)
            {
                Option previousOption = options[currentOptionIndex];
                previousOption.OnDeselected?.Invoke();
                currentOption.OnSelected?.Invoke();

                OnSelected?.Invoke(optionIndex);
            }
            previousOptionIndex = currentOptionIndex;
        }
        #endregion

        #region Structs
        [Serializable]
        public struct Option
        {
            public string Name;
            public UnityEvent OnSelected;
            public UnityEvent OnDeselected;
        }
        #endregion
    }
}