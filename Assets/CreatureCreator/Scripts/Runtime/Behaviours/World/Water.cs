// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Pinwheel.Poseidon.FX;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Water : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool allowSwimming;
        [SerializeField] private PWaterFX waterFX;
        [Space]
        [SerializeField] private Ability swimAbility;
        [SerializeField] private GameObject splashPrefab;
        [SerializeField] private UnityEvent<CreaturePlayerLocal> onPlayerEnter;
        #endregion

        #region Properties
        public BoxCollider Collider { get; private set; }
        #endregion

        #region Methods
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
                        bool isOnRaft = Physics.Raycast(transform.position + transform.up * player.Grounded.ContactDistance, -transform.up, 2f * player.Grounded.ContactDistance);
                        if (!isOnRaft)
                        {
                            player.Health.TakeDamage(player.Health.Health);
                        }
                    }
                    else
                    {
#if USE_STATS
                        StatsManager.Instance.UnlockAchievement("ACH_MAKE_A_SPLASH");
#endif
                        onPlayerEnter.Invoke(player);
                    }
                    if (waterFX != null)
                    {
                        waterFX.enabled = true;
                    }
                }
            }
        }
        #endregion
    }
}