using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class Bounds : MonoBehaviour
    {
        public abstract Vector3 RandomPointInBounds { get; }

        private void OnDrawGizmosSelected()
        {
            DrawBounds();
        }

        protected abstract void DrawBounds();

        public abstract bool IsPointInBounds(Vector3 point);

        public abstract Vector3 GetClosestPointOnBounds(Vector3 point, bool useY);
    }
}