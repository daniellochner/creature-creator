using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
        [SerializeField] private UnityEvent<KeyCode> onRebind;
        [Space]
        [SerializeField] private KeyCode defaultKey;

        [Header("Debug")]
        [SerializeField, ReadOnly] private KeyCode currentKey;
        #endregion

        #region Properties
        public string Action => actionText.text;
        public UnityEvent<KeyCode> OnRebind => onRebind;
        #endregion

        #region Methods
        private void Start()
        {
            rebindButton.onClick.AddListener(() => KeybindDialog.Rebind(this));
            keyText.text = defaultKey.ToString();
        }

        public void Rebind(KeyCode key, bool notify = true)
        {
            keyText.text = key.ToString();

            currentKey = key;
            resetGO.SetActive(true);

            if (notify)
            {
                onRebind.Invoke(key);
            }
        }

        public void Reset()
        {
            Rebind(defaultKey);
            resetGO.SetActive(false);
        }
        #endregion
    }
}