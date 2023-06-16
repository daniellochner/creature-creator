using System.Collections.Generic;

namespace DanielLochner.Assets.CreatureCreator
{
    public class IslandSurvivor : KingOfTheHill
    {
        protected override void OnApplyRestrictions()
        {
            base.OnApplyRestrictions();

            // Restrict body parts with the swim ability
            List<string> bodyParts = new List<string>();
            foreach (var obj in DatabaseManager.GetDatabase("Body Parts").Objects)
            {
                BodyPart bodyPart = obj.Value as BodyPart;
                if (bodyPart.Abilities.Find(x => x is Abilities.Swim))
                {
                    bodyParts.Add(obj.Key);
                }
            }
            EditorManager.Instance.SetRestrictedBodyParts(bodyParts);
        }
    }
}