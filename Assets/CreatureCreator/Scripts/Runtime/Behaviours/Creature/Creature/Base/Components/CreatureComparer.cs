using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureComparer : NetworkBehaviour
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
            if (IsLocalPlayer && collision.collider.gameObject.TryGetComponent(out CreatureNonPlayer npc))
            {
                if (npc.Comparer.CompareTo(Constructor))
                {
                    StatsManager.Instance.UnlockAchievement("ACH_WE_ARE_ALIKE");
                }
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
            if ((ColourUtility.GetColourDistance(c1.PrimaryColour, c2.PrimaryColour) > maxColorDiff) || (ColourUtility.GetColourDistance(c1.SecondaryColour, c2.SecondaryColour) > maxColorDiff))
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
        #endregion
    }
}