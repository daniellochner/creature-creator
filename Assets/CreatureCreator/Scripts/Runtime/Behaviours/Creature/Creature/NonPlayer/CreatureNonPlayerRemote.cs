// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureNonPlayerRemote : CreatureNonPlayer
    {
        public override void Setup()
        {
            base.Setup();

            if (!Loader.IsHidden)
            {
                Loader.RequestShow();
            }
            else
            {
                Loader.OnHide();
            }
        }
    }
}