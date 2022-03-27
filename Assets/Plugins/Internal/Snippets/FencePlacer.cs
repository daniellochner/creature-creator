using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class FencePlacer : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int count;
        [SerializeField] private float spacing;
        [SerializeField] private Vector3 rot;

        [SerializeField, Button("Setup")] private bool setup;

        public void Setup()
        {
            transform.DestroyChildren();

            for (int i = 0; i < count; i++)
            {
                GameObject fence = Instantiate(prefab, transform, false);
                fence.name = prefab.name;

                Vector3 pos = Vector3.forward * (spacing * i);
                fence.transform.localPosition = pos;
                fence.transform.localRotation = Quaternion.Euler(rot);
            }
        }
    }
}