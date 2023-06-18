using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class IndividualMinigame : Minigame
    {
        #region Fields
        [Header("Individual Minigame")]
        [SerializeField] private Transform[] spawnPoints;

        protected Dictionary<ulong, int> playerIndices = new Dictionary<ulong, int>();
        private int myIndex;
        #endregion

        #region Methods
        protected override void Setup()
        {
            base.Setup();

            waitingForPlayers.onExit += OnWaitingForPlayersExit;
            completing.onExit += OnCompletingExit;
        }

        #region Waiting For Players
        private void OnWaitingForPlayersExit()
        {
            Assign();
        }

        private void Assign()
        {
            // Randomize ordering of players
            players.Shuffle();

            // Assign indexes
            for (int i = 0; i < players.Count; i++)
            {
                AssignClientRpc(i, NetworkUtils.SendTo(players[i]));
                playerIndices[players[i]] = i;
            }
        }
        [ClientRpc]
        private void AssignClientRpc(int index, ClientRpcParams sendTo)
        {
            myIndex = index;
        }
        #endregion

        #region Starting
        public override void OnSetupScoreboard()
        {
            foreach (ulong clientId in players)
            {
                Scoreboard.Add(new Score()
                {
                    id = clientId.ToString(),
                    score = 0,
                    displayName = NetworkHostManager.Instance.Players[clientId].username
                });
            }
        }

        public override Transform GetSpawnPoint()
        {
            return spawnPoints[myIndex];
        }
        #endregion

        #region Playing
        public int  GetScore(ulong clientId)
        {
            return Scoreboard[playerIndices[clientId]].score;
        }
        public void SetScore(ulong clientId, int score)
        {
            Scoreboard[playerIndices[clientId]] = new Score(Scoreboard[playerIndices[clientId]], score);
        }
        #endregion

        #region Completing
        private void OnCompletingExit()
        {
            playerIndices.Clear();
        }

        protected override List<ulong> GetWinnerClientIds()
        {
            List<ulong> winnerClientIds = new List<ulong>();
            int targetScore = isAscendingOrder ? int.MaxValue : int.MinValue;

            for (int i = 0; i < Scoreboard.Count; i++)
            {
                Score score = Scoreboard[i];
                if ((!isAscendingOrder && score.score >= targetScore) || (isAscendingOrder && score.score <= targetScore))
                {
                    ulong clientId = ulong.Parse(score.id.ToString());
                    if (score.score == targetScore)
                    {
                        winnerClientIds.Add(clientId);
                    }
                    else
                    {
                        winnerClientIds.Clear();
                        winnerClientIds.Add(clientId);

                        targetScore = score.score;
                    }
                }
            }

            return winnerClientIds;
        }

        protected override string GetWinnerName()
        {
            return GetWinnerNames().JoinAnd();
        }
        private List<string> GetWinnerNames()
        {
            List<string> winnerNames = new List<string>();

            foreach (ulong clientId in GetWinnerClientIds())
            {
                winnerNames.Add(NetworkHostManager.Instance.Players[clientId].username);
            }

            return winnerNames;
        }
        #endregion
        #endregion
    }
}