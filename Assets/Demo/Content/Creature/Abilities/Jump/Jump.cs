// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Jump")]
    public class Jump : Ability
    {
        [Header("Jump")]
        [SerializeField] private float jumpForce;

        private CreatureMover creatureMover;

        public override bool CanPerform => creatureMover.IsGrounded;

        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);

            creatureMover = creatureAbilities.GetComponent<CreatureMover>();
        }
        public override void OnPerform()
        {
            creatureMover.GetComponent<Rigidbody>().AddForce(creatureMover.transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}