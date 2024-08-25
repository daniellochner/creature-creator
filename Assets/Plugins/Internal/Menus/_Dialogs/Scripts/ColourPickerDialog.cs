using System;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ColourPickerDialog : Dialog<ColourPickerDialog>
    {
        [SerializeField] private FlexibleColorPicker colorPicker;
        [SerializeField] private Button confirmButton;

        public static void Pick(Color start, Action<Color> onPick = null)
        {
            if (Instance.IsOpen)
            {
                Instance.ignoreButton.onClick.Invoke();
            }

            Instance.colorPicker.color = start;
            if (start == Color.white)
            {
                Instance.colorPicker.ChangeMode(FlexibleColorPicker.MainPickingMode.HS);
            }
            else 
            if (start == Color.black)
            {
                Instance.colorPicker.ChangeMode(FlexibleColorPicker.MainPickingMode.HV);
            }

            Instance.confirmButton.onClick.RemoveAllListeners();
            Instance.confirmButton.onClick.AddListener(delegate
            {
                if (Instance.IsOpen)
                {
                    Instance.Close();
                    onPick?.Invoke(Instance.colorPicker.color);
                }
            });

            Instance.gameObject.SetActive(true);
            Instance.Open();
        }
    }
}