using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Orchard : MonoBehaviour
    {
        [SerializeField] private GameObject[] trees;
        [Space]
        [SerializeField] private int count;
        [SerializeField] private float spacing;
        [SerializeField] private float offset;
        [Space]
        [SerializeField, Button("Place")] private bool place;

        public void Place()
        {
            transform.DestroyChildren();

            for (int i = 0; i < trees.Length; i++)
            {
                GameObject tree = trees[i];

                GameObject root = new GameObject(tree.name);
                root.transform.parent = transform;
                root.transform.localPosition = Vector3.right * (spacing * i);

                Placer placer = root.AddComponent<Placer>();
                placer.prefab = tree;
                placer.count = count;
                placer.spacing = spacing;
                placer.offset = offset;
                placer.randomRotation = true;
                placer.alignToSurface = true;
                placer.Place();
            }
        }
    }
}