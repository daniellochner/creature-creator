// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DemoKeys : ScriptableObject
    {
        #region Fields
        [SerializeField] private SecretKey googleAPI;
        [SerializeField] private SecretKey pinnedCommentId;
        [SerializeField] private SecretKey creatureEncryption;
        #endregion

        #region Properties
        public SecretKey GoogleAPI => googleAPI;
        public SecretKey PinnedCommentId => pinnedCommentId;
        public SecretKey CreatureEncryption => creatureEncryption;
        #endregion
    }
}