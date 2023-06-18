using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Holder : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private HoldableDummy dummyPrefab;

        private NetworkTransform networkTransform;
        private Rigidbody rigidBody;
        private Vector3 startPosition;
        private Quaternion startRotation;

        private Holdable holdable;
        #endregion

        #region Properties
        public NetworkVariable<FixedString64Bytes> Hand { get; set; } = new NetworkVariable<FixedString64Bytes>();

        public HoldableDummy Dummy { get; private set; }

        public bool IsHeld => !Hand.Value.IsEmpty;
        #endregion

        #region Methods
        private void Awake()
        {
            networkTransform = GetComponent<NetworkTransform>();
            rigidBody = GetComponent<Rigidbody>();
            holdable = GetComponent<Holdable>();
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
            if (IsServer && collision.collider.CompareTag("WorldBorder"))
            {
                networkTransform.Teleport(startPosition, startRotation, transform.localScale);
                rigidBody.velocity = rigidBody.angularVelocity = Vector3.zero;
            }
        }

        public void OnHandChanged(FixedString64Bytes oH, FixedString64Bytes nH)
        {
            bool isHeld = !nH.IsEmpty;
            if (isHeld)
            {
                Dummy = Instantiate(dummyPrefab);
                Dummy.Setup(holdable, nH.ConvertToString());
            }
            else
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    GetComponent<Unity.Netcode.Components.NetworkTransform>().Teleport(Dummy.transform.position, Dummy.transform.rotation, Dummy.transform.localScale);
                }
                Destroy(Dummy.gameObject);
            }
            gameObject.SetActive(!isHeld);
        }
        #endregion
    }
}