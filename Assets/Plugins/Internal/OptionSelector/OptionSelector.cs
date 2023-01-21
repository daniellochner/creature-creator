using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace DanielLochner.Assets
{
    public class OptionSelector : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI selectedOptionText;
        [Space]
        [SerializeField] private int startOption = -1;
        [SerializeField] private List<Option> options;
        [SerializeField] private UnityEvent<int> onSelected;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int currentOptionIndex;

        private int previousOptionIndex = -1;
        #endregion

        #region Properties
        public int Selected => currentOptionIndex;
        public List<Option> Options => options;
        public UnityEvent<int> OnSelected => onSelected;
        #endregion

        #region Methods
        private void Start()
        {
            if (startOption != -1)
            {
                Select(startOption);
            }
            LocalizationSettings.Instance.OnSelectedLocaleChanged += UpdateName;
        }
        private void OnDestroy()
        {
            LocalizationSettings.Instance.OnSelectedLocaleChanged -= UpdateName;
        }

        public void SetupUsingEnum<T>(bool localize)
        {
            foreach (var type in Enum.GetValues(typeof(T)))
            {
                string id = $"option_{typeof(T).Name}_{type.ToString()}".ToLower();
                if (!localize || !LocalizationUtility.HasEntry(id))
                {
                    id = type.ToString();
                }
                options.Add(new Option()
                {
                    Id = id
                });
            }
        }

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
        public void Select(Enum option, bool notify = true)
        {
            Select(Convert.ToInt32(option), notify);
        }

        private void UpdateName(Locale locale = default)
        {
            Select(currentOptionIndex, false);
        }
        #endregion

        #region Structs
        [Serializable]
        public struct Option
        {
            public string Id;
            public string Fallback;
            public UnityEvent OnSelected;
            public UnityEvent OnDeselected;

            public string Name
            {
                get
                {
                    if (LocalizationUtility.HasEntry(Id))
                    {
                        return LocalizationUtility.Localize(Id);
                    }
                    else
                    {
                        return Id;
                    }
                }
            }
        }
        #endregion
    }
}