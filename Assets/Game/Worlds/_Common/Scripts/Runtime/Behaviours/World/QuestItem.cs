using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class QuestItem : NetworkBehaviour
    {
        #region Fields
        private Unity.Netcode.Components.NetworkTransform networkTransform;
        private Vector3 startPosition;
        private Quaternion startRotation;
        #endregion

        #region Methods
        private void Awake()
        {
            networkTransform = GetComponent<Unity.Netcode.Components.NetworkTransform>();
        }
        private void Start()
        {
            startPosition = transform.position;
            startRotation = transform.rotation;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!IsServer) return;
            if (collision.collider.CompareTag("WorldBorder"))
            {
                networkTransform.Teleport(startPosition, startRotation, transform.localScale);
            }
        }
        #endregion
    }
}