// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Pinwheel.Poseidon;
using Pinwheel.Poseidon.FX;
using UnityEngine;

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
        [SerializeField] private GameObject lod;

        private PWater water;
        #endregion

        #region Properties
        public BoxCollider Collider { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Collider = GetComponent<BoxCollider>();
            water = GetComponent<PWater>();
        }
        private void Update()
        {
            water.ManualTimeSeconds += Time.deltaTime;
        }

        public void OnTriggerEnter(Collider other)
        {
            CreatureBase creature = other.GetComponent<CreatureBase>();
            if (creature != null)
            {
                Instantiate(splashPrefab, creature.transform.position, Quaternion.identity);

                CreaturePlayerLocal player = creature as CreaturePlayerLocal;
                if (player != null)
                {
                    if (!player.Underwater.IsOnRaft && (!player.Abilities.Abilities.Contains(swimAbility) || !allowSwimming))
                    {
                        player.Health.TakeDamage(player.Health.Health);
                    }
                    else
                    {
#if USE_STATS
                        StatsManager.Instance.UnlockAchievement("ACH_MAKE_A_SPLASH");
#endif
                    }
                    if (waterFX != null)
                    {
                        waterFX.enabled = true;
                    }
                }
            }
        }

        public void SetVisibility(bool isVisible)
        {
            if (lod != null && !CinematicManager.Instance.IsInCinematic && !(EditorManager.Instance.IsBuilding || EditorManager.Instance.IsPainting) && !Player.Instance.Health.IsDead)
            {
                water.enabled = isVisible;
                lod.SetActive(!isVisible);
            }
        }
        #endregion
    }
}