using UnityEngine;

namespace DanielLochner.Assets
{
    public class QuaternionUtility : MonoBehaviour
    {
        public static Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
        {
            Vector3 c = current.eulerAngles;
            Vector3 t = target.eulerAngles;

            float x = Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime);
            float y = Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime);
            float z = Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime);

            return Quaternion.Euler(x, y, z);
        }
    }
}