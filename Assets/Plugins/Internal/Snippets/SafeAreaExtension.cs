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
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, safeAreaRT.anchorMin.x * Display.main.systemWidth);
            }
            else 
            if (rectTransform.pivot.x == 0)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (1f - safeAreaRT.anchorMax.x) * Display.main.systemWidth);
            }
            else 
            if (rectTransform.pivot.y == 1)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, safeAreaRT.anchorMin.y * Display.main.systemHeight);
            }
            else
            if (rectTransform.pivot.y == 0)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (1f - safeAreaRT.anchorMax.y) * Display.main.systemHeight);
            }
            else
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Display.main.systemWidth);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Display.main.systemHeight);
            }
        }
    }
}