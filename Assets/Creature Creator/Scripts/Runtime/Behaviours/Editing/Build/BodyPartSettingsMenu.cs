// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class BodyPartSettingsMenu : Menu
    {
        #region Methods
        public void Reset()
        {
            foreach (string bodyPartId in EditorManager.Instance.HiddenBodyParts)
            {
                EditorManager.Instance.AddBodyPartUI(bodyPartId);
            }
            EditorManager.Instance.UpdateBodyPartTotals();
            EditorManager.Instance.UpdateLoadableCreatures();
            EditorManager.Instance.HiddenBodyParts.Clear();
        }
        #endregion
    }
}