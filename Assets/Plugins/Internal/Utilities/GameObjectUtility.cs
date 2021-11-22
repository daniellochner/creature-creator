using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public static class GameObjectUtility
    {
        public static List<GameObject> FindChildrenWithTag(this GameObject gameObject, string tag)
        {
            List<GameObject> gameObjects = new List<GameObject>();

            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag(tag))
                {
                    gameObjects.Add(child.gameObject);
                }
            }

            return gameObjects;
        }

        public static void SetLayerRecursively(this GameObject gameObject, int layer, List<string> ignoredLayers = null)
        {
            if (ignoredLayers == null || !ignoredLayers.Contains(LayerMask.LayerToName(gameObject.layer)))
            {
                gameObject.layer = layer;
            }

            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetLayerRecursively(layer, ignoredLayers);
            }
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}