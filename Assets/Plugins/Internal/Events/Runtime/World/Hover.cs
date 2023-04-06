// Events
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class Hover : MonoBehaviour
    {
        #region Fields
        [SerializeField] private UnityEvent onEnter = new UnityEvent();
        [SerializeField] private UnityEvent onExit = new UnityEvent();
        #endregion

        #region Properties
        public UnityEvent OnEnter => onEnter;
        public UnityEvent OnExit => onExit;

        public bool IsOver { get; private set; }
        #endregion

        #region Methods
        protected virtual void OnMouseEnter()
        {
            if (SystemUtility.IsDevice(DeviceType.Desktop) && !CanvasUtility.IsPointerOverUI)
            {
                OnEnter.Invoke();
                IsOver = true;
            }
        }
        protected virtual void OnMouseExit()
        {
            if (IsOver)
            {
                OnExit.Invoke();
                IsOver = false;
            }
        }
        #endregion
    }
}