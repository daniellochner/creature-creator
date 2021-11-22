// Credit: https://github.com/SebLague/Spherical-Gravity

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    [SerializeField] private GravityAttractor attractor;

    private Rigidbody rb;

    public GravityAttractor Attractor
    {
        get => attractor;
        set => attractor = value;
    }

    public float Resistance { get; set; } = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (attractor == null) return;

        attractor.Attract(rb, Resistance);
    }
}