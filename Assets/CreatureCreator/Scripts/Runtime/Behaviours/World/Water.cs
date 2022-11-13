// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Water : NetworkBehaviour
    {
        [SerializeField] private bool allowSwimming;
        [Space]
        [SerializeField] private Ability swimAbility;
        [SerializeField] private GameObject splashPrefab;

        public BoxCollider Collider { get; private set; }

        private void Awake()
        {
            Collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            CreatureBase creature = other.GetComponent<CreatureBase>();
            if (creature != null)
            {
                Instantiate(splashPrefab, creature.transform.position, Quaternion.identity);

                CreaturePlayerLocal player = creature as CreaturePlayerLocal;
                if (player != null)
                {
                    if (!player.Abilities.Abilities.Contains(swimAbility) || !allowSwimming)
                    {
                        player.Health.TakeDamage(player.Health.Health);
                    }
                    else
                    {
#if USE_STATS
                        StatsManager.Instance.SetAchievement("ACH_PHELPS");
#endif
                    }
                }
            }
        }
    }
}