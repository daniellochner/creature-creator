// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using Unity.Collections;
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
        public SerializableTransform serializableTransform;
        public Vector3 stretch;
        public Color primaryColour = default;
        public Color secondaryColour = default;

        public AttachedBodyPart()
        {

        }
        public AttachedBodyPart(string bodyPartID)
        {
            this.bodyPartID = bodyPartID;

            GUID = Guid.NewGuid().ToString();
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            Serialize(serializer);
        }

        public virtual void Serialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref GUID);
            serializer.SerializeValue(ref bodyPartID);
            serializer.SerializeValue(ref boneIndex);
            serializer.SerializeNetworkSerializable(ref serializableTransform);
            serializer.SerializeValue(ref stretch);
            serializer.SerializeValue(ref primaryColour);
            serializer.SerializeValue(ref secondaryColour);
        }

        // TODO: Fix for IL2CPP Code Generation FasterRuntime bug... (https://discord.com/channels/449263083769036810/1083129789583474790)
        private void FasterRuntimeFix()
        {
            AttachedLimb limb = new AttachedLimb();
            AttachedExtremity extremity = new AttachedExtremity();

            using (var writer = new FastBufferWriter(1024, Allocator.Persistent))
            {
                writer.WriteNetworkSerializable(limb);
                writer.WriteNetworkSerializable(extremity);

                using (var reader = new FastBufferReader(writer, Allocator.None))
                {
                    reader.ReadNetworkSerializable(out limb);
                    reader.ReadNetworkSerializable(out extremity);
                }
            }
        }
    }
}