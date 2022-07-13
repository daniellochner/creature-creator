// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class OptionMenu : Menu
    {
        private AudioSource click;

        protected override void Awake()
        {
            base.Awake();
            click = GetComponent<AudioSource>();
        }

        public override void OnBeginOpen()
        {
            base.OnBeginOpen();
            click.Play();
        }
    }
}