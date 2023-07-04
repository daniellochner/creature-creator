using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ButtonCooldown : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float cooldown;

        private Button button;
        #endregion

        #region Methods
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            button.interactable = false;

            this.Invoke(delegate
            {
                button.interactable = true;
            },
            cooldown);
        }
        #endregion
    }
}