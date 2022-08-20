using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LakeRaft : NetworkBehaviour
    {
        [SerializeField] private Transform pos1;
        [SerializeField] private Transform pos2;
        [SerializeField] private float time;
        private bool isMoving;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player/Local"))
            {
                MoveToOtherSideServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void MoveToOtherSideServerRpc()
        {
            if (!isMoving) { StartCoroutine(MoveRoutine()); }
        }

        private IEnumerator MoveRoutine()
        {
            isMoving = true;

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
            {
                transform.position = Vector3.Lerp(pos1.position, pos2.position, p);
            }, 
            time);

            Vector3 tmp = pos1.position;
            pos1.position = pos2.position;
            pos2.position = tmp;

            isMoving = false;
        }
    }
}