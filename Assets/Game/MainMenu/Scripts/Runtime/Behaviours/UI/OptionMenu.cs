// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class OptionMenu : Menu
    {
        private AudioSource source;

        protected override void Awake()
        {
            base.Awake();
            source = GetComponent<AudioSource>();
        }

        public override void Open(bool instant = false)
        {
            base.Open(instant);
            source.Play();
        }
    }
}