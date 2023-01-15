// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using Unity.Netcode;
using RotaryHeart.Lib.SerializableDictionary;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureLauncher : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private SerializableDictionaryBase<string, NetworkObject> projectiles;
        [SerializeField] private PlayerEffects.Sound[] launchSounds;
        #endregion

        #region Properties
        public PlayerEffects PlayerEffects { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            PlayerEffects = GetComponent<PlayerEffects>();
        }

        public void Launch(BodyPartLauncher bpl)
        {
            foreach (Transform spawnPoint in bpl.SpawnPoints)
            {
                LaunchServerRpc(bpl.ProjectileId, spawnPoint.position, spawnPoint.rotation, bpl.transform.localScale.x, bpl.Speed);
            }
        }

        [ServerRpc]
        private void LaunchServerRpc(string projectileId, Vector3 position, Quaternion rotation, float scale, float speed)
        {
            NetworkObject projectile = Instantiate(projectiles[projectileId], position, rotation, Dynamic.Transform);
            projectile.transform.localScale *= scale;

            projectile.GetComponent<Rigidbody>().velocity = speed * projectile.transform.forward;
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>()); // Ignore collision between this creature and the projectile!

            PlayerEffects.PlaySound(launchSounds);

            projectile.SpawnWithOwnership(OwnerClientId);
        }
        #endregion
    }
}