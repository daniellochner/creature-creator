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
        public override bool IsUnlocked => ProgressManager.Data.UnlockedPatterns.Contains(patternID);

        public Texture2D Pattern => DatabaseManager.GetDatabaseEntry<Texture2D>("Patterns", patternID);
        #endregion

        #region Methods
        protected override void OnUnlock()
        {
            EditorManager.Instance.UnlockPattern(patternID);
        }
        protected override void OnSpawn()
        {
            texture.material.mainTexture = Pattern;
        }
        #endregion
    }
}