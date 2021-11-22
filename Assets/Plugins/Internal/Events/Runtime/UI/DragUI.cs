// Events
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    public class DragUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region Fields
        [SerializeField] private float smoothing = 10f;
        [SerializeField] private bool draggable = true;
        [Space]
        [SerializeField] private UnityEvent onPress;
        [SerializeField] private UnityEvent onDrag;
        [SerializeField] private UnityEvent onRelease;

        private Vector2 offsetPosition, targetPosition;
        private RectTransform rectTransform;
        #endregion

        #region Properties
        public UnityEvent OnPress { get { return onPress; } }
        public UnityEvent OnRelease { get { return onRelease; } }
        public UnityEvent OnDrag { get { return onDrag; } }

        public bool IsPressing { get; set; }
        #endregion

        #region Methods
        private void Start()
        {
            rectTransform = transform as RectTransform;
        }
        private void Update()
        {
            if (IsPressing && draggable)
            {
                if (smoothing > 0)
                {
                    rectTransform.position = Vector3.Lerp(rectTransform.position, targetPosition, Time.deltaTime * smoothing);
                }
                else
                {
                    rectTransform.position = targetPosition;
                }

                targetPosition = (Vector2)Input.mousePosition - offsetPosition;

                OnDrag.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            offsetPosition = eventData.position - (Vector2)rectTransform.position;
            targetPosition = eventData.position - offsetPosition;

            OnPress.Invoke();

            IsPressing = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            OnRelease.Invoke();

            IsPressing = false;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            // Unreliable, however necessary to prevent parent OnDrag() from invoking.
        }
        #endregion
    }
}