using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class SizeMatcher : UIBehaviour
    {
        [SerializeField] private RectTransform target;

        private RectTransform RectTransform => transform as RectTransform;

        protected override void OnRectTransformDimensionsChange()
        {
            Match();
        }

        [ContextMenu("Match")]
        public void Match()
        {
            if (target != null)
            {
                target.sizeDelta = RectTransform.sizeDelta;
                LayoutRebuilder.MarkLayoutForRebuild(target);
            }
        }
    }
}