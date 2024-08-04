using UnityEngine;

namespace DanielLochner.Assets
{
    public class LinkedTextHint : MonoBehaviour
    {
        public RectTransform copied;

        private void Start()
        {
            RectTransform rt = transform as RectTransform;
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, copied.rect.width);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, copied.rect.height);
            rt.SetPositionAndRotation(copied.position, copied.rotation);
        }
    }
}