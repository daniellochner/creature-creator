// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Bite")]
    public class Bite : Ability
    {
        [SerializeField] private PlayerEffects.Sound[] biteSounds;
        [SerializeField] private MinMax biteDamage;
        [SerializeField] private float biteRadius;
        private bool hasDealtDamage;

        public override void OnPerform()
        {
            hasDealtDamage = false;

            Player.Instance.Animator.Animator.GetBehaviour<Animations.Bite>().OnBite += OnBite;
            Player.Instance.Animator.Params.SetTrigger("Body_Strike");
        }
        private void OnBite(MouthAnimator mouth)
        {
            CreatureAbilities.GetComponent<PlayerEffects>().PlaySound(biteSounds);
            if (!hasDealtDamage)
            {
                Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, biteRadius);
                foreach (Collider collider in colliders)
                {
                    CreatureBase creature = collider.GetComponent<CreatureBase>();
                    if (creature != null && creature != Player.Instance)
                    {
                        creature.Health.TakeDamage(biteDamage.Random);
                        hasDealtDamage = true;
                    }
                }
            }

            Player.Instance.Animator.Animator.GetBehaviour<Animations.Bite>().OnBite -= OnBite;
        }
    }
}