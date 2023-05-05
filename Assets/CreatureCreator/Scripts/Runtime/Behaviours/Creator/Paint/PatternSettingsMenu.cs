// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PatternSettingsMenu : Menu
    {
        #region Fields
        [SerializeField] private TMP_InputField tilingX;
        [SerializeField] private TMP_InputField tilingY;
        [SerializeField] private TMP_InputField offsetX;
        [SerializeField] private TMP_InputField offsetY;
        #endregion

        #region Properties
        public TMP_InputField TilingX => tilingX;
        public TMP_InputField TilingY => tilingY;
        public TMP_InputField OffsetX => offsetX;
        public TMP_InputField OffsetY => offsetY;
        #endregion

        #region Methods
        private void Start()
        {
            tilingX.onEndEdit.AddListener(UpdateTiling);
            tilingY.onEndEdit.AddListener(UpdateTiling);

            offsetX.onEndEdit.AddListener(UpdateOffset);
            offsetY.onEndEdit.AddListener(UpdateOffset);
        }
        public void Reset()
        {
            tilingX.text = (1f).ToString();
            tilingY.text = (1f).ToString();
            UpdateTiling(null);

            offsetX.text = (0f).ToString();
            offsetY.text = (0f).ToString();
            UpdateOffset(null);

            foreach (string patternId in SettingsManager.Data.HiddenPatterns)
            {
                EditorManager.Instance.AddPatternUI(patternId);
            }
            SettingsManager.Data.HiddenPatterns.Clear();

            EditorManager.Instance.UpdateLoadableCreatures();
        }

        private void UpdateTiling(string input)
        {
            if (!float.TryParse(tilingX.text, out float x)) return;
            if (!float.TryParse(tilingY.text, out float y)) return;
            EditorManager.Instance.Creature.Constructor.SetTiling(new Vector2(x, y));

            EditorManager.Instance.TakeSnapshot(Change.SetTiling);
        }
        private void UpdateOffset(string input)
        {
            if (!float.TryParse(offsetX.text, out float x)) return;
            if (!float.TryParse(offsetY.text, out float y)) return;
            EditorManager.Instance.Creature.Constructor.SetOffset(new Vector2(x, y));

            EditorManager.Instance.TakeSnapshot(Change.SetOffset);
        }
        #endregion
    }
}