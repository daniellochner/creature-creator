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
        private bool hasFoundCreature;

        public override bool CanPerform => base.CanPerform && !EditorManager.Instance.IsEditing && !Player.Instance.Animator.Animator.GetCurrentAnimatorStateInfo(0).IsName("Strike");

        public override void OnPerform()
        {
            hasFoundCreature = false;

            Player.Instance.Animator.Animator.GetBehaviour<Animations.Bite>().OnBiteMouth = OnBiteMouth;
            Player.Instance.Animator.Animator.GetBehaviour<Animations.Bite>().OnBite = OnBite;

            Player.Instance.Animator.Params.SetTrigger("Body_Strike");
        }

        private void OnBiteMouth(MouthAnimator mouth)
        {
            Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, biteRadius);
            foreach (Collider collider in colliders)
            {
                Edible edible = collider.GetComponent<Edible>();
                if (edible != null && edible.CanInteract(Player.Instance.Interactor))
                {
                    edible.Interact(Player.Instance.Interactor);
                }

                CreatureBase creature = collider.GetComponent<CreatureBase>();
                if (creature != null && creature != Player.Instance && !hasFoundCreature)
                {
                    bool ignore = (creature is CreaturePlayerRemote) && !WorldManager.Instance.EnablePVP;
                    if (!ignore)
                    {
                        creature.Health.TakeDamage(biteDamage.Random, DamageReason.BiteAttack, Player.Instance.OwnerClientId.ToString());
                    }
                    hasFoundCreature = true;
                }
            }
        }
        private void OnBite()
        {
            Player.Instance.Effects.PlaySound(biteSounds);
        }
    }
}