// Menus
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Animator))]
    public class Menu : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool isOpen;
        [SerializeField] private bool deactivateOnClose;

        private Animator animator;
        #endregion

        #region Properties
        public bool IsOpen
        {
            get => isOpen;
            set
            {
                if (animator != null)
                {
                    animator.SetBool("IsOpen", isOpen = value);
                }
            }
        }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            animator.SetBool("IsOpenByDefault", IsOpen = isOpen); // Set to the default value in the inspector.
        }

        public virtual void Open(bool instant = false)
        {
            if (deactivateOnClose)
            {
                gameObject.SetActive(true);
            }

            if (instant)
            {
                animator.Play("Open", 0, 1);
            }

            IsOpen = true;
        }
        public virtual void Close(bool instant = false)
        {
            if (instant)
            {
                animator.Play("Close", 0, 1);
            }

            IsOpen = false;
        }
        public void Toggle()
        {
            if (IsOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void OnOpened()
        {
            if (IsOpen)
            {
                OnEndOpen();
            }
            else
            {
                OnBeginClose();
            }
        }
        public void OnClosed()
        {
            if (IsOpen)
            {
                OnBeginOpen();
            }
            else
            {
                OnEndClose();
            }
        }

        public virtual void OnBeginOpen()
        {
        }
        public virtual void OnEndOpen()
        {
        }
        public virtual void OnBeginClose()
        {
        }
        public virtual void OnEndClose()
        {
            if (deactivateOnClose)
            {
                gameObject.SetActive(false);
            }
        }
        #endregion
    }
}