// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LoadFromTextAsset : MonoBehaviour
    {
        #region Fields
        [SerializeField] private CreatureConstructor baseCreaturePrefab;
        [Space]
        [SerializeField] private TextAsset creatureTextAsset;
        [SerializeField, Button("Load")] private bool load;
        #endregion

        #region Methods
        public void Load()
        {
            Instantiate(baseCreaturePrefab, transform).Construct(JsonUtility.FromJson<CreatureData>(creatureTextAsset.text));
        }
        #endregion
    }
}