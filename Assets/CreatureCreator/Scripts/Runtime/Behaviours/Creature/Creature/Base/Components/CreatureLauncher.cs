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
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
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

        public void LaunchFrom(BodyPartLauncher launcher)
        {
            LaunchFromServerRpc(launcher.name);
        }
        [ServerRpc]
        private void LaunchFromServerRpc(string launcherGUID)
        {
            if (TryGetLauncher(launcherGUID, out BodyPartLauncher launcher))
            {
                ProjectileGroup group = new GameObject("_ProjectileGroup").AddComponent<ProjectileGroup>();
                group.transform.SetParent(Dynamic.Transform);
                group.Count = launcher.SpawnPoints.Length;

                foreach (Transform spawnPoint in launcher.SpawnPoints)
                {
                    Projectile projectile = Instantiate(projectiles[launcher.ProjectileId], spawnPoint.position, spawnPoint.rotation, group.transform);
                    projectile.transform.localScale *= launcher.transform.localScale.x;

                    projectile.Launch(this, group, launcher.Speed);

                    projectile.NetworkObject.SpawnWithOwnership(OwnerClientId, true);
                }
            }
        }
        private bool TryGetLauncher(string launcherGUID, out BodyPartLauncher launcher)
        {
            foreach (BodyPartConstructor constructor in Constructor.BodyParts)
            {
                if (constructor.name == launcherGUID && constructor.IsVisible)
                {
                    launcher = constructor.GetComponent<BodyPartLauncher>();
                    return true;
                }
                else
                if (constructor.Flipped.name == launcherGUID && constructor.Flipped.IsVisible)
                {
                    launcher = constructor.Flipped.GetComponent<BodyPartLauncher>();
                    return true;
                }
            }
            launcher = null;
            return false;
        }
        #endregion
    }
}