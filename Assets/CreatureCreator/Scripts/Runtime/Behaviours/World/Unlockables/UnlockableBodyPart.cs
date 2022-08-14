// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class UnlockableBodyPart : UnlockableItem
    {
        #region Fields
        [SerializeField] private string bodyPartID;
        #endregion

        #region Properties
        public string BodyPartID => bodyPartID;
        public override bool IsUnlocked => ProgressManager.Data.UnlockedBodyParts.Contains(bodyPartID) || EditorManager.Instance.CreativeMode;

        public BodyPart BodyPart => DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartID);
        #endregion

        #region Methods
        protected override void OnUnlock()
        {
            EditorManager.Instance.UnlockBodyPart(bodyPartID);
        }
        protected override void OnSpawn()
        {
        }
        #endregion
    }
}