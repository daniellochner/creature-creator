// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Projectile : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private GameObject collidePrefab;
        [SerializeField] private MinMax minMaxDamage;
        [SerializeField] private float blastRadius;
        [SerializeField] private float lifetime;

        private Rigidbody rb;
        #endregion

        #region Properties
        public CreatureLauncher Launcher { get; set; }
        public ProjectileGroup Group { get; set; }
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
        public override void OnDestroy()
        {
            base.OnDestroy();
            if (Group != null)
            {
                Group.Count--;
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
                CollideAt(collision.GetContact(0).point);
            }
        }

        public virtual void Launch(CreatureLauncher launcher, ProjectileGroup group, float speed)
        {
            Launcher = launcher;
            Group = group;

            rb.velocity = speed * transform.forward;

            Physics.IgnoreCollision(Launcher.GetComponent<Collider>(), GetComponent<Collider>());
        }

        private void CollideAt(Vector3 point)
        {
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

            CollideAtClientRpc(point);
        }
        [ClientRpc]
        private void CollideAtClientRpc(Vector3 point)
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