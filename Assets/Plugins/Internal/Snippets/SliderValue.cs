using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Slider))]
    public class SliderValue : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private int precision;
        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(SetValue);
            SetValue(slider.value);
        }
        public void SetValue(float value)
        {
            valueText.text = Math.Round(value, precision).ToString();
        }
    }
}