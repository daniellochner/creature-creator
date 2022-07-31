// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LoadFromTextAsset : MonoBehaviour
    {
        #region Fields
        [SerializeField] private CreatureBase creature;
        [SerializeField] private TextAsset data;
        [Space]
        [SerializeField, Button("Load")] private bool load;
        #endregion

        #region Methods
#if UNITY_EDITOR
        private void OnValidate()
        {
            CreatureBase c = GetComponent<CreatureBase>();
            if (c != null)
            {
                creature = c;
            }
        }
#endif
        public void Load()
        {
            creature.Setup();
            creature.Constructor.Construct(JsonUtility.FromJson<CreatureData>(data.text));
        }
        #endregion
    }
}