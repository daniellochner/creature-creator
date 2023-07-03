// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    public class Launch : Ability
    {
        #region Fields
        [Header("Launch")]
        [SerializeField] private int maxLaunchers;
        #endregion

        #region Properties
        public CreatureLauncher CreatureLauncher { get; private set; }

        public override bool CanPerform => base.CanPerform && !EditorManager.Instance.IsEditing;

        public List<BodyPartLauncher> Launchers
        {
            get
            {
                List<BodyPartLauncher> launchers = new List<BodyPartLauncher>();
                foreach (BodyPartConstructor constructor in CreatureLauncher.Constructor.BodyParts)
                {
                    BodyPartLauncher launcher = constructor.GetComponent<BodyPartLauncher>();
                    if (launcher != null)
                    {
                        if (launcher.Constructor.BodyPart.Abilities.Contains(this))
                        {
                            launchers.Add(launcher);
                            launchers.Add(launcher.Flipped);
                        }
                        if (launchers.Count >= maxLaunchers)
                        {
                            break;
                        }
                    }
                }
                return launchers;
            }
        }
        #endregion

        #region Methods
        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);
            CreatureLauncher = creatureAbilities.GetComponent<CreatureLauncher>();
        }
        public override void Shutdown()
        {
            base.Shutdown();
            foreach (BodyPartLauncher launcher in Launchers)
            {
                foreach (Transform spawnPoint in launcher.SpawnPoints)
                {
                    spawnPoint.gameObject.SetActive(false);
                }
            }
        }

        public override void OnPrepare()
        {
            foreach (BodyPartLauncher launcher in Launchers)
            {
                foreach (Transform spawnPoint in launcher.SpawnPoints)
                {
                    spawnPoint.GetComponent<Trajectory>().Setup(launcher.Speed, 1f);
                    spawnPoint.gameObject.SetActive(true);
                }
            }
        }
        public override void OnPerform()
        {
            foreach (BodyPartLauncher launcher in Launchers)
            {
                CreatureLauncher.Launch(launcher);
                foreach (Transform spawnPoint in launcher.SpawnPoints)
                {
                    spawnPoint.gameObject.SetActive(false);
                }
            }
        }
        #endregion
    }
}