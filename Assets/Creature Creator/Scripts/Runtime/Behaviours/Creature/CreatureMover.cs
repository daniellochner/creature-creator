// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor), typeof(CreatureAnimator))]
    public class CreatureMover : MonoBehaviour
    {
        #region Fields
        [Header("Setup")]
        [SerializeField] private Follower cameraFollower;
        [SerializeField] private CameraOrbit cameraOrbit;
        [SerializeField] private GameObject targetPrefab;

        [Header("Physical")]
        [SerializeField] private bool requestToMove;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float movementSmoothing;
        [SerializeField] private float angleToMove;
        [SerializeField] private float contactDistance;
        [SerializeField] private float stoppingDistance;
        [SerializeField] private float thresholdWalkSpeed;
        
        [Header("Non-Physical")]
        [SerializeField] private float positionSmoothing;
        [SerializeField] private float rotationSmoothing;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Vector3 velocity;

        private Camera mainCamera;
        private Animator targetAnimator;
        private CapsuleCollider capsuleCollider;
        private GameObject targetGO;
        private Rigidbody rb;

        private bool isMovable, isGrounded, usePhysicalMovement;
        private int flapCount;
        private Vector3 moveDisplacement, targetPosition, keyboardForward, keyboardRight, previousPosition;
        private InputMode inputMode;
        #endregion

        #region Properties
        public bool RequestToMove => requestToMove;
        
        public CreatureConstructor CreatureConstructor { get; private set; }
        public CreatureAnimator CreatureAnimator { get; private set; }
        public Transform Platform { get; set; }

        public Action<Vector3> OnMoveRequest { get; set; }
        public Action<Quaternion> OnRotateRequest { get; set; }

        public bool IsGrounded
        {
            get => isGrounded;
        }
        public bool IsMovable
        {
            get => isMovable;
            set
            {
                isMovable = value;

                // Physics
                foreach (Transform bone in CreatureConstructor.Bones)
                {
                    bone.GetComponent<Rigidbody>().isKinematic = isMovable;
                }
                rb.constraints = isMovable ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezeAll; // Setting "isKinematic" to false will invoke OnTriggerExit().
            }
        }
        public bool UsePhysicalMovement
        {
            get => usePhysicalMovement;
            set
            {
                usePhysicalMovement = value;
                cameraFollower.useFixedUpdate = usePhysicalMovement; // Used to fix camera jittering issue.

                moveDisplacement = Vector3.zero;
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        private void Update()
        {
            if (IsMovable)
            {
                HandleInput();
            }
        }
        private void FixedUpdate()
        {
            if (UsePhysicalMovement)
            {
                HandlePhysicalMovement();
            }
        }
        private void LateUpdate()
        {
            if (!UsePhysicalMovement && !IsMovable)
            {
                HandleNonPhysicalMovement();
            }
        }

        private void Initialize()
        {
            CreatureConstructor = GetComponent<CreatureConstructor>();
            CreatureAnimator = GetComponent<CreatureAnimator>();

            capsuleCollider = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            mainCamera = Camera.main;

            Platform = transform;
        }

        private void HandleInput()
        {
            bool kInput = Input.GetButton("Vertical") || Input.GetButton("Horizontal");
            bool pInput = Input.GetMouseButton(1);

            if ((kInput || pInput) && !UsePhysicalMovement)
            {
                UsePhysicalMovement = true;
            }

            bool setK = kInput || (!pInput && inputMode == InputMode.Keyboard);
            bool setP = pInput || (!kInput && inputMode == InputMode.Pointer);

            if (setK)
            {
                SetInputMode(InputMode.Keyboard);
            }
            else if (setP)
            {
                SetInputMode(InputMode.Pointer);
            }

            Vector3 direction = Vector3.zero;
            bool canRotate = false, canMove = false;

            switch (inputMode)
            {
                #region Keyboard
                case InputMode.Keyboard:

                    if (!Input.GetKey(KeyCode.LeftAlt) || !kInput) // Free-look when holding ALT.
                    {
                        keyboardForward = Vector3.ProjectOnPlane(cameraOrbit.Camera.transform.forward, transform.up);
                        keyboardRight = Vector3.ProjectOnPlane(cameraOrbit.Camera.transform.right, transform.up);
                    }

                    Vector3 vertical = keyboardForward * Input.GetAxisRaw("Vertical");
                    Vector3 horizontal = keyboardRight * Input.GetAxisRaw("Horizontal");

                    direction = (vertical + horizontal).normalized;
                    canRotate = true;
                    canMove = kInput && !Input.GetKey(KeyCode.LeftControl);

                    break;
                    #endregion
                
                #region Pointer
                case InputMode.Pointer:

                    if (pInput && Physics.Raycast(RectTransformUtility.ScreenPointToRay(mainCamera, Input.mousePosition), out RaycastHit raycastHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                    {
                        Vector3 position = raycastHit.point;
                        Quaternion rotation = Quaternion.LookRotation(raycastHit.normal, transform.up);

                        if (Input.GetMouseButtonDown(1) || targetGO == null)
                        {
                            targetGO = Instantiate(targetPrefab, position, rotation, Dynamic.WorldCanvas);
                            targetAnimator = targetGO.GetComponent<Animator>();
                        }

                        targetGO.transform.SetPositionAndRotation(position, rotation);
                        SetTargetPosition(position);
                    }

                    Vector3 displacement = Vector3.ProjectOnPlane(targetPosition - transform.position, transform.up);

                    direction = displacement.normalized;
                    canRotate = displacement.magnitude > stoppingDistance;
                    canMove = canRotate;

                    break;
                    #endregion
            }

            if (targetAnimator != null)
            {
                targetAnimator.SetBool("IsHolding", pInput && !kInput);
            }

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up);
                if (canRotate)
                {
                    RequestRotate(targetRotation);
                }
                canMove &= Quaternion.Angle(CreatureConstructor.Body.rotation, targetRotation) < angleToMove;
            }
            RequestMove(canMove ? direction : Vector3.zero);
        }
        private void HandlePhysicalMovement()
        {
            rb.MovePosition(rb.position + moveDisplacement * Time.deltaTime);
            isGrounded = Physics.Raycast(transform.position + Vector3.up * contactDistance, -transform.up, 2f * contactDistance);
        }
        private void HandleNonPhysicalMovement()
        {
            transform.position = Vector3.Lerp(transform.position, Platform.transform.position, Time.deltaTime * positionSmoothing);
            CreatureConstructor.Body.rotation = Quaternion.Slerp(CreatureConstructor.Body.rotation, transform.rotation, Time.deltaTime * rotationSmoothing);
        }

        public void RequestMove(Vector3 direction)
        {
            if (requestToMove)
            {
                OnMoveRequest?.Invoke(direction);
            }
            else
            {
                Move(direction);
            }
        }
        public void RequestRotate(Quaternion rotation)
        {
            if (requestToMove)
            {
                OnRotateRequest?.Invoke(rotation);
            }
            else
            {
                Rotate(rotation);
            }
        }
        public void Move(Vector3 direction)
        {
            moveDisplacement = Vector3.SmoothDamp(moveDisplacement, direction * movementSpeed, ref velocity, movementSmoothing);
        }
        public void Rotate(Quaternion rotation)
        {
            CreatureConstructor.Body.rotation = Quaternion.RotateTowards(CreatureConstructor.Body.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        public void SetTargetPosition(Vector3 position)
        {
            targetPosition = position;
        }
        public void SetInputMode(InputMode mode)
        {
            inputMode = mode;
        }
        #endregion

        #region Enums
        public enum InputMode
        {
            None,
            Pointer,
            Keyboard
        }
        #endregion
    }
}