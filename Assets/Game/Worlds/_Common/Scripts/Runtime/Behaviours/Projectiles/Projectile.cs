using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Projectile : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private MinMax minMaxDamage;
        [SerializeField] private float blastRadius;
        [SerializeField] private GameObject collidePrefab;
        [SerializeField] private float lifetime;

        private Rigidbody rb;
        #endregion

        #region Methods
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        private IEnumerator Start()
        {
            if (IsServer)
            {
                yield return new WaitForSeconds(lifetime);
                NetworkObject.Despawn();
            }
        }
        private void LateUpdate()
        {
            if (IsServer)
            {
                transform.forward = rb.velocity;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsServer)
            {
                Vector3 point = collision.GetContact(0).point;

                Collider[] colliders = Physics.OverlapSphere(point, blastRadius);
                foreach (Collider collider in colliders)
                {
                    CreatureBase creature = collider.GetComponent<CreatureBase>();
                    if (creature != null)
                    {
                        bool ignore = (creature is CreaturePlayer) && (creature.OwnerClientId == OwnerClientId || !WorldManager.Instance.EnablePVP);
                        if (!ignore)
                        {
                            creature.Health.TakeDamage(minMaxDamage.Random, DamageReason.Projectile, OwnerClientId.ToString());
                        }
                    }
                }

                CollideClientRpc(point);
            }
        }

        [ClientRpc]
        private void CollideClientRpc(Vector3 point)
        {
            Instantiate(collidePrefab, point, Quaternion.identity, Dynamic.Transform);

            if (IsServer && NetworkObject.IsSpawned)
            {
                NetworkObject.Despawn();
            }
            gameObject.SetActive(false);
        }
        #endregion
    }
}