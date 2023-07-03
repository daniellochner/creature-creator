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
        [SerializeField] private SerializableDictionaryBase<string, Projectile> projectiles;
        [SerializeField] private PlayerEffects.Sound[] launchSounds;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; private set; }
        public PlayerEffects PlayerEffects { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Constructor = GetComponent<CreatureConstructor>();
            PlayerEffects = GetComponent<PlayerEffects>();
        }

        public void Setup()
        {
            Constructor.OnAddBodyPartPrefab += delegate (GameObject main, GameObject flipped)
            {
                BodyPartLauncher mainBPL = main.GetComponent<BodyPartLauncher>();
                mainBPL?.Setup(this);

                BodyPartLauncher flippedBPL = flipped.GetComponent<BodyPartLauncher>();
                flippedBPL?.SetFlipped(mainBPL);
                flippedBPL?.Setup(this);
            };
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
            Projectile projectile = Instantiate(projectiles[projectileId], position, rotation, Dynamic.Transform);
            projectile.transform.localScale *= scale;
            projectile.Rigidbody.velocity = speed * projectile.transform.forward;

            if (GetComponent<CreaturePlayer>())
            {
                projectile.LauncherClientId = OwnerClientId;
            }

            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>()); // Ignore collision between this creature and the projectile!

            PlayerEffects.PlaySound(launchSounds);

            projectile.NetworkObject.SpawnWithOwnership(OwnerClientId, true);
        }
        #endregion
    }
}