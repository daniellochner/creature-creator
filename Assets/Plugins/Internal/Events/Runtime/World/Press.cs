// Events
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class Press : MonoBehaviour
    {
        #region Fields
        [SerializeField] private UnityEvent onPress = new UnityEvent();
        [SerializeField] private UnityEvent onRelease = new UnityEvent();
        [SerializeField] private UnityEvent onHold = new UnityEvent();
        #endregion

        #region Properties
        public UnityEvent OnPress => onPress;
        public UnityEvent OnRelease => onRelease;
        public UnityEvent OnHold => onHold;

        public bool IsPressing { get; private set; }
        #endregion

        #region Methods
        private void Update()
        {
            if (IsPressing)
            {
                OnMouseHold();
            }
        }

        private void OnMouseHold()
        {
            OnHold.Invoke();
        }
        private void OnMouseDown()
        {
            if (CanvasUtility.IsPointerOverUI) return;
            OnPress.Invoke();
            IsPressing = true;
        }
        private void OnMouseUp()
        {
            if (CanvasUtility.IsPointerOverUI) return;
            IsPressing = false;
            onRelease.Invoke();
        }
        #endregion
    }
}