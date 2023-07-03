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
        [SerializeField] private ProjectileGroup groupPrefab;
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
            SerializableTransform[] spawnPoints = new SerializableTransform[bpl.SpawnPoints.Length];
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                spawnPoints[i] = new SerializableTransform(bpl.SpawnPoints[i], Dynamic.Transform);
            }

            LaunchServerRpc(bpl.ProjectileId, spawnPoints, bpl.transform.localScale.x, bpl.Speed);
        }

        [ServerRpc]
        private void LaunchServerRpc(string projectileId, SerializableTransform[] spawnPoints, float scale, float speed)
        {
            ProjectileGroup group = new GameObject("_ProjectileGroup").AddComponent<ProjectileGroup>();
            group.transform.SetParent(Dynamic.Transform);
            group.Count = spawnPoints.Length;

            foreach (SerializableTransform spawnPoint in spawnPoints)
            {
                LaunchProjectile(projectileId, group, spawnPoint.position, spawnPoint.rotation, spawnPoint.scale.x, speed);
            }
        }

        private void LaunchProjectile(string projectileId, ProjectileGroup group, Vector3 position, Quaternion rotation, float scale, float speed)
        {
            Projectile projectile = Instantiate(projectiles[projectileId], position, rotation, group.transform);
            projectile.transform.localScale *= scale;
            projectile.Rigidbody.velocity = speed * projectile.transform.forward;
            projectile.Launcher = this;
            projectile.Group = group;

            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>()); // Ignore collision between this creature and the projectile!

            PlayerEffects.PlaySound(launchSounds);

            projectile.NetworkObject.SpawnWithOwnership(OwnerClientId, true);
        }
        #endregion
    }
}