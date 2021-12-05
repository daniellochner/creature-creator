using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeFromUnity : MonoBehaviourSingleton<FadeFromUnity>
    {
        private CanvasGroup canvasGroup;

        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            base.Awake();

            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
        }
        private void Start()
        {
            StartCoroutine(canvasGroup.Fade(false, 1f, true, () => gameObject.SetActive(false)));
        }
    }
}