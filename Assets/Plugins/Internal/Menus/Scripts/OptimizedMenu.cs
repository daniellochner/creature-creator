using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [DefaultExecutionOrder(1)]
    public class OptimizedMenu : Menu
    {
        private void Start()
        {
            if (!IsOpen)
            {
                gameObject.SetActive(false);
            }
        }
        public override void Open(bool instant = false)
        {
            gameObject.SetActive(true);
            base.Open(instant);
        }
        public override void OnEndClose()
        {
            base.OnEndClose();
            gameObject.SetActive(false);
        }
    }
}