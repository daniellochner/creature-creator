using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class OceanRaft : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private int waitTime;
        [SerializeField] private Vector3 direction;
        [SerializeField] private float speed;

        private BuoyantObject buoyantObject;
        private bool move;
        #endregion

        #region Methods
        private void Awake()
        {
            buoyantObject = GetComponent<BuoyantObject>();
        }
        private IEnumerator Start()
        {
            if (!IsServer)
            {
                Destroy(buoyantObject);
            }

            yield return new WaitForSeconds(waitTime);
            move = true;
        }

        private void FixedUpdate()
        {
            if (IsServer && move)
            {
                transform.position += direction * speed * Time.fixedDeltaTime;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (IsServer && (collision.gameObject.layer == LayerMask.NameToLayer("Ground")))
            {
                move = false;
            }
        }
        #endregion
    }
}