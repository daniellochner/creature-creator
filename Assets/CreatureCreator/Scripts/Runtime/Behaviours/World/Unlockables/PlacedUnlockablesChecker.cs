using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PlacedUnlockablesChecker : MonoBehaviour
    {
        #region Fields
        [SerializeField] private PlacedUnlockables placed;

        [SerializeField] private Database bodypartsDB;
        [SerializeField] private Database patternsDB;
        #endregion

        #region Methods
        [ContextMenu("Add Placed")]
        public void AddPlaced()
        {
            foreach (var bp in FindObjectsOfType<UnlockableBodyPart>(true))
            {
                if (bp.BodyPartID != "" && !placed.bodyparts.Contains(bp.BodyPartID))
                {
                    placed.bodyparts.Add(bp.BodyPartID);
                }
            }

            foreach (var p in FindObjectsOfType<UnlockablePattern>(true))
            {
                if (p.PatternID != "" && !placed.patterns.Contains(p.PatternID))
                {
                    placed.patterns.Add(p.PatternID);
                }
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(placed);
#endif
        }

        [ContextMenu("Check Placed")]
        public void CheckPlaced()
        {
            List<string> placedBP = new List<string>();
            foreach (var bp in FindObjectsOfType<UnlockableBodyPart>(true))
            {
                if (string.IsNullOrEmpty(bp.BodyPartID))
                {
                    Debug.Log("Empty Body Part Found.", bp.gameObject);
                }
                else
                {
                    if (!placedBP.Contains(bp.BodyPartID))
                    {
                        placedBP.Add(bp.BodyPartID);
                    }
                    else
                    {
                        Debug.Log($"Duplicate Body Part Found ({bp.BodyPartID}).", bp.gameObject);
                    }
                }
            }

            List<string> placedP = new List<string>();
            foreach (var p in FindObjectsOfType<UnlockablePattern>(true))
            {
                if (string.IsNullOrEmpty(p.PatternID))
                {
                    Debug.Log("Empty Pattern Found.", p.gameObject);
                }
                else
                {
                    if (!placedP.Contains(p.PatternID))
                    {
                        placedP.Add(p.PatternID);
                    }
                    else
                    {
                        Debug.Log($"Duplicate Pattern Found ({p.PatternID}).", p.gameObject);
                    }
                }
            }
        }

        [ContextMenu("Find Unplaced")]
        public void FindUnplaced()
        {
            foreach (var bp in bodypartsDB.Objects.Keys)
            {
                if (!placed.bodyparts.Contains(bp))
                {
                    Debug.Log($"Body Part Not Found: {bp} ({bodypartsDB.Objects[bp].name})");
                }
            }

            foreach (var p in patternsDB.Objects.Keys)
            {
                if (!placed.patterns.Contains(p))
                {
                    Debug.Log($"Pattern Not Found: {p} ({patternsDB.Objects[p].name})");
                }
            }
        }
        #endregion
    }
}