using UnityEngine;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(1)]
    public class OptimizedMenu : Menu
    {
        protected virtual void Start()
        {
            if (!IsOpen)
            {
                gameObject.SetActive(false);
                animator.enabled = false;
            }
        }
        public override void Open(bool instant = false)
        {
            animator.enabled = true;
            gameObject.SetActive(true);
            base.Open(instant);
        }
        public override void OnEndClose()
        {
            base.OnEndClose();
            gameObject.SetActive(false);
            animator.enabled = false;
        }
    }
}