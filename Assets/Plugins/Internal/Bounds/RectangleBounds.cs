using UnityEngine;

namespace DanielLochner.Assets
{
    public class RectangleBounds : Bounds
    {
        [SerializeField] private float width;
        [SerializeField] private float length;

        public override Vector3 RandomPointInBounds
        {
            get
            {
                Vector3 offset = transform.TransformDirection(Random.Range(-width, width) / 2f, 0f, Random.Range(-length, length) / 2f);
                return transform.position + offset;
            }
        }

        protected override void DrawBounds()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(width, 0f, length));
        }
    }
}