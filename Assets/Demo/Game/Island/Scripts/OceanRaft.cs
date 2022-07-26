using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class OceanRaft : NetworkBehaviour
    {
        [SerializeField] private Vector3 direction;
        [SerializeField] private float speed;

        private void FixedUpdate()
        {
            transform.position += direction * speed * Time.fixedDeltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                enabled = false;
            }
        }
    }
}