using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class QuestItem : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Transform snapPoint;
        [SerializeField] private Transform snapParent;

        private NetworkTransform networkTransform;
        private Rigidbody rb;
        #endregion

        #region Properties
        public Holdable Holdable { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            networkTransform = GetComponent<NetworkTransform>();
            rb = GetComponent<Rigidbody>();

            Holdable = GetComponent<Holdable>();
        }
        private void Start()
        {
            if (WorldManager.Instance.World.CreativeMode)
            {
                if (IsServer)
                {
                    NetworkObject.Despawn();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        public override void OnNetworkDespawn()
        {
            gameObject.SetActive(false);
            base.OnNetworkDespawn();
        }

        public void Snap()
        {
            if (IsServer)
            {
                if (snapPoint != null)
                {
                    networkTransform.Teleport(snapPoint.position, snapPoint.rotation, transform.localScale);
                    rb.isKinematic = true;
                    Holdable.Held.CanHold.Value = false;
                }
                if (snapParent != null)
                {
                    transform.SetParent(snapParent, true);
                }
            }
        }
        #endregion
    }
}