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
        [SerializeField] private MinMax biteForce;
        [SerializeField] private float biteRadius;
        private bool hasFoundCreature;

        public override bool CanPerform => !Player.Instance.Animator.Animator.GetCurrentAnimatorStateInfo(0).IsName("Strike");

        public override void OnPerform()
        {
            hasFoundCreature = false;

            Player.Instance.Animator.Animator.GetBehaviour<Animations.Bite>().OnBite += OnBite;
            Player.Instance.Animator.Params.SetTrigger("Body_Strike");
        }
        private void OnBite(MouthAnimator mouth)
        {
            CreatureAbilities.GetComponent<PlayerEffects>().PlaySound(biteSounds);

            if (!hasFoundCreature)
            {
                Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, biteRadius);
                foreach (Collider collider in colliders)
                {
                    CreatureBase creature = collider.GetComponent<CreatureBase>();
                    if (creature != null && creature != Player.Instance)
                    {
                        bool dealDamage = true;
                        if ((creature is CreaturePlayerRemote) && !(WorldManager.Instance.World as WorldMP).EnablePVP)
                        {
                            dealDamage = false;
                        }

                        if (dealDamage)
                        {
                            creature.Health.TakeDamage(biteDamage.Random, CreatureAbilities.transform.forward * biteForce.Random);
                        }
                        hasFoundCreature = true;
                    }
                }
            }

            Player.Instance.Animator.Animator.GetBehaviour<Animations.Bite>().OnBite -= OnBite;
        }
    }
}