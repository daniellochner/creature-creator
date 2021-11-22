// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class AttachedLimb : AttachedBodyPart
    {
        public List<Bone> bones = new List<Bone>();

        public AttachedLimb(string bodyPartID) : base(bodyPartID) { }
    }
}