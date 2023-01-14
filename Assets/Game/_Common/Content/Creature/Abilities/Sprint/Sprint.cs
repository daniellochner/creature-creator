// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Sprint")]
    public class Sprint : Ability
    {
        [Header("Sprint")]
        [SerializeField] private float speed;
        [SerializeField] private float duration;
        [SerializeField] private float hungerCost;
        private CreatureSpeedup speedUp;

        public override bool CanPerform => Player.Instance.Hunger.Hunger > 0f;

        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);
            speedUp = creatureAbilities.GetComponent<CreatureSpeedup>();
        }
        public override void OnPerform()
        {
            base.OnPerform();
            speedUp.SpeedUp(speed, duration);

            if (!WorldManager.Instance.World.CreativeMode)
            {
                Player.Instance.Hunger.Hunger -= hungerCost;
            }
        }
    }
}