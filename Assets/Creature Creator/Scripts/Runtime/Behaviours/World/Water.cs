// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Water : MonoBehaviour
    {
        [SerializeField] private bool allowSwimming;
        [Space]
        [SerializeField] private Ability swimAbility;
        [SerializeField] private GameObject splashPrefab;

        private void OnTriggerEnter(Collider other)
        {
            CreatureBase creature = other.GetComponent<CreatureBase>();
            if (creature != null)
            {
                Instantiate(splashPrefab, creature.Constructor.Body.position, Quaternion.identity);

                CreaturePlayerLocal player = creature as CreaturePlayerLocal;
                if (player != null && (!player.Abilities.Abilities.Contains(swimAbility) || !allowSwimming))
                {
                    player.Health.Die();
                }
            }
        }
    }
}