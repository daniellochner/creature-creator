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
        [SerializeField] private float checkTime;

        private MeshRenderer mr;
        private MeshFilter mf;
        private Follower f;

        private Holdable holdable;
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
            if (!NetworkManager.Singleton.IsServer) return;
            if (!hand)
            {
                holdable.Drop();
            }
        }

        public void Setup(Holdable holdable, string limbGUID)
        {
            this.holdable = holdable;
            StartCoroutine(SetupRoutine(holdable, limbGUID));
        }
        private IEnumerator SetupRoutine(Holdable holdable, string limbGUID)
        {
            while (!hand)
            {
                GameObject h = GameObject.Find(limbGUID);
                if (h)
                {
                    hand = h.GetComponent<LimbConstructor>().Extremity;
                }
                else yield return new WaitForSeconds(checkTime);
            }

            transform.localScale = this.holdable.transform.localScale;
            mr.materials = holdable.GetComponent<MeshRenderer>().materials;
            mf.mesh = holdable.GetComponent<MeshFilter>().mesh;
            f.SetFollow(hand, true);
        }
        #endregion
    }
}