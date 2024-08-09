// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEditor;
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
        public override bool IsUnlocked => WorldManager.Instance.IsPatternUnlocked(PatternID);

        public Pattern Pattern => DatabaseManager.GetDatabaseEntry<Pattern>("Patterns", patternID);
        #endregion

        #region Methods
        protected override void OnUnlock()
        {
            switch (WorldManager.Instance.World.Mode)
            {
                case Mode.Adventure:
                    ProgressManager.Instance.UnlockPattern(patternID);
                    break;

                case Mode.Timed:
                    TimedManager.Instance.UnlockPattern(patternID);
                    break;
            }

            EditorManager.Instance.UnlockPattern(patternID);
        }
        protected override void OnSpawn()
        {
            texture.material.mainTexture = Pattern.Texture;
        }
        #endregion
    }
}