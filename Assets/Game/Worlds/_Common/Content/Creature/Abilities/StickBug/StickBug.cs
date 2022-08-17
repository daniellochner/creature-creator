using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/StickBug")]
    public class StickBug : Ability
    {
        public override bool CanPerform => !EditorManager.Instance.IsEditing && CreatureAbilities.CreatureConstructor.Legs.Count > 0;

        public override void OnPerform()
        {
            CreatureAbilities.GetComponent<PlayerEffects>().PlaySound("GetStickBugged");
            CreatureAbilities.GetComponent<AnimatorParams>().SetTrigger("Body_StickBug");
        }
    }
}