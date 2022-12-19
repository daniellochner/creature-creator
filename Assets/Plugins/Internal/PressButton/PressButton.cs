using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Animator))]
    public class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        private Animator animator;

        public bool IsPressed
        {
            get => animator.GetBool("IsPressed");
            set
            {
                animator.SetBool("IsPressed", value && gameObject.IsInteractable());
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsPressed)
            {
                IsPressed = false;
            }
        }
    }
}