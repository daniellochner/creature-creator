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
        [SerializeField] private float airDrag;

        private CreatureAnimator creatureAnimator;
        private Rigidbody rigidbody;
        private bool isAirborne;

        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);

            creatureAnimator = creatureAbilities.GetComponent<CreatureAnimator>();
            rigidbody = creatureAbilities.GetComponent<Rigidbody>();
        }
        public override void OnPerform()
        {
            rigidbody.AddForce(rigidbody.transform.up * flapForce, ForceMode.Impulse);

            foreach (WingAnimator wing in creatureAnimator.Wings)
            {
                wing.Flap();
            }

            if (!isAirborne)
            {
                CreatureAbilities.StartCoroutine(AirborneRoutine());
            }
        }

        private IEnumerator AirborneRoutine()
        {
            isAirborne = true;

            yield return new WaitForSeconds(0.25f);

            float prevDrag = rigidbody.drag;
            rigidbody.drag = airDrag;

            while (!creatureAnimator.IsGrounded)
            {
                yield return null;
            }

            rigidbody.drag = prevDrag;

            isAirborne = false;
        }
    }
}