using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TopKills : IndividualMinigame
    {
        #region Fields
        [Header("Top Kills")]
        [SerializeField] protected AnimalSpawner[] enemySpawners;
        [SerializeField] protected Transform[] enemySpawnPoints;
        [SerializeField] protected MinMax minMaxEnemies;
        [SerializeField] protected TrackRegion region;

        private List<AnimalLocal> spawned = new List<AnimalLocal>();
        #endregion

        #region Methods
        protected override void Setup()
        {
            base.Setup();

            playing.onEnter += OnPlayingEnter;

            completing.onEnter += OnCompletingEnter;
        }

        #region Playing
        private void OnPlayingEnter()
        {
            SetupPlayerCorpses(false);
            StartCoroutine(SpawnRoutine());
        }
        private void OnPlayingExit()
        {
            SetupPlayerCorpses(true);
        }

        private IEnumerator SpawnRoutine()
        {
            yield return new WaitUntil(() => PlayTimeLeft.Value != -1);

            while (State.Value == MinigameStateType.Playing)
            {
                float p = (1f - (float)PlayTimeLeft.Value / (float)playTime);
                int count = (int)Mathf.Lerp(minMaxEnemies.min, minMaxEnemies.max, p);

                int diff = count - spawned.Count;
                for (int i = 0; i < diff; i++)
                {
                    spawned.Add(SpawnEnemy());
                }

                yield return null;

                for (int i = 0; i < count; i++)
                {
                    AnimalLocal enemy = spawned[i];
                    if (enemy.Health.IsDead)
                    {
                        spawned.RemoveAt(i);
                        spawned.Insert(i, SpawnEnemy());
                    }
                }

                yield return null;
            }
        }

        protected virtual AnimalLocal SpawnEnemy()
        {
            AnimalSpawner spawner = enemySpawners[Random.Range(0, enemySpawners.Length)];
            spawner.transform.position = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].position;
            spawner.Spawn();

            AnimalAI animalAI = spawner.SpawnedNPC.GetComponent<AnimalAI>();
            animalAI.PVE = true;
            animalAI.Region = region;
            animalAI.Creature.Health.OnDie += OnDie;
            SetupCorpse(animalAI.Creature, false);

            return animalAI.Creature as AnimalLocal;
        }

        private void OnDie(DamageReason reason, string inflicter)
        {
            if (inflicter != null)
            {
                for (int i = 0; i < Scoreboard.Count; i++)
                {
                    Score score = Scoreboard[i];
                    if (score.id == inflicter)
                    {
                        Scoreboard[i] = new Score(score, score.score + 1);
                        return;
                    }
                }
            }
        }

        private void SetupPlayerCorpses(bool isActive)
        {
            foreach (ulong clientId in players)
            {
                SetupCorpse(NetworkManager.SpawnManager.GetPlayerNetworkObject(clientId).GetComponent<CreatureBase>(), isActive);
            }
        }
        private void SetupCorpse(CreatureBase creature, bool isActive)
        {
            creature.Corpse.GenerateRagdoll.Value = isActive;
        }
        #endregion

        #region Completing
        private void OnCompletingEnter()
        {
            foreach (AnimalLocal enemy in spawned)
            {
                enemy.Health.TakeDamage(enemy.Health.Health, DamageReason.Misc);
            }
            spawned.Clear();
        }
        #endregion
        #endregion
    }
}