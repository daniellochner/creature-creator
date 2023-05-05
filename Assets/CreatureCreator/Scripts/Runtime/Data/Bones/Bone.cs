// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class Bone : INetworkSerializable
    {
        public Vector3 position;
        public Quaternion rotation;
        public float weight;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref position);
            serializer.SerializeValue(ref rotation);
            serializer.SerializeValue(ref weight);
        }

        public Bone()
        {
        }
        public Bone(Bone other)
        {
            position = other.position;
            rotation = other.rotation;
            weight = other.weight;
        }
    }
}