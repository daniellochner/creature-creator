using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class KeybindUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI actionText;
        [SerializeField] private TextMeshProUGUI keyText;
        [SerializeField] private Button rebindButton;
        [SerializeField] private GameObject resetGO;
        [Space]
        [SerializeField] private KeyCode defaultKey;

        [Header("Debug")]
        [SerializeField, ReadOnly] private KeyCode currentKey;
        #endregion

        #region Properties
        public string Action => actionText.text;
        #endregion

        #region Methods
        private void Start()
        {
            rebindButton.onClick.AddListener(() => KeybindDialog.Rebind(this));
            keyText.text = defaultKey.ToString();
        }

        public void Rebind(KeyCode key)
        {
            keyText.text = key.ToString();

            currentKey = key;
            resetGO.SetActive(true);
        }

        public void Reset()
        {
            Rebind(defaultKey);
            resetGO.SetActive(false);
        }
        #endregion
    }
}