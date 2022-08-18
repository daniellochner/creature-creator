using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PlacedUnlockablesChecker : MonoBehaviour
    {
        public PlacedUnlockables placed;

        public Database bodypartsDB;
        public Database patternsDB;

        [ContextMenu("CHECK ALL")]
        public void CheckAll()
        {
            foreach (var bp in bodypartsDB.Objects.Keys)
            {
                if (!placed.bodyparts.Contains(bp))
                {
                    Debug.Log(bp);
                }
            }

            foreach (var p in patternsDB.Objects.Keys)
            {
                if (!placed.patterns.Contains(p))
                {
                    Debug.Log(p);
                }
            }
        }

        [ContextMenu("FIND EMPTY")]
        public void FindEmpty()
        {
            foreach (var bp in FindObjectsOfType<UnlockableBodyPart>())
            {
                if (string.IsNullOrEmpty(bp.BodyPartID))
                {
                    Debug.Log(bp.BodyPartID, bp.gameObject);
                }
            }

            foreach (var p in FindObjectsOfType<UnlockablePattern>())
            {
                if (string.IsNullOrEmpty(p.PatternID))
                {
                    Debug.Log(p.PatternID, p.gameObject);
                }
            }
        }

        [ContextMenu("ADD ALL")]
        public void AddAll()
        {
            foreach (var bp in FindObjectsOfType<UnlockableBodyPart>())
            {
                if (bp.BodyPartID != "" && !placed.bodyparts.Contains(bp.BodyPartID))
                {
                    placed.bodyparts.Add(bp.BodyPartID);
                }
            }

            foreach (var p in FindObjectsOfType<UnlockablePattern>())
            {
                if (p.PatternID != "" && placed.patterns.Contains(p.PatternID))
                {
                    placed.patterns.Add(p.PatternID);
                }
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(placed);
#endif
        }
    }
}