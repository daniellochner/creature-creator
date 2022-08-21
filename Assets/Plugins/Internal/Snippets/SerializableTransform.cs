using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    [Serializable]
    public class SerializableTransform : INetworkSerializable
    {
        #region Fields
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        #endregion

        #region Methods
        public SerializableTransform()
        {

        }
        public SerializableTransform(Transform transform, Transform world)
        {
            position = world.InverseTransformPoint(transform.position);
            scale = transform.localScale;
            rotation = Quaternion.Inverse(world.rotation) * transform.rotation;
        }
        public SerializableTransform(Vector3 position, Quaternion rotation, Vector3 scale, Transform world)
        {
            position = world.InverseTransformPoint(position);
            rotation = Quaternion.Inverse(world.rotation) * rotation;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref position);
            serializer.SerializeValue(ref scale);
            serializer.SerializeValue(ref rotation);
        }
        #endregion
    }
}