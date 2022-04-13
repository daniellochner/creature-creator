using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class Placer : MonoBehaviour
    {
        [SerializeField] public GameObject prefab;
        [SerializeField] public int count;
        [SerializeField] public float spacing;
        [SerializeField] public float offset;
        [SerializeField] public Vector3 defaultRotation;
        [SerializeField] public bool randomRotation;
        [SerializeField] public bool alignToSurface;

        [SerializeField, Button("Place")] private bool place;


        public void Place()
        {
            transform.DestroyChildren();

            for (int i = 0; i < count; i++)
            {
                GameObject fence = Instantiate(prefab, transform, false);
                fence.name = prefab.name;

                // Position
                Vector3 origin = transform.position + Vector3.forward * (spacing * i + Random.Range(-offset, offset));
                if (alignToSurface && Physics.Raycast(origin, Vector3.down, out RaycastHit hitInfo))
                {
                    fence.transform.position = hitInfo.point;
                }
                else
                {
                    fence.transform.position = origin;
                }

                // Rotation
                fence.transform.localRotation = Quaternion.Euler(defaultRotation);
                if (randomRotation)
                {
                    fence.transform.localRotation *= Quaternion.Euler(0f, Random.Range(0, 360), 0f);
                }
            }
        }
    }
}