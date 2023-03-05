// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Flap")]
    public class Flap : Ability
    {
        [Header("Flap")]
        [SerializeField] private float flapForce;
        [SerializeField] private float maxFlapForce;
        [SerializeField] private PlayerEffects.Sound[] flapSounds;
        [SerializeField] private float hungerCost;

        private CreatureAnimator creatureAnimator;
        private PlayerEffects creatureEffector;
        private Rigidbody rigidbody;

        public override bool CanPerform => base.CanPerform && Player.Instance.Hunger.Hunger > 0f;

        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);

            creatureAnimator = creatureAbilities.GetComponent<CreatureAnimator>();
            creatureEffector = creatureAbilities.GetComponent<PlayerEffects>();
            rigidbody = creatureAbilities.GetComponent<Rigidbody>();
        }

        public override void OnPerform()
        {
            int pairs = creatureAnimator.Wings.Count / 2;
            float force = Mathf.Min(flapForce * pairs, maxFlapForce);

            rigidbody.AddForce(rigidbody.transform.up * force, ForceMode.Impulse);
            creatureAnimator.Params.SetTrigger("Wings_Flap");
            creatureEffector.PlaySound(flapSounds);

            if (!WorldManager.Instance.World.CreativeMode)
            {
                Player.Instance.Hunger.Hunger -= hungerCost;
            }

#if USE_STATS
            StatsManager.Instance.UnlockAchievement("ACH_I_BELIEVE_I_CAN_FLY");
#endif
        }
    }
}