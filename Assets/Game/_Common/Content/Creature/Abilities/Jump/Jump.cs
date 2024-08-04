// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using static DanielLochner.Assets.FootstepEffects;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Jump")]
    public class Jump : Ability
    {
        [Header("Jump")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float maxJumpForce;
        [SerializeField] private float hungerCost;

        private CreatureAnimator creatureAnimator;
        private Rigidbody rigidbody;

        public override bool CanPerform => base.CanPerform && creatureAnimator.Grounded.IsGrounded && Player.Instance.Hunger.Hunger > 0f;

        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);

            creatureAnimator = creatureAbilities.GetComponent<CreatureAnimator>();
            rigidbody = creatureAbilities.GetComponent<Rigidbody>();
        }

        public override void OnPerform()
        {
            int pairs = creatureAnimator.Legs.Count / 2;
            float force = Mathf.Min(jumpForce * pairs, maxJumpForce);
            
            rigidbody.AddForce(rigidbody.transform.up * force, ForceMode.Impulse);

            foreach (LegAnimator leg in creatureAnimator.Legs)
            {
                leg.Step(StepType.JumpStart, 1f);
            }

            if (!WorldManager.Instance.IsCreative)
            {
                Player.Instance.Hunger.Hunger -= hungerCost;
            }
        }
    }
}