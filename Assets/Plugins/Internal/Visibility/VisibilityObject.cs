using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class VisibilityObject : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool isVisible;
        [SerializeField] private float radius = 100f;
        [SerializeField] private UnityEvent onShow = new UnityEvent();
        [SerializeField] private UnityEvent onHide = new UnityEvent();
        #endregion

        #region Properties
        public UnityEvent OnShow => onShow;
        public UnityEvent OnHide => onHide;
        #endregion

        #region Methods
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public void Show()
        {
            if (!isVisible)
            {
                isVisible = true;
                onShow.Invoke();
            }
        }
        public void Hide()
        {
            if (isVisible)
            {
                isVisible = false;
                onHide.Invoke();
            }
        }

        public void CheckVisibility(Vector3 source)
        {
            if (Vector3Utility.SqrDistanceComp(transform.position, source, radius))
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
        #endregion
    }
}