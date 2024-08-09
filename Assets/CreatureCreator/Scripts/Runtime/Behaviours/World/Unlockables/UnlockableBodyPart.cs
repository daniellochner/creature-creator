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
        public override bool IsUnlocked => WorldManager.Instance.IsBodyPartUnlocked(BodyPartID);

        public BodyPart BodyPart => DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartID);
        #endregion

        #region Methods
        protected override void OnUnlock()
        {
            switch (WorldManager.Instance.World.Mode)
            {
                case Mode.Adventure:
                    ProgressManager.Instance.UnlockBodyPart(bodyPartID);
                    break;

                case Mode.Timed:
                    TimedManager.Instance.UnlockBodyPart(bodyPartID);
                    break;
            }

            EditorManager.Instance.UnlockBodyPart(bodyPartID);
        }
        protected override void OnSpawn()
        {
        }
        #endregion
    }
}