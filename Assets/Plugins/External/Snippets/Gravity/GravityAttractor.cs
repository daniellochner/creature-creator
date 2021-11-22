// Credit: https://github.com/SebLague/Spherical-Gravity

using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    [SerializeField] private float gravity = -9.8f;

    public void Attract(Rigidbody body, float resistance = 1f)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 localUp = body.transform.up;

        body.AddForce(gravityUp * gravity / resistance);
        body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
    }
}