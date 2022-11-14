using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BattleManager : NetworkSingleton<BattleManager>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private TextMeshProUGUI remainingText;
        [SerializeField] private GameObject battleInfo;
        [SerializeField] private AudioSource bellAS;
        [SerializeField] private AudioSource victoryAS;
        [SerializeField] private Transform[] rounds;

        private NetworkVariable<int> round = new NetworkVariable<int>(-1);
        private NetworkVariable<int> remaining = new NetworkVariable<int>(-1);
        #endregion

        #region Properties
        public bool InBattle => round.Value >= 0 && round.Value < rounds.Length;
        #endregion

        #region Methods
        private void Start()
        {
            if (IsClient)
            {
                round.OnValueChanged += OnRoundChanged;
                remaining.OnValueChanged += OnRemainingChanged;

                if (InBattle)
                {
                    OnRoundChanged(default, round.Value);
                    OnRemainingChanged(default, remaining.Value);
                }
            }
        }

        public void TryBattle()
        {
            BattleServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void BattleServerRpc()
        {
            StartCoroutine(BattleRoutine());
        }
        private IEnumerator BattleRoutine()
        {
            round.Value = 0;

            for (int i = 0; i < rounds.Length; ++i)
            {
                StartRoundClientRpc();

                List<CreatureNonPlayer> spawned = SpawnEnemies(i);
                yield return new WaitUntil(() => 
                {
                    int r = 0;
                    foreach (CreatureNonPlayer npc in spawned)
                    {
                        if (!npc.Health.IsDead)
                        {
                            r++;
                        }
                    }
                    remaining.Value = r;
                    return (r == 0);
                });

                yield return new WaitForSeconds(1f);

                round.Value++;
            }

            WinClientRpc();
        }

        [ClientRpc]
        private void WinClientRpc()
        {
            victoryAS.Play();
#if USE_STATS
            StatsManager.Instance.SetAchievement("ACH_GLADIATOR");
#endif
        }
        [ClientRpc]
        private void StartRoundClientRpc()
        {
            bellAS.Play();
        }

        private void OnRoundChanged(int oldRound, int newRound)
        {
            roundText.text = $"Round: {newRound + 1}/{rounds.Length}";
            battleInfo.SetActive(InBattle);
        }
        private void OnRemainingChanged(int oldRemaining, int newRemaining)
        {
            remainingText.text = $"{newRemaining} remaining";
        }

        private List<CreatureNonPlayer> SpawnEnemies(int round)
        {
            List<CreatureNonPlayer> spawned = new List<CreatureNonPlayer>();
            foreach (AnimalSpawner spawner in rounds[round].GetComponentsInChildren<AnimalSpawner>())
            {
                spawner.Spawn();

                spawner.SpawnedNPC.GetComponent<AnimalAI>().PVE = true;

                spawned.Add(spawner.SpawnedNPC.GetComponent<CreatureNonPlayer>());
            }
            return spawned;
        }
        #endregion
    }
}