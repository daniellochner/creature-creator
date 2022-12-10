using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ToggleText : MonoBehaviour
    {
        [SerializeField] private Color onColor;
        [SerializeField] private Color offColor;
        [SerializeField] private Toggle toggle;
        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            
            toggle.onValueChanged.AddListener(OnToggleChanged);
            OnToggleChanged(toggle.isOn);
        }

        private void OnToggleChanged(bool isOn)
        {
            text.color = isOn ? onColor : offColor;
        }
    }
}