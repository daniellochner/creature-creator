using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class OceanRaft : NetworkBehaviour
    {
        [SerializeField] private int waitTime;
        [SerializeField] private Vector3 direction;
        [SerializeField] private float speed;
        private bool move;
        
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(waitTime);
            move = true;
        }

        private void FixedUpdate()
        {
            if (move)
            {
                transform.position += direction * speed * Time.fixedDeltaTime;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                move = false;
            }
        }
    }
}