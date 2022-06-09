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

        private Vector2 initialMousePosition;
        #endregion

        #region Properties
        public float Threshold
        {
            get => threshold;
            set => threshold = value;
        }
        public UnityEvent OnClick
        {
            get => onClick;
        }
        #endregion

        #region Methods
        private void OnMouseDown()
        {
            if (CanvasUtility.IsPointerOverUI) return;
            initialMousePosition = Input.mousePosition;
        }
        private void OnMouseUpAsButton()
        {
            if (CanvasUtility.IsPointerOverUI || !isActiveAndEnabled) return;
            if (Vector2.Distance(Input.mousePosition, initialMousePosition) <= threshold)
            {
                OnClick.Invoke();
            }
        }
        #endregion
    }
}