// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class FootConstructor : ExtremityConstructor
    {
        #region Methods
        private void LateUpdate()
        {
            HandleAlignment();
        }

        private void HandleAlignment()
        {
            if (ConnectedLimb != null)
            {
                transform.rotation = CreatureConstructor.Body.rotation;
            }
        }
        #endregion
    }
}