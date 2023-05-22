using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BattleManager : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private TextMeshProUGUI remainingText;
        [SerializeField] private Transform rounds;
        [SerializeField] private AudioSource bellAS;
        [SerializeField] private AudioSource victoryAS;

        private NetworkVariable<bool> complete = new NetworkVariable<bool>(false);
        private NetworkVariable<int> round = new NetworkVariable<int>(-1);
        private NetworkVariable<int> remaining = new NetworkVariable<int>(-1);
        #endregion

        #region Properties
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
            if (WorldManager.Instance.World.EnablePVE || complete.Value)
            {
                HideBattle();
            }
            else
            {
                OnRoundChanged(0, round.Value);
                OnRemainingChanged(0, remaining.Value);
            }
        }

        public void TryBattle()
        {
            BattleServerRpc();
        }

        private IEnumerator BattleRoutine()
        {
            round.Value = 0;

            for (int i = 0; i < rounds.childCount; ++i)
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
            complete.Value = true;

            WinClientRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void BattleServerRpc()
        {
            if (!InBattle) StartCoroutine(BattleRoutine());
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

        private List<CreatureNonPlayer> SpawnEnemies(int round)
        {
            List<CreatureNonPlayer> spawned = new List<CreatureNonPlayer>();
            foreach (AnimalSpawner spawner in rounds.GetChild(round).GetComponentsInChildren<AnimalSpawner>())
            {
                spawner.Spawn();

                spawner.SpawnedNPC.GetComponent<AnimalAI>().PVE = true;

                spawned.Add(spawner.SpawnedNPC.GetComponent<CreatureNonPlayer>());
            }
            return spawned;
        }
        private void HideBattle()
        {
            foreach (Transform t in transform) { t.gameObject.SetActive(false); }
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