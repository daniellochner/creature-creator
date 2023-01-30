using UnityEngine;

namespace DanielLochner.Assets
{
    public class BlackBars : MonoBehaviourSingleton<BlackBars>
    {
        private CanvasGroup canvasGroup;
        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public void SetVisibility(bool isVisible, float time = 0f)
        {
            StartCoroutine(canvasGroup.Fade(isVisible, time));
        }
    }
}