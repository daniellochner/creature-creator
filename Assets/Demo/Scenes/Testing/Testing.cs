using UnityEngine;

using DanielLochner.Assets.CreatureCreator;

public class Testing : MonoBehaviour
{
    public Transform c;
    public bool move;

    Vector3 md = Vector3.zero;

    Vector3 vel;

    private void LateUpdate()
    {
        Vector3 target = move ? c.forward : Vector3.zero;
        md = Vector3.SmoothDamp(md, target * Time.deltaTime, ref vel, 1f);

        c.position += md;
    }
}