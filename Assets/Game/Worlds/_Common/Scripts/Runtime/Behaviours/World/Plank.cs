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

        private AudioSource creakAudioSource;
        #endregion

        #region Properties
        public NetworkVariable<bool> IsBroken { get; set; } = new NetworkVariable<bool>(false);
        #endregion
        
        #region Methods
        private void Awake()
        {
            creakAudioSource = GetComponentInParent<AudioSource>();
        }
        private void Start()
        {
            if (IsBroken.Value)
            {
                SetVisibility(false);
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
            SetVisibility(false);
            Instantiate(breakFX, transform.position, transform.rotation);
        }
        [ClientRpc]
        private void FixClientRpc()
        {
            SetVisibility(true);
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
        private void SetVisibility(bool isVisible)
        {
            foreach (Collider c in GetComponentsInChildren<Collider>(true))
            {
                c.enabled = isVisible;
            }
            foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
            {
                r.enabled = isVisible;
            }
        }
        #endregion
    }
}