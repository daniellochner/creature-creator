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
        private Coroutine pingLobbyCoroutine;
        private ConcurrentQueue<string> createdLobbyIds = new ConcurrentQueue<string>();
        #endregion

        #region Methods
        private void OnApplicationQuit()
        {
            DeleteLobbies();
        }

        public void DeleteLobbies()
        {
            while (createdLobbyIds.TryDequeue(out var lobbyId))
            {
                Lobbies.Instance.DeleteLobbyAsync(lobbyId);
            }
        }
        public async Task<Lobby> CreateLobbyAsync(string name, int maxPlayers, CreateLobbyOptions options)
        {
            if (createdLobbyIds.Count > 3)
            {
                throw new LobbyServiceException(LobbyExceptionReason.Unauthorized, "Cannot create more than three active lobbies.");
            }

            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(name, maxPlayers, options);
            createdLobbyIds.Enqueue(lobby.Id);

            if (pingLobbyCoroutine != null)
            {
                StopCoroutine(pingLobbyCoroutine);
            }
            pingLobbyCoroutine = StartCoroutine(HeartbeatLobbyRoutine(lobby.Id, 10));

            return lobby;
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