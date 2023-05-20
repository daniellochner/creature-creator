using Crystal;
using UnityEngine;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(1)]
    public class SafeAreaExtension : MonoBehaviour
    {
        private SafeArea safeArea;

        private void Awake()
        {
            safeArea = GetComponentInParent<SafeArea>();

            RectTransform rectTransform = transform as RectTransform;
            RectTransform safeAreaRT = safeArea.transform as RectTransform;

            if (rectTransform.pivot.x == 1)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, safeAreaRT.anchorMin.x * Screen.width);
            }
            else 
            if (rectTransform.pivot.x == 0)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (1f - safeAreaRT.anchorMax.x) * Screen.width);
            }
            else 
            if (rectTransform.pivot.y == 1)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, safeAreaRT.anchorMin.y * Screen.height);
            }
            else
            if (rectTransform.pivot.y == 0)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1f - safeAreaRT.anchorMax.y) * Screen.height);
            }
            else
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
            }
        }
    }
}