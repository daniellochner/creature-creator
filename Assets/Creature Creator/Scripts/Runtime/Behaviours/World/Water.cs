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
            CreatureSourcePlayer player = other.GetComponent<CreatureSourcePlayer>();
            if (player != null)
            {
                if (!player.Abilities.Abilities.Contains(swimAbility) || !allowSwimming)
                {
                    player.Health.Die();
                }
                Instantiate(splashPrefab, player.Constructor.Body.position, Quaternion.identity);
            }
        }
    }
}