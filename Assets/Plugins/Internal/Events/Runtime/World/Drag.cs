// Events
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class Drag : MonoBehaviour
    {
        #region Fields
        // Movement
        public int mouseButton = 0;
        public MousePlaneAlignment mousePlaneAlignment = MousePlaneAlignment.ToLocalDirection;
        public Vector3 localDirection = new Vector3(1, 0, 0);
        public Vector3 worldDirection = new Vector3(1, 0, 0);
        public float smoothing = 0f;
        public float maxDistance = Mathf.Infinity;
        public Transform maxDistanceFromTarget = null;
        public EnabledAxes localMovement = new EnabledAxes()
        {
            x = true,
            y = true,
            z = true
        };

        // Bounds
        public bool isBounded = true;
        public BoundsShape boundsShape = BoundsShape.Cylinder;
        public float boxSize = Mathf.Infinity;
        public float sphereRadius = Mathf.Infinity;
        public float cylinderRadius = Mathf.Infinity;
        public float cylinderHeight = Mathf.Infinity;
        public Vector3 boundsOffset = Vector3.zero;

        // Other
        public Transform world = null;
        public bool controlDrag = true;
        public bool draggable = true;
        public bool resetOnRelease = false;
        public bool useOffsetPosition = true;
        public bool updatePlaneOnPress = false;
        public float dragThreshold = 0f;
        public Collider customCollider;

        // Events
        public UnityEvent onPress = new UnityEvent();
        public UnityEvent onRelease = new UnityEvent();
        public UnityEvent onDrag = new UnityEvent();
        public UnityEvent onBeginDrag = new UnityEvent();
        public UnityEvent onEndDrag = new UnityEvent();

        private Vector3 startWorldPosition, offsetPosition;
        private Vector2 initialMousePosition;
        #endregion

        #region Properties
        public UnityEvent OnPress => onPress;
        public UnityEvent OnRelease => onRelease;
        public UnityEvent OnDrag => onDrag;
        public UnityEvent OnBeginDrag => onBeginDrag;
        public UnityEvent OnEndDrag => onEndDrag;

        public Vector3 TargetPosition { get; private set; }
        public Vector3 ClampFromPosition { get; set; }

        public Plane Plane { get; set; }
        public bool IsPressing { get; set; }
        public bool IsDragging { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            UpdatePlane();
        }
        private void Update()
        {
            if (Input.GetMouseButtonUp(mouseButton) && IsPressing) // "OnMouseUp()" is unreliable.
            {
                if (resetOnRelease)
                {
                    transform.position = startWorldPosition;
                }

                if (IsDragging)
                {
                    OnEndDrag.Invoke();
                    IsDragging = false;
                }
                OnRelease.Invoke();

                IsPressing = false;
            }
        }
        private void FixedUpdate()
        {
            if (IsPressing && draggable)
            {
                Ray ray = CameraUtility.MainCamera.ScreenPointToRay(Input.mousePosition);

                Vector3? targetWorldPosition = null;
                if (customCollider != null)
                {
                    if (customCollider.Raycast(ray, out RaycastHit hitInfo, 100f))
                    {
                        targetWorldPosition = hitInfo.point;
                    }
                }
                else if (Plane.Raycast(ray, out float distance))
                {
                    targetWorldPosition = ray.GetPoint(distance);
                }

                if (targetWorldPosition != null)
                {
                    // Restrict movement along the specified LOCAL axes, then
                    // clamp the targeted position based on the specified WORLD
                    // bounds and maximum distance from starting position.

                    Vector3 targetLocalPosition = transform.InverseTransformPoint((Vector3)targetWorldPosition) - offsetPosition;
                    targetLocalPosition.x = localMovement.x ? targetLocalPosition.x : 0;
                    targetLocalPosition.y = localMovement.y ? targetLocalPosition.y : 0;
                    targetLocalPosition.z = localMovement.z ? targetLocalPosition.z : 0;

                    TargetPosition = ClampToBounds(transform.TransformPoint(targetLocalPosition));

                    if (controlDrag)
                    {
                        transform.position = TargetPosition;
                    }

                    if (IsDragging)
                    {
                        OnDrag.Invoke();
                    }
                    else if (Vector2.Distance(initialMousePosition, Input.mousePosition) > dragThreshold)
                    {
                        IsDragging = true;
                        OnBeginDrag.Invoke();
                    }
                }
            }
        }

        public void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(mouseButton))
            {
                OnMouseButtonDown();
            }
        }
        public void OnMouseButtonDown()
        {
            if (CanvasUtility.IsPointerOverUI) return;

            if (updatePlaneOnPress)
            {
                UpdatePlane();
            }

            Ray ray = CameraUtility.MainCamera.ScreenPointToRay(initialMousePosition = Input.mousePosition);
            if (Plane.Raycast(ray, out float distance))
            {
                ClampFromPosition = startWorldPosition = transform.position;
                if (useOffsetPosition)
                {
                    offsetPosition = transform.InverseTransformPoint(ray.GetPoint(distance));
                }
            }

            OnPress.Invoke();

            IsPressing = true;
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (world == null)
            {
                return;
            }

            if (UnityEditor.Selection.activeTransform == transform)
            {
                switch (boundsShape)
                {
                    case BoundsShape.Box:
                        Gizmos.DrawWireCube(world.position + world.TransformDirection(boundsOffset), Vector3.one * boxSize);
                        break;
                    case BoundsShape.Cylinder:
                        UnityEditor.Handles.DrawWireDisc(world.position + world.TransformDirection(boundsOffset), world.up, cylinderRadius);
                        UnityEditor.Handles.DrawWireDisc(world.position + world.TransformDirection(boundsOffset) + world.up * cylinderHeight, world.up, cylinderRadius);
                        break;
                    case BoundsShape.Sphere:
                        Gizmos.DrawWireSphere(world.position + world.TransformDirection(boundsOffset), sphereRadius);
                        break;
                }
            }
#endif
        }

        public Vector3 ClampToBounds(Vector3 position)
        {
            if (world == null || !isBounded)
            {
                return position;
            }

            Vector3 worldPosition = world.InverseTransformPoint(ClampFromPosition + Vector3.ClampMagnitude(position - ClampFromPosition, maxDistance));

            switch (boundsShape)
            {
                case BoundsShape.Box:
                    worldPosition.x = Mathf.Clamp(worldPosition.x, boundsOffset.x - boxSize / 2f, boundsOffset.x + boxSize / 2f);
                    worldPosition.y = Mathf.Clamp(worldPosition.y, boundsOffset.y - boxSize / 2f, boundsOffset.y + boxSize / 2f);
                    worldPosition.z = Mathf.Clamp(worldPosition.z, boundsOffset.z - boxSize / 2f, boundsOffset.z + boxSize / 2f);
                    break;

                case BoundsShape.Cylinder:
                    Vector2 xzPlane = new Vector2(boundsOffset.x, boundsOffset.z) + Vector2.ClampMagnitude(new Vector2(worldPosition.x - boundsOffset.x, worldPosition.z - boundsOffset.z), cylinderRadius);
                    worldPosition.x = xzPlane.x;
                    worldPosition.z = xzPlane.y;
                    worldPosition.y = Mathf.Clamp(worldPosition.y, boundsOffset.y, boundsOffset.y + cylinderHeight);
                    break;

                case BoundsShape.Sphere:
                    worldPosition = boundsOffset + Vector3.ClampMagnitude(worldPosition - boundsOffset, sphereRadius);
                    break;
            }

            return world.TransformPoint(worldPosition);
        }
        public void ResetEvents()
        {
            OnPress.RemoveAllListeners();
            OnRelease.RemoveAllListeners();
            OnDrag.RemoveAllListeners();
            OnBeginDrag.RemoveAllListeners();
            OnEndDrag.RemoveAllListeners();
        }

        private void UpdatePlane()
        {
            if (mousePlaneAlignment == MousePlaneAlignment.WithCamera)
            {
                Plane = new Plane(CameraUtility.MainCamera.transform.forward, transform.position);
            }
            else if (mousePlaneAlignment == MousePlaneAlignment.ToLocalDirection)
            {
                Plane = new Plane(transform.TransformDirection(localDirection), transform.position);
            }
            else if (mousePlaneAlignment == MousePlaneAlignment.ToWorldDirection)
            {
                Plane = new Plane(world == null ? worldDirection : world.TransformDirection(worldDirection), transform.position);
            }
        }
        #endregion

        #region Enumerators
        public enum MousePlaneAlignment
        {
            ToLocalDirection,
            ToWorldDirection,
            WithCamera
        }
        public enum BoundsShape
        {
            Box,
            Sphere,
            Cylinder
        }
        #endregion

        #region Nested
        [Serializable]
        public class EnabledAxes
        {
            public bool x, y, z;
        }
        #endregion
    }
}