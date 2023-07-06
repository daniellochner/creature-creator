using PathCreation;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LakeRaft : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private PathCreator pathCreator;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveDelay;
        [SerializeField] private NetworkVariable<bool> isMoving;

        private TrackRegion region;
        private Coroutine moveCoroutine;
        private float distance;
        private bool isReady = true;
        #endregion

        #region Methods
        private void Awake()
        {
            region = GetComponent<TrackRegion>();
        }
        private void Start()
        {
            SnapToPath(0);

            region.OnTrack += OnTrack;
            region.OnLoseTrackOf += OnLoseTrackOf;
        }

        private void OnTriggerStay(Collider other)
        {
            OnCreature(other, delegate (CreatureBase creature)
            {
                foreach (LegAnimator leg in creature.Animator.Legs)
                {
                    Vector3 pos = creature.Constructor.transform.L2WSpace(leg.DefaultFootLocalPos);
                    leg.Target.position = leg.Anchor.position = pos;
                    leg.Target.rotation = leg.Anchor.rotation = Quaternion.identity;
                }

                CreaturePlayerLocal player = creature as CreaturePlayerLocal;
                if (player)
                {
                    player.Mover.enabled = !isMoving.Value;
                }
            });
        }

        private void OnTrack(Collider other)
        {
            if (IsServer)
            {
                OnCreature(other, delegate
                {
                    if (!isMoving.Value && isReady)
                    {
                        this.StopStartCoroutine(MoveRoutine(), ref moveCoroutine);
                    }
                });
            }
        }
        private void OnLoseTrackOf(Collider other)
        {
            OnCreature(other, delegate (CreatureBase creature)
            {
                CreaturePlayerLocal player = creature as CreaturePlayerLocal;
                if (player)
                {
                    player.Mover.enabled = true;
                }
            });
        }

        private IEnumerator MoveRoutine()
        {
            yield return new WaitForSeconds(moveDelay);

            isReady = false;
            {
                isMoving.Value = true;
                {
                    float target = distance + pathCreator.path.length;
                    while (distance <= target)
                    {
                        distance += moveSpeed * Time.fixedDeltaTime;
                        SnapToPath(distance);

                        yield return new WaitForFixedUpdate();
                    }
                }
                isMoving.Value = false;

                yield return new WaitUntil(() => region.tracked.Count == 0);
            }
            isReady = true;
        }

        private void OnCreature(Collider other, UnityAction<CreatureBase> onCreature)
        {
            CreatureBase creature = other.GetComponent<CreatureBase>();
            if (creature != null)
            {
                onCreature?.Invoke(creature);
            }
        }
        private void SnapToPath(float distance)
        {
            transform.position = pathCreator.path.GetPointAtDistance(distance, EndOfPathInstruction.Reverse);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distance, EndOfPathInstruction.Reverse);
        }
        #endregion
    }
}