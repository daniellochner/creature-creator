using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CoalDeposit : CreatureInteractable
    {
        #region Fields
        [SerializeField] private QuestItem coal;
        [SerializeField] private int hitsToBreak;
        [SerializeField] private float mineCooldown;
        [SerializeField] private Transform[] coalT;

        [SerializeField] private AudioClip mineSFX;
        [SerializeField] private AudioClip breakSFX;

        [SerializeField] private GameObject breakFX;
        [SerializeField] private GameObject mineFX;

        private NetworkVariable<int> hits = new NetworkVariable<int>();

        private AudioSource audioSource;
        private Animator animator;

        private float mineTimeLeft;
        #endregion


        #region Methods
        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (WorldManager.Instance.World.CreativeMode || hits.Value >= hitsToBreak)
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
            audioSource.PlayOneShot(mineSFX);
            animator.SetTrigger("Wobble");

            Instantiate(mineFX, coalT[Random.Range(0, coalT.Length)].position, Quaternion.identity, Dynamic.Transform);
        }

        [ClientRpc]
        private void BreakClientRpc()
        {
            audioSource.PlayOneShot(breakSFX);

            Instantiate(breakFX, transform.position, Quaternion.identity, Dynamic.Transform);

            gameObject.SetActive(false);
            coal.gameObject.SetActive(true);
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