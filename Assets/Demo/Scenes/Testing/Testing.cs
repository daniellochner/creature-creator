using DanielLochner.Assets;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Vector3 prev;

    private void FixedUpdate()
    {
        Vector3 d = transform.position - prev;
        Debug.Log(d / Time.deltaTime);
        prev = transform.position;
    }
}