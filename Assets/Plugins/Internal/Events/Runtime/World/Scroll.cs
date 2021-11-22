// Events
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class Scroll : MonoBehaviour
    {
        #region Fields
        [SerializeField] private UnityEvent onScrollUp = new UnityEvent();
        [SerializeField] private UnityEvent onScrollDown = new UnityEvent();
        #endregion

        #region Properties
        public UnityEvent OnScrollUp { get { return onScrollUp; } }
        public UnityEvent OnScrollDown { get { return onScrollDown; } }
        #endregion

        #region Methods
        private void OnMouseOver()
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                OnScrollUp.Invoke();
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                OnScrollDown.Invoke();
            }
        }
        #endregion
    }
}