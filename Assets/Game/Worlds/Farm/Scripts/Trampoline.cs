// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Trampoline : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private float bounceForce;
        [SerializeField] private float bounceTime;
        [SerializeField] private float minRelVelocity;

        private AudioSource bounceAudioSource;
        private SkinnedMeshRenderer skinnedMeshRenderer;

        private Coroutine bounceCoroutine;
        #endregion

        #region Methods
        private void Awake()
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            bounceAudioSource = GetComponentInParent<AudioSource>();
        }
        private void OnCollisionEnter(Collision other)
        {
            CreatureBase creature = other.collider.GetComponent<CreatureBase>();
            if (creature != null && creature.IsOwner && other.relativeVelocity.magnitude > minRelVelocity)
            {
                creature.Rigidbody.AddForce(transform.forward * bounceForce, ForceMode.Impulse);
                BounceServerRpc();
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void BounceServerRpc()
        {
            BounceClientRpc();
        }
        [ClientRpc]
        private void BounceClientRpc()
        {
            bounceAudioSource.pitch = Random.Range(0.75f, 1.25f);
            bounceAudioSource.Play();

            if (bounceCoroutine != null)
            {
                StopCoroutine(bounceCoroutine);
            }
            bounceCoroutine = this.InvokeOverTime(delegate (float p)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, 100f * Mathf.Cos(Mathf.PI * (p - 0.5f)));
            }, 
            bounceTime);
        }
        #endregion
    }
}