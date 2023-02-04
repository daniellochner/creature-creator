// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LoadOnStart : MonoBehaviour
    {
        [SerializeField] private TextAsset data;
        [SerializeField] private CreatureConstructor creature;
        private void Start()
        {
            creature.Construct(JsonUtility.FromJson<CreatureData>(data.text));
        }
    }
}