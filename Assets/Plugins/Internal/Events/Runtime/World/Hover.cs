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
            if (CanvasUtility.IsPointerOverUI) return;
            OnEnter.Invoke();
            IsOver = true;
        }
        protected virtual void OnMouseExit()
        {
            if (CanvasUtility.IsPointerOverUI) return;
            OnExit.Invoke();
            IsOver = false;
        }
        #endregion
    }
}