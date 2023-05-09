using DanielLochner.Assets;
using DanielLochner.Assets.CreatureCreator;
using UnityEngine;
public class Testing : MonoBehaviour
{
    [ContextMenu("SetALl")]
    public void SetAll()
    {
        foreach (UnlockableBodyPart ubp in FindObjectsOfType<UnlockableBodyPart>(true))
        {
            DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", ubp.BodyPartID).premium = false;

        }
        foreach (UnlockablePattern p in FindObjectsOfType<UnlockablePattern>(true))
        {
            DatabaseManager.GetDatabaseEntry<Pattern>("Patterns", p.PatternID).premium = false;

        }
    }
}