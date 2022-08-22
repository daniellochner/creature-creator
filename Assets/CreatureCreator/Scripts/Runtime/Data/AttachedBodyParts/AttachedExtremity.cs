// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class AttachedExtremity : AttachedBodyPart
    {
        public string connectedLimbGUID;

        public AttachedExtremity(string bodyPartID) : base(bodyPartID) { }

        public override void Serialize<T>(BufferSerializer<T> serializer)
        {
            base.Serialize(serializer);

            serializer.SerializeValue(ref connectedLimbGUID);
        }
    }
}