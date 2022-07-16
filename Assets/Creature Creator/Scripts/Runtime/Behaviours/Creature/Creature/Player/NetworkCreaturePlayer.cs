// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Text;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class NetworkCreaturePlayer : NetworkCreature
    {
        #region Fields
        [SerializeField] private Player player;
        #endregion

        #region Properties
        public Player Player => player;

        #endregion

        #region Methods
        public override void Setup()
        {
            base.Setup();

            if (NetworkConnectionManager.IsConnected)
            {
                if (IsOwner)
                {
                    if (player.Creature.Mover.RequestToMove)
                    {
                        player.Creature.Mover.OnTurnRequest += RequestToTurn;
                        player.Creature.Mover.OnMoveRequest += RequestToMove;
                    }

                    player.Creature.Health.OnDie += () => SendDeathMsgServerRpc(OwnerClientId);

                    
                }
                else
                {
                    //target.Setup();
                }
            }
        }
        
        #region Death Messages
        private readonly string[] DEATH_MESSAGES = new string[]
        {
            "{username} died.",
            "{username} kicked the bucket.",
            "{username} passed away.",
            "{username} expired.",
            "{username} bit the dust.",
            "{username} perished.",
        };

        [ServerRpc]
        private void SendDeathMsgServerRpc(ulong clientId)
        {
            string message = DEATH_MESSAGES[Random.Range(0, DEATH_MESSAGES.Length)];
            SendDeathMsgClientRpc(message.Replace("{username}", NetworkHostManager.Instance.Players[clientId].username));
        }
        [ClientRpc]
        private void SendDeathMsgClientRpc(string message)
        {
            NotificationsManager.Notify(message);
        }
        #endregion
        
        #region Load Player
        [ClientRpc]
        public void LoadPlayerClientRpc(PlayerData playerData, string creatureData, ClientRpcParams clientRpcParams)
        {
            NetworkCreaturesMenu.Instance.AddPlayer(playerData);
            if (!IsHidden.Value)
            {
                ReconstructAndShowClientRpc(creatureData);
            }
        }
        #endregion

        #region Move
        [ServerRpc]
        private void RequestToMoveServerRpc(Vector3 direction, ulong clientId)
        {
            RequestToMoveClientRpc(direction, NetworkUtils.SendTo(clientId));
        }
        [ClientRpc]
        private void RequestToMoveClientRpc(Vector3 direction, ClientRpcParams clientRpcParams)
        {
            player.Creature.Mover.Move(direction);
        }
        private void RequestToMove(Vector3 direction)
        {
            RequestToMoveServerRpc(direction, OwnerClientId);
        }
        #endregion

        #region Turn
        [ServerRpc]
        private void RequestToTurnServerRpc(float angle, ulong clientId)
        {
            RequestToTurnClientRpc(angle, NetworkUtils.SendTo(clientId));
        }
        [ClientRpc]
        private void RequestToTurnClientRpc(float angle, ClientRpcParams clientRpcParams)
        {
            player.Creature.Mover.Turn(angle);
        }
        private void RequestToTurn(float angle)
        {
            RequestToTurnServerRpc(angle, OwnerClientId);
        }
        #endregion
        #endregion
    }
}