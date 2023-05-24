using UnityEngine;

namespace DanielLochner.Assets
{
    public static class SkinnedMeshRendererUtility
    {
        public static void RecalculateBounds(this SkinnedMeshRenderer skinnedMeshRenderer)
        {
            Mesh tmpMesh = new Mesh();
            skinnedMeshRenderer.BakeMesh(tmpMesh);

            MinMax minMaxX = new MinMax(Mathf.Infinity, Mathf.NegativeInfinity);
            MinMax minMaxY = new MinMax(Mathf.Infinity, Mathf.NegativeInfinity);
            MinMax minMaxZ = new MinMax(Mathf.Infinity, Mathf.NegativeInfinity);
            foreach (Vector3 vertex in tmpMesh.vertices)
            {
                if (vertex.x < minMaxX.min)
                {
                    minMaxX.min = vertex.x;
                }
                if (vertex.x > minMaxX.max)
                {
                    minMaxX.max = vertex.x;
                }

                if (vertex.y < minMaxY.min)
                {
                    minMaxY.min = vertex.y;
                }
                if (vertex.y > minMaxY.max)
                {
                    minMaxY.max = vertex.y;
                }

                if (vertex.z < minMaxZ.min)
                {
                    minMaxZ.min = vertex.z;
                }
                if (vertex.z > minMaxZ.max)
                {
                    minMaxZ.max = vertex.z;
                }
            }

            Vector3 center = new Vector3(minMaxX.Average, minMaxY.Average, minMaxZ.Average);
            Vector3 size = new Vector3(minMaxX.Range, minMaxY.Range, minMaxZ.Range);
            skinnedMeshRenderer.localBounds = new UnityEngine.Bounds(center, size);

            Object.Destroy(tmpMesh);
        }
    }
}