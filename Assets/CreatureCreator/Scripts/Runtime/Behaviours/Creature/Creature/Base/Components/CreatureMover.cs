// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAnimator))]
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreatureCamera))]
    [RequireComponent(typeof(CreatureGrounded))]
    [RequireComponent(typeof(CreatureUnderwater))]
    [RequireComponent(typeof(CreatureAbilities))]
    [RequireComponent(typeof(BuoyantObject))]
    public class CreatureMover : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool requestToMove;
        [SerializeField] private float baseMovementSpeed;
        [SerializeField] private float baseTurnSpeed;
        [SerializeField] private float moveSmoothTime;
        [SerializeField] private float turnSmoothTime;
        [SerializeField] private float angleToMove;
        [SerializeField] private float thresholdWalkSpeed;
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private float airDrag;
        [SerializeField] private float joystickThreshold;
        [SerializeField] private float touchThreshold;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Vector3 velocity;
        [SerializeField, ReadOnly] private Vector3 angularVelocity;

        private Animator targetAnimator;
        private CapsuleCollider capsuleCollider;
        private GameObject targetGO;
        private new Rigidbody rigidbody;

        private bool isMovable;
        private Vector3 keyboardForward, keyboardRight, moveDisplacement, targetPosition;
        private Vector2 prevTouchPosition;
        private InputMode inputMode;
        private Vector3? touchPos = null;
        private Quaternion? touchRot = null;

#if USE_STATS
        private float displacementBuffer = 0f;
