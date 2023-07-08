// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Follower))]
    public class Emission : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private float tickCooldown;

        private Follower follower;
        private ParticleSystem[] particles;

        private bool hasStoppedEmitting;
        private float tickTimeLeft;
        #endregion

        #region Properties
        public BodyPartEmitter Emitter { get; set; }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            follower = GetComponent<Follower>();
            particles = GetComponentsInChildren<ParticleSystem>();

            tickTimeLeft = tickCooldown;
        }
        private void Update()
        {
            if (IsServer && !hasStoppedEmitting)
            {
                TimerUtility.OnTimer(ref tickTimeLeft, tickCooldown, Time.deltaTime, OnTick);
            }
        }

        public virtual void Emit(BodyPartEmitter emitter, float duration)
        {
            Emitter = emitter;
            Emitter.CreatureEmitter.Emitting.Add(emitter.name, this);

            follower.SetFollow(emitter.SpawnPoint);

            Physics.IgnoreCollision(Emitter.CreatureEmitter.GetComponent<Collider>(), GetComponent<Collider>());

            this.Invoke(StopEmitting, duration);
        }

        public void StopEmitting()
        {
            StopEmittingClientRpc();
        }
        [ClientRpc]
        private void StopEmittingClientRpc()
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }

            if (IsServer && !hasStoppedEmitting)
            {
                Emitter.CreatureEmitter.Emitting.Remove(Emitter.name);
                hasStoppedEmitting = true;

                this.Invoke(() => NetworkObject.Despawn(true), 5f);
            }
        }

        public virtual void OnTick()
        {
        }
        #endregion
    }
}