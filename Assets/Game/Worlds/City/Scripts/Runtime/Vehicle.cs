using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Vehicle : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private List<Transform> wheelsLeft;
        [SerializeField] private List<Transform> wheelsRight;
        [SerializeField] private float speed;
        [SerializeField] private float radius;
        [SerializeField] private bool canCollide = true;
        [SerializeField] private float syncCooldown = 10f;

        private NetworkTransform networkTransform;
        private PathFollower follower;
        #endregion

        #region Properties
        public NetworkVariable<float> DistanceTravelled { get; set; } = new NetworkVariable<float>();
        #endregion

        #region Methods
        private void Awake()
        {
            networkTransform = GetComponent<NetworkTransform>();
            follower = GetComponent<PathFollower>();
        }
        private void Start()
        {
            if (IsServer)
            {
                StartCoroutine(SyncDistanceTravelledRoutine());
            }
            else
            {
                if (NetworkObject.IsSpawned)
                {
                    DistanceTravelled.OnValueChanged += OnDistanceTravelledChanged;
                    OnDistanceTravelledChanged(0f, DistanceTravelled.Value);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private IEnumerator SyncDistanceTravelledRoutine()
        {
            while (true)
            {
                DistanceTravelled.Value = follower.distanceTravelled;
                yield return new WaitForSeconds(syncCooldown);
            }
        }
        private void OnDistanceTravelledChanged(float oldDT, float newDT)
        {
            follower.distanceTravelled = newDT;
        }

#if UNITY_STANDALONE
        private void FixedUpdate()
        {
            float w = (speed / radius) * Mathf.Rad2Deg;
            float a = w * Time.fixedDeltaTime;

            foreach (Transform wheel in wheelsLeft)
            {
                wheel.Rotate(Vector3.right, a, Space.Self);
            }
            foreach (Transform wheel in wheelsRight)
            {
                wheel.Rotate(Vector3.left, a, Space.Self);
            }
        }
#endif

        private void OnCollisionEnter(Collision collision)
        {
            if (IsServer && canCollide)
            {
                CreatureBase creature = collision.collider.GetComponent<CreatureBase>();
                if (creature != null && !creature.Health.IsDead)
                {
                    creature.Health.Kill(DamageReason.Vehicle);
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (follower != null && follower.pathCreator != null)
            {
                follower.speed = speed;

                float d = follower.pathCreator.path.GetClosestDistanceAlongPath(transform.position);
                transform.position = follower.pathCreator.path.GetPointAtDistance(d);
                transform.rotation = follower.pathCreator.path.GetRotationAtDistance(d);
            }
        }
#endif
        #endregion
    }
}