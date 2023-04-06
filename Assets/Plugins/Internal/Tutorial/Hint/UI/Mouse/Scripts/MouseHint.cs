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
            Vector3 pos = target ? target.position : Vector3.zero;
            if (inWorld)
            {
                pos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
            }
            return pos;
        }
        #endregion
    }
}