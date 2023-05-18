using Crystal;
using UnityEngine;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(1)]
    public class SafeAreaExtension : MonoBehaviour
    {
        [SerializeField] private SafeArea safeArea;

        private void Awake()
        {
            RectTransform rectTransform = transform as RectTransform;
            RectTransform safeAreaRT = safeArea.transform as RectTransform;

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;

            if (rectTransform.pivot.x == 1)
            {
                width = safeAreaRT.anchorMin.x * Screen.width;
            }
            else 
            if (rectTransform.pivot.x == 0)
            {
                width = safeAreaRT.anchorMax.x * Screen.width;
            }
            else 
            if (rectTransform.pivot.y == 0)
            {
                height = safeAreaRT.anchorMax.y * Screen.height;
            }
            else 
            if (rectTransform.pivot.y == 1)
            {
                height = safeAreaRT.anchorMin.y * Screen.height;
            }

            rectTransform.sizeDelta = new Vector2(width, height);
        }
    }
}