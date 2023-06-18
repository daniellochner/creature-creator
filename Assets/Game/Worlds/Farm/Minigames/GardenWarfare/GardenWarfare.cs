using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class GardenWarfare : TeamMinigame
    {
        #region Fields
        [Header("Apples Vs Oranges")]
        [SerializeField] private TrackRegion[] teamRegions;
        [SerializeField] private FoodSpawner[] foodSpawners;
        #endregion

        #region Methods
        protected override void Start()
        {
            base.Start();

            if (IsServer)
            {
                for (int i = 0; i < teamRegions.Length; i++)
                {
                    int index = i;
                    teamRegions[i].OnTrack = teamRegions[i].OnLoseTrackOf = delegate
                    {
                        SetTeamScore(index, teamRegions[index].tracked.Count);
                    };
                }
            }
        }

        protected override void Setup()
        {
            base.Setup();

            introducing.onEnter += OnIntroducingEnter;
        }

        #region Starting
        private void OnIntroducingEnter()
        {
            foreach (FoodSpawner foodSpawner in foodSpawners)
            {
                foodSpawner.Despawn();
            }
        }
        #endregion
        #endregion

    }
}