// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureHealth : PlayerHealth
    {
        [SerializeField] private PlayerEffects.Sound[] takeDamageSounds;
        [SerializeField] private float damageTime;
        [SerializeField] private Material damageMaterial;
        private Rigidbody rb;

        private Coroutine damageCoroutine;

        public CreatureConstructor Constructor { get; private set; }
        public PlayerEffects Effects { get; private set; }

        public override float MaxHealth => Constructor.Statistics.Health;

        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Effects = GetComponent<PlayerEffects>();

            rb = GetComponent<Rigidbody>();
        }

        protected override void Start()
        {
            base.Start();
            OnTakeDamage += delegate (float damage, Vector3 force)
            {
                if (IsOwner) // only one source should play a sound
                {
                    Effects.PlaySound(takeDamageSounds);
                }
                if (damageCoroutine == null)
                {
                    damageCoroutine = StartCoroutine(DamageRoutine());
                }
            };
        }

        private IEnumerator DamageRoutine()
        {
            Dictionary<Renderer, Material[]> rm = new Dictionary<Renderer, Material[]>();

            foreach (Renderer renderer in Constructor.Body.GetComponentsInChildren<Renderer>(true))
            {
                rm[renderer] = renderer.sharedMaterials;

                Material[] placeholder = new Material[renderer.materials.Length];
                for (int i = 0; i < placeholder.Length; ++i)
                {
                    placeholder[i] = damageMaterial;
                }
                renderer.materials = placeholder;
            }

            yield return new WaitForSeconds(damageTime);

            foreach (Renderer renderer in rm.Keys)
            {
                renderer.materials = rm[renderer];
            }

            damageCoroutine = null;
        }
    }
}