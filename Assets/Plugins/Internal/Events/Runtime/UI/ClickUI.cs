// Events
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    public class ClickUI : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        #region Fields
        [SerializeField] private float threshold = 0f;
        [SerializeField] private UnityEvent onLeftClick = new UnityEvent();
        [SerializeField] private UnityEvent onRightClick = new UnityEvent();

        private Vector2 initialMousePosition;
        #endregion

        #region Properties
        public float Threshold
        {
            get => threshold;
            set => threshold = value;
        }

        public UnityEvent OnLeftClick => onLeftClick;
        public UnityEvent OnRightClick => onRightClick;
        #endregion

        #region Methods
        public void OnPointerDown(PointerEventData eventData)
        {
            initialMousePosition = Input.mousePosition;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isActiveAndEnabled) return;

            if (Vector2.Distance(Input.mousePosition, initialMousePosition) <= threshold)
            {
                switch (eventData.button)
                {
                    case PointerEventData.InputButton.Left:
                        OnLeftClick.Invoke();
                        break;
                    case PointerEventData.InputButton.Right:
                        OnRightClick.Invoke();
                        break;
                }
            }
        }
        #endregion
    }
}