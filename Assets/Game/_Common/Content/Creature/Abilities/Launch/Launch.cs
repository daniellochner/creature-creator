// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

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
        #endregion

        #region Methods
        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);
            CreatureLauncher = creatureAbilities.GetComponent<CreatureLauncher>();
        }

        public override void OnPerform()
        {
            int count = 0;
            foreach (BodyPartLauncher launcher in CreatureLauncher.GetComponentsInChildren<BodyPartLauncher>())
            {
                if (launcher.BodyPartConstructor.BodyPart.Abilities.Contains(this))
                {
                    CreatureLauncher.Launch(launcher);
                    count++;
                }
                if (count >= maxLaunchers)
                {
                    break;
                }
            }
        }
        #endregion
    }
}