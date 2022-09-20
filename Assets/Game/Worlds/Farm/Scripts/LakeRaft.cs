using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LakeRaft : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Transform pos1;
        [SerializeField] private Transform pos2;
        [SerializeField] private float moveTime;
        [SerializeField] private float moveCooldown;
        [SerializeField] private float moveDelay;
        [SerializeField] private NetworkVariable<bool> isMoving;

        private bool canMove = true;
        #endregion

        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            OnCreature(other, delegate (CreatureBase creature)
            {
                if (creature is CreaturePlayerLocal)
                {
                    TryMoveServerRpc();
                }
            });
        }
        private void OnTriggerStay(Collider other)
        {
            OnCreature(other, delegate (CreatureBase creature)
            {
                HandleCreatureOnPlatform(creature, isMoving.Value);
            });
        }
        private void OnTriggerExit(Collider other)
        {
            OnCreature(other, delegate (CreatureBase creature)
            {
                HandleCreatureOnPlatform(creature, false);
            });
        }

        private void HandleCreatureOnPlatform(CreatureBase creature, bool isOnMovingPlatform)
        {
            creature.Animator.Animator.enabled = !isOnMovingPlatform;
            creature.Animator.Rig.weight = isOnMovingPlatform ? 0f : 1f;
            if (isOnMovingPlatform)
            {
                foreach (LegAnimator leg in creature.Animator.Legs)
                {
                    leg.Target.position = leg.Anchor.position = creature.transform.L2WSpace(leg.DefaultFootLocalPos);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void TryMoveServerRpc()
        {
            if (!isMoving.Value && canMove) StartCoroutine(MoveRoutine());
        }
        private IEnumerator MoveRoutine()
        {
            canMove = false;
            yield return new WaitForSeconds(moveDelay);
            isMoving.Value = true;

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
            {
                transform.position = Vector3.Lerp(pos1.position, pos2.position, p);
            }, 
            moveTime);

            Vector3 tmp = pos1.position;
            pos1.position = pos2.position;
            pos2.position = tmp;

            isMoving.Value = false;
            yield return new WaitForSeconds(moveCooldown);
            canMove = true;
        }

        private void OnCreature(Collider other, UnityAction<CreatureBase> onCreature)
        {
            CreatureBase creature = other.GetComponent<CreatureBase>();
            if (creature != null)
            {
                onCreature?.Invoke(creature);
            }
        }
        #endregion
    }
}