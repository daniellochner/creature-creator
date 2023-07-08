// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Fire : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private int ticks;
        [SerializeField] private float damageTime;
        [SerializeField] private MinMax minMaxDamage;
        [SerializeField] private AudioClip extinguishFX;

        private Follower follower;
        private ParticleSystem particles;
        private AudioSource source;
        private new Light light;

        private Coroutine burnCoroutine;
        #endregion

        #region Properties
        public CreatureBase BurningCreature { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            follower = GetComponent<Follower>();
            particles = GetComponent<ParticleSystem>();
            source = GetComponent<AudioSource>();
            light = GetComponent<Light>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (IsServer)
            {
                if (other.CompareTag("Water"))
                {
                    StopCoroutine(burnCoroutine);
                    ExtinguishClientRpc();
                }
            }
        }

        public void Burn(CreatureBase creature)
        {
            BurningCreature = creature;

            burnCoroutine = StartCoroutine(BurnRoutine());
        }

        private IEnumerator BurnRoutine()
        {
            BurningCreature.Burner.IsBurning.Value = true;

            follower.SetFollow(BurningCreature.Constructor.Body);

            for (int i = 0; i < ticks; i++)
            {
                BurningCreature.Health.TakeDamage(minMaxDamage.Random, DamageReason.Fire);
                yield return new WaitForSeconds(damageTime);

                if (BurningCreature.Health.IsDead)
                {
                    break;
                }
            }

            ExtinguishClientRpc();
        }

        [ClientRpc]
        private void ExtinguishClientRpc()
        {
            source.PlayOneShot(extinguishFX);

            particles.Stop();
            StartCoroutine(source.FadeRoutine(1f, 0f));
            StartCoroutine(light.FadeRoutine(1f, 0f));

            if (IsServer)
            {
                if (BurningCreature && !BurningCreature.Health.IsDead)
                {
                    BurningCreature.Burner.IsBurning.Value = false;
                }

                this.Invoke(() => NetworkObject.Despawn(true), 5f);
            }
        }
        #endregion
    }
}