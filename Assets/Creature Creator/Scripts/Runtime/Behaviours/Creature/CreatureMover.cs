// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAnimator))]
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureMover : MonoBehaviour
    {
        #region Fields
        [Header("Setup")]
        [SerializeField] private Follower cameraFollower;
        [SerializeField] private CameraOrbit cameraOrbit;
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private Platform platform;

        [Header("Physical")]
        [SerializeField] private bool requestToMove;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float moveSmoothTime;
        [SerializeField] private float turnSmoothTime;
        [SerializeField] private float angleToMove;
        [SerializeField] private float thresholdWalkSpeed;
        
        [Header("Non-Physical")]
        [SerializeField] private float positionSmoothing;
        [SerializeField] private float rotationSmoothing;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Vector3 velocity;
        [SerializeField, ReadOnly] private Vector3 angularVelocity;

        private Camera mainCamera;
        private Animator targetAnimator;
        private CapsuleCollider capsuleCollider;
        private GameObject targetGO;
        private new Rigidbody rigidbody;

        private bool isMovable;
        private Vector3 keyboardForward, keyboardRight, moveDisplacement;
        private InputMode inputMode;
        #endregion

        #region Properties
        public bool RequestToMove => requestToMove;
        
        public CreatureConstructor CreatureConstructor { get; private set; }
        public CreatureAnimator CreatureAnimator { get; private set; }

        public Platform Platform { get; set; }
        public Vector3 TargetPosition { get; set; }

        public Action<Vector3> OnMoveRequest { get; set; }
        public Action<float> OnTurnRequest { get; set; }

        public bool IsMovable
        {
            get => isMovable;
            set
            {
                isMovable = value;

                //cameraFollower.useFixedUpdate = isMovable; // Used to fix camera jittering issue.

                if (!isMovable)
                {
                    moveDisplacement = Vector3.zero;
                }

                // Physics
                foreach (Transform bone in CreatureConstructor.Bones)
                {
                    bone.GetComponent<Rigidbody>().isKinematic = isMovable;
                }
                rigidbody.constraints = isMovable ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezeAll; // Setting "isKinematic" to false will invoke OnTriggerExit().
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        private void LateUpdate()
        {
            if (IsMovable)
            {
                HandleMovement();
            }
            else
            {
                HandlePlatform();
            }
        }

        private void Initialize()
        {
            CreatureConstructor = GetComponent<CreatureConstructor>();
            CreatureAnimator = GetComponent<CreatureAnimator>();

            capsuleCollider = GetComponent<CapsuleCollider>();
            rigidbody = GetComponent<Rigidbody>();
            mainCamera = Camera.main;

            Platform = platform;
        }

        private void HandleMovement()
        {
            bool kInput = Input.GetButton("Vertical") || Input.GetButton("Horizontal");
            bool pInput = Input.GetMouseButton(1);

            bool setK = kInput || (!pInput && inputMode == InputMode.Keyboard);
            bool setP = pInput || (!kInput && inputMode == InputMode.Pointer);

            if (setK)
            {
                inputMode = InputMode.Keyboard;
            }
            else if (setP)
            {
                inputMode = InputMode.Pointer;
            }

            Vector3 direction = Vector3.zero;
            bool canTurn = false, canMove = false;

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
                    canTurn = true;
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
                        TargetPosition = position;
                    }

                    Vector3 displacement = Vector3.ProjectOnPlane(TargetPosition - transform.position, transform.up);

                    direction = displacement.normalized;
                    canTurn = displacement.magnitude > moveSpeed * moveSmoothTime;
                    canMove = canTurn;

                    break;
                    #endregion
            }

            if (targetAnimator != null)
            {
                targetAnimator.SetBool("IsHolding", pInput && !kInput);
            }

            if (direction != Vector3.zero)
            {
                float angle = Vector3.SignedAngle(transform.forward, direction, transform.up);
                if (canTurn)
                {
                    RequestTurn(angle);
                }
                canMove &= Vector3.Angle(CreatureConstructor.Body.forward, direction) < angleToMove;
            }
            RequestMove(canMove ? direction : Vector3.zero);
        }
        private void HandlePlatform()
        {
            transform.LerpTo(Platform.transform.position, positionSmoothing);
            CreatureConstructor.Body.SlerpTo(transform.rotation, rotationSmoothing);
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
        public void RequestTurn(float angle)
        {
            if (requestToMove)
            {
                OnTurnRequest?.Invoke(angle);
            }
            else
            {
                Turn(angle);
            }
        }
        public void Move(Vector3 direction)
        {
            Vector3 displacement = direction * moveSpeed * Time.deltaTime;
            moveDisplacement = Vector3.SmoothDamp(moveDisplacement, displacement, ref velocity, moveSmoothTime);
            transform.position += moveDisplacement;
        }
        public void Turn(float angle)
        {
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
            CreatureConstructor.Body.localRotation = QuaternionUtility.SmoothDamp(CreatureConstructor.Body.localRotation, rotation, ref angularVelocity, turnSmoothTime, turnSpeed, Time.deltaTime);
        }

        public void Teleport(Vector3 position, Quaternion rotation)
        {
            transform.position = (TargetPosition = position);
            CreatureConstructor.Body.rotation = transform.rotation;

            moveDisplacement = Vector3.zero;
        }
        public void TeleportToPlatform()
        {
            Teleport(Platform.transform.position, Platform.transform.rotation);
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