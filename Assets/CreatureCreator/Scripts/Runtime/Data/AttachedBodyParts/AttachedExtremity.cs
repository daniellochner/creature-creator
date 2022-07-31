// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class AttachedExtremity : AttachedBodyPart
    {
        public string connectedLimbGUID;

        public AttachedExtremity(string bodyPartID) : base(bodyPartID) { }
    }
}