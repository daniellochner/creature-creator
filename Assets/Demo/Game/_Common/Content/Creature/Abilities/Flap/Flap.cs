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

        private CreatureAnimator creatureAnimator;
        private Rigidbody rigidbody;

        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);

            creatureAnimator = creatureAbilities.GetComponent<CreatureAnimator>();
            rigidbody = creatureAbilities.GetComponent<Rigidbody>();
        }
        public override void OnPerform()
        {
            rigidbody.AddForce(rigidbody.transform.up * flapForce, ForceMode.Impulse);
            creatureAnimator.Params.SetTrigger("Wings_Flap");
        }
    }
}