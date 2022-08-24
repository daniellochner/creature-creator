using UnityEngine;

namespace DanielLochner.Assets
{
    public class VisibilitySource : MonoBehaviour
    {
        #region Methods
        private void OnEnable()
        {
            VisibilityManager.Instance.Source = this;
        }
        private void OnDisable()
        {
            if (VisibilityManager.Instance.Source == this)
            {
                VisibilityManager.Instance.Source = null;
            }
        }
        #endregion
    }
}