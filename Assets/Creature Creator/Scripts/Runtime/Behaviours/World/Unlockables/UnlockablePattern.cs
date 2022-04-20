// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class UnlockablePattern : UnlockableItem
    {
        #region Fields
        [SerializeField] public string patternID;
        #endregion

        #region Properties
        public override bool IsUnlocked => ProgressManager.Data.UnlockedPatterns.Contains(patternID);
        #endregion

        #region Methods
        protected override void OnUnlock()
        {
            EditorManager.Instance.UnlockPattern(patternID);
        }
        #endregion
    }
}