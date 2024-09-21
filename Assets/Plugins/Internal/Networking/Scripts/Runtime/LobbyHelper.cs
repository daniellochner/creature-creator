using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class LobbyHelper : MonoBehaviourSingleton<LobbyHelper>
    {
        #region Fields
        private Coroutine heartbeatLobbyCoroutine;
        private ConcurrentQueue<string> createdLobbyIds = new ConcurrentQueue<string>();
        #endregion

        #region Properties
        public Lobby JoinedLobby
        {
            get; private set;
        }
        #endregion

        #region Methods
        private void OnApplicationQuit()
        {
            DeleteActiveLobbies();
        }

        public async Task<Lobby> CreateLobbyAsync(string name, int maxPlayers, CreateLobbyOptions options)
        {
            if (createdLobbyIds.Count > 0)
            {
                DeleteActiveLobbies();
            }
            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(name, maxPlayers, options);
            createdLobbyIds.Enqueue(lobby.Id);
            JoinedLobby = lobby;

            CoroutineUtility.StopStartCoroutine(this, HeartbeatLobbyRoutine(lobby.Id, 10), ref heartbeatLobbyCoroutine);

            return lobby;
        }
        public async Task<Lobby> JoinLobbyByIdAsync(string lobbyId, JoinLobbyByIdOptions options)
        {
            return (JoinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, options));
        }
        public async Task<Lobby> JoinLobbyByCodeAsync(string lobbyCode, JoinLobbyByCodeOptions options)
        {
            return (JoinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, options));
        }

        public void DeleteActiveLobbies()
        {
            while (createdLobbyIds.TryDequeue(out var lobbyId))
            {
                Lobbies.Instance.DeleteLobbyAsync(lobbyId);
            }
            JoinedLobby = null;

            if (heartbeatLobbyCoroutine != null)
            {
                StopCoroutine(heartbeatLobbyCoroutine);
            }
        }
        private IEnumerator HeartbeatLobbyRoutine(string lobbyId, float waitTime)
        {
            var delay = new WaitForSecondsRealtime(waitTime);
            while (true)
            {
                Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
                yield return delay;
            }
        }
        #endregion
    }
}