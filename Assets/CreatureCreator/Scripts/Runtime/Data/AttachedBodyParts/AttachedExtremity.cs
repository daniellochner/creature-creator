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

        public AttachedExtremity()
        {
        }
        public AttachedExtremity(string bodyPartID) : base(bodyPartID)
        {
        }

        public override void NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            base.NetworkSerialize(serializer);

            serializer.SerializeValue(ref connectedLimbGUID);
        }
    }
}