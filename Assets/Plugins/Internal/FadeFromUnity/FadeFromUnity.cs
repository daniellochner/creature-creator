using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeFromUnity : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
        }
        private void Start()
        {
            StartCoroutine(canvasGroup.Fade(false, 1f, true, () => Destroy(gameObject)));
        }
    }
}