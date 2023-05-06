// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class UnlockablePattern : UnlockableItem
    {
        #region Fields
        [SerializeField] private string patternID;

        [Header("Internal References")]
        [SerializeField] private MeshRenderer texture;
        #endregion

        #region Properties
        public string PatternID => patternID;
        public override bool IsUnlocked => ProgressManager.Data.UnlockedPatterns.Contains(patternID) || EditorManager.Instance.CreativeMode;

        public Pattern Pattern => DatabaseManager.GetDatabaseEntry<Pattern>("Patterns", patternID);
        #endregion

        #region Methods
        protected override void OnUnlock()
        {
            EditorManager.Instance.UnlockPattern(patternID);

#if USE_STATS
            StatsManager.Instance.UnlockedPatterns++;
#endif
        }
        protected override void OnSpawn()
        {
            texture.material.mainTexture = Pattern.Texture;
        }
        #endregion
    }
}