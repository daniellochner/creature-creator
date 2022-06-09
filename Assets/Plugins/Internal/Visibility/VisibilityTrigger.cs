using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class VisibilityTrigger : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float radius = 100f;
        [SerializeField] private UnityEvent onShow = new UnityEvent();
        [SerializeField] private UnityEvent onHide = new UnityEvent();

        private TriggerRegion region;
        private Transform root;
        #endregion

        #region Properties
        public UnityEvent OnShow => onShow;
        public UnityEvent OnHide => onHide;
        #endregion

        #region Methods
        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                VisibilitySource.Instance.Triggers.Remove(this);
                Destroy(root.gameObject);
            }
        }
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, radius);
        }

        public void Setup()
        {
            // Trigger
            root = new GameObject("Trigger").transform;
            root.parent = transform;
            root.localPosition = Vector3.zero;
            root.gameObject.layer = LayerMask.NameToLayer("Visibility");

            SphereCollider col = root.gameObject.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = radius;

            region = root.gameObject.AddComponent<TriggerRegion>();
            region.OnCanEnter = delegate (Collider collider)
            {
                return collider.GetComponentInParent<VisibilitySource>() != null;
            };
            region.OnEnter.AddListener(delegate
            {
                Show();
            });
            region.OnExit.AddListener(delegate
            {
                Hide();
            });

            // Hide
            if (Vector3.Distance(VisibilitySource.Instance.transform.position, transform.position) > radius)
            {
                Hide();
            }
        }

        public void Show()
        {
            root.parent = transform;
            gameObject.SetActive(true);
            onShow.Invoke();
        }
        public void Hide()
        {
            root.parent = Dynamic.Transform;
            gameObject.SetActive(false);
            onHide.Invoke();
        }
        public void Validate()
        {
            region.Validate();
        }
        #endregion
    }
}