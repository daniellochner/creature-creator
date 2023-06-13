// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Spin")]
    public class Spin : Ability
    {
        [SerializeField] private PlayerEffects.Sound[] spinSounds;
        [SerializeField] private MinMax spinDamage;
        [SerializeField] private float spinRadius;
        private List<CreatureBase> damagedCreatures = new List<CreatureBase>();

        public override bool CanPerform => base.CanPerform && !EditorManager.Instance.IsEditing && !Player.Instance.Animator.Animator.GetCurrentAnimatorStateInfo(0).IsName("Spin");

        public override void OnPerform()
        {
            damagedCreatures.Clear();

            Player.Instance.Animator.Animator.GetBehaviour<Animations.Spin>().OnSpinArm = OnSpinArm;
            Player.Instance.Animator.Animator.GetBehaviour<Animations.Spin>().OnSpin = OnSpin;

            Player.Instance.Animator.Params.SetTrigger("Body_Spin");
        }

        private void OnSpinArm(ArmAnimator arm)
        {
            Collider[] colliders = Physics.OverlapSphere(arm.LimbConstructor.Extremity.position, spinRadius);
            foreach (Collider collider in colliders)
            {
                CreatureBase creature = collider.GetComponent<CreatureBase>();
                if (creature != null && creature != Player.Instance && !damagedCreatures.Contains(creature))
                {
                    bool ignorePVP = (creature is CreaturePlayerRemote) && !(WorldManager.Instance.World as WorldMP).EnablePVP;
                    if (!ignorePVP)
                    {
                        float damage = spinDamage.Random;
                        creature.Health.TakeDamage(damage, DamageReason.SpinAttack, Player.Instance.OwnerClientId.ToString());

#if USE_STATS
                        if (creature.Health.Health - damage <= 0)
                        {
                            StatsManager.Instance.Kills++;
                        }
#endif
                    }
                    damagedCreatures.Add(creature);
                }
            }
        }
        private void OnSpin()
        {
            Player.Instance.Effects.PlaySound(spinSounds);
        }
    }
}