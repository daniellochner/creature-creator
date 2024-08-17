using Crystal;
using UnityEngine;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(1)]
    public class SafeAreaExtension : MonoBehaviour
    {
        private SafeArea safeArea;
        private float prevWidth, prevHeight;

        private RectTransform RectTransform => transform as RectTransform;

        private void Awake()
        {
            safeArea = GetComponentInParent<SafeArea>();
            Setup();
        }

        private void Update()
        {
            Setup();
        }

        public void Setup()
        {
            RectTransform safeAreaRT = safeArea.transform as RectTransform;
            if (RectTransform.pivot.x == 1)
            {
                SetWidth(safeAreaRT.anchorMin.x * Display.main.systemWidth);
            }
            else
            if (RectTransform.pivot.x == 0)
            {
                SetWidth((1f - safeAreaRT.anchorMax.x) * Display.main.systemWidth);
            }
            else
            if (RectTransform.pivot.y == 1)
            {
                SetHeight(safeAreaRT.anchorMin.y * Display.main.systemHeight);
            }
            else
            if (RectTransform.pivot.y == 0)
            {
                SetHeight((1f - safeAreaRT.anchorMax.y) * Display.main.systemHeight);
            }
            else
            {
                SetWidth(Display.main.systemWidth);
                SetHeight(Display.main.systemHeight);
            }
        }

        private void SetWidth(float width)
        {
            if (width != prevWidth)
            {
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                prevWidth = width;
            }
        }
        private void SetHeight(float height)
        {
            if (height != prevHeight)
            {
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                prevHeight = height;
            }
        }
    }
}