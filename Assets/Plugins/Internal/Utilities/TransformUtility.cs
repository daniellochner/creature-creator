using UnityEngine;

namespace DanielLochner.Assets
{
    public static class TransformUtility
    {
        public static void DestroyChildren(this Transform transform)
        {
            while (transform.childCount > 0)
            {
                Object.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        public static void Set(this Transform transform, SerializableTransform serializableTransform, Transform world)
        {
            transform.position = world.TransformPoint(serializableTransform.position);
            transform.localScale = serializableTransform.scale;
            transform.rotation = world.rotation * serializableTransform.rotation;
        }
    }
}