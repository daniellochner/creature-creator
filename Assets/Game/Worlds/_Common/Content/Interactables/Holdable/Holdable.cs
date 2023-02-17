// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Holdable : CreatureInteractable
    {
        #region Fields
        [SerializeField] private HoldableDummy dummyPrefab;

        private Unity.Netcode.Components.NetworkTransform networkTransform;
        private Vector3 startPosition;
        private Quaternion startRotation;
        private HoldableDummy dummy;
        #endregion

        #region Properties
        public NetworkVariable<FixedString64Bytes> Hand { get; set; } = new NetworkVariable<FixedString64Bytes>();

        public HoldableDummy Dummy => dummy;

        public bool IsHeld => !Hand.Value.IsEmpty;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            networkTransform = GetComponent<Unity.Netcode.Components.NetworkTransform>();
        }
        private void Start()
        {
            startPosition = transform.position;
            startRotation = transform.rotation;

            Hand.OnValueChanged += OnHandChanged;

            if (IsHeld)
            {
                OnHandChanged("", Hand.Value);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsServer) return;
            if (collision.collider.CompareTag("WorldBorder"))
            {
                networkTransform.Teleport(startPosition, startRotation, transform.localScale);
            }
        }

        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && !EditorManager.Instance.IsEditing && Player.Instance.Holder.enabled && interactor.GetComponent<CreatureAnimator>().Arms.Count > 0;
        }
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            Player.Instance.Holder.TryHold(this);
        }
        
        public void Hold(LimbConstructor arm)
        {
            Hand.Value = arm.name;
        }
        public void Drop()
        {
            Hand.Value = "";
        }

        public void OnHandChanged(FixedString64Bytes oH, FixedString64Bytes nH)
        {
            bool isHeld = !nH.IsEmpty;
            if (isHeld)
            {
                dummy = Instantiate(dummyPrefab);
                dummy.Setup(this, nH.ConvertToString());
            }
            else
            {
                if (IsServer)
                {
                    GetComponent<Unity.Netcode.Components.NetworkTransform>().Teleport(dummy.transform.position, dummy.transform.rotation, dummy.transform.localScale);
                }
                Destroy(dummy.gameObject);
            }
            gameObject.SetActive(!isHeld);
        }
        #endregion
    }
}