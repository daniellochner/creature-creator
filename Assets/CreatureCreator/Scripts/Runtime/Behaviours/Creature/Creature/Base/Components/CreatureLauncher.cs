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
                LaunchServerRpc(bpl.ProjectileId, spawnPoint.position, spawnPoint.rotation, bpl.Speed);
            }
        }

        [ServerRpc]
        private void LaunchServerRpc(string projectileId, Vector3 position, Quaternion rotation, float speed)
        {
            NetworkObject projectile = Instantiate(projectiles[projectileId], position, rotation, Dynamic.Transform);
            projectile.GetComponent<Rigidbody>().velocity = speed * projectile.transform.forward;

            PlayerEffects.PlaySound(launchSounds);

            projectile.SpawnWithOwnership(OwnerClientId);
        }
        #endregion
    }
}