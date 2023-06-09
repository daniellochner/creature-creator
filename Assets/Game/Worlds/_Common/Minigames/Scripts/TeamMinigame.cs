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

        private int myTeamId, myTeamIndex;
        private List<List<ulong>> teamPlayers = new List<List<ulong>>();
        #endregion

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
            players.Shuffle();

            int counter = 0;
            for (int t = 0; t < teams.Length; t++)
            {
                teamPlayers.Add(new List<ulong>());

                for (int i = 0; i < players.Count / teams.Length; i++, counter++)
                {
                    AssignTeam(players[counter], t, i);
                }
            }

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

        public override Transform GetSpawnPoint()
        {
            return teams[myTeamId].spawnPoints[myTeamIndex];
        }

        public override void OnSetupScoreboard()
        {
            foreach (TeamData teamData in teams)
            {
                Scoreboard.Add(new Score()
                {
                    id = teamData.nameId,
                    score = 0,
                    displayName = teamData.Name
                });
            }
        }

        #endregion




        #region Completing

        protected override void OnShutdown()
        {
            base.OnShutdown();

            teamPlayers.Clear();
        }

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
                return teams[winningTeam].Name;
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

        #endregion



        #region Nested

        [Serializable]
        public class TeamData
        {
            public string nameId;
            public Color colour;
            public TextAsset creaturePreset;
            public Transform[] spawnPoints;

            public string Name => LocalizationUtility.Localize(nameId);
        }
        #endregion
    }
}