using System.Collections.Generic;
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

        private List<int> pressed = new List<int>();

        private Vector3 initialPosition, initialRotation;
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

            initialPosition = OffsetPosition = zoomTransform.localPosition;
            initialRotation = targetRotation = rotationTransform.localEulerAngles;
        }
        private void Update()
        {
            HandlePressed();

            OnRotate();
            OnZoom();
        }
        public void Reset()
        {
            zoomTransform.localPosition = OffsetPosition = initialPosition;
            rotationTransform.localEulerAngles = targetRotation = initialRotation;

            velocity = Vector2.zero;
            targetZoom = 1f;
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
                        pressed.Add(touch.fingerId);
                    }
                    else
                    if (touch.phase == TouchPhase.Ended)
                    {
                        if ((++hasEnded) == Input.touchCount)
                        {
                            isPressing = false;
                        }
                        pressed.Remove(touch.fingerId);
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
                    if (Input.touchCount >= 2 && pressed.Count >= 2)
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
            if (isPressing && !freezeRotation && !IsFrozen && CanInput && pressed.Count < 2)
            {
                if (SystemUtility.IsDevice(DeviceType.Desktop))
                {
                    float deltaX = Input.GetAxis("Mouse X");
                    float deltaY = Input.GetAxis("Mouse Y");
                    AddVelocity(deltaX, deltaY, ref velocity);
                }
                else
                if (SystemUtility.IsDevice(DeviceType.Handheld))
                {
                    foreach (int fingerId in pressed)
                    {
                        if (fingerId > Input.touchCount - 1) continue;

                        Touch touch = Input.GetTouch(fingerId);
                        float deltaX = touch.deltaPosition.x * 0.1f;
                        float deltaY = touch.deltaPosition.y * 0.1f;
                        AddVelocity(deltaX, deltaY, ref velocity);
                    }
                }
            }

            targetRotation.y += velocity.x;
            targetRotation.x -= velocity.y;
            targetRotation.x = QuaternionUtility.ClampAngle(targetRotation.x, minMaxRotation.x, minMaxRotation.y);

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

        private void AddVelocity(float deltaX, float deltaY, ref Vector3 velocity)
        {
            float x = (invertMouseX ? -1f : 1f) * deltaX;
            float y = (invertMouseY ? -1f : 1f) * deltaY;

            velocity.x += 5f * x * mouseSensitivity.x;
            velocity.y += 5f * y * mouseSensitivity.y;
        }
        #endregion
    }
}