using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class NavBarButtonUGUI : MonoBehaviour
    {
        [SerializeField] UnityEvent m_OnSelected = null;
        [SerializeField] UnityEvent m_OnDeselected = null;
        [field: SerializeField] public Button button { get; private set; }

        public void Init()
        {
            button.onClick.AddListener(Select);
            Deselect();
        }

        public void Select()
        {
            m_OnSelected?.Invoke();
        }
        
        public void Deselect()
        {
            m_OnDeselected?.Invoke();
        }
    }
}