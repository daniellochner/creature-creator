using MoreMountains.NiceVibrations;
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
        protected override void Awake()
        {
            base.Awake();
            round.OnValueChanged += OnRoundChanged;
            remaining.OnValueChanged += OnRemainingChanged;
        }
        private void Start()
        {
            battleInfo.SetActive(InBattle);
            OnRoundChanged(0, round.Value);
            OnRemainingChanged(0, remaining.Value);
        }

        public void TryBattle()
        {
            BattleServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void BattleServerRpc()
        {
            if (!InBattle)
            {
                StartCoroutine(BattleRoutine());
            }
        }
        private IEnumerator BattleRoutine()
        {
            battleInfo.SetActive(true);
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
            battleInfo.SetActive(false);
            victoryAS.Play();
#if USE_STATS
            StatsManager.Instance.UnlockAchievement("ACH_GLADIATOR");
#endif

            MMVibrationManager.Haptic(HapticTypes.Success);
        }
        [ClientRpc]
        private void StartRoundClientRpc()
        {
            battleInfo.SetActive(true);
            bellAS.Play();
        }

        private void OnRoundChanged(int oldRound, int newRound)
        {
            roundText.SetArguments(newRound + 1, rounds.Length);
        }
        private void OnRemainingChanged(int oldRemaining, int newRemaining)
        {
            remainingText.SetArguments(newRemaining);
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