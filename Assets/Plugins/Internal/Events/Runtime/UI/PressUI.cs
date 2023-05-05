using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    public class PressUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Fields
        [SerializeField] private UnityEvent onPress = new UnityEvent();
        [SerializeField] private UnityEvent onRelease = new UnityEvent();
        #endregion

        #region Properties
        public UnityEvent OnPress => onPress;
        public UnityEvent OnRelease => onRelease;
        #endregion

        #region Methods
        public void OnPointerDown(PointerEventData eventData)
        {
            onPress.Invoke();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            onRelease.Invoke();
        }
        #endregion
    }
}