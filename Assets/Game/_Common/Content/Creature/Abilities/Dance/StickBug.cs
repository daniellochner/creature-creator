using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/StickBug")]
    public class StickBug : Ability
    {
        public override bool CanPerform => base.CanPerform && !EditorManager.Instance.IsEditing && Player.Instance.Constructor.Legs.Count > 0 && !Player.Instance.Underwater.IsUnderwater;

        public override void OnPerform()
        {
            Player.Instance.Effects.PlaySound("GetStickBugged");
            Player.Instance.Animator.Params.SetTrigger("Body_StickBug");

            Player.Instance.Mover.StopMoving();
        }
    }
}