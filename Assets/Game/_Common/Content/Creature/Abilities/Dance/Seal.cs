using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Seal")]
    public class Seal : Ability
    {
        public override bool CanPerform => !EditorManager.Instance.IsEditing && Player.Instance.Underwater.IsUnderwater;

        public override void OnPerform()
        {
            Player.Instance.Effects.PlaySound("SealVibing");
            Player.Instance.Animator.Params.SetTrigger("Body_Seal");

            Player.Instance.Mover.StopMoving();
        }
    }
}