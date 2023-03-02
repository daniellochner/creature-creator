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
        [SerializeField] private bool invertMouseX;
        [SerializeField] private bool invertMouseY;

        [Header("Zoom")]
        [SerializeField] private Transform zoomTransform;
        [SerializeField] private bool freezeZoom;
        [SerializeField] private float scrollWheelSensitivity;
        [SerializeField] private float zoomSmoothing;
        [SerializeField] private Vector2 minMaxZoom;

        [Header("Other")]
        [SerializeField] private bool handleClipping;
        [SerializeField] private bool snapClipping;
        [SerializeField] private LayerMask clippingMask;

        private float targetZoom = 1f;
        private Vector3 targetRotation;
        private Vector2 velocity;
        private float prevClippingZoom = -1;

        private float currentDistance, initialDistance, initialZoom;
        private bool isInitialTouch = true, isPressing;
        #endregion

        #region Properties
        public Vector2 MouseSensitivity
        {
            get => mouseSensitivity;
            set => mouseSensitivity = value;
        }
        public bool InvertMouseX
        {
            get => invertMouseX;
            set => invertMouseX = value;
        }
        public bool InvertMouseY
        {
            get => invertMouseY;
            set => invertMouseY = value;
        }
        public bool HandleClipping
        {
            get => handleClipping;
            set => handleClipping = value;
        }

        public virtual bool CanInput { get; set; } = true;
        public bool IsFrozen { get; private set; }
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
        private void Update()
        {
            HandlePressed();

            OnRotate();
            OnZoom();
        }

        private void HandlePressed()
        {
            if (SystemUtility.IsDevice(DeviceType.Desktop))
            {
                if (Input.GetMouseButtonDown(0) && !CanvasUtility.IsPointerOverUI)
                {
                    isPressing = true;
                }
                else
                if (Input.GetMouseButtonUp(0))
                {
                    isPressing = false;
                }
            }
            else
            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                int hasEnded = 0;
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        isPressing = true;
                    }
                    else
                    if (touch.phase == TouchPhase.Ended)
                    {
                        if ((++hasEnded) == Input.touchCount)
                        {
                            isPressing = false;
                        }
                    }
                }
            }
        }

        private void OnZoom()
        {
            if (!freezeZoom && !IsFrozen && CanInput)
            {
                float zoom = targetZoom;
                if (SystemUtility.IsDevice(DeviceType.Desktop))
                {
                    if (Input.mouseScrollDelta != Vector2.zero && !CanvasUtility.IsPointerOverUI)
                    {
                        zoom = Mathf.Clamp(targetZoom - Input.mouseScrollDelta.y * scrollWheelSensitivity, minMaxZoom.x, minMaxZoom.y);
                    }
                }
                else
                if (SystemUtility.IsDevice(DeviceType.Handheld))
                {
                    if (Input.touchCount >= 2 && !CanvasUtility.IsPointerOverUI)
                    {
                        Vector2 pos1 = Input.touches[0].position;
                        Vector2 pos2 = Input.touches[1].position;

                        currentDistance = Vector2.Distance(pos1, pos2);

                        if (isInitialTouch)
                        {
                            initialZoom = targetZoom;
                            initialDistance = currentDistance;
                            isInitialTouch = false;
                        }

                        zoom = Mathf.Clamp(initialZoom * (initialDistance / currentDistance), minMaxZoom.x, minMaxZoom.y);
                    }
                    else
                    {
                        isInitialTouch = true;
                    }
                }
                OnHandleClipping(zoom);
            }

            zoomTransform.localPosition = Vector3.Lerp(zoomTransform.localPosition, OffsetPosition * targetZoom, Time.deltaTime * zoomSmoothing);
        }
        private void OnRotate()
        {
            Vector3 velocity = Vector3.zero;
            if (isPressing && Input.GetMouseButton(0) && !freezeRotation && !IsFrozen && CanInput)
            {
                float pointerX = Input.GetAxis("Mouse X");
                float pointerY = Input.GetAxis("Mouse Y");
                if (SystemUtility.IsDevice(DeviceType.Handheld))
                {
                    foreach (Touch touch in Input.touches)
                    {
                        if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            pointerX = touch.deltaPosition.x / 5f;
                            pointerY = touch.deltaPosition.y / 5f;
                            break;
                        }
                    }
                }

                float x = (invertMouseX ? -1f : 1f) * pointerX;
                float y = (invertMouseY ? -1f : 1f) * pointerY;
                
                velocity.x += 5f * x * mouseSensitivity.x;
                velocity.y += 5f * y * mouseSensitivity.y;
            }

            targetRotation.y += velocity.x;
            targetRotation.x -= velocity.y;
            targetRotation.x = ClampAngle(targetRotation.x, minMaxRotation.x, minMaxRotation.y);

            rotationTransform.localRotation = Quaternion.Euler(targetRotation.x, targetRotation.y, 0);
        }

        private void OnHandleClipping(float zoom)
        {
            if (handleClipping)
            {
                float offsetDistance = OffsetPosition.magnitude;

                Vector3 dir = (Camera.transform.position - transform.position).normalized;
                Vector3 origin = transform.position + (dir * minMaxZoom.x);

                float maxDistance = offsetDistance * zoom - minMaxZoom.x;

                if (prevClippingZoom != -1)
                {
                    maxDistance = offsetDistance * prevClippingZoom - minMaxZoom.x;
                }

                if (Physics.Raycast(origin, dir, out RaycastHit hitInfo, maxDistance, clippingMask))
                {
                    float d = Mathf.Clamp(Vector3.Distance(hitInfo.point, transform.position), offsetDistance * minMaxZoom.x, offsetDistance * minMaxZoom.y);

                    float p = Mathf.InverseLerp(offsetDistance * minMaxZoom.x, offsetDistance * minMaxZoom.y, d);
                    float t = Mathf.Lerp(minMaxZoom.x, minMaxZoom.y, p);

                    if (prevClippingZoom == -1)
                    {
                        prevClippingZoom = targetZoom;
                    }

                    targetZoom = t;
                    if (snapClipping)
                    {
                        zoomTransform.localPosition = OffsetPosition * targetZoom;
                    }
                }
                else
                {
                    if (prevClippingZoom != -1)
                    {
                        targetZoom = prevClippingZoom;
                        prevClippingZoom = -1;
                    }
                    else
                    {
                        targetZoom = zoom;
                    }

                }
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

        public void SetOffset(Vector3 offset, bool instant = false)
        {
            OffsetPosition = offset;
            if (instant)
            {
                zoomTransform.localPosition = offset;
            }
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