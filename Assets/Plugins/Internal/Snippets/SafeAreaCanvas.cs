using Crystal;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-999)]
    [RequireComponent(typeof(Canvas))]
    public class SafeAreaCanvas : MonoBehaviour
    {
        [SerializeField] private bool conformX = true;
        [SerializeField] private bool conformY = true;
        [SerializeField] private List<Transform> ignored;

        private void Awake()
        {
            RectTransform canvasRT = transform as RectTransform;

            // Record immediate children
            Transform[] children = new Transform[canvasRT.childCount];
            for (int i = 0; i < canvasRT.childCount; i++)
            {
                children[i] = canvasRT.GetChild(i);
            }

            // Create safe area
            RectTransform safeAreaRT = new GameObject("SafeArea", typeof(RectTransform)).transform as RectTransform;
            safeAreaRT.ExpandTo(canvasRT);

            // Set previously recorded children as children of safe area
            foreach (Transform child in children)
            {
                if (!ignored.Contains(child))
                {
                    child.SetParent(safeAreaRT, true);
                }
            }

            // Add safe area component
            SafeArea safeArea = safeAreaRT.gameObject.AddComponent<SafeArea>();
            safeArea.ConformX = conformX;
            safeArea.ConformY = conformY;
            safeArea.Apply(safeArea.GetSafeArea());
        }
    }
}