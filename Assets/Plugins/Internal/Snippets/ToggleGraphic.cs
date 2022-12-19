using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ToggleGraphic : MonoBehaviour
    {
        [SerializeField] private Color onColor;
        [SerializeField] private Color offColor;
        private Toggle toggle;
        private Graphic graphic;

        private void Awake()
        {
            graphic = GetComponent<Graphic>();
            toggle = GetComponentInParent<Toggle>();

            toggle.onValueChanged.AddListener(OnToggleChanged);
            OnToggleChanged(toggle.isOn);
        }

        private void OnToggleChanged(bool isOn)
        {
            graphic.color = isOn ? onColor : offColor;
        }
    }
}