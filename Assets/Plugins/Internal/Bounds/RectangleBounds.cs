using UnityEngine;

namespace DanielLochner.Assets
{
    public class RectangleBounds : Bounds
    {
        [SerializeField] private float width;
        [SerializeField] private float length;

        public float Width => width * transform.localScale.x;
        public float Length => length * transform.localScale.z;

        public override Vector3 RandomPointInBounds
        {
            get
            {
                Vector3 offset = transform.TransformDirection(Random.Range(-Width, Width) / 2f, 0f, Random.Range(-Length, Length) / 2f);
                return transform.position + offset;
            }
        }

        protected override void DrawBounds()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(Width, 0f, Length));
        }
    }
}