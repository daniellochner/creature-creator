using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(Follower))]
    public class HoldableDummy : MonoBehaviour
    {
        #region Fields
        private MeshRenderer mr;
        private MeshFilter mf;
        private Follower f;

        private Held held;
        private Transform hand;
        #endregion

        #region Methods
        private void Awake()
        {
            mr = GetComponent<MeshRenderer>();
            mf = GetComponent<MeshFilter>();
            f = GetComponent<Follower>();
        }
        private void Update()
        {
            if (NetworkManager.Singleton.IsServer && !hand)
            {
                held.Hand.Value = default;
            }
        }

        public void Setup(Held held, Held.HeldData data)
        {
            this.held = held;

            // Hand
            foreach (CreatureBase creature in CreatureBase.Creatures)
            {
                if (creature.NetworkObjectId == data.networkObjectId)
                {
                    ArmAnimator arm = creature.Animator.Arms.Find(x => x.name == data.armGUID.ToString());
                    if (arm.ArmConstructor.ConnectedHand != null)
                    {
                        hand = arm.ArmConstructor.ConnectedHand.Palm;
                    }
                    else
                    {
                        hand = arm.ArmConstructor.Extremity;
                    }

                    break;
                }
            }

            // Dummy
            transform.localScale = this.held.transform.localScale;
            mr.materials = held.GetComponent<MeshRenderer>().materials;
            mf.mesh = held.GetComponent<MeshFilter>().mesh;
            f.SetFollow(hand, true);
        }
        #endregion
    }
}