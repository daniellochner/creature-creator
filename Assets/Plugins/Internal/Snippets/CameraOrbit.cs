using UnityEngine;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    public class CameraOrbit : MonoBehaviour
    {
        #region Fields
        [Header("Rotate")]
        [SerializeField] private Transform rotationTransform;
        [SerializeField] private bool freezeRotation;
        [SerializeField] private Vector2 mouseSensitivity;
        [SerializeField] private float rotationSmoothing;
        [SerializeField] private Vector2 minMaxRotation;

        [Header("Zoom")]
        [SerializeField] private Transform zoomTransform;
        [SerializeField] private bool freezeZoom;
        [SerializeField] private float scrollWheelSensitivity;
        [SerializeField] private float zoomSmoothing;
        [SerializeField] private Vector2 minMaxZoom;

        [Header("Other")]
        [SerializeField] private bool handleClipping;

        private float targetZoom = 1f;
        private Vector3 targetRotation;
        private Vector2 velocity;
        #endregion

        #region Properties
        public bool IsFrozen { get; private set; }
        public bool HasInteractedWithUI { get; private set; }
        public Vector3 OffsetPosition { get; set; }

        public Camera Camera { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Camera = GetComponentInChildren<Camera>();

            OffsetPosition = zoomTransform.localPosition;
            targetRotation = rotationTransform.localEulerAngles;
        }
        private void LateUpdate()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0) || Input.mouseScrollDelta.magnitude > 0)
                {
                    Freeze();
                    HasInteractedWithUI = true;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Unfreeze();
                }
            }
            else if ((Input.GetMouseButtonUp(0) || Input.mouseScrollDelta.magnitude > 0) && HasInteractedWithUI)
            {
                Unfreeze();
                HasInteractedWithUI = false;
            }

            OnRotate();
            OnZoom();
        }

        private void OnZoom()
        {
            if (!freezeZoom && !IsFrozen)
            {
                HandleClipping(Mathf.Clamp(targetZoom - Input.mouseScrollDelta.y * scrollWheelSensitivity, minMaxZoom.x, minMaxZoom.y));
            }

            zoomTransform.localPosition = Vector3.Lerp(zoomTransform.localPosition, OffsetPosition * targetZoom, Time.deltaTime * zoomSmoothing);
        }
        private void OnRotate()
        {
            if (Input.GetMouseButton(0) && !freezeRotation && !IsFrozen)
            {
                velocity.x += mouseSensitivity.x * Input.GetAxis("Mouse X");
                velocity.y += mouseSensitivity.y * Input.GetAxis("Mouse Y");
            }

            targetRotation.y += velocity.x;
            targetRotation.x -= velocity.y;
            targetRotation.x = ClampAngle(targetRotation.x, minMaxRotation.x, minMaxRotation.y);

            rotationTransform.localRotation = Quaternion.Euler(targetRotation.x, targetRotation.y, 0);

            velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * rotationSmoothing);
            velocity.y = Mathf.Lerp(velocity.y, 0, Time.deltaTime * rotationSmoothing);
        }

        private void HandleClipping(float zoom)
        {
            if (handleClipping && Physics.Raycast(transform.position, Camera.transform.position - transform.position, out RaycastHit hitInfo, minMaxZoom.y))
            {
                targetZoom = Vector3.Distance(hitInfo.point, transform.position);
            }
            else
            {
                targetZoom = zoom;
            }
        }

        public void SetFrozen(bool isFrozen)
        {
            IsFrozen = isFrozen;
        }
        public void Freeze()
        {
            SetFrozen(true);
        }
        public void Unfreeze()
        {
            SetFrozen(false);
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) { angle += 360f; }
            if (angle > 360f) { angle -= 360f; }

            return Mathf.Clamp(angle, min, max);
        }
        #endregion
    }
}