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

        private Rigidbody rb;
        #endregion

        #region Methods
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
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
                    if (creature != null && creature != Player.Instance)
                    {
                        if (!((creature is CreaturePlayerRemote) && !(WorldManager.Instance.World as WorldMP).EnablePVP))
                        {
                            float damage = minMaxDamage.Random;
                            creature.Health.TakeDamage(damage);

                            if (creature.Health.Health - damage <= 0)
                            {
                                KillClientRpc(NetworkUtils.SendTo(OwnerClientId));
                            }
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

        [ClientRpc]
        private void KillClientRpc(ClientRpcParams clientRpcParams = default)
        {
#if USE_STATS
            StatsManager.Instance.Kills++;
#endif
        }
        #endregion
    }
}