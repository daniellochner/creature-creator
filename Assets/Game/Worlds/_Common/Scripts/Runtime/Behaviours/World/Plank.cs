// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Plank : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private float breakProbability;
        [SerializeField] private float weightThreshold;
        [SerializeField] private bool autoFix;
        [SerializeField, DrawIf("autoFix", true)] private float autoFixTime;
        [SerializeField] private GameObject breakFX;

        private MeshRenderer meshRenderer;
        private MeshCollider meshCollider;
        private AudioSource creakAudioSource;
        #endregion

        #region Properties
        public NetworkVariable<bool> IsBroken { get; set; } = new NetworkVariable<bool>(false);
        #endregion
        
        #region Methods
        private void Awake()
        {
            creakAudioSource = GetComponentInParent<AudioSource>();
            meshCollider = GetComponent<MeshCollider>();
            meshRenderer = GetComponent<MeshRenderer>();
        }
        private void Start()
        {
            if (IsBroken.Value)
            {
                meshCollider.enabled = meshRenderer.enabled = false;
            }
        }
        private void OnCollisionEnter(Collision other)
        {
            CreatureBase creature = other.gameObject.GetComponent<CreatureBase>();
            if (creature != null)
            {
                TryBreak(creature);
            }
        }
        
        public void TryBreak(CreatureBase creature)
        {
            if (creature.Constructor.Statistics.Weight > weightThreshold)
            {
                if (Random.Range(0f, 1f) < breakProbability)
                {
                    BreakServerRpc();
                }
                else
                {
                    CreakServerRpc();
                }
            }
        }
        [ServerRpc]
        private void BreakServerRpc()
        {
            StartCoroutine(BreakRoutine());
        }
        [ClientRpc]
        private void BreakClientRpc()
        {
            meshCollider.enabled = meshRenderer.enabled = false;
            Instantiate(breakFX, transform.position, transform.rotation);
        }
        [ClientRpc]
        private void FixClientRpc()
        {
            meshCollider.enabled = meshRenderer.enabled = true;
        }
        [ServerRpc]
        private void CreakServerRpc()
        {
            CreakClientRpc();
        }
        [ClientRpc]
        private void CreakClientRpc()
        {
            creakAudioSource.pitch = Random.Range(0.75f, 1.25f);
            creakAudioSource.Play();
        }

        private IEnumerator BreakRoutine()
        {
            IsBroken.Value = true;

            BreakClientRpc();
            if (autoFix)
            {
                yield return new WaitForSeconds(autoFixTime);
                FixClientRpc();
                IsBroken.Value = false;
            }
        }
        #endregion
    }
}