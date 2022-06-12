// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Jump")]
    public class Jump : Ability
    {
        [Header("Jump")]
        [SerializeField] private float jumpForce;

        private CreatureAnimator creatureAnimator;
        private Rigidbody rigidbody;

        public override bool CanPerform => creatureAnimator.IsGrounded;

        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);

            creatureAnimator = creatureAbilities.GetComponent<CreatureAnimator>();
            rigidbody = creatureAbilities.GetComponent<Rigidbody>();
        }
        public override void OnPerform()
        {
            rigidbody.AddForce(rigidbody.transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}