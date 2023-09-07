using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Ball : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private GameObject bouncePrefab;
        [SerializeField] private float kickForce;
        [SerializeField] private float correctionFactor;
        [SerializeField] private Transform[] goals;

        private Rigidbody rb;
        private Unity.Netcode.Components.NetworkTransform networkTransform;
        #endregion

        #region Methods
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            networkTransform = GetComponent<Unity.Netcode.Components.NetworkTransform>();
        }
        private void Start()
        {
            rb.isKinematic = !IsServer;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Vector3 point = collision.GetContact(0).point;

            CreaturePlayer player = collision.gameObject.GetComponent<CreaturePlayer>();
            if (player != null)
            {
                Vector3 force = (transform.position - point).normalized * player.Velocity.LSpeedPercentage * kickForce;

                // Target goal
                Transform targetGoal = null;
                float maxDot = Mathf.NegativeInfinity;
                foreach (Transform goal in goals)
                {
                    Vector3 dir = goal.position - point;
                    float dot = Vector3.Dot(force, dir);

                    if (dot > maxDot)
                    {
                        targetGoal = goal;
                        maxDot = dot;
                    }
                }
                
                // Perfect force towards targeted goal
                Vector3 perfectForce = (targetGoal.position - point).normalized * force.magnitude;

                // Corrected force
                Vector3 correctedForce = (correctionFactor * perfectForce) + ((1f - correctionFactor) * force);

                KickServerRpc(point, correctedForce);
            }
            else if (IsServer)
            {
                BounceClientRpc(point);
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void KickServerRpc(Vector3 point, Vector3 force)
        {
            rb.angularVelocity = Vector3.zero;
            rb.AddForceAtPosition(force, point, ForceMode.Impulse);

            BounceClientRpc(point);
        }

        [ClientRpc]
        private void BounceClientRpc(Vector3 point)
        {
            Instantiate(bouncePrefab, point, Quaternion.identity, Dynamic.Transform);
        }
        
        public void Teleport(Vector3 position)
        {
            networkTransform.Teleport(position, transform.rotation, transform.localScale);
            rb.velocity = rb.angularVelocity = Vector3.zero;
        }
        #endregion
    }
}