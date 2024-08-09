// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureHealth : PlayerHealth
    {
        #region Fields
        [SerializeField] private PlayerEffects.Sound[] takeDamageSounds;
        [SerializeField] private float damageTime;
        [SerializeField] private Material damageMaterial;
        [SerializeField] private DamageNumber damagePopupPrefab;

        private Coroutine damageCoroutine;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; private set; }
        public CreatureOptimizer Optimizer { get; private set; }
        public PlayerEffects Effects { get; private set; }

        public override float MaxHealth => Constructor.Statistics.Health;

        public override bool CanTakeDamage => base.CanTakeDamage && !CinematicManager.Instance.IsInCinematic && !IsImmune.Value;

        public bool IsTakingDamage => damageCoroutine != null;

        public NetworkVariable<bool> IsImmune { get; set; } = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Optimizer = GetComponent<CreatureOptimizer>();
            Effects = GetComponent<PlayerEffects>();
        }

        protected override void Start()
        {
            base.Start();
            OnTakeDamage += delegate (float damage, DamageReason reason, string inflicter)
            {
                if (IsOwner)
                {
                    Effects.PlaySound(takeDamageSounds);
                }
                damagePopupPrefab.Spawn(Constructor.Body.position, damage);

                if (damageCoroutine == null)
                {
                    damageCoroutine = StartCoroutine(DamageRoutine());
                }
            };
        }

        private IEnumerator DamageRoutine()
        {
            if (Optimizer.IsOptimized)
            {
                Material[] prev = Optimizer.OptimizedCreature.materials;

                Optimizer.OptimizedCreature.materials = GetDamageMaterials(prev.Length);
                yield return new WaitForSeconds(damageTime);
                Optimizer.OptimizedCreature.materials = prev;
            }
            else
            {
                Dictionary<Renderer, Material[]> rm = new Dictionary<Renderer, Material[]>();

                foreach (BodyPartConstructor bpc in Constructor.BodyParts)
                {
                    RecordRenderer(ref rm, bpc.Renderer);
                    RecordRenderer(ref rm, bpc.Flipped.Renderer);
                }
                RecordRenderer(ref rm, Constructor.SkinnedMeshRenderer);

                yield return new WaitForSeconds(damageTime);

                foreach (Renderer renderer in rm.Keys)
                {
                    renderer.materials = rm[renderer];
                }
            }

            damageCoroutine = null;
        }

        private void RecordRenderer(ref Dictionary<Renderer, Material[]> rm, Renderer renderer)
        {
            rm[renderer] = renderer.sharedMaterials;
            renderer.materials = GetDamageMaterials(renderer.materials.Length);
        }
        private Material[] GetDamageMaterials(int size)
        {
            Material[] placeholder = new Material[size];
            for (int i = 0; i < placeholder.Length; ++i)
            {
                placeholder[i] = damageMaterial;
            }
            return placeholder;
        }
        #endregion
    }
}