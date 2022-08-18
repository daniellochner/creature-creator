using UnityEngine;

namespace DanielLochner.Assets
{
    public static class PhysicsUtility
    {
        public static Vector3? RaycastCone(Vector3 origin, Vector3 dir, float length, float angle, int n, int m, int layerMask, Vector3 normal, float alignmentThreshold)
        {
            // Center
            if (Physics.Raycast(origin, dir, out RaycastHit hitInfo1, length, layerMask) && Vector3.Dot(hitInfo1.normal, normal) > alignmentThreshold)
            {
                return hitInfo1.point;
            }

            // Cone
            for (int i = 1; i < n + 1; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    dir = Quaternion.Euler(i * angle, j / 360f, 0) * dir;

                    if (Physics.Raycast(origin, dir, out RaycastHit hitInfo2, length, layerMask) && Vector3.Dot(hitInfo2.normal, normal) > alignmentThreshold)
                    {
                        return hitInfo2.point;
                    }
                }
            }
            return null;
        }
    }
}