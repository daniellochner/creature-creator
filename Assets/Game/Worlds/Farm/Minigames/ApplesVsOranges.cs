using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ApplesVsOranges : TeamMinigame
    {
        #region Fields
        [Header("Apples Vs Oranges")]
        [SerializeField] private TrackRegion[] teamRegions;
        [SerializeField] private FoodCrateSpawner[] foodSpawners;
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

            starting.onEnter += OnStartingEnter;
        }

        #region Starting
        private void OnStartingEnter()
        {
            foreach (FoodCrateSpawner foodSpawner in foodSpawners)
            {
                foodSpawner.Reset();
            }
        }
        #endregion
        #endregion

    }
}