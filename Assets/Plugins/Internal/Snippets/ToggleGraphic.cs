using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ToggleGraphic : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Color onColor;
        [SerializeField] private Color offColor;

        private Toggle toggle;
        private Graphic graphic;
        #endregion

        #region Methods
        private void Awake()
        {
            graphic = GetComponent<Graphic>();
            toggle = GetComponentInParent<Toggle>();

            toggle.onValueChanged.AddListener(SetGraphic);
            SetGraphic(toggle.isOn);
        }

        public void SetGraphic(bool isOn)
        {
            graphic.color = isOn ? onColor : offColor;
        }

        public void SetIsOnWithoutNotify(bool isOn)
        {
            toggle.SetIsOnWithoutNotify(isOn);

            if (toggle.group != null)
            {
                foreach (var graphic in toggle.group.GetComponentsInChildren<ToggleGraphic>())
                {
                    graphic.SetGraphic(false);
                }
            }
            SetGraphic(isOn);
        }
        #endregion
    }
}