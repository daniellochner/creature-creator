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
        [SerializeField] private UnityEvent<Keybinding> onRebind;
        [Space]
        [SerializeField] private Keybinding defaultKeybind;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Keybinding currentKeybind;
        #endregion

        #region Properties
        public string Action => actionText.text;
        public UnityEvent<Keybinding> OnRebind => onRebind;
        public Keybinding Selected => currentKeybind;
        #endregion

        #region Methods
        private void Start()
        {
            keyText.text = defaultKeybind.ToString();
            rebindButton.onClick.AddListener(() => KeybindingsDialog.Rebind(this));
        }
        public void Reset()
        {
            Rebind(defaultKeybind);
            resetGO.SetActive(false);
        }

        public void Rebind(Keybinding key, bool notify = true)
        {
            keyText.text = (currentKeybind = key).ToString();

            resetGO.SetActive(!currentKeybind.Equals(defaultKeybind));
            if (notify)
            {
                onRebind.Invoke(key);
            }
        }
        #endregion
    }
}