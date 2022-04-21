// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class UnlockableBodyPart : UnlockableItem
    {
        #region Fields
        [SerializeField] public string bodyPartID;
        #endregion

        #region Properties
        public override bool IsUnlocked => ProgressManager.Data.UnlockedBodyParts.Contains(bodyPartID);
        #endregion

        #region Methods
        protected override void OnUnlock()
        {
            EditorManager.Instance.UnlockBodyPart(bodyPartID);
        }
        #endregion
    }
}