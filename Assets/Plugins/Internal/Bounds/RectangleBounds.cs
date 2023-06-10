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
            Gizmos.DrawWireCube(transform.position, transform.TransformVector(width, 0f, length));
        }

        public override bool IsPointInBounds(Vector3 point)
        {
            Vector3 size = transform.TransformVector(width, 0f, length);

            if (Mathf.Abs(point.x - transform.position.x) > size.x / 2f)
            {
                return false;
            }

            if (Mathf.Abs(point.z - transform.position.z) > size.z / 2f)
            {
                return false;
            }

            return true;
        }

        public override Vector3 GetClosestPointOnBounds(Vector3 point, bool useY)
        {
            Vector3 cp = new Vector3(0f, useY ? point.y : transform.position.y, 0f);

            float boundsX1 = transform.TransformPoint(-(width  / 2f), 0f, 0f).x;
            float boundsX2 = transform.TransformPoint(+(width  / 2f), 0f, 0f).x;
            float boundsZ1 = transform.TransformPoint(0f, 0f, -(length / 2f)).z;
            float boundsZ2 = transform.TransformPoint(0f, 0f, +(length / 2f)).z;

            point.x = Mathf.Clamp(point.x, boundsX1, boundsX2);
            point.z = Mathf.Clamp(point.z, boundsZ1, boundsZ2);

            float distX1 = Mathf.Abs(boundsX1 - point.x);
            float distX2 = Mathf.Abs(boundsX2 - point.x);
            float distZ1 = Mathf.Abs(boundsZ1 - point.z);
            float distZ2 = Mathf.Abs(boundsZ2 - point.z);

            float min = Mathf.Min(distX1, distX2, distZ1, distZ2);
            if (min == distX1)
            {
                cp.x = boundsX1;
                cp.z = point.z;
            }
            else
            if (min == distX2)
            {
                cp.x = boundsX2;
                cp.z = point.z;
            }
            else
            if (min == distZ1)
            {
                cp.z = boundsZ1;
                cp.x = point.x;
            }
            else
            if (min == distZ2)
            {
                cp.z = boundsZ2;
                cp.x = point.x;
            }

            return cp;
        }
    }
}