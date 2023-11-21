// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class AttachedBodyPart : INetworkSerializable
    {
        public string GUID;
        public string bodyPartID;
        public int boneIndex = -1;
        public SerializableTransform serializableTransform = new SerializableTransform();
        public Vector3 stretch = default;
        public Color primaryColour = default;
        public Color secondaryColour = default;
        public bool hideMain = false;
        public bool hideFlipped = false;

        public AttachedBodyPart()
        {

        }
        public AttachedBodyPart(string bodyPartID)
        {
            this.bodyPartID = bodyPartID;

            GUID = Guid.NewGuid().ToString();
        }

        public virtual void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref GUID);
            serializer.SerializeValue(ref bodyPartID);
            serializer.SerializeValue(ref boneIndex);
            serializer.SerializeNetworkSerializable(ref serializableTransform);
            serializer.SerializeValue(ref stretch);
            serializer.SerializeValue(ref primaryColour);
            serializer.SerializeValue(ref secondaryColour);
            serializer.SerializeValue(ref hideMain);
            serializer.SerializeValue(ref hideFlipped);
        }
    }
}