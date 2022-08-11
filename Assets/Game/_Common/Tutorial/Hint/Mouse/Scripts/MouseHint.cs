using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public abstract class MouseHint : MonoBehaviour
    {
        protected Image icon;
        private void Awake()
        {
            icon = GetComponent<Image>();
        }

        protected Vector3 GetPosition(Transform target, bool inWorld)
        {
            Vector3 pos = target.position;
            if (inWorld)
            {
                pos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
            }
            return pos;
        }
    }
}