#endif
        #endregion

        #region Properties        
        public CreatureConstructor Constructor { get; private set; }
        public CreatureAnimator Animator { get; private set; }
        public CreatureCamera Camera { get; private set; }
        public CreatureGrounded Grounded { get; private set; }
        public CreatureUnderwater Underwater { get; private set; }
        public BuoyantObject BuoyantObject { get; private set; }
        public CreatureAbilities Abilities { get; private set; }

        public Action<Vector3> OnMoveRequest { get; set; }
        public Action<float> OnTurnRequest { get; set; }

        public bool CanMove = true, CanTurn = true;
        public Vector3 Direction { get; set; } = Vector3.zero;

        public bool CanInput
        {
            get
            {
                return !CinematicManager.Instance.IsInCinematic && EditorManager.Instance.IsPlaying && !InputDialog.Instance.IsOpen && !ConfirmationDialog.Instance.IsOpen && !InformationDialog.Instance.IsOpen && !PauseMenu.Instance.IsOpen;
            }
        }

        public float MoveSpeed
        {
            get => baseMovementSpeed * Constructor.Statistics.Speed;
        }
        public float TurnSpeed
        {
            get => baseTurnSpeed;
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        private void Update()
        {
            if (CanInput)
            {
                HandleInput();
            }
        }
        private void FixedUpdate()
        {
            HandleMovement();
            HandleGliding();
        }
        private void OnEnable()
        {
            StopMoving();

            foreach (Transform bone in Constructor.Bones)
            {
                bone.GetComponent<Rigidbody>().isKinematic = true;
            }
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            BuoyantObject.enabled = Abilities.Abilities.Find(x => x is Abilities.Swim) != null;
        }
        private void OnDisable()
        {
            if (targetAnimator != null)
            {
                targetAnimator.SetBool("IsHolding", false);
            }
            if (targetGO != null)
            {
                Destroy(targetGO);
            }

            BuoyantObject.enabled = false;
        }
        
        private void Initialize()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Animator = GetComponent<CreatureAnimator>();
            Camera = GetComponent<CreatureCamera>();
            Grounded = GetComponent<CreatureGrounded>();
            Underwater = GetComponent<CreatureUnderwater>();
            Abilities = GetComponent<CreatureAbilities>();
            BuoyantObject = GetComponent<BuoyantObject>();

            capsuleCollider = GetComponent<CapsuleCollider>();
            rigidbody = GetComponent<Rigidbody>();
        }

        private void HandleInput()
        {
            Direction = Vector3.zero;
            CanTurn = false;
            CanMove = false;

            if (SystemUtility.IsDevice(DeviceType.Desktop))
            {
                if (InputUtility.GetKey(KeybindingsManager.Data.WalkForwards) || InputUtility.GetKey(KeybindingsManager.Data.WalkBackwards) || InputUtility.GetKey(KeybindingsManager.Data.WalkLeft) || InputUtility.GetKey(KeybindingsManager.Data.WalkRight))
                {
                    inputMode = InputMode.Keyboard;
                }
                else
                if (Input.GetMouseButton(1))
                {
                    inputMode = InputMode.Pointer;
                }
            }
            else
            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                if (MobileControlsManager.Instance.Joystick.IsPressed)
                {
                    inputMode = InputMode.Joystick;
                }
                else
                if (Input.GetMouseButtonDown(0))
                {
                    inputMode = InputMode.Touch;
                }
            }

            switch (inputMode)
            {
                case InputMode.Keyboard:
                    HandleKeyboard();
                    break;

                case InputMode.Pointer:
                    HandlePointer();
                    break;

                case InputMode.Joystick:
                    HandleJoystick();
                    break;

                case InputMode.Touch:
                    HandleTouch();
                    break;
            }
        }
        private void HandleKeyboard()
        {
            if (!InputUtility.GetKey(KeybindingsManager.Data.FreeLook))
            {
                keyboardForward = Vector3.ProjectOnPlane(Camera.MainCamera.transform.forward, transform.up);
                keyboardRight = Vector3.ProjectOnPlane(Camera.MainCamera.transform.right, transform.up);
            }

            int vAxisRaw = InputUtility.GetKey(KeybindingsManager.Data.WalkForwards) ? 1 : (InputUtility.GetKey(KeybindingsManager.Data.WalkBackwards) ? -1 : 0);
            int hAxisRaw = InputUtility.GetKey(KeybindingsManager.Data.WalkRight) ? 1 : (InputUtility.GetKey(KeybindingsManager.Data.WalkLeft) ? -1 : 0);

            Vector3 vertical = keyboardForward * vAxisRaw;
            Vector3 horizontal = keyboardRight * hAxisRaw;

            Direction = (vertical + horizontal).normalized;
            CanMove = !InputUtility.GetKey(KeybindingsManager.Data.StopMove);
            CanTurn = true;

            targetPosition = transform.position;

            if (targetAnimator != null)
            {
                targetAnimator.SetBool("IsHolding", false);
            }
        }
        private void HandlePointer()
        {
            if (Input.GetMouseButton(1) && Physics.Raycast(RectTransformUtility.ScreenPointToRay(Camera.MainCamera, Input.mousePosition), out RaycastHit raycastHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Vector3 position = raycastHit.point;
                Quaternion rotation = Quaternion.LookRotation(raycastHit.normal, transform.up);

                if (Input.GetMouseButtonDown(1) || targetGO == null)
                {
                    targetGO = Instantiate(targetPrefab, position, rotation, Dynamic.WorldCanvas);
                    targetAnimator = targetGO.GetComponent<Animator>();
                }

                targetGO.transform.SetPositionAndRotation(position, rotation);
                targetPosition = position;
            }

            Vector3 displacement = Vector3.ProjectOnPlane(targetPosition - transform.position, transform.up);

            Direction = displacement.normalized;
            CanMove = CanTurn = displacement.magnitude > MoveSpeed * moveSmoothTime;

            if (targetAnimator != null)
            {
                targetAnimator.SetBool("IsHolding", Input.GetMouseButton(1));
            }
        }
        private void HandleJoystick()
        {
            Joystick joystick = MobileControlsManager.Instance.Joystick;

            Vector3 keyboardForward = Vector3.ProjectOnPlane(Camera.MainCamera.transform.forward, transform.up);
            Vector3 keyboardRight   = Vector3.ProjectOnPlane(Camera.MainCamera.transform.right,   transform.up);

            float v = Mathf.Abs(joystick.Vertical)   > joystickThreshold ? joystick.Vertical   : 0f;
            float h = Mathf.Abs(joystick.Horizontal) > joystickThreshold ? joystick.Horizontal : 0f;

            Vector3 vertical   = keyboardForward * v;
            Vector3 horizontal = keyboardRight * h;

            Direction = (vertical + horizontal).normalized;
            CanMove = CanTurn = true;

            targetPosition = transform.position;
        }
        private void HandleTouch()
        {
            if (Input.GetMouseButtonDown(0) && !CanvasUtility.IsPointerOverUI
                && Physics.Raycast(RectTransformUtility.ScreenPointToRay(Camera.MainCamera, Input.mousePosition), out RaycastHit raycastHit, Mathf.Infinity)
                && raycastHit.collider.GetComponent<Interactable>() == null
                && raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                prevTouchPosition = Input.mousePosition;

                touchPos = raycastHit.point;
                touchRot = Quaternion.LookRotation(raycastHit.normal, transform.up);
            }

            if (Input.GetMouseButtonUp(0) && !CanvasUtility.IsPointerOverUI 
                && Vector2.Distance(Input.mousePosition, prevTouchPosition) <= touchThreshold
                && touchPos != null
                && touchRot != null)
            {
                targetGO = Instantiate(targetPrefab, (Vector3)touchPos, (Quaternion)touchRot, Dynamic.WorldCanvas);
                targetPosition = (Vector3)touchPos;

                touchPos = null;
                touchRot = null;
            }

            Vector3 displacement = Vector3.ProjectOnPlane(targetPosition - transform.position, transform.up);

            Direction = displacement.normalized;
            CanMove = CanTurn = displacement.magnitude > MoveSpeed * moveSmoothTime;
        }

        private void HandleMovement()
        {
            if (Direction != Vector3.zero)
            {
                float angle = Vector3.SignedAngle(transform.forward, Direction, transform.up);
                if (CanTurn)
                {
                    RequestTurn(angle);
                }
                CanMove &= Mathf.Abs(angle) < angleToMove;
            }
            RequestMove(CanMove && CanInput ? Direction : Vector3.zero);
        }
        private void HandleGliding()
        {
            if (Underwater.IsUnderwater || Animator.Wings.Count == 0) return;
            rigidbody.drag = (!Grounded.IsGrounded) ? airDrag : 0f;
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
            Vector3 displacement = direction * MoveSpeed * Time.fixedDeltaTime;
            moveDisplacement = Vector3.SmoothDamp(moveDisplacement, displacement, ref velocity, moveSmoothTime);
            rigidbody.position += moveDisplacement;

#if USE_STATS
            displacementBuffer += moveDisplacement.magnitude;
            if (displacementBuffer >= 1)
            {
                StatsManager.Instance.DistanceTravelled++;
                displacementBuffer -= 1;
            }
#endif
        }
        public void Turn(float angle)
        {
            Quaternion rotation = Quaternion.Euler(0f, angle * baseTurnSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 0f);
            rigidbody.rotation *= rotation;
        }

        public void Teleport(Vector3 position)
        {
            transform.position = targetPosition = position;

            moveDisplacement = Vector3.zero;

            Camera.CameraOrbit.HandleClipping = false;
            this.Invoke(delegate
            {
                Camera.CameraOrbit.HandleClipping = true;
            },
            1f);
        }
        public void Teleport(Platform platform, bool start = false)
        {
            Teleport(platform.Position);

            if (start)
            {
                transform.rotation = platform.Rotation;
                Camera.Root.SetPositionAndRotation(platform.Position, platform.Rotation);
            }
        }

        public void StopMoving()
        {
            targetPosition = transform.position;
            moveDisplacement = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
        }
        #endregion

        #region Enums
        public enum InputMode
        {
            None,
            Pointer,
            Keyboard,
            Touch,
            Joystick
        }
        #endregion
    }
}