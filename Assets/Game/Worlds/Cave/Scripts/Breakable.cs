using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Breakable : CreatureInteractable
    {
        #region Fields
        [SerializeField] private int hitsToBreak;
        [SerializeField] private float mineCooldown;
        [SerializeField] private GameObject mineFX;
        [SerializeField] private GameObject breakFX;
        [SerializeField] private UnityEvent onBreak;

        private NetworkVariable<int> hits = new NetworkVariable<int>();
        private float mineTimeLeft;
        #endregion

        #region Methods
        private void Start()
        {
            if (hits.Value >= hitsToBreak)
            {
                if (IsServer)
                {
                    NetworkObject.Despawn();
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
        private void Update()
        {
            if (IsServer)
            {
                mineTimeLeft -= Time.deltaTime;
            }
        }
        public override void OnNetworkDespawn()
        {
            gameObject.SetActive(false);
            base.OnNetworkDespawn();
        }

        [ServerRpc(RequireOwnership = false)]
        public void TryMineServerRpc(NetworkObjectReference netObj)
        {
            if (mineTimeLeft < 0 && netObj.TryGet(out NetworkObject obj))
            {
                List<Holdable> held = new List<Holdable>(obj.GetComponent<CreatureBase>().Holder.Held.Values);
                if (held.Find(x => x is Pickaxe))
                {
                    hits.Value++;

                    if (hits.Value < hitsToBreak)
                    {
                        MineClientRpc();
                    }
                    else
                    {
                        BreakClientRpc();
                    }

                    mineTimeLeft = mineCooldown;
                }
            }
        }
        [ClientRpc]
        private void MineClientRpc()
        {
            List<Transform> points = new List<Transform>(GetComponentsInChildren<Transform>());
            if (points.Count > 1)
            {
                points.Remove(transform);
            }
            Instantiate(mineFX, points[Random.Range(0, points.Count)].position, Quaternion.identity, Dynamic.Transform);
        }
        [ClientRpc]
        private void BreakClientRpc()
        {
            Instantiate(breakFX, transform.position, Quaternion.identity, Dynamic.Transform);

            if (IsServer)
            {
                onBreak.Invoke();
            }
            gameObject.SetActive(false);
        }

        public override bool CanHighlight(Interactor interactor)
        {
            return base.CanHighlight(interactor) && Player.Instance.Holder.IsHolding;
        }
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            TryMineServerRpc(interactor.NetworkObject);
        }
        #endregion
    }
}