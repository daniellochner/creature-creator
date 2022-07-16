// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreaturePlayer : CreatureBase
    {
        [SerializeField] private CreatureNamer namer;

        public CreatureNamer Namer => namer;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            namer = GetComponent<CreatureNamer>();
        }
#endif

        public override void Setup()
        {
            base.Setup();
            namer.Setup();
        }
    }
}