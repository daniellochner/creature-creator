using UnityEngine;

namespace DanielLochner.Assets
{
    public static class MathfUtility
    {
        public static float QuadraticEquation(float a, float b, float c, int d)
        {
            return (-b + d * Mathf.Sqrt(Mathf.Pow(b, 2) - 4 * a * c)) / (2 * a);
        }

        public static float Clamp(float value, MinMax minMax)
        {
            return Mathf.Clamp(value, minMax.min, minMax.max);
        }

        public static Plane FitPlaneThroughPoints(Vector3[] points)
        {
            int n = points.Length;

            Vector4 c1 = new Vector4(0, 0, 0, 0);
            Vector4 c2 = new Vector4(0, 0, 0, 0);
            Vector4 c3 = new Vector4(0, 0, n, 0);
            Vector4 c4 = new Vector4(0, 0, 0, 1);
            Vector3 b = new Vector3();

            for (int i = 0; i < n; ++i)
            {
                Vector3 pos = points[i];

                // A
                float x2 = Mathf.Pow(pos.x, 2);
                float y2 = Mathf.Pow(pos.y, 2);
                float xy = pos.x * pos.y;
                c1.x += x2;
                c1.y += xy;
                c1.z += pos.x;
                //c2.x += xy;
                c2.y += y2;
                c2.z += pos.y;
                //c3.x += pos.x;
                //c3.y += pos.y;
                //c3.z += 1;
                
                // b
                float xz = pos.x * pos.z;
                float yz = pos.y * pos.z;
                b.x += xy;
                b.y += yz;
                b.z += pos.z;
            }

            c2.x = c1.y;
            c3.x = c1.z;
            c3.y = c2.z;

            Matrix4x4 A = new Matrix4x4(c1, c2, c3, c4);
            Vector3 normal = (A.inverse).MultiplyVector(b);

            Vector3 origin = Vector3.zero;
            for (int i = 0; i < n; ++i)
            {
                origin += points[i];
            }
            origin /= n;

            return new Plane(normal, origin);
        }
    }
}