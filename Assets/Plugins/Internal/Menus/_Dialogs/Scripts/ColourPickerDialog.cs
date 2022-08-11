using System;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ColourPickerDialog : Dialog<ColourPickerDialog>
    {
        [SerializeField] private FlexibleColorPicker colorPicker;
        [SerializeField] private Button confirmButton;

        public static void Pick(Action<Color> onPick = null)
        {
            if (Instance.IsOpen)
            {
                Instance.ignoreButton.onClick.Invoke();
            }

            Instance.confirmButton.onClick.RemoveAllListeners();
            Instance.confirmButton.onClick.AddListener(delegate
            {
                Instance.Close();
                onPick?.Invoke(Instance.colorPicker.color);
            });

            Instance.gameObject.SetActive(true);
            Instance.Open();
        }
    }
}