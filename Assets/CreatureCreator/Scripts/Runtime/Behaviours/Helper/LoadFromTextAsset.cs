// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LoadFromTextAsset : MonoBehaviour
    {
        #region Fields
        [SerializeField] private CreatureConstructor creaturePrefab;
        [SerializeField] private TextAsset data;
        [Space]
        [SerializeField, Button("Load")] private bool load;
        #endregion

        #region Methods
        public CreatureConstructor Load()
        {
            CreatureConstructor creature = Instantiate(creaturePrefab, transform.position, transform.rotation, transform);
            creature.Construct(JsonUtility.FromJson<CreatureData>(data.text));
            return creature;
        }
        #endregion
    }
}