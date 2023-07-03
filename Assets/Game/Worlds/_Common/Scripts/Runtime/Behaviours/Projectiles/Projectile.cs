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
        #endregion

        #region Properties
        public Rigidbody Rigidbody { get; private set; }

        public CreatureLauncher Launcher { get; set; }
        public ProjectileGroup Group { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
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
                transform.forward = Rigidbody.velocity;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Group.Count--;
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
                        bool ignore = (creature is CreaturePlayer) && (creature.NetworkObjectId == Launcher.NetworkObjectId || !WorldManager.Instance.EnablePVP);
                        if (!ignore && !Group.HasDamaged)
                        {
                            string inflicter = null;
                            if (Launcher.GetComponent<CreaturePlayer>())
                            {
                                inflicter = Launcher.OwnerClientId.ToString(); // TODO: Tidy up...
                            }

                            creature.Health.TakeDamage(minMaxDamage.Random, DamageReason.Projectile, inflicter);
                            Group.HasDamaged = true;
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