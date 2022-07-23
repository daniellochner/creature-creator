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

            offsetX.text = (0.25f).ToString();
            offsetY.text = (0f).ToString();
            UpdateOffset(null);

            foreach (string patternId in EditorManager.Instance.HiddenPatterns)
            {
                EditorManager.Instance.AddPatternUI(patternId);
            }
            EditorManager.Instance.UpdateLoadableCreatures();
            EditorManager.Instance.HiddenPatterns.Clear();
        }

        private void UpdateTiling(string input)
        {
            EditorManager.Instance.Creature.Constructor.SetTiling(new Vector2(float.Parse(tilingX.text), float.Parse(tilingY.text)));
        }
        private void UpdateOffset(string input)
        {
            EditorManager.Instance.Creature.Constructor.SetOffset(new Vector2(float.Parse(offsetX.text), float.Parse(offsetY.text)));
        }
        #endregion
    }
}