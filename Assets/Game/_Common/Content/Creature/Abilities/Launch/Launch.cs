// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    public class Launch : Ability
    {
        #region Properties
        public CreatureLauncher CreatureLauncher { get; private set; }
        #endregion

        #region Methods
        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);
            CreatureLauncher = creatureAbilities.GetComponent<CreatureLauncher>();
        }

        public override void OnPerform()
        {
            foreach (BodyPartLauncher launcher in CreatureLauncher.GetComponentsInChildren<BodyPartLauncher>())
            {
                if (launcher.BodyPartConstructor.BodyPart.Abilities.Contains(this))
                {
                    CreatureLauncher.Launch(launcher);
                }
            }
        }
        #endregion
    }
}