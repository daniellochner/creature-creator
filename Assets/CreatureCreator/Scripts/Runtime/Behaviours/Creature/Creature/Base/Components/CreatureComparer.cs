using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureComparer : MonoBehaviour
    {
        #region Fields
        [SerializeField] private int maxBonesDiff;
        [SerializeField] private float maxColorDiff;
        #endregion

        #region Properties
        private CreatureConstructor Constructor { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            CreatureNonPlayer npc = collision.collider.GetComponent<CreatureNonPlayer>();
            if (npc != null && npc.Comparer.CompareTo(Constructor))
            {
#if USE_STATS
                StatsManager.Instance.SetAchievement("ACH_WE_ARE_ALIKE");
#endif
            }
        }

        public bool CompareTo(CreatureConstructor other)
        {
            CreatureData c1 = Constructor.Data;
            CreatureData c2 = other.Data;

            // Pattern
            if (c1.PatternID != c2.PatternID)
            {
                return false;
            }

            // Bones
            if (Mathf.Abs(c1.Bones.Count - c2.Bones.Count) > maxBonesDiff)
            {
                return false;
            }

            // Colours
            if ((GetColourDistance(c1.PrimaryColour, c2.PrimaryColour) > maxColorDiff) || (GetColourDistance(c1.SecondaryColour, c2.SecondaryColour) > maxColorDiff))
            {
                return false;
            }

            // Body Parts
            if (c1.AttachedBodyParts.Count == c2.AttachedBodyParts.Count)
            {
                foreach (AttachedBodyPart attachedBodyPart in c1.AttachedBodyParts)
                {
                    if (c2.AttachedBodyParts.Find((x) => x.bodyPartID == attachedBodyPart.bodyPartID) == null)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private float GetColourDistance(Color c1, Color c2)
        {
            Vector3 v1 = new Vector3(c1.r, c1.g, c1.b);
            Vector3 v2 = new Vector3(c2.r, c2.g, c2.b);
            return Vector3.Distance(v1, v2);
        }
        #endregion
    }
}