// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class BodyPartSettingsMenu : Menu
    {
        #region Methods
        public void Reset()
        {
            foreach (string bodyPartId in SettingsManager.Data.HiddenBodyParts)
            {
                EditorManager.Instance.AddBodyPartUI(bodyPartId);
            }
            SettingsManager.Data.HiddenBodyParts.Clear();

            EditorManager.Instance.UpdateBodyPartTotals();
            EditorManager.Instance.UpdateLoadableCreatures();
        }
        #endregion
    }
}