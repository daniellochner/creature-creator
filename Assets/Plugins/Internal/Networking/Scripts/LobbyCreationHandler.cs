using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class LobbyCreationHandler : MonoBehaviourSingleton<LobbyCreationHandler>
    {
        #region Fields
        private Coroutine heartbeatLobbyCoroutine;
        private ConcurrentQueue<string> createdLobbyIds = new ConcurrentQueue<string>();
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

            if (heartbeatLobbyCoroutine != null)
            {
                StopCoroutine(heartbeatLobbyCoroutine);
            }
            heartbeatLobbyCoroutine = StartCoroutine(HeartbeatLobbyRoutine(lobby.Id, 10));

            return lobby;
        }

        private void DeleteActiveLobbies()
        {
            while (createdLobbyIds.TryDequeue(out var lobbyId))
            {
                Lobbies.Instance.DeleteLobbyAsync(lobbyId);
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