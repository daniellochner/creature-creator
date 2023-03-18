// Events
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class Click : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float threshold = 0f;
        [SerializeField] private UnityEvent onClick = new UnityEvent();
        [SerializeField] private UnityEvent onRightClick = new UnityEvent();

        private Vector2 initialMousePosition;
        private bool pressed;
        #endregion

        #region Properties
        public float Threshold
        {
            get => threshold;
            set => threshold = value;
        }

        public UnityEvent OnLeftClick => onClick;
        public UnityEvent OnRightClick => onRightClick;
        #endregion

        #region Methods
        private void OnMouseOver()
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !CanvasUtility.IsPointerOverUI)
            {
                initialMousePosition = Input.mousePosition;
                pressed = true;
            }
        }
        private void Update()
        {
            if (pressed)
            {
                if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    if (Vector2.Distance(Input.mousePosition, initialMousePosition) <= threshold)
                    {
                        if (Input.GetMouseButtonUp(0))
                        {
                            OnLeftClick.Invoke();
                        }
                        if (Input.GetMouseButtonUp(1))
                        {
                            OnRightClick.Invoke();
                        }
                    }
                    pressed = false;
                }
            }
        }
        #endregion
    }
}