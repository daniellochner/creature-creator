using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TeamMinigame : Minigame
    {
        #region Fields
        [Header("Team Minigame")]
        [SerializeField] protected TeamData[] teams;

        protected List<List<ulong>> teamPlayers = new List<List<ulong>>();
        protected int myTeamId, myTeamIndex;
        #endregion

        #region Methods
        protected override void Setup()
        {
            base.Setup();

            waitingForPlayers.onExit += OnWaitingForPlayersExit;
        }

        #region Waiting For Players
        private void OnWaitingForPlayersExit()
        {
            AssignTeams();
        }

        private void AssignTeams()
        {
            // Randomize ordering of players
            players.Shuffle();

            // Assign players evenly to teams
            int counter = 0;
            for (int t = 0; t < teams.Length; t++)
            {
                teamPlayers.Add(new List<ulong>());

                for (int i = 0; i < players.Count / teams.Length; i++, counter++)
                {
                    AssignTeam(players[counter], t, i);
                }
            }

            // Assign remaining players to random teams
            int playersLeft = players.Count - counter;
            if (playersLeft > 0)
            {
                int[] randomTeamIds = new int[teams.Length];
                for (int i = 0; i < randomTeamIds.Length; i++) { randomTeamIds[i] = i; }
                randomTeamIds.Shuffle();

                for (int i = 0; i < playersLeft; i++, counter++)
                {
                    int teamId = randomTeamIds[i];
                    AssignTeam(players[counter], teamId, teamPlayers[teamId].Count);
                }
            }
        }
        private void AssignTeam(ulong clientId, int teamId, int teamIndex)
        {
            teamPlayers[teamId].Add(clientId);

            AssignTeamClientRpc(teamId, teamIndex, NetworkUtils.SendTo(clientId));
        }
        [ClientRpc]
        private void AssignTeamClientRpc(int teamId, int teamIndex, ClientRpcParams sendTo)
        {
            myTeamId = teamId;
            myTeamIndex = teamIndex;

            OnTeamAssigned(teamId, teamIndex);
        }

        protected virtual void OnTeamAssigned(int teamId, int teamIndex)
        {
            creaturePreset = teams[teamId].creaturePreset;
        }
        #endregion

        #region Building
        protected override void OnApplyRestrictions()
        {
            base.OnApplyRestrictions();

            EditorManager.Instance.SetRestrictedColour(teams[myTeamId].colour);
        }
        #endregion

        #region Starting
        public override void OnSetupScoreboard()
        {
            foreach (TeamData teamData in teams)
            {
                Scoreboard.Add(new Score()
                {
                    id = teamData.name,
                    score = 0,
                    displayName = teamData.name
                });
            }
        }

        public override Transform GetSpawnPoint()
        {
            return teams[myTeamId].spawnPoints[myTeamIndex];
        }
        #endregion

        #region Playing
        protected void SetTeamScore(int teamId, int score)
        {
            if ((teamId < 0) || (teamId > teams.Length - 1) || (teamId > Scoreboard.Count - 1)) return;

            TeamData data = teams[teamId];
            Scoreboard[teamId] = new Score()
            {
                id = data.name,
                displayName = data.name,
                score = score
            };
        }
        #endregion

        #region Completing
        protected override List<ulong> GetWinnerClientIds()
        {
            int winningTeam = GetWinningTeam();
            if (winningTeam != -1)
            {
                return teamPlayers[winningTeam];
            }
            else
            {
                return new List<ulong>();
            }
        }
        protected override string GetWinnerName()
        {
            int winningTeam = GetWinningTeam();
            if (winningTeam != -1)
            {
                return teams[winningTeam].name;
            }
            else
            {
                return LocalizationUtility.Localize("minigame_nobody");
            }
        }

        private int GetWinningTeam()
        {
            int targetScore = isAscendingOrder ? int.MaxValue : int.MinValue;
            int winningTeam = -1;

            for (int i = 0; i < Scoreboard.Count; i++)
            {
                Score score = Scoreboard[i];
                if ((!isAscendingOrder && score.score >= targetScore) || (isAscendingOrder && score.score <= targetScore))
                {
                    if (score.score == targetScore)
                    {
                        winningTeam = -1;
                    }
                    else
                    {
                        winningTeam = i;
                        targetScore = score.score;
                    }
                }
            }

            return winningTeam;
        }

        protected override void OnServerShutdown()
        {
            teamPlayers.Clear();

            base.OnServerShutdown();
        }
        protected override void OnClientShutdown()
        {
            if (InMinigame)
            {
                myTeamId = myTeamIndex = 0;
            }

            base.OnClientShutdown();
        }

        protected override void OnClientDisconnectCallback(ulong clientId)
        {
            base.OnClientDisconnectCallback(clientId);

            foreach (var teamPlayerIds in teamPlayers)
            {
                for (int i = 0; i < teamPlayerIds.Count; i++)
                {
                    if (teamPlayerIds[i] == clientId)
                    {
                        teamPlayerIds.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        #endregion
        #endregion

        #region Nested
        [Serializable]
        public class TeamData
        {
            public string name;
            public Color colour;
            public TextAsset creaturePreset;
            public Transform[] spawnPoints;
        }
        #endregion
    }
}