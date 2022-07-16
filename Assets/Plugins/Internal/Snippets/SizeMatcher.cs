using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(RectTransform))]
    public class SizeMatcher : MonoBehaviour
    {
        [SerializeField] private RectTransform source;

        private RectTransform RectTransform => transform as RectTransform;

        private void LateUpdate()
        {
            Match();
        }

        [ContextMenu("Match")]
        public void Match()
        {
            if (source != null)
            {
                RectTransform.sizeDelta = source.sizeDelta;
            }
        }
    }
}