using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    [Serializable]
    public class SerializableTransform
    {
        #region Fields
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        #endregion

        #region Methods
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
        #endregion
    }
}