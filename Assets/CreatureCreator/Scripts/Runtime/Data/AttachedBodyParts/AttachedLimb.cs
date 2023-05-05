// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class AttachedLimb : AttachedBodyPart
    {
        public List<Bone> bones = new List<Bone>();

        public AttachedLimb()
        {
        }
        public AttachedLimb(string bodyPartID) : base(bodyPartID)
        {
        }

        public override void Serialize<T>(BufferSerializer<T> serializer)
        {
            base.Serialize(serializer);

            Bone[] b = null;
            if (serializer.IsWriter)
            {
                b = bones.ToArray();
            }
            serializer.SerializeValue(ref b);
            if (serializer.IsReader)
            {
                bones = new List<Bone>(b);
            }
        }
    }
}