using UnityEngine;

namespace DanielLochner.Assets
{
    public class BlackBars : MonoBehaviourSingleton<BlackBars>
    {
        private CanvasGroup canvasGroup;
        private Coroutine coroutine;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public void SetVisibility(bool isVisible, float time = 0f)
        {
            this.StopStartCoroutine(canvasGroup.FadeRoutine(isVisible, time), ref coroutine);
        }
    }
}