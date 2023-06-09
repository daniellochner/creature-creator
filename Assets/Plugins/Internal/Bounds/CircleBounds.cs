using UnityEngine;

namespace DanielLochner.Assets
{
    public class CircleBounds : Bounds
    {
        [SerializeField] public float radius;

        public override Vector3 RandomPointInBounds
        {
            get
            {
                Vector3 direction = Quaternion.AngleAxis(Random.Range(0f, 360f), transform.up) * transform.forward;
                float   distance  = Random.Range(0f, radius);

                return transform.position + (direction * distance);
            }
        }

        protected override void DrawBounds()
        {
#if UNITY_EDITOR
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, radius);
#endif
        }

        public override Vector3 GetClosestPointOnBounds(Vector3 point, bool useY)
        {
            throw new System.NotImplementedException();
        }
    }
}