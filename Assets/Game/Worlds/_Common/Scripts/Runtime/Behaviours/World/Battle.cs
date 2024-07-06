using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Battle : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private TextMeshProUGUI remainingText;
        [SerializeField] private Transform rounds;
        [SerializeField] private AudioSource bellAS;
        [SerializeField] private AudioSource victoryAS;
        [SerializeField] private TrackRegion region;
        [SerializeField] private Bounds bounds;
        [SerializeField] private GameObject info;
        [SerializeField] private string musicId;
        [SerializeField] private int experience;

        private NetworkVariable<bool> complete = new NetworkVariable<bool>(false);
        private NetworkVariable<int> round = new NetworkVariable<int>(-1);
        private NetworkVariable<int> remaining = new NetworkVariable<int>(-1);

        private List<AnimalLocal> spawned = new List<AnimalLocal>();
        #endregion

        #region Properties
        public TrackRegion Region => region;

        public bool InBattle => region.tracked.Contains(Player.Instance.Collider.Hitbox);

        public bool HasStarted => round.Value >= 0 && round.Value < rounds.childCount;
        public bool IsComplete => complete.Value;
        #endregion

        #region Methods
        private void Awake()
        {
            round.OnValueChanged += OnRoundChanged;
            remaining.OnValueChanged += OnRemainingChanged;
        }
        private void Start()
        {
            if (!WorldManager.Instance.World.SpawnNPC || !WorldManager.Instance.World.EnablePVE || complete.Value)
            {
                HideBattle();
            }
            else
            {
                region.OnTrack += OnPlayerEnter;
                region.OnLoseTrackOf += OnPlayerExit;

                OnRoundChanged(0, round.Value);
                OnRemainingChanged(0, remaining.Value);
            }
        }

        private IEnumerator BattleRoutine()
        {
            round.Value = 0;

            for (int i = 0; i < rounds.childCount; ++i)
            {
                StartRoundClientRpc();

                SpawnEnemies(i);
                yield return new WaitUntil(() =>
                {
                    int r = 0;
                    foreach (AnimalLocal npc in spawned)
                    {
                        if (!npc.Health.IsDead)
                        {
                            r++;
                        }
                    }
                    remaining.Value = r;
                    return (r == 0);
                });
                spawned.Clear();

                yield return new WaitForSeconds(1f);

                round.Value++;
            }
            complete.Value = true;

            WinClientRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void BattleServerRpc()
        {
            if (!HasStarted)
            {
                StartCoroutine(BattleRoutine());
            }
        }      
        [ClientRpc]
        private void StartRoundClientRpc()
        {
            if (InBattle)
            {
                info.SetActive(true);
                bellAS.Play();
            }
        }
        [ClientRpc]
        private void WinClientRpc()
        {
            if (InBattle)
            {
                MusicManager.Instance.FadeTo(SettingsManager.Data.InGameMusicId);

                ProgressManager.Data.Experience += experience;
                ProgressManager.Instance.Save();

                StatsManager.Instance.ExperienceEarned += experience;

                NotificationsManager.Notify(LocalizationUtility.Localize("experience-earned", experience));

                victoryAS.Play();
                MMVibrationManager.Haptic(HapticTypes.Success);

                StatsManager.Instance.CompletedBattles++;
            }
            HideBattle();
        }

        private void SpawnEnemies(int round)
        {
            foreach (AnimalSpawner spawner in rounds.GetChild(round).GetComponentsInChildren<AnimalSpawner>())
            {
                spawner.wanderBounds = bounds;
                spawner.Spawn();

                AnimalAI animalAI = spawner.SpawnedNPC.GetComponent<AnimalAI>();
                animalAI.PVE = true;
                animalAI.Region = region;

                spawned.Add(spawner.SpawnedNPC.GetComponent<AnimalLocal>());
            }
        }
        private void HideBattle()
        {
            foreach (Transform t in transform) { t.gameObject.SetActive(false); }
        }

        private void OnPlayerEnter(Collider col)
        {
            if (col.CompareTag("Player/Local") && rounds.childCount > 0)
            {
                if (HasStarted)
                {
                    info.SetActive(true);
                }
                else
                {
                    BattleServerRpc();
                }

                MusicManager.Instance.FadeTo(musicId);
            }
        }
        private void OnPlayerExit(Collider col)
        {
            if (col.CompareTag("Player/Local"))
            {
                info.SetActive(false);
                MusicManager.Instance.FadeTo(SettingsManager.Data.InGameMusicId);
            }
        }
        private void OnRoundChanged(int oldRound, int newRound)
        {
            roundText.SetArguments(newRound + 1, rounds.childCount);
        }
        private void OnRemainingChanged(int oldRemaining, int newRemaining)
        {
            remainingText.SetArguments(newRemaining);
        }
        #endregion
    }
}