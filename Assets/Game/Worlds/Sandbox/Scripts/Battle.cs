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

        private NetworkVariable<bool> complete = new NetworkVariable<bool>(false);
        private NetworkVariable<int> round = new NetworkVariable<int>(-1);
        private NetworkVariable<int> remaining = new NetworkVariable<int>(-1);

        private List<AnimalLocal> spawned = new List<AnimalLocal>();
        #endregion

        #region Properties
        public TrackRegion Region => region;

        public List<Collider> Players => Region.tracked;

        public bool InBattle => round.Value >= 0 && round.Value < rounds.childCount;
        #endregion

        #region Methods
        private void Awake()
        {
            round.OnValueChanged += OnRoundChanged;
            remaining.OnValueChanged += OnRemainingChanged;
        }
        private void Start()
        {
            if (!WorldManager.Instance.World.EnablePVE || complete.Value)
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
            StartCoroutine(BattleRoutine());
        }      
        [ClientRpc]
        private void StartRoundClientRpc()
        {
            bellAS.Play();
        }
        [ClientRpc]
        private void WinClientRpc()
        {
            victoryAS.Play();
            MMVibrationManager.Haptic(HapticTypes.Success);

#if USE_STATS
            StatsManager.Instance.CompletedBattles++;
#endif

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
                animalAI.Battle = this;

                spawned.Add(spawner.SpawnedNPC.GetComponent<AnimalLocal>());
            }
        }
        private void HideBattle()
        {
            foreach (Transform t in transform) { t.gameObject.SetActive(false); }
        }

        private void OnPlayerEnter(Collider col)
        {
            if (col.CompareTag("Player/Local"))
            {
                if (!InBattle)
                {
                    BattleServerRpc();
                }
                info.SetActive(true);
            }
        }
        private void OnPlayerExit(Collider col)
        {
            if (col.CompareTag("Player/Local"))
            {
                info.SetActive(false);
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