using System;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public abstract class MouseHint : MonoBehaviour
    {
        #region Fields
        protected Image icon;
        #endregion
        
        #region Methods
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

        public void Setup(Hint hint)
        {
            icon.sprite = hint.icon;

            RectTransform rt = transform as RectTransform;
            rt.sizeDelta *= hint.scale;
            rt.pivot = hint.pivot;
        }
        #endregion

        #region Nested
        [Serializable]
        public class Hint
        {
            public Sprite icon;
            public Vector2 pivot;
            public float scale;
        }
        #endregion
    }
}