using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public static bool IsOver(this GameObject gameObject)
        {
            if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(Camera.main, Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, 1 << gameObject.layer))
            {
                return hitInfo.collider.gameObject == gameObject;
            }
            return false;
        }

        public static bool IsInteractable(this GameObject gameObject)
        {
            if (gameObject == null)
                return false;

            Selectable selectable = gameObject.GetComponent<Selectable>();
            if (selectable != null && !selectable.interactable)
                return false;

            List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();
            bool interactableCheck = true;

            Transform cg_transform = gameObject.transform;
            while (cg_transform != null)
            {
                cg_transform.GetComponents(m_CanvasGroupCache);
                bool ignoreParentGroups = false;

                for (int i = 0, count = m_CanvasGroupCache.Count; i < count; i++)
                {
                    var canvasGroup = m_CanvasGroupCache[i];

                    interactableCheck &= canvasGroup.interactable;
                    ignoreParentGroups |= canvasGroup.ignoreParentGroups || !canvasGroup.interactable;
                }

                if (ignoreParentGroups)
                {
                    break;
                }

                cg_transform = cg_transform.parent;
            }

            return interactableCheck;
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